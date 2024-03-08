using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;

namespace HospitalLeaveApplication.ViewModels
{
    public class ProxyListViewModel : BaseViewModel
    {
        private string status;
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
                StaticCredential.ProxyStatus = value;
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
                SelectedLeaveStatus = StaticCredential.ProxyStatus ?? "All";
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
                LeaveStatusList.Add("All");
                LeaveStatusList.Add("Pending");
                LeaveStatusList.Add("Agreed");
                LeaveStatusList.Add("Denied");
            }
            catch (Exception ex)
            {

            }
        }

        private async Task GetLeaveApplications()
        {
            try
            {
                IsBusy = true;
                status = SelectedLeaveStatus == "All" ? null : SelectedLeaveStatus;
                var list = await LeaveApplicationService.GetLeaveApplicationsByProxyAsync(LoggedInUser.Email, status);
                MainThread.BeginInvokeOnMainThread(() => 
                {
                    LeaveApplicationList.Clear();
                    LeaveApplicationList.AddRange(list);
                });
            }
            catch (Exception ex)
            {

            }
            finally { IsBusy = false; }
        }
    }
}
