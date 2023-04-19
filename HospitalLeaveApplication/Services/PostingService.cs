using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Utilities;

namespace HospitalLeaveApplication.Services
{
    public class PostingService
    {
        public async static Task<List<Posting>> GetUnionHealthComplexesAsync()
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<Posting> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("UnionHealthComplex").OnceAsync<Posting>()).Select(u => u.Object).ToList();
            return firebaseObjects;
        }
        public async static Task<List<Posting>> GetUnionsAsync()
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<Posting> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("Union").OnceAsync<Posting>()).Select(u => u.Object).ToList();
            return firebaseObjects;
        }
        public async static Task<List<Posting>> GetCommunitiesAsync()
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            List<Posting> firebaseObjects = null;
            firebaseObjects = (await firebaseClient.Child("Community").OnceAsync<Posting>()).Select(u => u.Object).ToList();
            return firebaseObjects;
        }
    }
}
