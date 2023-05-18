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
        private string selectedLeaveStatus;
        private List<string> pathways;
        public ObservableRangeCollection<LeaveApplication> LeaveApplicationList { get; }
        public ObservableRangeCollection<string> LeaveStatusList { get; }
        public Pathway Pathway { get => pathway; set => SetProperty(ref pathway, value); }
        public string SelectedLeaveStatus
        {
            get => selectedLeaveStatus;
            set
            {
                SetProperty(ref selectedLeaveStatus, value);
                Task.Run(async () => await GetLeaveApplications());
            }
        }
        public List<string> Pathways { get => pathways; set => SetProperty(ref pathways, value); }

        public NotificationListViewModel()
        {
            LeaveStatusList = new ObservableRangeCollection<string>();
            Pathways = new List<string>();
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
                if (LoggedInUser.Category == "UHFPO")
                {
                    LeaveStatusList.Add("Accepted");
                    LeaveStatusList.Add("Approved");
                    SelectedLeaveStatus = StaticCredential.NotificationStatus != null ? StaticCredential.NotificationStatus : "Accepted";
                }
                else if(LoggedInUser.Category == "Approver")
                {
                    LeaveStatusList.Add("Reccomended");
                    LeaveStatusList.Add("Accepted");
                    SelectedLeaveStatus = StaticCredential.NotificationStatus != null ? StaticCredential.NotificationStatus : "Reccomended";
                }
                else
                {
                    LeaveStatusList.Add("Forwarded");
                    LeaveStatusList.Add("Reccomended");
                    SelectedLeaveStatus = StaticCredential.NotificationStatus != null ? StaticCredential.NotificationStatus : "Forwarded";
                }
                LeaveStatusList.Add("Rejected");
                Task.Run(async () => {
                    await GetPathways();
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async Task GetPathways()
        {
            try
            {
                var path = await RecommendingPathwayService.GetPathway(LoggedInUser.SubCategory);
                Pathways.Clear();
                Pathways.AddRange(path.Recommend.Split("/").ToList());
            }
            catch(Exception ex)
            {

            }
        }

        private async Task GetLeaveApplications()
        {
            try
            {
                if (LoggedInUser.Category == "UHFPO" || LoggedInUser.Category == "Approver")
                {
                    Pathways.Clear();
                    Pathways.Add("RMO");
                    Pathways.Add("Statistician");
                    Pathways.Add("Office Sohokari");
                    var list1 = await LeaveApplicationService.GetLeaveApplicationsByRecommendedRoleAsync(Pathways, SelectedLeaveStatus);
                    var list2 = await LeaveApplicationService.GetLeaveApplicationsByStatusAsync(SelectedLeaveStatus);
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        LeaveApplicationList.Clear();
                        LeaveApplicationList.AddRange(list1);
                        LeaveApplicationList.AddRange(list2);
                    });
                }
                else
                {
                    var list = await LeaveApplicationService.GetLeaveApplicationsByRecommendedRoleAsync(Pathways, SelectedLeaveStatus);
                    MainThread.BeginInvokeOnMainThread(async () => {
                        LeaveApplicationList.Clear();
                        LeaveApplicationList.AddRange(list);
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
