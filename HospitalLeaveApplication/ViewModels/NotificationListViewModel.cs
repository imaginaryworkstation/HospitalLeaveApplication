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
        private List<Pathway> pathways;
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
        public List<Pathway> Pathways { get => pathways; set => SetProperty(ref pathways, value); }

        public NotificationListViewModel()
        {
            LeaveStatusList = new ObservableRangeCollection<string>();
            Pathways = new List<Pathway>();
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
                LeaveStatusList.AddRange(StaticCredential.GetLeaveStatus());
                if (LoggedInUser.Category != "UHFPO")
                {
                    LeaveStatusList.Remove("Approved");
                }
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
            Pathways.Clear();
            Pathways.AddRange(await PathwayService.GetRecommendingPersonnel(LoggedInUser.SubCategory));
            await GetLeaveApplications();
        }

        private async Task GetLeaveApplications()
        {
            try
            {
                if(LoggedInUser.Category == "UHFPO")
                {
                    LeaveApplicationList.Clear();
                    Pathways.Clear();
                    Pathways.Add(new Pathway { Role = "RMO", Forward = "MO"});
                    Pathways.Add(new Pathway { Role = "Statistician", Forward = "Office Sohokari" });
                    Pathways.Add(new Pathway { Role = "Office Sohokari", Forward = "Office Sohokari" });
                    var list1 = await LeaveApplicationService.GetLeaveApplicationsByRecommendedRoleAsync(Pathways, SelectedLeaveStatus);
                    var list2 = await LeaveApplicationService.GetLeaveApplicationsByStatusAsync("Forwarded");
                    LeaveApplicationList.AddRange(list1);
                    LeaveApplicationList.AddRange(list2);
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () => {
                        LeaveApplicationList.Clear();
                        LeaveApplicationList.AddRange(await LeaveApplicationService.GetLeaveApplicationsByRecommendedRoleAsync(Pathways, SelectedLeaveStatus));
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
