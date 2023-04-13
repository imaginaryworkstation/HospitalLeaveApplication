using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    [QueryProperty(nameof(key), "key")]
    public class ProxyLeaveDetailViewModel : BaseViewModel
    {
        private string FirebaseKey { get; set; }
        public string key { get; set; }
        FirebaseObject<LeaveApplication> firebaseLeaveApplication;
        private LeaveApplication leaveApplication;
        private User user;
        private bool isResidenceEnable;

        public FirebaseObject<LeaveApplication> FirebaseLeaveApplication { get => firebaseLeaveApplication; set => SetProperty(ref firebaseLeaveApplication, value); }
        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public User User { get => user; set => SetProperty(ref user, value); }
        public bool IsResidenceEnable { get => isResidenceEnable; set => SetProperty(ref isResidenceEnable, value); }

        public ProxyLeaveDetailViewModel()
        {
            
        }

        private async Task ExecuteLeaveApplication()
        {
            await LeaveApplicationService.UpdateLeaveApplicationAsync(FirebaseKey, LeaveApplication);
            await Shell.Current.GoToAsync("..");
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
            if (LeaveApplication != null)
            {
                //await GetUserDetail();
            }
        }

        private async Task GetUserDetail()
        {
            User = await UserService.GetUserAsync(LeaveApplication.Email);
        }
    }
}
