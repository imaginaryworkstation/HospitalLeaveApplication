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
        public ObservableRangeCollection<LeaveApplication> LeaveApplicationList { get; }

        public ProxyListViewModel()
        {
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
                await GetLeaveApplications();
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
                LeaveApplicationList.AddRange(await LeaveApplicationService.GetLeaveApplicationsByProxyAsync(LoggedInUser.Email));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
