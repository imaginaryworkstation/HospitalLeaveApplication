using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class ProfilePage : ContentPage
{
    private ProfileViewModel viewModel;
    public ProfilePage()
	{
		InitializeComponent();
		viewModel = BindingContext as ProfileViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}