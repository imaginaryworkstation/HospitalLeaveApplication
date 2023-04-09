﻿using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    [QueryProperty(nameof(key), "key")]
    public class ProxyLeaveDetailViewModel : BaseViewModel
    {
        private string FirebaseKey { get; set; }
        public string key { get; set; }
        FirebaseObject<LeaveApplication> firebaseLeaveApplication;
        private LeaveApplication leaveApplication;
        private User user;
        private User proxyUser;
        private bool isResidenceEnable;

        public ICommand LeaveApplicationCommand { get; }
        public ObservableRangeCollection<string> LeaveStatusList { get; }
        public FirebaseObject<LeaveApplication> FirebaseLeaveApplication { get => firebaseLeaveApplication; set => SetProperty(ref firebaseLeaveApplication, value); }
        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public User User { get => user; set => SetProperty(ref user, value); }
        public User ProxyUser { get => proxyUser; set => SetProperty(ref proxyUser, value); }
        public bool IsResidenceEnable { get => isResidenceEnable; set => SetProperty(ref isResidenceEnable, value); }

        public ProxyLeaveDetailViewModel()
        {
            LeaveStatusList = new ObservableRangeCollection<string>();
            LeaveApplicationCommand = new AsyncCommand(ExecuteLeaveApplication);
        }

        private async Task ExecuteLeaveApplication()
        {
            await LeaveApplicationService.UpdateLeaveApplicationAsync(FirebaseKey, LeaveApplication);
            await Shell.Current.GoToAsync("..");
        }

        public void OnAppearing()
        {
            GetLeaveStatusList();
            Task.Run(async () => await GetLeaveApplicationDetail());
        }
        private async Task GetLeaveApplicationDetail()
        {
            FirebaseObject<LeaveApplication> FirebaseLeaveApplication = await LeaveApplicationService.GetLeaveApplicationByKeyAsync(key);
            LeaveApplication = FirebaseLeaveApplication.Object as LeaveApplication;
            FirebaseKey = FirebaseLeaveApplication.Key;
            IsResidenceEnable = LeaveApplication.LeaveType == "Casual" ? false : true;
            if (LeaveApplication != null)
            {
                await GetUserDetail();
            }
        }

        private void GetLeaveStatusList()
        {
            try
            {
                LeaveStatusList.Clear();
                LeaveStatusList.AddRange(StaticCredential.GetLeaveStatus());
            }
            catch (Exception ex)
            {

            }
        }

        private async Task GetUserDetail()
        {
            User = await UserService.GetUserAsync(LeaveApplication.Email);
            ProxyUser = await UserService.GetUserAsync(LeaveApplication.Proxy);
        }
    }
}