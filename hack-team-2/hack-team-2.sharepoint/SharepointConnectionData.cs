using System.Configuration;
using System.Security;

namespace hack_team_2.sharepoint
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
                foreach (var c in ConfigurationManager.AppSettings["SharepointUrl"])
                {
                    password.AppendChar(c);
                }
                return password;
            }
        }
    }
}
