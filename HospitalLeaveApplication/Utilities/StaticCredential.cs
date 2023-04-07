using HospitalLeaveApplication.Models;
using System;
namespace HospitalLeaveApplication.Utilities
{
	public class StaticCredential
	{
        public static string DatabaseUrl = "https://hospitalleave-default-rtdb.asia-southeast1.firebasedatabase.app/";
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
                "Rejected"
            };
            return StatusList;
        }
    }
}

