using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    [QueryProperty(nameof(key), "key")]

    class LeaveApplicationDetailViewModel : BaseViewModel
    {
        private bool isNotPending;
        private string FirebaseKey { get; set; }
        public string key { get; set; }
        FirebaseObject<LeaveApplication> firebaseLeaveApplication;
        private bool isApproved;

        private LeaveApplication leaveApplication;
        private User user;
        private User proxyUser;
        private bool isResidenceEnable;

        public ICommand LeaveApplicationCommand { get; }
        public FirebaseObject<LeaveApplication> FirebaseLeaveApplication { get => firebaseLeaveApplication; set => SetProperty(ref firebaseLeaveApplication, value); }

        public bool IsNotPending { get => isNotPending; set => SetProperty(ref isNotPending, value); }
        public bool IsApproved { get => isApproved; set => SetProperty(ref isApproved, value); }
        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public User User { get => user; set => SetProperty(ref user, value); }
        public User ProxyUser { get => proxyUser; set => SetProperty(ref proxyUser, value); }
        public bool IsResidenceEnable { get => isResidenceEnable; set => SetProperty(ref isResidenceEnable, value); }

        public LeaveApplicationDetailViewModel()
        {

            LeaveApplicationCommand = new AsyncCommand(ExecuteLeaveApplication);
        }

        private async Task ExecuteLeaveApplication()
        {
            await LeaveApplicationService.UpdateLeaveApplicationAsync(FirebaseKey, LeaveApplication);
        }

        public void OnAppearing()
        {
            Task.Run(async () => await GetLeaveApplicationDetail());
        }
        private async Task GetLeaveApplicationDetail()
        {
            FirebaseObject<LeaveApplication> FirebaseLeaveApplication = await LeaveApplicationService.GetLeaveApplicationByKeyAsync(key);
            LeaveApplication = FirebaseLeaveApplication.Object as LeaveApplication;
            FirebaseKey = FirebaseLeaveApplication.Key;
            IsResidenceEnable = LeaveApplication.LeaveType == "Casual" ? false : true;
            if(LeaveApplication != null)
            {
                IsApproved = LeaveApplication.Status == "Approved";
                IsNotPending = LeaveApplication.Status != "Pending";
                await GetUserDetail();
            }
        }

        private async Task GetUserDetail()
        {
            User = await UserService.GetUserAsync(LeaveApplication.Email);
        }
    }
}
