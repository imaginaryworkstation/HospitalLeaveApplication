using HospitalLeaveApplication.Views;

namespace HospitalLeaveApplication;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute("Login", typeof(LoginPage));
    }
}
