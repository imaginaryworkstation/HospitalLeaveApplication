using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class LeaveApplicationPage : ContentPage
{
    private LeaveApplicationViewModel viewModel;
    public LeaveApplicationPage()
	{
		InitializeComponent();
        viewModel = BindingContext as LeaveApplicationViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }

    void FromDatePicker_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
    {
        viewModel.MinToDate = e.NewDate.AddDays(1);
    }
}
