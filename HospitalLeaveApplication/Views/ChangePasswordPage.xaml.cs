using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class ChangePasswordPage : ContentPage
{
	private ChangePasswordViewModel viewModel;
	public ChangePasswordPage()
	{
		InitializeComponent();
		viewModel = BindingContext as ChangePasswordViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }
}