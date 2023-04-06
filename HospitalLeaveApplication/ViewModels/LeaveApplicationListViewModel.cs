using System;
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
        public ICommand SelectionChangedCommand { get; }
        public ObservableRangeCollection<LeaveApplication> LeaveApplicationList { get; }
        public LeaveApplicationListViewModel()
        {
            NavigateToNewLeaveApplicationsCommand = new AsyncCommand(ExecuteNavigateToNewLeaveApplications);
            SelectionChangedCommand = new AsyncCommand<LeaveApplication>(ExecuteSelectionChanged);
            LeaveApplicationList = new ObservableRangeCollection<LeaveApplication>();
        }
        private async Task ExecuteSelectionChanged(LeaveApplication leaveApplication)
        {
            await Shell.Current.GoToAsync(nameof(LeaveApplicationDetailPage));
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
                LeaveApplicationList.AddRange(await LeaveApplicationService.GetLeaveApplicationsAsync());
            }
            catch (Exception ex)
            {

            }
        }
    }
}

