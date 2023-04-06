using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
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
    public class DashboardViewModel : BaseViewModel
    {
        public ICommand NavigateToLeaveApplicationsCommand { get; }
        public DashboardViewModel()
        {
            NavigateToLeaveApplicationsCommand = new AsyncCommand(ExecuteNavigateToLeaveApplications);
        }

        private async Task ExecuteNavigateToLeaveApplications()
        {
            await Shell.Current.GoToAsync("//LeaveApplicationListPage");
        }
        public void OnAppearing()
        {
            
        }
    }
}
