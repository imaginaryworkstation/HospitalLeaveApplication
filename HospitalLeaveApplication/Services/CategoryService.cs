using System;
using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Utilities;

namespace HospitalLeaveApplication.Services
{
	public class CategoryService
	{
        public async static Task<List<Category>> GetCategoriesAsync()
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<Category> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("Categories").OnceAsync<Category>()).Select(u => u.Object).ToList();
            return firebaseObjects;
        }
    }
}

