using System.Windows.Input;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace HospitalLeaveApplication.ViewModels
{
    public class LeaveApplicationViewModel : BaseViewModel
    {
        private bool hasError;
        private string errorMessage;
        private DateTime minFromDate;
        private DateTime minToDate;
        private User loggedInUser;
        private LeaveApplication leaveApplication;
        private string selectedLeaveType;
        private User selectedProxyUser;
        private bool isResidenceEnable;

        public ICommand LeaveApplicationCommand { get; }
        public ObservableRangeCollection<string> LeaveTypes { get; }
        public ObservableRangeCollection<User> ProxyUserList { get; }
        public bool HasError { get => hasError; set => SetProperty(ref hasError, value); }
        public string ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }
        public DateTime MinFromDate { get => minFromDate; set => SetProperty(ref minFromDate, value); }
        public DateTime MinToDate { get => minToDate; set => SetProperty(ref minToDate, value); }
        public User LoggedInUser { get => loggedInUser; set => SetProperty(ref loggedInUser, value); }
        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public User SelectedProxyUser { get => selectedProxyUser; set => SetProperty(ref selectedProxyUser, value); }
        public string SelectedLeaveType
        {
            get => selectedLeaveType;
            set
            {
                if(value == "Casual")
                {
                    IsResidenceEnable = false;
                }
                else
                {
                    IsResidenceEnable = true;
                }
                SetProperty(ref selectedLeaveType, value);
            }
        }
        public bool IsResidenceEnable { get => isResidenceEnable; set => SetProperty(ref isResidenceEnable, value); }

        public LeaveApplicationViewModel()
        {
            MinFromDate = DateTime.Now;
            MinToDate = DateTime.Now;
            LoggedInUser = new User();
            LeaveApplication = new LeaveApplication()
            {
                FromDate = MinFromDate,
                ToDate = MinToDate
            };
            SelectedProxyUser = new User();
            LeaveApplicationCommand = new AsyncCommand(ExecuteLeaveApplication);
            LeaveTypes = new ObservableRangeCollection<string>();
            ProxyUserList = new ObservableRangeCollection<User>();
        }

        private async Task ExecuteLeaveApplication()
        {
            try
            {
                if(SelectedProxyUser == null)
                {
                    HasError = true;
                    ErrorMessage = "Please select a proxy user";
                    return;
                }
                bool isValid = await ValidateApplication(LoggedInUser.Email);
                bool isValidProxy = await ValidateApplication(SelectedProxyUser.Email);
                if (!isValid)
                {
                    HasError = true;
                    ErrorMessage = "Already has leave in these date";
                    return;
                }
                if (!isValidProxy)
                {
                    HasError = true;
                    ErrorMessage = "Proxy already has leave in these date";
                    return;
                }
                isValid = await ValidateApplication(LoggedInUser.Email, "proxy");
                isValidProxy = await ValidateApplication(SelectedProxyUser.Email, "proxy");
                if (!isValidProxy)
                {
                    HasError = true;
                    ErrorMessage = "Proxy already has another proxy in these date";
                    return;
                }
                if (isValid && isValidProxy)
                {
                    LeaveApplication.Key = string.Format("{0}{1}", LoggedInUser.Email, DateTime.Now.ToString("yyyyMMddHHmmss"));
                    LeaveApplication.LeaveType = SelectedLeaveType;
                    LeaveApplication.Days = (LeaveApplication.ToDate - LeaveApplication.FromDate).Days + 1;
                    LeaveApplication.ProxyName = SelectedProxyUser.Name;
                    leaveApplication.ProxyEmail = SelectedProxyUser.Email;
                    LeaveApplication.Role = LoggedInUser.SubCategory;
                    LeaveApplication.Status = "Pending";
                    FirebaseResponse response = await LeaveApplicationService.StoreLeaveApplication(LeaveApplication);
                    if (response != null && response.Code == 200)
                    {
                        await Shell.Current.DisplayAlert("Submitted", response.Message, "Ok");
                        StaticCredential.LeaveApplicationStatus = "Pending";
                        await Shell.Current.GoToAsync("//LeaveApplicationListPage");
                    }
                    else if (response != null)
                    {
                        await Shell.Current.DisplayAlert("Submitted", response.Message, "Ok");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Internal error occured, please try again later.", "Ok");
                    }
                }
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Internal error occured, please try again later.", "Ok");
                SelectedProxyUser = null;
            }
        }

        public void OnAppearing()
        {
            HasError = false;
            GetLeaveTypes();
            SelectedLeaveType = "Casual";
            Task.Run(async () => await GetToken());
        }

        private async Task GetToken()
        {
            LoggedInUser = StaticCredential.User;
            if(LoggedInUser == null)
            {
                LoggedInUser = await LocalDBService.GetToken();
            }
            await GetUserDetails();
            await GetUserList();
        }

        private void GetLeaveTypes()
        {
            LeaveTypes.Clear();
            LeaveTypes.AddRange(StaticCredential.GetLeaveTypes());
        }

        private async Task GetUserDetails()
        {
            try
            {
                LoggedInUser = await UserService.GetUserAsync(LoggedInUser.Email);
                LeaveApplication.Name = LoggedInUser.Name;
                LeaveApplication.Email = LoggedInUser.Email;
            }
            catch(Exception ex)
            {   

            }
        }

        private async Task GetUserList()
        {
            try
            {
                Pathway pathway = await PathwayService.GetPathwayAsync(LoggedInUser.SubCategory);
                List<User> users = await UserService.GetUsersAsync(pathway.Forward.Split("/").ToList());
                MainThread.BeginInvokeOnMainThread(() => {
                    ProxyUserList.Clear();
                    ProxyUserList.AddRange(users.Where(u => u.Email != LoggedInUser.Email).ToList());
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async Task<bool> ValidateApplication(string email, string type = null)
        {
            List<LeaveApplication> leaveApplications = new List<LeaveApplication>();
            if (type == "proxy")
            {
                leaveApplications = await LeaveApplicationService.GetLeaveApplicationsByProxyAsync(email);
            }
            else
            {
                leaveApplications = await LeaveApplicationService.GetLeaveApplicationsByEmailAsync(email);
            }
            leaveApplications = leaveApplications.Where(l => l.Status != "Rejected").ToList();
            int fromCount = leaveApplications.Where(l => l.FromDate.Date >= leaveApplication.FromDate.Date && l.FromDate.Date <= leaveApplication.ToDate.Date).Count();
            int toCount = leaveApplications.Where(l => l.ToDate.Date >= leaveApplication.FromDate.Date && l.ToDate.Date <= leaveApplication.ToDate.Date).Count();
            int midCount = leaveApplications.Where(l => l.FromDate.Date >= leaveApplication.FromDate.Date && l.ToDate.Date <= leaveApplication.ToDate.Date).Count();
            int outCount = leaveApplications.Where(l => l.FromDate.Date <= leaveApplication.FromDate.Date && l.ToDate.Date >= leaveApplication.ToDate.Date).Count();
            return fromCount + toCount + midCount + outCount <= 0;
        }
    }
}

