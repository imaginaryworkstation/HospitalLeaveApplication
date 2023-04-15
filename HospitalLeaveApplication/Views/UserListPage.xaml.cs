using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class UserListPage : ContentPage
{
	private UserListViewModel viewModel;
	public UserListPage()
	{
		InitializeComponent();
		viewModel = BindingContext as UserListViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        User user = e.CurrentSelection.FirstOrDefault() as User;
        await Shell.Current.GoToAsync($"{nameof(UserDetailPage)}?email={user.Email}");
    }
}
