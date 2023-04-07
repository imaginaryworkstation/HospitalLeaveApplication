using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class LoginPage : ContentPage
{
	private LoginViewModel viewModel;
	public LoginPage()
	{
		InitializeComponent();
		viewModel = BindingContext as LoginViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }
}