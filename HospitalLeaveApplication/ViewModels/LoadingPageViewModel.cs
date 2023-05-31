using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services.Helpers;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    public class LoadingPageViewModel : BaseViewModel
    {
        private User user;
        public ICommand GetStartedCommand { get; }
        public LoadingPageViewModel()
        {
            GetStartedCommand = new AsyncCommand(GetToken);
        }
        public void OnAppearing()
        {
            //Task.Run(async () => await GetToken());
        }

        public async Task GetToken()
        {
            try
            {
                user = await LocalDBService.GetToken();
                if (user != null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() => { Application.Current.MainPage = new UserShell(); });
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync("Login"));
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
