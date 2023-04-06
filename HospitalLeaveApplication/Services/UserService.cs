using System;
using Firebase.Database;
using Firebase.Database.Query;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Utilities;

namespace HospitalLeaveApplication.Services
{
	public class UserService
	{
        public async static Task<FirebaseResponse> StoreUser(User user)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            FirebaseObject<User> firebaseObject = await firebaseClient.Child("Users").PostAsync(user);
            return new FirebaseResponse
            {
                Code = 200,
                Message = "User successfully added"
            };
        }

        public async static Task<User> GetUserAsync(string email)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            FirebaseObject<User> firebaseObject = null;
            if (email != null && email != "")
            {
                firebaseObject = (await firebaseClient
                    .Child("Users")
                    .OnceAsync<User>()).FirstOrDefault(a => a.Object.Email == email);
            }
            else
            {
                firebaseObject = (await firebaseClient
                    .Child("Users")
                    .OnceAsync<User>()).FirstOrDefault(a => a.Object.Email == email);
            }
            if (firebaseObject != null && firebaseObject.Object != null)
            {
                return firebaseObject.Object as User;
            }
            return null;
        }

        public async static Task<List<User>> GetUsersAsync()
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<User> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("Users").OnceAsync<User>()).Select(u => u.Object).ToList();
            return firebaseObjects;
        }

        public async static Task<List<User>> GetUsersAsync(string Subcategory)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<User> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("Users").OnceAsync<User>()).Select(u => u.Object).ToList();
            firebaseObjects = firebaseObjects.Where(s => s.SubCategory == Subcategory).ToList();
            return firebaseObjects;
        }
    }
}

