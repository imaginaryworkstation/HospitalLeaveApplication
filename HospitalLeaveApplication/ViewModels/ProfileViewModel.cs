﻿using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
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
        private User Token;
        private string FirebaseKey { get; set; }

        public ICommand LogoutCommand { get; }
        public ICommand UpdateProfileCommand { get; }
        public User User { get => user; set => SetProperty(ref user, value); }
        public ProfileViewModel()
        {
            LogoutCommand = new AsyncCommand(ExecuteLogout);
            UpdateProfileCommand = new AsyncCommand(ExecuteUpdateProfile);
        }

        private async Task ExecuteLogout()
        {
            await LocalDBService.RemoveToken();
            await MainThread.InvokeOnMainThreadAsync(() => { Application.Current.MainPage = new AppShell(); });
        }

        private async Task ExecuteUpdateProfile()
        {
            await UserService.UpdateUserAsync(FirebaseKey, User);
        }

        public void OnAppearing()
        {
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
                FirebaseObject<User> firebaseObject = await UserService.GetUserWithKeyAsync(Token.Email);
                FirebaseKey = firebaseObject.Key;
                User = firebaseObject.Object as User;
            }
            catch(Exception ex)
            {

            }
        }
    }
}
