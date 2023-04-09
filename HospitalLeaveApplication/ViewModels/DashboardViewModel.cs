using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public ICommand NavigateToNotificationListCommand { get; }
        public ICommand NavigateToLeaveApplicationListCommand { get; }
        public ICommand NavigateToNewLeaveApplicationCommand { get; }
        public ICommand NavigateToProfileCommand { get; }
        public DashboardViewModel()
        {
            NavigateToNotificationListCommand = new AsyncCommand(ExecuteNavigateToNotificationList);
            NavigateToLeaveApplicationListCommand = new AsyncCommand(ExecuteNavigateToLeaveApplicationList);
            NavigateToNewLeaveApplicationCommand = new AsyncCommand(ExecuteNavigateToNewLeaveApplication);
            NavigateToProfileCommand = new AsyncCommand(ExecuteNavigateToProfile);
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
        public void OnAppearing()
        {
            
        }
    }
}
