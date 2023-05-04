using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Services;
using MvvmHelpers;

namespace HospitalLeaveApplication.ViewModels
{
    public class LeaveCertificateViewModel : BaseViewModel
    {
        private LeaveApplication leaveApplication;
        private Certificate certificate;
        private string template;

        public LeaveApplication LeaveApplication { get => leaveApplication; set => SetProperty(ref leaveApplication, value); }
        public Certificate Certificate { get => certificate; set => SetProperty(ref certificate, value); }
        public string Template { get => template; set => SetProperty(ref template, value); }

        public void OnAppearing()
        {
            Task.Run(async () => await GenerateCertificate());
        }

        private async Task GenerateCertificate()
        {
            try
            {
                Certificate = await CertificateService.GetCertificate(LeaveApplication.LeaveType);
                Template = Certificate.Template.Replace("[name]", leaveApplication.Name);
                Template = Template.Replace("[post]", leaveApplication.Role);
                Template = Template.Replace("[fromdate]", leaveApplication.FromDate.ToString("dd-MM-yyyy"));
                Template = Template.Replace("[todate]", leaveApplication.ToDate.ToString("dd-MM-yyyy"));
                if(LeaveApplication.LeaveType != "Casual")
                {
                    Template = Template.Replace("[residence]", leaveApplication.Residence);
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
