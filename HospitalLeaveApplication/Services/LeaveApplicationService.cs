﻿using System;
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

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByEmailAsync(string email)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(l => l.Object.Email == email)
                .OrderBy(l => l.Object.FromDate)
                .Select(u => u.Object).ToList();
            return firebaseObjects;
        }

        public async static Task<List<LeaveApplication>> GetLeaveApplicationsByProxyAsync(string email)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<LeaveApplication> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("LeaveApplications")
                .OnceAsync<LeaveApplication>())
                .Where(l => l.Object.Proxy == email)
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

