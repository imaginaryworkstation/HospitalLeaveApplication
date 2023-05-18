using Firebase.Database;
using HospitalLeaveApplication.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Utilities;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Views;
using HospitalLeaveApplication.Models.HelperModels;

namespace HospitalLeaveApplication.ViewModels
{
    [QueryProperty(nameof(email), "email")]
    public class UserDetailViewModel : BaseViewModel
    {
        private User LoggedInUser;
        private string FirebaseKey { get; set; }
        public string email { get; set; }
        private User user;
        public ICommand NavigateToChangePassword { get; }
        public ICommand UpdateProfileCommand { get; }
        public User User { get => user; set => SetProperty(ref user, value); }
        public UserDetailViewModel()
        {
            UpdateProfileCommand = new AsyncCommand(ExecuteUpdateProfile);
            NavigateToChangePassword = new AsyncCommand(ExecuteNavigateToChangePassword);
        }

        private async Task ExecuteUpdateProfile()
        {
            try
            {
                FirebaseResponse response = await UserService.UpdateUserAsync(FirebaseKey, User);
                if (response != null && response.Code == 200)
                {
                    await Shell.Current.DisplayAlert("Updated", response.Message, "Ok");
                    await Shell.Current.GoToAsync("//UserListPage");
                }
                else if (response != null)
                {
                    await Shell.Current.DisplayAlert("Error", response.Message, "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Internal error occured, please try again later.", "Ok");
                }
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Internal error occured, please try again later.", "Ok");
            }
        }

        private async Task ExecuteNavigateToChangePassword()
        {
            await Shell.Current.GoToAsync($"{nameof(ChangePasswordPage)}?email={email}");
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
