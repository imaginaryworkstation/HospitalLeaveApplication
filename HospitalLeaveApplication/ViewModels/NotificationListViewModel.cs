﻿using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Services.Helpers;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;

namespace HospitalLeaveApplication.ViewModels
{
    public class NotificationListViewModel : BaseViewModel
    {
        private List<LeaveApplication> list1;
        private List<LeaveApplication> list2;
        private User LoggedInUser;
        private Pathway pathway;
        private string selectedLeaveStatus;
        private List<string> pathways;
        private List<string> selectedLeaveStatusList;
        public ObservableRangeCollection<LeaveApplication> LeaveApplicationList { get; }
        public ObservableRangeCollection<string> LeaveStatusList { get; }
        public Pathway Pathway { get => pathway; set => SetProperty(ref pathway, value); }
        public string SelectedLeaveStatus
        {
            get => selectedLeaveStatus;
            set
            {
                SetProperty(ref selectedLeaveStatus, value);
                StaticCredential.NotificationStatus = value;
                Task.Run(async () => await GetLeaveApplications());
            }
        }
        public List<string> Pathways { get => pathways; set => SetProperty(ref pathways, value); }
        public List<string> SelectedLeaveStatusList { get => selectedLeaveStatusList; set => SetProperty(ref selectedLeaveStatusList, value); }

        public NotificationListViewModel()
        {
            list1 = new List<LeaveApplication>();
            list2 = new List<LeaveApplication>();
            LeaveStatusList = new ObservableRangeCollection<string>();
            SelectedLeaveStatusList = new List<string>();
            Pathways = new List<string>();
            LeaveApplicationList = new ObservableRangeCollection<LeaveApplication>();
        }
        public void OnAppearing()
        {
            Task.Run(async () => await GetLoggedInUser());
        }

        private async Task GetLoggedInUser()
        {
            try
            {
                LoggedInUser = StaticCredential.User;
                if (LoggedInUser == null)
                {
                    LoggedInUser = await LocalDBService.GetToken();
                }
                GetLeaveStatusList();
            }
            catch (Exception ex)
            {

            }
        }

        private void GetLeaveStatusList()
        {
            try
            {
                LeaveStatusList.Clear();
                LeaveStatusList.Add("All");
                if (LoggedInUser.SubCategory == "UHFPO")
                {
                    LeaveStatusList.Add("Forwarded");
                    LeaveStatusList.Add("Approved");
                    LeaveStatusList.Add("Rejected");
                }
                else if(LoggedInUser.SubCategory == "Admin")
                {
                    LeaveStatusList.Add("Pending");
                    LeaveStatusList.Add("Recommended");
                    LeaveStatusList.Add("Forwarded");
                    LeaveStatusList.Add("Sent back");
                }
                else
                {
                    LeaveStatusList.Add("Agreed");
                    LeaveStatusList.Add("Recommended");
                    LeaveStatusList.Add("Declined");
                    LeaveStatusList.Add("Pending");
                }
                Task.Run(async () => {
                    await GetPathways();
                });
                SelectedLeaveStatus = StaticCredential.NotificationStatus ?? "All";
            }
            catch (Exception ex)
            {

            }
        }

        private async Task GetPathways()
        {
            try
            {
                var path = await RecommendingPathwayService.GetPathway(LoggedInUser.SubCategory);
                Pathways.Clear();
                Pathways.AddRange(path.Recommend.Split("/").ToList());
            }
            catch(Exception ex)
            {

            }
        }

        private async Task GetLeaveApplications()
        {
            try
            {
                IsBusy = true;
                list1.Clear();
                list2.Clear();
                SelectedLeaveStatusList.Clear();
                if (LoggedInUser.SubCategory == "UHFPO")
                {
                    if (SelectedLeaveStatus == "All")
                    {
                        SelectedLeaveStatusList.Add("Forwarded");
                        SelectedLeaveStatusList.Add("Approved");
                        SelectedLeaveStatusList.Add("Rejected");
                    }
                    else
                    {
                        SelectedLeaveStatusList.Add(SelectedLeaveStatus);
                    }
                    list1 = await LeaveApplicationService.GetLeaveApplicationsByStatusAsync(SelectedLeaveStatusList);
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        LeaveApplicationList.Clear();
                        LeaveApplicationList.AddRange(list1);
                    });
                }
                else if (LoggedInUser.SubCategory == "Admin")
                {
                    if (SelectedLeaveStatus == "All")
                    {
                        SelectedLeaveStatusList.Add("Recommended");
                        SelectedLeaveStatusList.Add("Forwarded");
                        SelectedLeaveStatusList.Add("Sent back");
                    }
                    else
                    {
                        SelectedLeaveStatusList.Add(SelectedLeaveStatus);
                    }
                    Pathways.Add("RMO");
                    Pathways.Add("Statistician");
                    Pathways.Add("Office Sohokari");
                    if (SelectedLeaveStatus == "All" || SelectedLeaveStatus == "Pending")
                    {
                        list1 = await LeaveApplicationService.GetLeaveApplicationsByRoleAsync(Pathways);
                    }
                    if (SelectedLeaveStatus != "Pending")
                    {
                        list2 = await LeaveApplicationService.GetLeaveApplicationsByStatusAsync(SelectedLeaveStatusList);
                    }
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        LeaveApplicationList.Clear();
                        LeaveApplicationList.AddRange(list1);
                        LeaveApplicationList.AddRange(list2);
                    });
                }
                else
                {
                    if (pathways.Count == 0)
                    {
                        await GetPathways();
                    }
                    if (SelectedLeaveStatus == "All")
                    {
                        SelectedLeaveStatusList.Add("Agreed");
                        SelectedLeaveStatusList.Add("Recommended");
                        SelectedLeaveStatusList.Add("Declined");
                    }
                    else
                    {
                        SelectedLeaveStatusList.Add(SelectedLeaveStatus);
                    }
                    if (SelectedLeaveStatus != "Pending")
                    {
                        list1 = await LeaveApplicationService.GetLeaveApplicationsByStatusAsync(SelectedLeaveStatusList, pathways);
                    }
                    if (SelectedLeaveStatus == "All" || SelectedLeaveStatus == "Pending")
                    {
                        list2 = await LeaveApplicationService.GetLeaveApplicationsByStatusNoProxyAsync(new List<string> { "Pending" }, pathways);
                    }
                    MainThread.BeginInvokeOnMainThread(() => {
                        LeaveApplicationList.Clear();
                        LeaveApplicationList.AddRange(list1);
                        LeaveApplicationList.AddRange(list2);
                    });
                }
            }
            catch (Exception ex)
            {
                LeaveApplicationList.Clear();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
