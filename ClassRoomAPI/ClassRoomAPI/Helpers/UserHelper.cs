using ClassRoomAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ClassRoomAPI.Helpers
{
    public class UserHelper
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public static string GetUserNumber()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            return localSettings.Values["username"].ToString();
        }

        public static void SetUserInfo(Password password)
        {
            LocalSettingHelper.SetLocalSettings<string>("username", password.username);

            var vault = new Windows.Security.Credentials.PasswordVault();
            vault.Add(new Windows.Security.Credentials.PasswordCredential(
                "Tsinghua_Learn_Website", password.username, password.password));

        }

        public static void SetUserEmailName(string name)
        {
            LocalSettingHelper.SetLocalSettings<string>("useremailname", name);
        }
        public static string GetUserEmailName()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            return localSettings.Values["useremailname"].ToString();
        }
        public static string GetUserPassword()
        {
            var username = GetUserNumber();
            var vault = new Windows.Security.Credentials.PasswordVault();
            string password = vault.Retrieve("Tsinghua_Learn_Website", username).Password;
            return password;
        }

        public static bool IsDemo()
        {
            return
               localSettings.Values["username"] != null &&
               localSettings.Values["username"].ToString() == "233333";
        }

        static public bool CredentialAbsent()
        {
            var username = localSettings.Values["username"];
            return username == null
                || username.ToString() == "__anonymous";
        }

        static public bool SupposedToWorkAnonymously()
        {
            var username = localSettings.Values["username"];
            return username != null
                && username.ToString() == "__anonymous";
        }

        public static bool CheckDemo(string a)
        {
            return a == "233333";
        }



    }
}
