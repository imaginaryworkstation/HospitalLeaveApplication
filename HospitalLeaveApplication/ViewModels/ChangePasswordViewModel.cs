using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    [QueryProperty(nameof(email), "email")]
    public class ChangePasswordViewModel : BaseViewModel
    {
        public string email { get; set; }
        private string FirebaseKey { get; set; }
        private User user;
        private User Token;
        private bool isError;
        private bool isSuccess;
        private string errorMesasge;
        private string password;
        private string confirmPassword;

        public ICommand ChangePasswordCommand { get; }
        public User User { get => user; set => SetProperty(ref user, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string ConfirmPassword { get => confirmPassword; set => SetProperty(ref confirmPassword, value); }
        public bool IsError
        {
            get => isError;
            set
            {
                SetProperty(ref isError, value);
                IsSucess = !value;
            }
        }
        public bool IsSucess { get => isSuccess; set => SetProperty(ref isSuccess, value); }
        public string ErrorMesasge { get => errorMesasge; set => SetProperty(ref errorMesasge, value); }

        public ChangePasswordViewModel()
        {
            ChangePasswordCommand = new AsyncCommand(ExecuteChangePassword);
        }

        private async Task ExecuteChangePassword()
        {
            try
            {
                if (Password == null || Password.Length < 6)
                {
                    IsError = true;
                    ErrorMesasge = "Password length sould be atleast 6";
                    return;
                }
                else if (!Password.Equals(ConfirmPassword))
                {
                    IsError = true;
                    ErrorMesasge = "Password and confirm password must have to be same";
                    return;
                }
                else
                {
                    User.Password = Password;
                    try
                    {
                        FirebaseResponse response = await UserService.UpdateUserAsync(FirebaseKey, User);
                        if (response != null && response.Code == 200)
                        {
                            await Shell.Current.DisplayAlert("Added", response.Message, "Ok");
                            IsError = false;
                            ErrorMesasge = "Password successfully updated";
                            await Shell.Current.GoToAsync("..");
                        }
                        else if (response != null)
                        {
                            await Shell.Current.DisplayAlert("Error", "Profile update failed.", "Ok");
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
            }
            catch(Exception ex)
            {
                IsError = true;
                ErrorMesasge = "Internal error occured!";
            }
        }

        public void OnAppearing()
        {
            Password = ConfirmPassword = null;
            IsError = false;
            IsSucess = false;
            Task.Run(async () => await GetToken());
        }

        private async Task GetToken()
        {
            Token = StaticCredential.User;
            if (Token == null)
            {
                Token = await LocalDBService.GetToken();
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
