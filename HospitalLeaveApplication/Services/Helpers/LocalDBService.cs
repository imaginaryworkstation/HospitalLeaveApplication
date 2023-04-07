using HospitalLeaveApplication.Models;
using SQLite;

namespace HospitalLeaveApplication.Services.Helpers
{
    public class LocalDBService
    {
        static User user;
        static SQLiteAsyncConnection db;
        public async static Task InitDB()
        {
            if (db != null)
            {
                return;
            }
            string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"MoksudpurUHL.db";

            db = new SQLiteAsyncConnection(databasePath);
            try
            {
                PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (status == PermissionStatus.Denied)
                {
                    status = await MainThread.InvokeOnMainThreadAsync(() => Permissions.RequestAsync<Permissions.StorageWrite>()); ;
                }
                await db.CreateTableAsync<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public async static Task InsertToken(User t)
        {
            user = t;
            await InitDB();
            await db.InsertOrReplaceAsync(t);
        }

        public async static Task<User> GetToken()
        {
            await InitDB();
            try
            {
                var query = db.Table<User>();

                var result = await query.ToListAsync();

                foreach (var s in result)
                {
                    user = s;
                }
            }
            catch (Exception e)
            {

            }

            return user;
        }

        public async static Task RemoveToken()
        {
            try
            {
                await InitDB();
                await db.DeleteAllAsync<User>();
                user = null;
            }
            catch (Exception e)
            {

            }
        }
    }
}
