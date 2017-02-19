using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using hack_team_2.OCR;
using hack_team_2.sharepoint.Config;
using Microsoft.SharePoint.Client;
using File = System.IO.File;

namespace hack_team_2.sharepoint
{
    public class DocumentOperation
    {
        public async void UploadImageAsWordDocument(string imageFilePath)
        {
            string wordDocumentFilePath = CreateWordDocument(imageFilePath);
            string textFromImage = await ExtractTextFromImage(imageFilePath);

            UploadDocumentToSharepoint(wordDocumentFilePath, textFromImage);
        }

        private void UploadDocumentToSharepoint(string wordDocumentFilePath, string textFromImage)
        {
            using (var context = new ClientContext(SharepointConnectionData.WebFullUrl))
            {
                context.Credentials = new SharePointOnlineCredentials(SharepointConnectionData.Username, SharepointConnectionData.Password);
                var web = context.Web;
                var newFile = new FileCreationInformation
                {
                    Content = File.ReadAllBytes(wordDocumentFilePath),
                    Url = Path.GetFileName(wordDocumentFilePath)

                };
                var docs = web.Lists.GetByTitle("Documents");
                Microsoft.SharePoint.Client.File doc = docs.RootFolder.Files.Add(newFile);
                var item = doc.ListItemAllFields;
                item["OCR"] = textFromImage;
                item.Update();

                context.ExecuteQuery();
            }
        }

        private string CreateWordDocument(string imageFilePath)
        {
            throw new NotImplementedException();
        }

        private async Task<string> ExtractTextFromImage(string imageFilePath)
        {
            string subscriptionKey = CognitiveServicesConnectionData.SubscriptionKey;
            OcrCore ocrCore = new OcrCore(subscriptionKey);

            using (Stream imageStream = new FileStream(imageFilePath, FileMode.Open))
            {
                string text = await ocrCore.ExtractTextFromImage(imageStream, "unk");
                return text;
            }

            throw new Exception("exception thrown from ExtractTextFromImage");
        }
    }
}
