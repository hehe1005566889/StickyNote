using FlyBirdCommon.Logger;
using LevelDB;
using StickyNotes.Protocol.Tools;
using System;
using System.Text;
using Logger = FlyBirdCommon.Logger.Logger;

namespace NotesServer.Controller
{
    class LoginFactory : IDisposable
    {
        private readonly Logger logger = SharedLoggers.GetPlatformLogger();
        private readonly DB DB;

        private static LoginFactory Factory;
        public static LoginFactory GetLoginFactory()
        {
            if (Factory is null)
                Factory = new LoginFactory();

            return Factory;
        }

        public LoginFactory()
        {
            logger.Info("Login Factory Init (debug)");
            using (Options op = new Options() { CreateIfMissing = true })
            {
                DB = new DB(op, "accounts");
            }

            GC.Collect();
        }

        public bool Login(string username, string password)
        {
            return DB.Get(username) == password;
        }

        public bool Register(string username, string password)
        {
            WriteBatch batch = new WriteBatch();

            string md5 = Convert.ToBase64String(MD5Helper.GetMD5Hash(Encoding.UTF8.GetBytes(password)));
            batch.Put(username, md5);
            DB.Write(batch);

            return DB.Get(username) == md5;
        }

        public bool ChangePassWord(string name, string password, string new_password)
        {
            bool login = Login(name, password);
            if (login is false)
                return login;

            WriteBatch batch = new WriteBatch();

            string md5 = Convert.ToBase64String(MD5Helper.GetMD5Hash(Encoding.UTF8.GetBytes(new_password)));
            batch.Put(name, md5);
            DB.Write(batch);

            return DB.Get(name) == md5;
        }

        public void Close()
        {
            DB.Close();
            DB.Dispose();
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
