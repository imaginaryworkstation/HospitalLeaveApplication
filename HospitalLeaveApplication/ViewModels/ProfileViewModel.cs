using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private User user;

        public ICommand LogoutCommand { get; }
        public User User { get => user; set => SetProperty(ref user, value); }
        public ProfileViewModel()
        {
            LogoutCommand = new AsyncCommand(ExecuteLogout);
        }

        private async Task ExecuteLogout()
        {
            await LocalDBService.RemoveToken();
            await MainThread.InvokeOnMainThreadAsync(() => { Application.Current.MainPage = new AppShell(); });
        }

        public void OnAppearing()
        {
            Task.Run(async () => await GetUserDetail());
        }

        private async Task GetUserDetail()
        {
            User = StaticCredential.User;
            if (User == null)
            {
                User = await LocalDBService.GetToken();
            }
        }
    }
}
