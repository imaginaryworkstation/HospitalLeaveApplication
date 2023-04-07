using System.Windows.Input;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace HospitalLeaveApplication.ViewModels
{
    public class LeaveApplicationViewModel : BaseViewModel
    {
        private DateTime minFromDate;
        private DateTime minToDate;
        private User user;
        private LeaveApplication leaveApplication;
        private string selectedLeaveType;
        private User selectedUser;
        private bool isResidenceEnable;

        public ICommand LeaveApplicationCommand { get; }
        public ObservableRangeCollection<string> LeaveTypes { get; }
        public ObservableRangeCollection<User> UserList { get; }
        public DateTime MinFromDate { get => minFromDate; set => SetProperty(ref minFromDate, value); }
        public DateTime MinToDate { get => minToDate; set => SetProperty(ref minToDate, value); }
        public User User { get => user; set => SetProperty(ref user, value); }
        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public User SelectedUser { get => selectedUser; set => SetProperty(ref selectedUser, value); }
        public string SelectedLeaveType
        {
            get => selectedLeaveType;
            set
            {
                if(value == "Casual")
                {
                    IsResidenceEnable = false;
                }
                else
                {
                    IsResidenceEnable = true;
                }
                SetProperty(ref selectedLeaveType, value);
            }
        }
        public bool IsResidenceEnable { get => isResidenceEnable; set => SetProperty(ref isResidenceEnable, value); }

        public LeaveApplicationViewModel()
        {
            MinFromDate = DateTime.Now;
            MinToDate = DateTime.Now.AddDays(1);
            User = new User();
            LeaveApplication = new LeaveApplication()
            {
                FromDate = MinFromDate,
                ToDate = MinToDate
            };
            SelectedUser = new User();
            LeaveApplicationCommand = new AsyncCommand(ExecuteLeaveApplication);
            LeaveTypes = new ObservableRangeCollection<string>();
            UserList = new ObservableRangeCollection<User>();
        }

        public void OnAppearing()
        {
            GetLeaveTypes();
            Task.Run(async () => await GetUserDetails());
            Task.Run(async () => await GetUserList());
        }

        private void GetLeaveTypes()
        {
            LeaveTypes.Clear();
            LeaveTypes.AddRange(StaticCredential.GetLeaveTypes());
        }

        private async Task GetUserDetails()
        {
            try
            {
                User = await UserService.GetUserAsync("testuser@gmail.com");
                LeaveApplication.Name = User.Name;
                LeaveApplication.Email = User.Email;
            }
            catch(Exception ex)
            {

            }
        }

        private async Task GetUserList()
        {
            try
            {
                List<User> users = await UserService.GetUsersAsync("Senior Staff Nurse");
                UserList.Clear();
                UserList.AddRange(users.Where(u => u.Email != User.Email).ToList());
            }
            catch (Exception ex)
            {

            }
        }

        private async Task ExecuteLeaveApplication()
        {
            LeaveApplication.Key = string.Format("{0}{1}", User.Email, DateTime.Now.ToString("yyyyMMddHHmmss"));
            LeaveApplication.LeaveType = SelectedLeaveType;
            LeaveApplication.Days = (LeaveApplication.ToDate - LeaveApplication.FromDate).Days + 1;
            leaveApplication.Proxy = SelectedUser.Email;
            LeaveApplication.Role = User.SubCategory;
            LeaveApplication.Status = "Pending";
            FirebaseResponse response = await LeaveApplicationService.StoreLeaveApplication(LeaveApplication);
            await Shell.Current.GoToAsync("//LeaveApplicationListPage");
        }
    }
}

