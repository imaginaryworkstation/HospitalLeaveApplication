using System.Windows.Input;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace HospitalLeaveApplication.ViewModels
{
    public class NewUserViewModel : BaseViewModel
	{
		private User user;
        private string selectedPostingLocation;
        private string selectedSubPostingLocation;
        private string selectedWardLocation;
        private string errorMessage;
        private string selectedCategory;
        private bool isError;
        private bool isSubPosting;
        private bool isWard;

        public ICommand NewUserCommand { get; }
		public ObservableRangeCollection<string> CategoryList { get; }
        public ObservableRangeCollection<string> SubcategoryList { get; }
        public ObservableRangeCollection<string> PostingLocationList { get; }
        public ObservableRangeCollection<string> SubPostingLocationList { get; }
        public ObservableRangeCollection<string> WardLocationList { get; }
        public User User { get => user; set => SetProperty(ref user, value); }
        public string SelectedPostingLocation
        {
            get => selectedPostingLocation;
            set
            {
                SetProperty(ref selectedPostingLocation, value);
                Task.Run(async () => await UpdatePosting());
            }
        }
        public string SelectedSubPostingLocation
        {
            get => selectedSubPostingLocation;
            set
            {
                SetProperty(ref selectedSubPostingLocation, value);
                UpdateSubPosting();
            }
        }

        public string SelectedWardLocation
        {
            get => selectedWardLocation;
            set
            {
                SetProperty(ref selectedWardLocation, value);
                UpdateWardPosting();
            }
        }
        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                Task.Run(async () => await GetSubcategories());
            }
        }
        public bool IsSubPosting { get => isSubPosting; set => SetProperty(ref isSubPosting, value); }
        public bool IsWard { get => isWard; set => SetProperty(ref isWard, value); }
        public string ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }
        public bool IsError { get => isError; set => SetProperty(ref isError, value); }

        public NewUserViewModel()
		{
            CategoryList = new ObservableRangeCollection<string>();
            SubcategoryList = new ObservableRangeCollection<string>();
            PostingLocationList = new ObservableRangeCollection<string>();
            SubPostingLocationList = new ObservableRangeCollection<string>();
            WardLocationList = new ObservableRangeCollection<string>();
            User = new User()
            {
                Remaining = 20
            };

            NewUserCommand = new AsyncCommand(ExecuteNewuser);
        }

        private async Task ExecuteNewuser()
        {
            User.Category = SelectedCategory;
            if (string.IsNullOrEmpty(User.Name))
            {
                IsError = true;
                ErrorMessage = "Name is required";
                return;
            }
            else if(string.IsNullOrEmpty(User.Email))
            {
                IsError = true;
                ErrorMessage = "Email is required";
                return;
            }
            else if (string.IsNullOrEmpty(User.Phone))
            {
                IsError = true;
                ErrorMessage = "Phone number is required";
                return;
            }
            else if (string.IsNullOrEmpty(User.Category))
            {
                IsError = true;
                ErrorMessage = "Please select a category";
                return;
            }
            else if (string.IsNullOrEmpty(User.SubCategory))
            {
                IsError = true;
                ErrorMessage = "Please select a sub category";
                return;
            }
            else if (string.IsNullOrEmpty(User.Password))
            {
                IsError = true;
                ErrorMessage = "Password is required";
                return;
            }
            else if (User.Remaining <= 0)
            {
                IsError = true;
                ErrorMessage = "Leave can not be less or equal 0";
                return;
            }
            else if (string.IsNullOrEmpty(User.Posting))
            {
                IsError = true;
                ErrorMessage = "Please select a posting place";
                return;
            }
            User.Enjoyed = 20 - User.Remaining;
            try
            {
                FirebaseResponse response = await UserService.StoreUser(User);
                if (response != null && response.Code == 200)
                {
                    await Shell.Current.DisplayAlert("Added", response.Message, "Ok");
                    await Shell.Current.GoToAsync("//UserListPage");
                }
                else if (response != null)
                {
                    await Shell.Current.DisplayAlert("Error", "Profile update failed.", "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Internal error occured, please try again later.", "Ok");
                }
                IsError = false;
                ErrorMessage = null;
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Internal error occured, please try again later.", "Ok");
                IsError = true;
                ErrorMessage = "Iternal error occured.";
            }
        }

        public void OnAppearing()
        {
            IsError = false;
            IsSubPosting = IsWard = false;
            ErrorMessage = null;
            Task.Run(async () => await GetCategories());
            GetPostingLocations();
        }

        private async Task GetCategories()
		{
            var a = await CategoryService.GetCategoriesAsync();
            try
            {
                await MainThread.InvokeOnMainThreadAsync(() => {
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
            try
            {
                var list = await SubcategoryService.GetSubcategoriesAsync(SelectedCategory);
                await MainThread.InvokeOnMainThreadAsync(() => {
                    SubcategoryList.Clear();
                    SubcategoryList.AddRange(list.Select(s => s.Name).ToList());
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void GetPostingLocations()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                PostingLocationList.Clear();
                PostingLocationList.AddRange(StaticCredential.GetPostingLocations());
            });
        }

        private async Task UpdatePosting()
        {
            if (SelectedPostingLocation == "Union Health Complex")
            {
                var uhc = await PostingService.GetUnionHealthComplexesAsync();
                await MainThread.InvokeOnMainThreadAsync(async () => {
                    SubPostingLocationList.Clear();
                    SubPostingLocationList.AddRange(uhc.Select(s => s.Name).ToList());
                });
                IsSubPosting = true;
                IsWard = false;
            }
            else if (SelectedPostingLocation == "Union")
            {
                var u = await PostingService.GetUnionsAsync();
                await MainThread.InvokeOnMainThreadAsync(async () => {
                    SubPostingLocationList.Clear();
                    SubPostingLocationList.AddRange(u.Select(s => s.Name).ToList());
                });
                IsSubPosting = true;
                IsWard = false;
            }
            else if(SelectedPostingLocation == "Community Clinic")
            {
                var c = await PostingService.GetCommunitiesAsync();
                await MainThread.InvokeOnMainThreadAsync(async () => {
                    SubPostingLocationList.Clear();
                    SubPostingLocationList.AddRange(c.Select(s => s.Name).ToList());
                });
                IsSubPosting = true;
                IsWard = false;
            }
            else
            {
                IsSubPosting = IsWard = false;
                User.Posting = SelectedPostingLocation;
            }
        }

        private void UpdateSubPosting()
        {
            if(SelectedPostingLocation == "Union")
            {
                IsWard = true;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    WardLocationList.Clear();
                    WardLocationList.AddRange(StaticCredential.GetPostingWards());
                });
            }
            else
            {
                User.Posting = SelectedSubPostingLocation;
                IsWard = false;
                WardLocationList.Clear();
            }
        }

        private void UpdateWardPosting()
        {
            User.Posting = SelectedSubPostingLocation + " (" + SelectedWardLocation + ")";
        }
	}
}

