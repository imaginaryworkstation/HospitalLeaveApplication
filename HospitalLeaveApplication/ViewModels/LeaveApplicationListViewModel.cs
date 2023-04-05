using System;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using MvvmHelpers;

namespace HospitalLeaveApplication.ViewModels
{
	public class LeaveApplicationListViewModel : BaseViewModel
	{
        public ObservableRangeCollection<LeaveApplication> LeaveApplicationList { get; }
        public LeaveApplicationListViewModel()
        {
            LeaveApplicationList = new ObservableRangeCollection<LeaveApplication>();
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

