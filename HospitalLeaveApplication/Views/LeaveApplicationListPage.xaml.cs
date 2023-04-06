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

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var a = e.CurrentSelection;
    }
}