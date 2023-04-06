using HospitalLeaveApplication.Views;

namespace HospitalLeaveApplication;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("LeaveApplicationPage", typeof(LeaveApplicationPage));
		Routing.RegisterRoute("LeaveApplicationDetailPage", typeof(LeaveApplicationDetailPage));
        Routing.RegisterRoute("NewUserPage", typeof(NewUserPage));
    }
}
