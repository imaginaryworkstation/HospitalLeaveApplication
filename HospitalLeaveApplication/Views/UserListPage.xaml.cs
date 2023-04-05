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

    async void AddUser_Clicked(System.Object sender, System.EventArgs e)
    {
        await MainThread.InvokeOnMainThreadAsync(() => { Navigation.PushAsync(new NewUserPage()); });
    }
}
