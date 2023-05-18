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
        //public static string FirestoreUrl = "zohar-bible.appspot.com";
        //public static string DatabaseAPIKey = "AIzaSyAT-Lb9IpLaPVSNgrQ5zfCB3HxJbVbd-4U";

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
                "Approved",
                "Rejected",
                "Forwarded",
                "Reccomended"
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
    }
}

