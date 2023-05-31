using System;
using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Utilities;

namespace HospitalLeaveApplication.Services
{
	public class SubcategoryService
    {
        public async static Task<List<Subcategory>> GetSubcategoriesAsync(string category = null)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<Subcategory> firebaseObjects = null;
            var query = (await firebaseClient.Child("Subcategories").OnceAsync<Subcategory>()).Select(u => u.Object);
            if (category != null)
            {
                query = query.Where(l => l.Category == category);
            }
            firebaseObjects = query.ToList();
            return firebaseObjects;
        }
    }
}

