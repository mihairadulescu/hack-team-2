using hack_team_2.sharepoint.Config;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hack_team_2.sharepoint
{
    public class DocumentChanges
    {
        private string _libraryTitle;

        public DocumentChanges(string libraryTitle)
        {
            _libraryTitle = libraryTitle;
        }

        public int GetAddedDocuments()
        {
            using (var context = new ClientContext(SharepointConnectionData.WebFullUrl))
            {
                context.Credentials = new SharePointOnlineCredentials(SharepointConnectionData.Username, SharepointConnectionData.Password);
                var web = context.Web;

                var documents = web.Lists.GetByTitle("OCR");

                context.Load(documents);
                ChangeQuery documentChangeQuery = OnlyNewDocuments();
                var documentChanges = documents.GetChanges(documentChangeQuery);
                context.Load(documentChanges);
                context.ExecuteQuery();

                foreach (Change change in documentChanges)
                {
                    Console.WriteLine("{0}-{1}", change.ChangeType, change.TypedObject);
                    if(change is ChangeFile)
                    {
                        var fileChange = change as ChangeFile;
                       
                        Console.WriteLine("{0}", fileChange.UniqueId);
                    }
                }
            }

            return 0;
        }

        private static ChangeQuery OnlyNewDocuments()
        {
            ChangeQuery documentChangeQuery = new ChangeQuery(false, false);
            documentChangeQuery.File = true;
            documentChangeQuery.Add = true;
            return documentChangeQuery;
        }
    }
}
