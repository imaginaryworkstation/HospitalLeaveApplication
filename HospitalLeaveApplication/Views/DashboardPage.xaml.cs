using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class DashboardPage : ContentPage
{
    private DashboardViewModel viewModel;
	public DashboardPage()
	{
		InitializeComponent();
        viewModel = BindingContext as DashboardViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}