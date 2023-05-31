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
    public class NotificationDetailViewModel : BaseViewModel
    {
        private string AcceptStatus;
        private string RejectStatus;
        private User LoggedInUser;
        private string FirebaseKey { get; set; }
        private string UserFirebsaeKey { get; set; }
        public string key { get; set; }
        FirebaseObject<LeaveApplication> firebaseLeaveApplication;
        private LeaveApplication leaveApplication;
        private User user;
        private User proxyUser;
        private bool isResidenceEnable;
        private string acceptButtonText;
        private string rejectButtonText;
        private bool isButtonVisible;
        private bool isButtonNotVisible;

        public ICommand AcceptApplicationCommand { get; }
        public ICommand RejectApplicationCommand { get; }
        public FirebaseObject<LeaveApplication> FirebaseLeaveApplication { get => firebaseLeaveApplication; set => SetProperty(ref firebaseLeaveApplication, value); }
        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public User User { get => user; set => SetProperty(ref user, value); }
        public User ProxyUser { get => proxyUser; set => SetProperty(ref proxyUser, value); }
        public bool IsResidenceEnable { get => isResidenceEnable; set => SetProperty(ref isResidenceEnable, value); }
        public bool IsButtonVisible
        {
            get => isButtonVisible;
            set
            {
                SetProperty(ref isButtonVisible, value);
                IsButtonNotVisible = !value;
            }
        }
        public bool IsButtonNotVisible { get => isButtonNotVisible; set => SetProperty(ref isButtonNotVisible, value); }
        public string AcceptButtonText { get => acceptButtonText; set => SetProperty(ref acceptButtonText, value); }
        public string RejectButtonText { get => rejectButtonText; set => SetProperty(ref rejectButtonText, value); }

        public NotificationDetailViewModel()
        {
            AcceptApplicationCommand = new AsyncCommand(ExecuteAcceptLeaveCommand);
            RejectApplicationCommand = new AsyncCommand(ExecuteRejectLeaveCommand);
        }

        private async Task ExecuteAcceptLeaveCommand()
        {
            LeaveApplication.Status = AcceptStatus;
            await ExecuteLeaveApplication();
        }

        private async Task ExecuteRejectLeaveCommand()
        {
            LeaveApplication.Status = RejectStatus;
            await ExecuteLeaveApplication();
        }

        private async Task ExecuteLeaveApplication()
        {
            try
            {
                if (LeaveApplication.Status == "Approved")
                {
                    LeaveApplication.ApproveDate = DateTime.Now;
                    if (LeaveApplication.LeaveType != "Casual")
                    {
                        User.Enjoyed += LeaveApplication.Days;
                        User.Remaining -= LeaveApplication.Days;
                    }
                }
                FirebaseResponse response = await LeaveApplicationService.UpdateLeaveApplicationAsync(FirebaseKey, LeaveApplication);
                if (response != null && response.Code == 200)
                {
                    await UserService.UpdateUserAsync(UserFirebsaeKey, User);
                    await Shell.Current.DisplayAlert(LeaveApplication.Status, response.Message, "Ok");
                    StaticCredential.NotificationStatus = LeaveApplication.Status;
                    await Shell.Current.GoToAsync("..");
                }
                else if (response != null)
                {
                    await Shell.Current.DisplayAlert("Error", response.Message, "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Internal error occured, please try again later.", "Ok");
                }
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Internal error occured, please try again later.", "Ok");
            }
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
                await GetLeaveApplicationDetail();
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
            IsResidenceEnable = LeaveApplication.LeaveType != "Casual";
            if (LeaveApplication != null)
            {
                if (LoggedInUser.Category == "UHFPO")
                {
                    AcceptButtonText = "Approve";
                    RejectButtonText = "Reject";
                    IsButtonVisible = LeaveApplication.Status == "Forwarded";
                    AcceptStatus = "Approved";
                    RejectStatus = "Rejected";
                }
                else if (LoggedInUser.Category == "Admin")
                {
                    AcceptButtonText = "Forward";
                    RejectButtonText = "Send back";
                    IsButtonVisible = LeaveApplication.Status == "Recommended" || LeaveApplication.Status == "Pending";
                    AcceptStatus = "Forwarded";
                    RejectStatus = "Sent back";
                }
                else
                {
                    AcceptButtonText = "Recommend";
                    RejectButtonText = "Decline";
                    IsButtonVisible = LeaveApplication.Status == "Agreed";
                    AcceptStatus = "Recommended";
                    RejectStatus = "Declined";
                }
                await GetUserDetail();
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
