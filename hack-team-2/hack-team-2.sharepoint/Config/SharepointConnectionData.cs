using System.Configuration;
using System.Security;

namespace hack_team_2.sharepoint.Config
{
    public class SharepointConnectionData
    {
        public static string WebFullUrl => ConfigurationManager.AppSettings["SharepointUrl"];
        public static string Username => ConfigurationManager.AppSettings["Username"];
        public static SecureString Password
        {
            get
            {
                var password = new SecureString();
                foreach (var character in ConfigurationManager.AppSettings["Password"])
                {
                    password.AppendChar(character);
                }
                return password;
            }
        }
    }
}
