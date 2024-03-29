﻿using System;
using System.Windows.Input;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using HospitalLeaveApplication.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace HospitalLeaveApplication.ViewModels
{
	public class UserListViewModel : BaseViewModel
    {
        private User LoggedinUser;
        public ICommand NavigateToNewUserCommand { get; }
        public ObservableRangeCollection<User> UserList { get; }

        public UserListViewModel()
        {
            UserList = new ObservableRangeCollection<User>();
            NavigateToNewUserCommand = new AsyncCommand(ExecuteNavigateToNewUser);
        }

        private async Task ExecuteNavigateToNewUser()
        {
            await Shell.Current.GoToAsync(nameof(NewUserPage));
        }

        public void OnAppearing()
        {
            Task.Run(async () => await GetUsers());
        }

        private async Task GetUsers()
        {
            try
            {
                IsBusy = true;
                LoggedinUser = StaticCredential.User;
                if(LoggedinUser == null)
                {
                    LoggedinUser = await LocalDBService.GetToken();
                }
                var users = await UserService.GetUsersAsync(LoggedinUser.Email);
                UserList.Clear();
                UserList.AddRange(users);
                
            }
            catch (Exception ex)
            {

            }
            finally { IsBusy = false; }
        }
    }
}

