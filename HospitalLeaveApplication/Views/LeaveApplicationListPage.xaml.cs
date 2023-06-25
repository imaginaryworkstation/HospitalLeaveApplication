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

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync($"{nameof(DashboardPage)}");
        return true;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LeaveApplication leaveApplication = e.CurrentSelection.FirstOrDefault() as LeaveApplication;
        await Shell.Current.GoToAsync($"{nameof(LeaveApplicationDetailPage)}?key={leaveApplication.Key}");
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        LeaveApplication leaveApplication = e.Item as LeaveApplication;
        await Shell.Current.GoToAsync($"{nameof(LeaveApplicationDetailPage)}?key={leaveApplication.Key}");
    }
}