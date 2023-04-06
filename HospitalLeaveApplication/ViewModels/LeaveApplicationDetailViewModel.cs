using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLeaveApplication.ViewModels
{
    class LeaveApplicationDetailViewModel : BaseViewModel
    {
        public void OnAppearing()
        {
            Task.Run(async () => await GetLeaveApplicationDetail());
        }

        private async Task GetLeaveApplicationDetail()
        {

        }
    }
}
