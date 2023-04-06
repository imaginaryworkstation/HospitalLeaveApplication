using System;
using System.Windows.Input;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace HospitalLeaveApplication.ViewModels
{
	public class UserListViewModel : BaseViewModel
    {
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
                UserList.Clear();
                UserList.AddRange(await UserService.GetUsersAsync());
            }
            catch (Exception ex)
            {

            }
        }
    }
}

