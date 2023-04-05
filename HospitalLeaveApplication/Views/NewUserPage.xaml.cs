using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class NewUserPage : ContentPage
{
	private NewUserViewModel viewModel;
	public NewUserPage()
	{
		InitializeComponent();
		viewModel = BindingContext as NewUserViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }
}
