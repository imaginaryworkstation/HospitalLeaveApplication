using System.Windows.Input;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using HospitalLeaveApplication.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace HospitalLeaveApplication.ViewModels
{
    public class LeaveApplicationListViewModel : BaseViewModel
    {
        private User LoggedInUser;
        private string selectedLeaveStatus;
        public ICommand NavigateToNewLeaveApplicationsCommand { get; }
        public ObservableRangeCollection<LeaveApplication> LeaveApplicationList { get; }
        public ObservableRangeCollection<string> LeaveStatusList { get; }
        public string SelectedLeaveStatus
        {
            get => selectedLeaveStatus;
            set
            {
                SetProperty(ref selectedLeaveStatus, value);
                StaticCredential.LeaveApplicationStatus = value;
                Task.Run(async () => await GetLeaveApplications());
            }
        }
        public LeaveApplicationListViewModel()
        {
            LeaveStatusList = new ObservableRangeCollection<string>();
            NavigateToNewLeaveApplicationsCommand = new AsyncCommand(ExecuteNavigateToNewLeaveApplications);
            LeaveApplicationList = new ObservableRangeCollection<LeaveApplication>();
        }

        private async Task ExecuteNavigateToNewLeaveApplications()
        {
            await Shell.Current.GoToAsync(nameof(LeaveApplicationPage));
        }
        public void OnAppearing()
        {
            GetLeaveStatusList();
        }

        private void GetLeaveStatusList()
        {
            try
            {
                LeaveStatusList.Clear();
                LeaveStatusList.Add("All");
                LeaveStatusList.AddRange(StaticCredential.GetLeaveStatus());
                Task.Run(async () => {
                    await GetUser();
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async Task GetUser()
        {
            try
            {
                LoggedInUser = StaticCredential.User;
                if(LoggedInUser == null)
                {
                    LoggedInUser = await LocalDBService.GetToken();
                }
                SelectedLeaveStatus = StaticCredential.LeaveApplicationStatus ?? "All";
            }
            catch(Exception ex)
            {

            }
        }

        private async Task GetLeaveApplications()
        {
            try
            {
                IsBusy = true;
                var status = SelectedLeaveStatus == "All" ? null : SelectedLeaveStatus;
                var list = await LeaveApplicationService.GetLeaveApplicationsByEmailAsync(LoggedInUser.Email, status);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LeaveApplicationList.Clear();
                    LeaveApplicationList.AddRange(list);
                });
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

