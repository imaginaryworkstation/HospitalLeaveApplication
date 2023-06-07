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

        public ICommand LoginCommand { get; }
        public User User { get => user; set => SetProperty(ref user, value); }
        public bool HasError { get => hasError; set => SetProperty(ref hasError, value); }
        public string ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }

        public LoginViewModel()
        {
            User = new User();
            LoginCommand = new AsyncCommand(ExecuteLogin);
        }

        private async Task ExecuteLogin()
        {
            try
            {
                HasError = false;
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

            }
        }

        public void OnAppearing()
        {
            HasError = false;
        }
    }
}
