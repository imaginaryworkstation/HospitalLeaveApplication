using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    [QueryProperty(nameof(key), "key")]
    public class NotificationDetailViewModel : BaseViewModel
    {
        private User LoggedInUser;
        private string FirebaseKey { get; set; }
        private string UserFirebsaeKey { get; set; }
        public string key { get; set; }
        private bool isEditable;
        private bool isNotEditable;
        FirebaseObject<LeaveApplication> firebaseLeaveApplication;
        private LeaveApplication leaveApplication;
        private User user;
        private User proxyUser;
        private bool isResidenceEnable;

        public ICommand LeaveApplicationCommand { get; }
        public bool IsEditable
        {
            get => isEditable;
            set
            {
                SetProperty(ref isEditable, value);
                IsNotEditable = !value;
            }
        }
        public bool IsNotEditable { get => isNotEditable; set => SetProperty(ref isNotEditable, value); }
        public ObservableRangeCollection<string> LeaveStatusList { get; }
        public FirebaseObject<LeaveApplication> FirebaseLeaveApplication { get => firebaseLeaveApplication; set => SetProperty(ref firebaseLeaveApplication, value); }
        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public User User { get => user; set => SetProperty(ref user, value); }
        public User ProxyUser { get => proxyUser; set => SetProperty(ref proxyUser, value); }
        public bool IsResidenceEnable { get => isResidenceEnable; set => SetProperty(ref isResidenceEnable, value); }

        public NotificationDetailViewModel()
        {
            LeaveStatusList = new ObservableRangeCollection<string>();
            LeaveApplicationCommand = new AsyncCommand(ExecuteLeaveApplication);
        }

        private async Task ExecuteLeaveApplication()
        {
            await LeaveApplicationService.UpdateLeaveApplicationAsync(FirebaseKey, LeaveApplication);
            User.Enjoyed += LeaveApplication.Days;
            User.Remaining -= LeaveApplication.Days;
            await UserService.UpdateUserAsync(UserFirebsaeKey, User);
            await Shell.Current.GoToAsync("..");
        }

        public void OnAppearing()
        {
            Task.Run(async () => await GetLoggedInUser());
        }

        private async Task GetLoggedInUser()
        {
            try
            {
                LoggedInUser = StaticCredential.User;
                if (LoggedInUser == null)
                {
                    LoggedInUser = await LocalDBService.GetToken();
                }
                GetLeaveStatusList();
            }
            catch (Exception ex)
            {

            }
        }
        private async Task GetLeaveApplicationDetail()
        {
            FirebaseObject<LeaveApplication> FirebaseLeaveApplication = await LeaveApplicationService.GetLeaveApplicationByKeyAsync(key);
            LeaveApplication = FirebaseLeaveApplication.Object as LeaveApplication;
            FirebaseKey = FirebaseLeaveApplication.Key;
            IsResidenceEnable = LeaveApplication.LeaveType == "Casual" ? false : true;
            if (LeaveApplication != null)
            {
                LeaveStatusList.Clear();
                if (LoggedInUser.Category == "UHFPO")
                {
                    if (LeaveApplication.Status == "Pending")
                    {
                        LeaveStatusList.Add("Pending");
                    }
                    LeaveStatusList.Add("Accepted");
                    LeaveStatusList.Add("Approved");
                    IsEditable = LeaveApplication.Status == "Accepted";
                }
                else if (LoggedInUser.Category == "Approver")
                {
                    if (LeaveApplication.Status == "Pending")
                    {
                        LeaveStatusList.Add("Pending");
                    }
                    LeaveStatusList.Add("Reccomended");
                    LeaveStatusList.Add("Accepted");
                    IsEditable = LeaveApplication.Status == "Reccomended";
                }
                else
                {
                    LeaveStatusList.Add("Forwarded");
                    LeaveStatusList.Add("Reccomended");
                    IsEditable = LeaveApplication.Status == "Forwarded";
                }
                LeaveStatusList.Add("Rejected");
                await GetUserDetail();
            }
        }

        private void GetLeaveStatusList()
        {
            try
            {
                //LeaveStatusList.Clear();
                //if (LoggedInUser.Category == "UHFPO")
                //{
                //    LeaveStatusList.Add("Pending");
                //    LeaveStatusList.Add("Accepted");
                //    LeaveStatusList.Add("Approved");
                //}
                //else if (LoggedInUser.Category == "Approver")
                //{
                //    LeaveStatusList.Add("Pending");
                //    LeaveStatusList.Add("Reccomended");
                //    LeaveStatusList.Add("Accepted");
                //}
                //else
                //{
                //    LeaveStatusList.Add("Forwarded");
                //    LeaveStatusList.Add("Reccomended");
                //}
                Task.Run(async () => await GetLeaveApplicationDetail());
            }
            catch(Exception ex)
            {

            }
        }

        private async Task GetUserDetail()
        {
            try
            {
                FirebaseObject<User> firebaseUser = await UserService.GetUserWithKeyAsync(LeaveApplication.Email);
                UserFirebsaeKey = firebaseUser.Key;
                User = firebaseUser.Object as User;
                ProxyUser = await UserService.GetUserAsync(LeaveApplication.ProxyEmail);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
