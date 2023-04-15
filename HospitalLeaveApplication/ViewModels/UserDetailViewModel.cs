using Firebase.Database;
using HospitalLeaveApplication.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Utilities;
using HospitalLeaveApplication.Services.Helpers;

namespace HospitalLeaveApplication.ViewModels
{
    [QueryProperty(nameof(email), "email")]
    public class UserDetailViewModel : BaseViewModel
    {
        private User LoggedInUser;
        private string FirebaseKey { get; set; }
        public string email { get; set; }
        private User user;
        public ICommand UpdateProfileCommand { get; }
        public User User { get => user; set => SetProperty(ref user, value); }
        public UserDetailViewModel()
        {
            UpdateProfileCommand = new AsyncCommand(ExecuteUpdateProfile);
        }

        private async Task ExecuteUpdateProfile()
        {
            await UserService.UpdateUserAsync(FirebaseKey, User);
            await Shell.Current.GoToAsync("//UserListPage");
        }

        public void OnAppearing()
        {
            Task.Run(async () => await GetToken());
        }

        private async Task GetToken()
        {
            LoggedInUser = StaticCredential.User;
            if (LoggedInUser == null)
            {
                LoggedInUser = await LocalDBService.GetToken();
            }
            await GetUserDetail();
        }

        private async Task GetUserDetail()
        {
            try
            {
                FirebaseObject<User> firebaseObject = await UserService.GetUserWithKeyAsync(email);
                FirebaseKey = firebaseObject.Key;
                User = firebaseObject.Object as User;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
