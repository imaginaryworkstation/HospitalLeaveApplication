using HospitalLeaveApplication.ViewModels;

namespace HospitalLeaveApplication.Views;

public partial class NotificationListPage : ContentPage
{
	private NotificationListViewModel viewModel;
	public NotificationListPage()
	{
		InitializeComponent();
		viewModel = BindingContext as NotificationListViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.OnAppearing();
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}