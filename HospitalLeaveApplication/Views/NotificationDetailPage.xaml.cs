using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class NotificationDetailPage : ContentPage
{
	private NotificationDetailViewModel viewModel;
	public NotificationDetailPage()
	{
		InitializeComponent();
		viewModel = BindingContext as NotificationDetailViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }
}