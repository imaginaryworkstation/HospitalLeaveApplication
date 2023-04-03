using System;
namespace HospitalLeaveApplication.Models
{
	public class LeaveApplication
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string LeaveType { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public int Days { get; set; }
	}
}

