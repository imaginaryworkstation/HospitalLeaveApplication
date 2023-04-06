using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HospitalLeaveApplication.ViewModels
{
    [QueryProperty(nameof(key), "key")]

    class LeaveApplicationDetailViewModel : BaseViewModel
    {
        public string key { get; set; }
        private LeaveApplication leaveApplication;
        private User user;
        private User proxyUser;
        private bool isResidenceEnable;
        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public User User { get => user; set => SetProperty(ref user, value); }
        public User ProxyUser { get => proxyUser; set => SetProperty(ref proxyUser, value); }
        public bool IsResidenceEnable { get => isResidenceEnable; set => SetProperty(ref isResidenceEnable, value); }
        public void OnAppearing()
        {
            Task.Run(async () => await GetLeaveApplicationDetail());
        }

        private async Task GetLeaveApplicationDetail()
        {
            LeaveApplication = await LeaveApplicationService.GetLeaveApplicationAsync(key);
            IsResidenceEnable = LeaveApplication.LeaveType == "Casual" ? false : true;
            if(LeaveApplication != null)
            {
                await GetUserDetail();
            }
        }
        private async Task GetUserDetail()
        {
            User = await UserService.GetUserAsync(LeaveApplication.Email);
            ProxyUser = await UserService.GetUserAsync(LeaveApplication.Proxy);
        }
    }
}
