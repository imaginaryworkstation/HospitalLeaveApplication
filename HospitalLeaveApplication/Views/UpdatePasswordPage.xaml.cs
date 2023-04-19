using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class UpdatePasswordPage : ContentPage
{
	private UpdatePasswordViewModel viewModel;
	public UpdatePasswordPage()
	{
		InitializeComponent();
		viewModel = BindingContext as UpdatePasswordViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }
}