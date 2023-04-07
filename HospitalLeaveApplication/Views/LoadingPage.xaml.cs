using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class LoadingPage : ContentPage
{
	private LoadingPageViewModel viewModel;
	public LoadingPage()
	{
		InitializeComponent();
		viewModel = BindingContext as LoadingPageViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }
}