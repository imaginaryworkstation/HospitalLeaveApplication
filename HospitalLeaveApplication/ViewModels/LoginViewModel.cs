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

        public ICommand LoginCommand { get; }
        public User User { get => user; set => SetProperty(ref user, value); }
        public LoginViewModel()
        {
            User = new User();
            LoginCommand = new AsyncCommand(ExecuteLogin);
        }

        private async Task ExecuteLogin()
        {
            User LoginUser = await UserService.GetUserAsync(User.Email);
            if(LoginUser != null && LoginUser.Password == User.Password)
            {
                await LocalDBService.RemoveToken();
                await LocalDBService.InsertToken(LoginUser);
                StaticCredential.User = LoginUser;
            }
        }

        public void OnAppearing()
        {

        }
    }
}
