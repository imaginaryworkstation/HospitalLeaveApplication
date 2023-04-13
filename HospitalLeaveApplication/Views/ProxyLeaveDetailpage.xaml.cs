using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class ProxyLeaveDetailpage : ContentPage
{
	private ProxyLeaveDetailViewModel viewModel;
	public ProxyLeaveDetailpage()
	{
		InitializeComponent();
		viewModel = BindingContext as ProxyLeaveDetailViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }
}