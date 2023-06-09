﻿using System;
namespace HospitalLeaveApplication.Models
{
	public class User
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Category { get; set; }
		public string SubCategory { get; set; }
		public int Enjoyed { get; set; }
        public int Remaining { get; set; }
		public string Posting { get; set; }
		public string Password { get; set; }
		public DateTime LastLogin { get; set; }
    }
}

