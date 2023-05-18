using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;

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
                SelectedLeaveStatus = StaticCredential.ProxyStatus != null ? StaticCredential.ProxyStatus : "Forwarded";
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
                LeaveStatusList.Add("Pending");
                LeaveStatusList.Add("Forwarded");
                LeaveStatusList.Add("Approved");
            }
            catch (Exception ex)
            {

            }
        }

        private async Task GetLeaveApplications()
        {
            try
            {
                var list = await LeaveApplicationService.GetLeaveApplicationsByProxyAsync(LoggedInUser.Email, SelectedLeaveStatus);
                MainThread.BeginInvokeOnMainThread(() => 
                {
                    LeaveApplicationList.Clear();
                    LeaveApplicationList.AddRange(list);
                });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
