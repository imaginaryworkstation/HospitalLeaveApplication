using System.Windows.Input;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace HospitalLeaveApplication.ViewModels
{
    public class LeaveApplicationListViewModel : BaseViewModel
    {
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
            Task.Run(async () => await GetLeaveApplications());
        }

        private async Task GetLeaveApplications()
        {
            try
            {
                LeaveApplicationList.Clear();
                LeaveApplicationList.AddRange(await LeaveApplicationService.GetLeaveApplicationsByEmailAsync("testuser@gmail.com"));
            }
            catch (Exception ex)
            {

            }
        }
    }
}

