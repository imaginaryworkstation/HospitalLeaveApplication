using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private User user;
        private bool hasError;
        private string errorMessage;
        private string loginText;

        public ICommand LoginCommand { get; }
        public User User { get => user; set => SetProperty(ref user, value); }
        public bool HasError { get => hasError; set => SetProperty(ref hasError, value); }
        public string ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }
        public string LoginText { get => loginText; set => SetProperty(ref loginText, value); }

        public LoginViewModel()
        {
            LoginText = "Login";
            User = new User();
            LoginCommand = new AsyncCommand(ExecuteLogin);
        }

        private async Task ExecuteLogin()
        {
            try
            {
                IsBusy = true;
                HasError = false;
                LoginText = "Logging in...";
                User LoginUser = await UserService.GetUserAsync(User.Email);
                if (LoginUser != null && LoginUser.Password == User.Password)
                {
                    await LocalDBService.RemoveToken();
                    await LocalDBService.InsertToken(LoginUser);
                    StaticCredential.User = LoginUser;
                    await MainThread.InvokeOnMainThreadAsync(() => { Application.Current.MainPage = new UserShell(); });
                }
                else
                {
                    HasError = true;
                    ErrorMessage = "Wrong email or password!";
                }
            }
            catch(Exception ex)
            {
                HasError = true;
                ErrorMessage = "Unexpected error occured, please restart the app!";
            }
            finally
            {
                LoginText = "Login";
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            HasError = false;
        }
    }
}
