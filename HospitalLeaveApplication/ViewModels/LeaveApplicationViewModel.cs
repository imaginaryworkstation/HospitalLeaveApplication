using System;
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

        public ICommand LeaveApplicationCommand { get; }
        public ObservableRangeCollection<string> LeaveTypes { get; }
        public DateTime MinFromDate { get => minFromDate; set => SetProperty(ref minFromDate, value); }
        public DateTime MinToDate { get => minToDate; set => SetProperty(ref minToDate, value); }
        public User User { get => user; set => SetProperty(ref user, value); }
        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }

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
            LeaveApplicationCommand = new AsyncCommand(ExecuteLeaveApplication);
            LeaveTypes = new ObservableRangeCollection<string>();
        }

        public void OnAppearing()
        {
            GetLeaveTypes();
            Task.Run(async () => await GetUserDetails());
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
                LeaveApplication.Email = LeaveApplication.Email;
            }
            catch(Exception ex)
            {

            }
        }

        private async Task ExecuteLeaveApplication()
        {
            LeaveApplication.Days = (LeaveApplication.ToDate - LeaveApplication.FromDate).Days + 1;
            FirebaseResponse response = await LeaveApplicationService.StoreLeaveApplication(LeaveApplication);
        }
    }
}

