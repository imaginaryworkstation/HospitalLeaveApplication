using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Services;
using HospitalLeaveApplication.Utilities;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace HospitalLeaveApplication.ViewModels
{
    [QueryProperty(nameof(key), "key")]
    public class ProxyLeaveDetailViewModel : BaseViewModel
    {
        private string FirebaseKey { get; set; }
        public string key { get; set; }
        private bool isPending;
        private bool isNotPending;
        FirebaseObject<LeaveApplication> firebaseLeaveApplication;
        private LeaveApplication leaveApplication;
        private User user;
        private bool isResidenceEnable;

        public ICommand AgreeLeaveApplicationCommand { get; }
        public ICommand DenyLeaveApplicationCommand { get; }

        public ObservableRangeCollection<string> LeaveStatusList { get; }

        public bool IsPending
        {
            get => isPending;
            set
            {
                SetProperty(ref isPending, value);
                IsNotPending = !value;
            }
        }
        public bool IsNotPending { get => isNotPending; set => SetProperty(ref isNotPending, value); }
        public FirebaseObject<LeaveApplication> FirebaseLeaveApplication { get => firebaseLeaveApplication; set => SetProperty(ref firebaseLeaveApplication, value); }
        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public User User { get => user; set => SetProperty(ref user, value); }
        public bool IsResidenceEnable { get => isResidenceEnable; set => SetProperty(ref isResidenceEnable, value); }

        public ProxyLeaveDetailViewModel()
        {
            LeaveStatusList = new ObservableRangeCollection<string>();
            AgreeLeaveApplicationCommand = new AsyncCommand(ExecuteAgreeLeaveCommand);
            DenyLeaveApplicationCommand = new AsyncCommand(ExecuteDenyLeaveCommand);
        }

        private async Task ExecuteAgreeLeaveCommand()
        {
            LeaveApplication.Status = "Agreed";
            await ExecuteLeaveApplication();
        }

        private async Task ExecuteDenyLeaveCommand()
        {
            LeaveApplication.Status = "Denied";
            await ExecuteLeaveApplication();
        }

        private async Task ExecuteLeaveApplication()
        {
            try
            {
                FirebaseResponse response = await LeaveApplicationService.UpdateLeaveApplicationAsync(FirebaseKey, LeaveApplication);
                if (response != null && response.Code == 200)
                {
                    await Shell.Current.DisplayAlert(LeaveApplication.Status, response.Message, "Ok");
                    StaticCredential.ProxyStatus = LeaveApplication.Status;
                    await Shell.Current.GoToAsync("..");
                }
                else if (response != null)
                {
                    await Shell.Current.DisplayAlert("Error", response.Message, "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Internal error occured, please try again later.", "Ok");
                }
            }
            catch(Exception Ex)
            {
                await Shell.Current.DisplayAlert("Error", "Internal error occured, please try again later.", "Ok");
            }
        }

        public void OnAppearing()
        {
            GetLeaveStatusList();
        }

        private void GetLeaveStatusList()
        {
            try
            {
                LeaveStatusList.Clear();
                LeaveStatusList.Add("Pending");
                LeaveStatusList.Add("Agreed");
                LeaveStatusList.Add("Denied");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Task.Run(async () => await GetLeaveApplicationDetail());
            }
        }

        private async Task GetLeaveApplicationDetail()
        {
            FirebaseObject<LeaveApplication> FirebaseLeaveApplication = await LeaveApplicationService.GetLeaveApplicationByKeyAsync(key);
            LeaveApplication = FirebaseLeaveApplication.Object as LeaveApplication;
            FirebaseKey = FirebaseLeaveApplication.Key;
            IsResidenceEnable = LeaveApplication.LeaveType == "Casual" ? false : true;
            if (LeaveApplication != null)
            {
                IsPending = LeaveApplication.Status == "Pending";
                //await GetUserDetail();
            }
        }

        private async Task GetUserDetail()
        {
            User = await UserService.GetUserAsync(LeaveApplication.Email);
        }
    }
}
