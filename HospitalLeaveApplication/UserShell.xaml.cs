using HospitalLeaveApplication.Views;

namespace HospitalLeaveApplication;

public partial class UserShell : Shell
{
    private UserShellViewModel viewModel;
    private Stack<ShellNavigationState> Uri { get; set; } // Navigation stack.
    private ShellNavigationState temp; // Prevents applications from adding redundant data to the stack when the back button is clicked. 
    public UserShell()
	{
		InitializeComponent();
        Uri = new Stack<ShellNavigationState>();
        viewModel = BindingContext as UserShellViewModel;
        Routing.RegisterRoute("DashboardPage", typeof(DashboardPage));
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

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        if (Uri != null && args.Current != null && (temp == null || args.Current.Location != temp.Location))
        {
            Uri.Push(args.Current);
        }
    }


    protected override bool OnBackButtonPressed()
    {
        if (Uri.Count > 0)
        {
            temp = Uri.Pop();
            Shell.Current.GoToAsync(temp);
            return true;
        }
        else
        {
            return false;
        }
    }
}