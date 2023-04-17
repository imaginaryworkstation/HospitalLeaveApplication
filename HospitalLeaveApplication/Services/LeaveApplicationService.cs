using System;
using System.Linq;
using Firebase.Database;
using Firebase.Database.Query;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Utilities;
using Microsoft.Maui.Storage;

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

        public async static Task<FirebaseObject<LeaveApplication>> GetLeaveApplicationByKeyAsync(string key)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            FirebaseObject<LeaveApplication> firebaseObject = null;
            if (key != null && key != "")
            {
                firebaseObject = (await firebaseClient
                    .Child("LeaveApplications")
                    .OnceAsync<LeaveApplication>()).FirstOrDefault(a => a.Object.Key == key);
            }
            else
            {
                firebaseObject = (await firebaseClient
                    .Child("LeaveApplications")
                    .OnceAsync<LeaveApplication>()).FirstOrDefault(a => a.Object.Key == key);
            }
            if (firebaseObject != null && firebaseObject.Object != null)
            {
                return firebaseObject;
            }
            return null;
        }

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByEmailAsync(string email, string status = null)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            var query = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(l => l.Object.Email == email)
                .OrderBy(l => l.Object.FromDate);
            if (status != null)
            {
                firebaseObjects = query.Where(l => l.Object.Status == status).Select(u => u.Object).ToList();
            }
            else
            {
                firebaseObjects = query.Select(u => u.Object).ToList();
            }
            return firebaseObjects;
        }

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByProxyAsync(string email)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(l => l.Object.Proxy == email)
                .Where(l => l.Object.Status == "Approved")
                .OrderBy(l => l.Object.FromDate)
                .Select(u => u.Object).ToList();
            return firebaseObjects;
        }

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByRoleAsync(string role)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(l => l.Object.Role == role)
                .OrderBy(l => l.Object.FromDate)
                .Select(u => u.Object).ToList();
            return firebaseObjects;
        }

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByRecommendedRoleAsync(List<Pathway> roles, string status = null)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            var query = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(p => roles.Any(c => p.Object.Role.Contains(c.Role)))
                .OrderBy(l => l.Object.FromDate);
            if(status != null)
            {
                firebaseObjects = query.Where(l => l.Object.Status == status).Select(u => u.Object).ToList();
            }
            else
            {
                firebaseObjects = query.Select(u => u.Object).ToList();
            }
            return firebaseObjects;
        }

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByStatusAsync(string status)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(l => l.Object.Status == status)
                .OrderBy(l => l.Object.FromDate)
                .Select(u => u.Object).ToList();
            return firebaseObjects;
        }

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsAsync()
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("LeaveApplications").OnceAsync<LeaveApplication>()).Select(u => u.Object).ToList();
            return firebaseObjects;
        }

        public async static Task<bool> UpdateLeaveApplicationAsync(string key, LeaveApplication leaveApplication)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            await firebaseClient.Child("LeaveApplications").Child(key).PutAsync(leaveApplication);
            return true;
        }
    }
}

