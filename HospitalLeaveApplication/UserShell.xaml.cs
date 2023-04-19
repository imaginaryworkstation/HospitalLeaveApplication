using HospitalLeaveApplication.Views;

namespace HospitalLeaveApplication;

public partial class UserShell : Shell
{
    private UserShellViewModel viewModel;
	public UserShell()
	{
		InitializeComponent();
        viewModel = BindingContext as UserShellViewModel;
        Routing.RegisterRoute("LeaveApplicationDetailPage", typeof(LeaveApplicationDetailPage));
        Routing.RegisterRoute("NotificationDetailPage", typeof(NotificationDetailPage));
        Routing.RegisterRoute("ProxyLeaveDetailpage", typeof(ProxyLeaveDetailpage));
        Routing.RegisterRoute("NewUserPage", typeof(NewUserPage));
        Routing.RegisterRoute("UserDetailPage", typeof(UserDetailPage));
        Routing.RegisterRoute("ChangePasswordPage", typeof(ChangePasswordPage));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}