﻿using Firebase.Database;
using Firebase.Database.Query;
using HospitalLeaveApplication.Models;
using HospitalLeaveApplication.Models.HelperModels;
using HospitalLeaveApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLeaveApplication.Services
{
    public class PathwayService
    {
        public async static Task<Pathway> GetPathwayAsync(string role)
        {
            FirebaseClient firebaseClient = new FirebaseClient(StaticCredential.DatabaseUrl);
            FirebaseObject<Pathway> firebaseObject = null;
            if (role != null && role != "")
            {
                firebaseObject = (await firebaseClient
                    .Child("Pathway")
                    .OnceAsync<Pathway>()).FirstOrDefault(a => a.Object.Role == role);
            }
            else
            {
                firebaseObject = (await firebaseClient
                    .Child("Pathway")
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
