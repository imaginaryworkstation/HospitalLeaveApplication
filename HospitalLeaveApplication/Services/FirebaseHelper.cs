using System;
using Firebase.Database;
using HospitalLeaveApplication.Utilities;

namespace HospitalLeaveApplication.Services
{
	public class FirebaseHelper
	{
        static FirebaseClient firebaseClient = null;
        //static FirebaseAuthProvider authProvider = null;
        public static FirebaseClient GetFirebaseClient()
        {
            if (firebaseClient == null)
            {
                firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            }
            return firebaseClient;
        }
    }
}

