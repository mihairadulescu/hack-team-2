using System.IO;
using System.Security;
using Microsoft.SharePoint.Client;
using File = System.IO.File;

namespace hack_team_2.sharepoint
{
    public class DocumentOperation
    {
        public void UploadImageAsWordDocument(string imageFilePath)
        {
            using (var context = new ClientContext("https://wwwrita.sharepoint.com"))
            {
                

                var passWord = new SecureString();
                foreach (var c in "!Pios1234")
                {
                    passWord.AppendChar(c);
                }
                context.Credentials = new SharePointOnlineCredentials("daniela.ilie@wwwrita.onmicrosoft.com", passWord);
                var web = context.Web;
                var newFile = new FileCreationInformation
                {
                    Content = File.ReadAllBytes(@"C:\Users\Ovidiu\Documents\Document1.docx"),
                    Url = Path.GetFileName(@"C:\Users\Ovidiu\Documents\Document1.docx"),

                };
                var docs = web.Lists.GetByTitle("Documents");
                Microsoft.SharePoint.Client.File doc = docs.RootFolder.Files.Add(newFile);
                var item = doc.ListItemAllFields;
                item["OCR"] = "bmw 2050";
                item.Update();

                context.ExecuteQuery();
            }
        }
    }
}
