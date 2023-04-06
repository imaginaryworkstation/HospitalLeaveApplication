using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class LeaveApplicationListPage : ContentPage
{
    private LeaveApplicationListViewModel viewModel;
    public LeaveApplicationListPage()
	{
		InitializeComponent();
        viewModel = BindingContext as LeaveApplicationListViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LeaveApplication leaveApplication = e.CurrentSelection.FirstOrDefault() as LeaveApplication;
        await Shell.Current.GoToAsync($"{nameof(LeaveApplicationDetailPage)}?key={leaveApplication.Key}");
    }
}