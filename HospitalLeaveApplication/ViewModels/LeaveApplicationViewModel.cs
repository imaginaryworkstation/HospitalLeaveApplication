﻿using System.Windows.Input;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace HospitalLeaveApplication.ViewModels
{
    public class LeaveApplicationViewModel : BaseViewModel
    {
        private bool hasError;
        private string errorMessage;
        private DateTime minFromDate;
        private DateTime minToDate;
        private User loggedInUser;
        private LeaveApplication leaveApplication;
        private string selectedLeaveType;
        private User selectedUser;
        private bool isResidenceEnable;

        public ICommand LeaveApplicationCommand { get; }
        public ObservableRangeCollection<string> LeaveTypes { get; }
        public ObservableRangeCollection<User> UserList { get; }
        public bool HasError { get => hasError; set => SetProperty(ref hasError, value); }
        public string ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }
        public DateTime MinFromDate { get => minFromDate; set => SetProperty(ref minFromDate, value); }
        public DateTime MinToDate { get => minToDate; set => SetProperty(ref minToDate, value); }
        public User LoggedInUser { get => loggedInUser; set => SetProperty(ref loggedInUser, value); }
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
            LoggedInUser = new User();
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

        private async Task ExecuteLeaveApplication()
        {
            try
            {
                bool isValid = await ValidateApplication(LoggedInUser.Email);
                bool isValidProxy = await ValidateApplication(SelectedUser.Email);
                if (!isValid)
                {
                    HasError = true;
                    ErrorMessage = "Already has leave in these date";
                }
                if (!isValidProxy)
                {
                    HasError = true;
                    ErrorMessage = "Proxy already has leave in these date";
                }
                if (isValid && isValidProxy)
                {
                    LeaveApplication.Key = string.Format("{0}{1}", LoggedInUser.Email, DateTime.Now.ToString("yyyyMMddHHmmss"));
                    LeaveApplication.LeaveType = SelectedLeaveType;
                    LeaveApplication.Days = (LeaveApplication.ToDate - LeaveApplication.FromDate).Days + 1;
                    leaveApplication.Proxy = SelectedUser.Email;
                    LeaveApplication.Role = LoggedInUser.SubCategory;
                    LeaveApplication.Status = "Pending";
                    FirebaseResponse response = await LeaveApplicationService.StoreLeaveApplication(LeaveApplication);
                    await Shell.Current.GoToAsync("//LeaveApplicationListPage");
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void OnAppearing()
        {
            GetLeaveTypes();
            Task.Run(async () => await GetToken());
        }

        private async Task GetToken()
        {
            LoggedInUser = StaticCredential.User;
            if(LoggedInUser == null)
            {
                LoggedInUser = await LocalDBService.GetToken();
            }
            await GetUserDetails();
            await GetUserList();
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
                LoggedInUser = await UserService.GetUserAsync(LoggedInUser.Email);
                LeaveApplication.Name = LoggedInUser.Name;
                LeaveApplication.Email = LoggedInUser.Email;
            }
            catch(Exception ex)
            {

            }
        }

        private async Task GetUserList()
        {
            try
            {
                Pathway pathway = await PathwayService.GetPathwayAsync(LoggedInUser.SubCategory);
                List<User> users = await UserService.GetUsersAsync(pathway.Forward);
                MainThread.BeginInvokeOnMainThread(() => {
                    UserList.Clear();
                    UserList.AddRange(users.Where(u => u.Email != LoggedInUser.Email).ToList());
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async Task<bool> ValidateApplication(string email)
        {
            List<LeaveApplication> leaveApplications = await LeaveApplicationService.GetLeaveApplicationsByEmailAsync(email);
            leaveApplications = leaveApplications.Where(l => l.Status != "Declined").ToList();
            int fromCount = leaveApplications.Where(l => l.FromDate >= leaveApplication.FromDate && l.FromDate <= leaveApplication.ToDate).Count();
            int toCount = leaveApplications.Where(l => l.ToDate >= leaveApplication.FromDate && l.ToDate <= leaveApplication.ToDate).Count();
            int midCount = leaveApplications.Where(l => l.FromDate >= leaveApplication.FromDate && l.ToDate <= leaveApplication.ToDate).Count();
            int outCount = leaveApplications.Where(l => l.FromDate <= leaveApplication.FromDate && l.ToDate >= leaveApplication.ToDate).Count();
            return fromCount + toCount + midCount + outCount <= 0;
        }
    }
}

