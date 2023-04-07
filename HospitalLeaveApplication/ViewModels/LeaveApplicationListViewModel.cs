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
        public ICommand NavigateToNewLeaveApplicationsCommand { get; }
        public ObservableRangeCollection<LeaveApplication> LeaveApplicationList { get; }
        public LeaveApplicationListViewModel()
        {
            NavigateToNewLeaveApplicationsCommand = new AsyncCommand(ExecuteNavigateToNewLeaveApplications);
            LeaveApplicationList = new ObservableRangeCollection<LeaveApplication>();
        }

        private async Task ExecuteNavigateToNewLeaveApplications()
        {
            await Shell.Current.GoToAsync(nameof(LeaveApplicationPage));
        }
        public void OnAppearing()
        {
            Task.Run(async () => await GetUser());
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
                await GetLeaveApplications();
            }
            catch(Exception ex)
            {

            }
        }

        private async Task GetLeaveApplications()
        {
            try
            {
                LeaveApplicationList.Clear();
                LeaveApplicationList.AddRange(await LeaveApplicationService.GetLeaveApplicationsByEmailAsync(LoggedInUser.Email));
            }
            catch (Exception ex)
            {

            }
        }
    }
}

