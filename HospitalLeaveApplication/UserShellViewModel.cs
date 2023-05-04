using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services.Helpers;
using MvvmHelpers;

namespace HospitalLeaveApplication
{
    public class UserShellViewModel : BaseViewModel
    {
        private bool isAdmin;
        private bool isNotAdmin;
        private bool isSuperAdmin;

        public bool IsAdmin { get => isAdmin; set => SetProperty(ref isAdmin, value); }
        public bool IsNotAdmin { get => isNotAdmin; set => SetProperty(ref isNotAdmin, value); }
        public bool IsSuperAdmin { get => isSuperAdmin; set => SetProperty(ref isSuperAdmin, value); }

        public void OnAppearing()
        {
            IsAdmin = false;
            Task.Run(async () => { await GetToken(); });
        }
        private async Task GetToken()
        {
            User user = await LocalDBService.GetToken();
            if (user != null && (user.Category == "UHFPO" || user.Category == "Approver"))
            {
                IsAdmin = true;
            }
            IsNotAdmin = !IsAdmin;
        }
    }
}
