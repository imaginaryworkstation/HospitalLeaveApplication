using HospitalLeaveApplication.Utilities;
using System;
namespace HospitalLeaveApplication.Models
{
	public class LeaveApplication
	{
		public string Key { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string LeaveType { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public int Days { get; set; }
		public string Residence { get; set; }
		public string ProxyName { get; set; }
		public string ProxyEmail { get; set; }
		public string Status { get; set; }
		public string Role { get; set; }
		public string Message { get; set; }
		public DateTime? ApproveDate { get; set; }
		public bool BlackText { get { return StaticCredential.BlackTextList().Contains(Status); } }
		public bool BlueText { get { return StaticCredential.BlueTextList().Contains(Status); } }
    }
}

