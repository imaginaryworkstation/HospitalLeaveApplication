using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class LeaveCertificatePage : ContentPage
{
	private LeaveCertificateViewModel viewModel;
	public LeaveCertificatePage()
	{
		InitializeComponent();
        this.BackgroundColor = new Color(0f, 0f, 0f, 0.2f);
    }

    public LeaveCertificatePage(LeaveApplication leaveApplication)
    {
        InitializeComponent();
        this.BackgroundColor = new Color(0f, 0f, 0f, 0.2f);
        viewModel = BindingContext as LeaveCertificateViewModel;
        viewModel.LeaveApplication = leaveApplication;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}