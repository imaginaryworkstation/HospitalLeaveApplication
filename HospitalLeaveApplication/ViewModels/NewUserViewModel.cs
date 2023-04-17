using System;
using System.Windows.Input;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace HospitalLeaveApplication.ViewModels
{
	public class NewUserViewModel : BaseViewModel
	{
		private User user;

        public ICommand NewUserCommand { get; }
		public ObservableRangeCollection<string> CategoryList { get; }
        public ObservableRangeCollection<string> SubcategoryList { get; }
        public User User { get => user; set => SetProperty(ref user, value); }

        public NewUserViewModel()
		{
            CategoryList = new ObservableRangeCollection<string>();
            SubcategoryList = new ObservableRangeCollection<string>();
			User = new User();

            NewUserCommand = new AsyncCommand(ExecuteNewuser);
        }

        private async Task ExecuteNewuser()
        {
            await UserService.StoreUser(User);
            await Shell.Current.GoToAsync("//UserListPage");
        }

        public void OnAppearing()
        {
            Task.Run(async () => await GetCategories());
            Task.Run(async () => await GetSubcategories());
        }

        private async Task GetCategories()
		{
            var a = await CategoryService.GetCategoriesAsync();
            try
            {
                await MainThread.InvokeOnMainThreadAsync(async () => {
                    CategoryList.Clear();
                    CategoryList.AddRange(a.Select(s => s.Name).ToList());
                });
            }
            catch (Exception ex)
            {

            }
        }

		private async Task GetSubcategories()
		{
            var a = await SubcategoryService.GetSubcategoriesAsync();
            try
            {
                SubcategoryList.Clear();
                SubcategoryList.AddRange(a.Select(s => s.Name).ToList());
            }
            catch (Exception ex)
            {

            }
        }
	}
}

