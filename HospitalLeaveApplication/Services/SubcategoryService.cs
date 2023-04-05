using System;
using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Utilities;

namespace HospitalLeaveApplication.Services
{
	public class SubcategoryService
    {
        public async static Task<List<Subcategory>> GetSubcategoriesAsync()
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<Subcategory> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("Subcategories").OnceAsync<Subcategory>()).Select(u => u.Object).ToList();
            return firebaseObjects;
        }
    }
}

