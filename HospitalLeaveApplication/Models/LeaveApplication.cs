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
		public string Proxy { get; set; }
		public string Status { get; set; }
		public string Message { get; set; }
	}
}

