using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLeaveApplication.ViewModels
{
    public class ProxyListViewModel : BaseViewModel
    {
        private User LoggedInUser;
        private string selectedLeaveStatus;
        public ObservableRangeCollection<LeaveApplication> LeaveApplicationList { get; }
        public ObservableRangeCollection<string> LeaveStatusList { get; }
        public string SelectedLeaveStatus
        {
            get => selectedLeaveStatus;
            set
            {
                SetProperty(ref selectedLeaveStatus, value);
                Task.Run(async () => await GetLeaveApplications());
            }
        }

        public ProxyListViewModel()
        {
            LeaveStatusList = new ObservableRangeCollection<string>();
            LeaveApplicationList = new ObservableRangeCollection<LeaveApplication>();
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
                SelectedLeaveStatus = "Approved";
            }
            catch (Exception ex)
            {

            }
        }

        private void GetLeaveStatusList()
        {
            try
            {
                LeaveStatusList.Clear();
                LeaveStatusList.AddRange(StaticCredential.GetLeaveStatus());
            }
            catch (Exception ex)
            {

            }
        }

        private async Task GetLeaveApplications()
        {
            try
            {
                LeaveApplicationList.Clear();
                LeaveApplicationList.AddRange(await LeaveApplicationService.GetLeaveApplicationsByProxyAsync(LoggedInUser.Email, SelectedLeaveStatus));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
