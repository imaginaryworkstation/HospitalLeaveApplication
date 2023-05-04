using Firebase.Database;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Utilities;

namespace HospitalLeaveApplication.Services
{
    public class CertificateService
    {
        public async static Task<Certificate> GetCertificate(string type)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            FirebaseObject<Certificate> firebaseObject = null;
            firebaseObject = (await firebaseClient
                    .Child("Certificates")
                    .OnceAsync<Certificate>()).FirstOrDefault(a => a.Object.Type == type);
            if (firebaseObject != null && firebaseObject.Object != null)
            {
                return firebaseObject.Object as Certificate;
            }
            return null;
        }
    }
}
