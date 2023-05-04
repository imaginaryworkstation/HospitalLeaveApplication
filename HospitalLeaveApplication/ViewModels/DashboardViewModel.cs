using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services.Helpers;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private bool isAdmin;
        private bool isNotAdmin;

        public bool IsAdmin { get => isAdmin; set => SetProperty(ref isAdmin, value); }
        public bool IsNotAdmin { get => isNotAdmin; set => SetProperty(ref isNotAdmin, value); }

        public ICommand NavigateToNotificationListCommand { get; }
        public ICommand NavigateToLeaveApplicationListCommand { get; }
        public ICommand NavigateToNewLeaveApplicationCommand { get; }
        public ICommand NavigateToProxyListCommand { get; }
        public ICommand NavigateToUserCommand { get; }
        public ICommand NavigateToProfileCommand { get; }
        public DashboardViewModel()
        {
            NavigateToNotificationListCommand = new AsyncCommand(ExecuteNavigateToNotificationList);
            NavigateToLeaveApplicationListCommand = new AsyncCommand(ExecuteNavigateToLeaveApplicationList);
            NavigateToNewLeaveApplicationCommand = new AsyncCommand(ExecuteNavigateToNewLeaveApplication);
            NavigateToProxyListCommand = new AsyncCommand(ExecuteNavigateToProxyList);
            NavigateToProfileCommand = new AsyncCommand(ExecuteNavigateToProfile);
            NavigateToUserCommand = new AsyncCommand(ExecuteNavigateToUserList);
        }

        private async Task ExecuteNavigateToNotificationList()
        {
            await Shell.Current.GoToAsync("//NotificationListPage");
        }

        private async Task ExecuteNavigateToLeaveApplicationList()
        {
            await Shell.Current.GoToAsync("//LeaveApplicationListPage");
        }

        private async Task ExecuteNavigateToNewLeaveApplication()
        {
            await Shell.Current.GoToAsync("//LeaveApplicationPage");
        }

        private async Task ExecuteNavigateToProfile()
        {
            await Shell.Current.GoToAsync("//ProfilePage");
        }

        private async Task ExecuteNavigateToProxyList()
        {
            await Shell.Current.GoToAsync("//ProxyListPage");
        }

        private async Task ExecuteNavigateToUserList()
        {
            await Shell.Current.GoToAsync("//UserListPage");
        }
        public void OnAppearing()
        {
            IsAdmin = false;
            Task.Run(async () => { await GetToken(); });
        }
        private async Task GetToken()
        {
            User user = await LocalDBService.GetToken();
            if (user != null && (user.SubCategory == "UHFPO" || user.SubCategory == "Approver"))
            {
                IsAdmin = true;
            }
            IsNotAdmin = !IsAdmin;
        }
    }
}
