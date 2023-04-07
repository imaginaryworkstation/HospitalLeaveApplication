using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;

namespace HospitalLeaveApplication.ViewModels
{
    public class NotificationListViewModel : BaseViewModel
    {
        private User LoggedInUser;
        private Pathway pathway;
        public ObservableRangeCollection<LeaveApplication> LeaveApplicationList { get; }
        public Pathway Pathway { get => pathway; set => SetProperty(ref pathway, value); }

        public NotificationListViewModel()
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
                await GetPathway();
            }
            catch(Exception ex)
            {

            }
        }

        private async Task GetPathway()
        {
            Pathway = await PathwayService.GetPathwayAsync(LoggedInUser.SubCategory);
            await GetLeaveApplications();
        }

        private async Task GetLeaveApplications()
        {
            try
            {
                LeaveApplicationList.Clear();
                LeaveApplicationList.AddRange(await LeaveApplicationService.GetLeaveApplicationsByRoleAsync(Pathway.Recommend));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
