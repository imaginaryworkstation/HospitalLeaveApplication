using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    [QueryProperty(nameof(key), "key")]

    class LeaveApplicationDetailViewModel : BaseViewModel
    {
        private string FirebaseKey { get; set; }
        public string key { get; set; }
        FirebaseObject<LeaveApplication> firebaseLeaveApplication;

        private User loggedInUser;
        private DateTime minFromDate;
        private DateTime minToDate;
        private string selectedLeaveType;
        private User selectedProxyUser;
        private string selectedProxyUserString;
        private bool isPending;
        private bool isNotPending;

        private bool isApproved;
        private bool isShowing;
        private bool isEditing;
        private bool hasError;
        private string errorMessage;

        private LeaveApplication leaveApplication;
        private User user;
        private User proxyUser;
        private bool isResidenceEnable;

        private bool dropdownNotClicked;
        private bool dropdownClicked;

        public ICommand LeaveApplicationCommand { get; }
        public ICommand UpdateLeaveApplicationCommand { get; }
        public ICommand ToggleFormCommand { get; }
        public ICommand ProxyTapCommand { get; }
        public ObservableRangeCollection<string> LeaveTypeList { get; }
        public ObservableRangeCollection<User> ProxyUserList { get; }
        public ObservableRangeCollection<string> ProxyUserStringList { get; }

        public FirebaseObject<LeaveApplication> FirebaseLeaveApplication { get => firebaseLeaveApplication; set => SetProperty(ref firebaseLeaveApplication, value); }


        public User LoggedInUser { get => loggedInUser; set => SetProperty(ref loggedInUser, value); }
        public DateTime MinFromDate { get => minFromDate; set => SetProperty(ref minFromDate, value); }
        public DateTime MinToDate { get => minToDate; set => SetProperty(ref minToDate, value); }
        public User SelectedProxyUser { get => selectedProxyUser; set => SetProperty(ref selectedProxyUser, value); }
        public string SelectedProxyUserString { get => selectedProxyUserString; set => SetProperty(ref selectedProxyUserString, value); }
        public string SelectedLeaveType
        {
            get => selectedLeaveType;
            set
            {
                if (value == "Casual")
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
        public bool IsPending
        {
            get => isPending;
            set
            {
                SetProperty(ref isPending, value);
                IsNotPending = !value;
            }
        }
        public bool IsNotPending { get => isNotPending; set => SetProperty(ref isNotPending, value); }
        public bool IsApproved { get => isApproved; set => SetProperty(ref isApproved, value); }
        public bool IsShowing
        {
            get => isShowing;
            set
            {
                SetProperty(ref isShowing, value);
                IsEditing = !value;
            }
        }
        public bool IsEditing { get => isEditing; set => SetProperty(ref isEditing, value); }

        public bool HasError { get => hasError; set => SetProperty(ref hasError, value); }
        public string ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }

        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public User User { get => user; set => SetProperty(ref user, value); }
        public User ProxyUser { get => proxyUser; set => SetProperty(ref proxyUser, value); }
        public bool IsResidenceEnable { get => isResidenceEnable; set => SetProperty(ref isResidenceEnable, value); }
        public bool DropdownNotClicked 
        { 
            get => dropdownNotClicked;
            set
            {
                SetProperty(ref dropdownNotClicked, value);
                DropdownClicked = !value;
            }
        }
        public bool DropdownClicked { get => dropdownClicked; set => SetProperty(ref dropdownClicked, value); }

        public LeaveApplicationDetailViewModel()
        {
            MinFromDate = DateTime.Now;
            MinToDate = DateTime.Now;
            SelectedProxyUser = new User();
            LeaveApplicationCommand = new AsyncCommand(ExecuteLeaveApplication);
            UpdateLeaveApplicationCommand = new AsyncCommand(ExecuteUpdateLeaveApplication);
            ToggleFormCommand = new AsyncCommand(ExecuteToggleForm);
            ProxyTapCommand = new AsyncCommand(ExecuteProxyTap);
            LeaveTypeList = new ObservableRangeCollection<string>();
            ProxyUserList = new ObservableRangeCollection<User>();
        }

        private async Task ExecuteProxyTap()
        {
            DropdownClicked = true;
        }

        private async Task ExecuteToggleForm()
        {
            IsShowing = !IsShowing;
            if (!IsShowing)
            {
                DropdownNotClicked = true;
                await GetEditInfo();
            }
        }

        private async Task ExecuteLeaveApplication()
        {
            await LeaveApplicationService.UpdateLeaveApplicationAsync(FirebaseKey, LeaveApplication);
        }

        private async Task ExecuteUpdateLeaveApplication()
        {
            bool isValid = await ValidateApplication(LoggedInUser.Email);
            bool isValidProxy = true;
            if (SelectedProxyUser != null && SelectedProxyUser.Email != null)
            {
                isValidProxy = await ValidateApplication(SelectedProxyUser.Email);
            }
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
            if (SelectedProxyUser != null && SelectedProxyUser.Email != null)
            {
                isValidProxy = await ValidateApplication(SelectedProxyUser.Email, "proxy");
            }
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
                LeaveApplication.Days = (LeaveApplication.ToDate.Date - LeaveApplication.FromDate.Date).Days + 1;
                LeaveApplication.ProxyName = SelectedProxyUser != null && SelectedProxyUser.Name != null ? SelectedProxyUser.Name : "";
                leaveApplication.ProxyEmail = SelectedProxyUser != null && SelectedProxyUser.Email != null ? SelectedProxyUser.Email : "";
                LeaveApplication.Role = LoggedInUser.SubCategory;
                LeaveApplication.Status = "Pending";
                FirebaseResponse response = await LeaveApplicationService.UpdateLeaveApplicationAsync(FirebaseKey, LeaveApplication);
                if (response != null && response.Code == 200)
                {
                    await Shell.Current.DisplayAlert("Submitted", "Leave status successfully updated", "Ok");
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

        public void OnAppearing()
        {
            Task.Run(async () => await GetToken());
        }

        private async Task GetToken()
        {
            LoggedInUser = StaticCredential.User;
            if (LoggedInUser == null)
            {
                LoggedInUser = await LocalDBService.GetToken();
            }
            await GetLeaveApplicationDetail();
            await GetUserList();
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

        private async Task GetLeaveApplicationDetail()
        {
            IsShowing = true;
            FirebaseObject<LeaveApplication> FirebaseLeaveApplication = await LeaveApplicationService.GetLeaveApplicationByKeyAsync(key);
            LeaveApplication = FirebaseLeaveApplication.Object as LeaveApplication;
            FirebaseKey = FirebaseLeaveApplication.Key;
            IsResidenceEnable = LeaveApplication.LeaveType == "Casual" ? false : true;
            if(LeaveApplication != null)
            {
                IsApproved = LeaveApplication.Status == "Approved";
                IsPending = LeaveApplication.Status == "Pending";
                await GetUserDetail();
            }
        }

        private async Task GetUserDetail()
        {
            User = await UserService.GetUserAsync(LeaveApplication.Email);
        }
        private async Task GetEditInfo()
        {
            GetLeaveTypes();
            await GetProxyDetails();
            SelectedLeaveType = LeaveApplication.LeaveType;
        }
        private void GetLeaveTypes()
        {
            LeaveTypeList.Clear();
            LeaveTypeList.AddRange(StaticCredential.GetLeaveTypes());
        }

        private async Task GetProxyDetails()
        {
            SelectedProxyUser = await UserService.GetUserAsync(LeaveApplication.ProxyEmail);
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
            leaveApplications = leaveApplications.Where(l => l.Key != LeaveApplication.Key).ToList();
            leaveApplications = leaveApplications.Where(l => l.Status != "Rejected" && l.Status != "Declined" && l.Status != "Denied").ToList();
            int fromCount = leaveApplications.Where(l => l.FromDate.Date >= leaveApplication.FromDate.Date && l.FromDate.Date <= leaveApplication.ToDate.Date).Count();
            int toCount = leaveApplications.Where(l => l.ToDate.Date >= leaveApplication.FromDate.Date && l.ToDate.Date <= leaveApplication.ToDate.Date).Count();
            int midCount = leaveApplications.Where(l => l.FromDate.Date >= leaveApplication.FromDate.Date && l.ToDate.Date <= leaveApplication.ToDate.Date).Count();
            int outCount = leaveApplications.Where(l => l.FromDate.Date <= leaveApplication.FromDate.Date && l.ToDate.Date >= leaveApplication.ToDate.Date).Count();
            return fromCount + toCount + midCount + outCount <= 0;
        }
    }
}
