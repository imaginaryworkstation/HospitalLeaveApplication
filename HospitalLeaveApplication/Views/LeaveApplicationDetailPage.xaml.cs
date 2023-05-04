using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class LeaveApplicationDetailPage : ContentPage
{
	private LeaveApplicationDetailViewModel viewModel;
	public LeaveApplicationDetailPage()
	{
		InitializeComponent();
		viewModel = BindingContext as LeaveApplicationDetailViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LeaveCertificatePage(viewModel.LeaveApplication));
    }
}