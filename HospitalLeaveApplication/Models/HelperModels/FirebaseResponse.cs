using System;
namespace HospitalLeaveApplication.Models.HelperModels
{
    public class FirebaseResponse
    {
        private int code;
        private string message;

        public int Code { get => code; set => code = value; }
        public string Message { get => message; set => message = value; }
    }
}

