using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Utilities;

namespace HospitalLeaveApplication.Services
{
    public class RecommendingPathwayService
    {
        public async static Task<Pathway> GetPathway(string role)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            FirebaseObject<Pathway> firebaseObject = null;
            if (role != null && role != "")
            {
                firebaseObject = (await firebaseClient
                    .Child("RecommendingPathway")
                    .OnceAsync<Pathway>()).FirstOrDefault(a => a.Object.Role == role);
            }
            else
            {
                firebaseObject = (await firebaseClient
                    .Child("RecommendingPathway")
                    .OnceAsync<Pathway>()).FirstOrDefault(a => a.Object.Role == role);
            }
            if (firebaseObject != null && firebaseObject.Object != null)
            {
                return firebaseObject.Object as Pathway;
            }
            return null;
        }
    }
}
