using HospitalLeaveApplication.Models;
using System;
namespace HospitalLeaveApplication.Utilities
{
	public class StaticCredential
	{
        public static string DatabaseUrl = "https://hospitalleave-default-rtdb.asia-southeast1.firebasedatabase.app/";
        public static string LeaveApplicationStatus = null;
        public static string NotificationStatus = null;
        public static string ProxyStatus = null;

        public static User User = null;

        public static List<String> GetLeaveTypes()
        {
            List<string> LeaveTypes = new List<string>
            {
                "Casual",
                "Station Leave",
                "Casual with station Leave"
            };
            return LeaveTypes;
        }
        public static List<String> GetLeaveStatus()
        {
            List<string> StatusList = new List<string>
            {
                "Pending",
                "Agreed",
                "Denied",
                "Forwarded",
                "Declined",
                "Reccomended",
                "Sent back",
                "Approved",
                "Rejected",
            };
            return StatusList;
        }
        public static List<String> GetPostingLocations()
        {
            List<string> PostingLocations = new List<string>
            {
                "Health Complex",
                "Union Health Complex",
                "Union",
                "Community Clinic"
            };
            return PostingLocations;
        }
        public static List<String> GetPostingWards()
        {
            List<string> PostingWards = new List<string>
            {
                "Ward 1",
                "Ward 2",
                "Ward 3"
            };
            return PostingWards;
        }

        public static List<string> BlackTextList()
        {
            return new List<string>
            {
                "Rejected", "Sent back", "Declined", "Denied", "Pending"
            };
        }

        public static List<string> BlueTextList()
        {
            return new List<string>
            {
                "Approved", "Forwarded", "Recommended", "Agreed"
            };
        }

        public static List<string> SeniorList()
        {
            return new List<string>
            {
                "RMO", "Nursing Supervisor", "NS", "Office Shohokari", "HI in charge", "HI", "AHI", "Admin", "UHFPO"
            };
        }
    }
}

