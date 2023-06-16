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
            try
            {
                FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
                FirebaseObject<LeaveApplication> firebaseObject = await firebaseClient.Child("LeaveApplications").PostAsync(leaveApplication);
                return new FirebaseResponse
                {
                    Code = 200,
                    Message = "Leave application successfully submitted"
                };
            }
            catch(Exception ex)
            {
                return new FirebaseResponse
                {
                    Code = 500,
                    Message = "Internal error occured, please try again."
                };
            }
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

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByProxyAsync(string email, string status = null)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            var query = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(l => l.Object.ProxyEmail == email)
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

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByRoleAsync(List<string> roles)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(p => roles.Any(r => p.Object.Role.Contains(r)))
                .Where(l => l.Object.Status == "Pending")
                .OrderBy(l => l.Object.FromDate)
                .Select(u => u.Object).ToList();
            return firebaseObjects;
        }

        //public async static Task<List<LeaveApplication>> GetLeaveApplicationsByRecommendedRoleAsync(List<string> roles, string status = null)
        //{
        //    FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
        //    List<LeaveApplication> firebaseObjects = null;
        //    var query = (await firebaseClient.Child("LeaveApplications")
        //        .OnceAsync<LeaveApplication>())
        //        .Where(p => roles.Any(r => p.Object.Role.Contains(r)))
        //        .OrderBy(l => l.Object.FromDate);
        //    if(status != null)
        //    {
        //        firebaseObjects = query.Where(l => l.Object.Status == status).Select(u => u.Object).ToList();
        //    }
        //    else
        //    {
        //        firebaseObjects = query.Select(u => u.Object).ToList();
        //    }
        //    return firebaseObjects;
        //}

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByStatusAsync(List<string> statusList, List<string> roles = null)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            var query = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(p => statusList.Any(r => p.Object.Status.Contains(r)));
            if (roles != null)
            {
                firebaseObjects = query.Where(p => roles.Any(r => p.Object.Role.Contains(r)))
                    .OrderBy(l => l.Object.FromDate)
                    .Select(u => u.Object).ToList();
            }
            else
            {
                firebaseObjects = query.OrderBy(l => l.Object.FromDate).Select(u => u.Object).ToList();
            }
                
            return firebaseObjects;
        }

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByStatusNoProxyAsync(List<string> statusList, List<string> roles = null)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            var query = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(p => p.Object.ProxyEmail == "")
                .Where(p => statusList.Any(r => p.Object.Status.Contains(r)));
            if (roles != null)
            {
                firebaseObjects = query.Where(p => roles.Any(r => p.Object.Role.Contains(r)))
                    .OrderBy(l => l.Object.FromDate)
                    .Select(u => u.Object).ToList();
            }
            else
            {
                firebaseObjects = query.OrderBy(l => l.Object.FromDate).Select(u => u.Object).ToList();
            }

            return firebaseObjects;
        }

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsAsync()
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("LeaveApplications").OnceAsync<LeaveApplication>()).Select(u => u.Object).ToList();
            return firebaseObjects;
        }

        public async static Task<FirebaseResponse> UpdateLeaveApplicationAsync(string key, LeaveApplication leaveApplication)
        {
            try
            {
                FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
                await firebaseClient.Child("LeaveApplications").Child(key).PutAsync(leaveApplication);
                return new FirebaseResponse
                {
                    Code = 200,
                    Message = "Leave application successfully " + leaveApplication.Status
                };
            }
            catch (Exception ex)
            {
                return new FirebaseResponse
                {
                    Code = 500,
                    Message = "Internal error occured, please try again."
                };
            }
        }
    }
}

