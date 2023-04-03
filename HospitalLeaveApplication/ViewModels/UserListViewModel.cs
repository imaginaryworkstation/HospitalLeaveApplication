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
            //var a = await UserService.StoreUser(new User
            //{
            //    Name = "Test user",
            //    Email = "testuser@gmail.com",
            //    Phone = "01924241969",
            //    Category = "Nurse",
            //    SubCategory = "Senior nurse"
            //});
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

