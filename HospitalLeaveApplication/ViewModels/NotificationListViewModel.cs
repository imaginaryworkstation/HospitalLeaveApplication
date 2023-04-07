using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using MvvmHelpers;

namespace HospitalLeaveApplication.ViewModels
{
    public class NotificationListViewModel : BaseViewModel
    {
        public ObservableRangeCollection<LeaveApplication> LeaveApplicationList { get; }
        public NotificationListViewModel()
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
                LeaveApplicationList.AddRange(await LeaveApplicationService.GetLeaveApplicationsByRoleAsync("Senior Staff Nurse"));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
