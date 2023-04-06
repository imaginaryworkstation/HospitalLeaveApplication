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

    async void LeaveButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await MainThread.InvokeOnMainThreadAsync(() => { Navigation.PushAsync(new LeaveApplicationPage()); });
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var a = e.CurrentSelection;
    }
}