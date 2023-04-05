using System;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using MvvmHelpers;

namespace HospitalLeaveApplication.ViewModels
{
	public class UserListViewModel : BaseViewModel
    {
        public ObservableRangeCollection<User> UserList { get; }

        public UserListViewModel()
        {
            UserList = new ObservableRangeCollection<User>();
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

