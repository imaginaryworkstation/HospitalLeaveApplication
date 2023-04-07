using HospitalLeaveApplication.Views;

namespace HospitalLeaveApplication;

public partial class UserShell : Shell
{
	public UserShell()
	{
		InitializeComponent();
        Routing.RegisterRoute("LeaveApplicationDetailPage", typeof(LeaveApplicationDetailPage));
        Routing.RegisterRoute("NotificationDetailPage", typeof(NotificationDetailPage));
        Routing.RegisterRoute("NewUserPage", typeof(NewUserPage));
    }
}