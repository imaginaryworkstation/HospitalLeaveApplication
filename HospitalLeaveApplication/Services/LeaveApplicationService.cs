using System;
using Firebase.Database;
using Firebase.Database.Query;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Utilities;

namespace HospitalLeaveApplication.Services
{
	public class LeaveApplicationService
	{
        public async static Task<FirebaseResponse> StoreLeaveApplication(LeaveApplication leaveApplication)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            FirebaseObject<LeaveApplication> firebaseObject = await firebaseClient.Child("LeaveApplications").PostAsync(leaveApplication);
            return new FirebaseResponse
            {
                Code = 200,
                Message = "Leave application successfully submitted"
            };
        }

        public async static Task<LeaveApplication> GetLeaveApplicationAsync(string email)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            FirebaseObject<LeaveApplication> firebaseObject = null;
            if (email != null && email != "")
            {
                firebaseObject = (await firebaseClient
                    .Child("LeaveApplications")
                    .OnceAsync<LeaveApplication>()).FirstOrDefault(a => a.Object.Email == email);
            }
            else
            {
                firebaseObject = (await firebaseClient
                    .Child("LeaveApplications")
                    .OnceAsync<LeaveApplication>()).FirstOrDefault(a => a.Object.Email == email);
            }
            if (firebaseObject != null && firebaseObject.Object != null)
            {
                return firebaseObject.Object as LeaveApplication;
            }
            return null;
        }

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsAsync()
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("Users").OnceAsync<LeaveApplication>()).Select(u => u.Object).ToList();
            return firebaseObjects;
        }
    }
}

