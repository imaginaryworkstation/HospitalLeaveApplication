using HospitalLeaveApplication.Views;

namespace HospitalLeaveApplication;

public partial class UserShell : Shell
{
    private bool isBack;
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
        isBack = false;
        viewModel.OnAppearing();
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        if (Uri != null && args.Previous != null && isBack == false)
        {
            if (args.Previous.Location.ToString() != "//LeaveApplicationListPage/LeaveApplicationDetailPage")
            {
                Uri.Push(args.Previous);
            }
            
        }
        isBack = false;
    }


    protected override bool OnBackButtonPressed()
    {
        isBack = true;
        if (Uri.Count > 0)
        {
            Shell.Current.GoToAsync(Uri.Pop());
            return true;
        }
        else if(CurrentPage.GetType().Name == "DashboardPage")
        {
            return false;
        }
        else
        {
            Shell.Current.GoToAsync("//DashboardPage");
            return true;
        }
    }
}