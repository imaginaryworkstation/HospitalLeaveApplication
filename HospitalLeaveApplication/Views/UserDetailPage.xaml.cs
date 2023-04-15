using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class UserDetailPage : ContentPage
{
	private UserDetailViewModel viewModel;
	public UserDetailPage()
	{
		InitializeComponent();
		viewModel = BindingContext as UserDetailViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }
}