using System;
using System.Data;
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
            try
            {
                FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
                FirebaseObject<User> firebaseObject = await firebaseClient.Child("Users").PostAsync(user);
                return new FirebaseResponse
                {
                    Code = 200,
                    Message = "User successfully added"
                };
            }
            catch (Exception ex)
            {
                return new FirebaseResponse
                {
                    Code = 500,
                    Message = "Internal error occured."
                };
            }
        }

        public async static Task<FirebaseObject<User>> GetUserWithKeyAsync(string email)
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
                return firebaseObject;
            }
            return null;
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

        public async static Task<List<User>> GetUsersAsync(string email = null)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<User> firebaseObjects = null;
            var query = (await firebaseClient.Child("Users").OnceAsync<User>()).OrderBy(u => u.Object.Name);
            if (email != null)
            {
                firebaseObjects = query.Where(u => u.Object.Email != email).Select(u => u.Object).ToList();
            }
            else
            {
                firebaseObjects = query.Select(u => u.Object).ToList();
            }
            
            return firebaseObjects;
        }

        public async static Task<List<User>> GetUsersAsync(List<string> subcategories)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<User> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("Users").OnceAsync<User>()).Select(u => u.Object).ToList();

            firebaseObjects = firebaseObjects
                .Where(s => subcategories.Any(r => s.SubCategory.Contains(r)))
                //.Where(s => s.SubCategory == Subcategory)
                .OrderBy(u => u.Name)
                .ToList();
            return firebaseObjects;
        }
        public async static Task<FirebaseResponse> UpdateUserAsync(string key, User user)
        {
            try
            {
                FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
                await firebaseClient.Child("Users").Child(key).PutAsync(user);
                return new FirebaseResponse
                {
                    Code = 200,
                    Message = "User successfully updated"
                };
            }
            catch (Exception ex)
            {
                return new FirebaseResponse
                {
                    Code = 500,
                    Message = "Internal error occured."
                };
            }
        }
    }
}

