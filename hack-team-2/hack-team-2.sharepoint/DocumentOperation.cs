using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using hack_team_2.OCR;
using Microsoft.SharePoint.Client;
using File = System.IO.File;

namespace hack_team_2.sharepoint
{
    public class DocumentOperation
    {
        public byte[] DownloadFile(string fileName)
        {
            byte[] fileAsByte;
            Uri filename = new Uri(string.Format(@"{0}ocr/{1}",SharepointConnectionData.WebFullUrl, fileName));
            string server = filename.AbsoluteUri.Replace(filename.AbsolutePath, "");
            string serverrelative = filename.AbsolutePath;

            using (var context = new ClientContext(SharepointConnectionData.WebFullUrl))
            {
                context.Credentials = new SharePointOnlineCredentials(SharepointConnectionData.Username, SharepointConnectionData.Password);
                var web = context.Web;
                if (context.HasPendingRequest)
                    context.ExecuteQuery();

                using (FileInformation fileInformation = Microsoft.SharePoint.Client.File.OpenBinaryDirect(context, serverrelative))
                {
                    fileAsByte = ReadAsByte(fileInformation.Stream);
                }
            }

            return fileAsByte;
        }

        private static byte[] ReadAsByte(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public async void UploadImageAsWordDocument(string imageFilePath)
        {
            string wordDocumentFilePath = CreateWordDocument(imageFilePath);
            string textFromImage = await ExtractTextFromImage(imageFilePath);

            UploadDocumentToSharepoint(wordDocumentFilePath, textFromImage);
        }

        public void UploadDocumentToSharepoint(string wordDocumentFilePath, string textFromImage)
        {
            using (var context = new ClientContext(SharepointConnectionData.WebFullUrl))
            {
                context.Credentials = new SharePointOnlineCredentials(SharepointConnectionData.Username, SharepointConnectionData.Password);
                var web = context.Web;
                var newFile = new FileCreationInformation
                {
                    Content = File.ReadAllBytes(wordDocumentFilePath),
                    Url = Path.GetFileName(wordDocumentFilePath),
                    Overwrite = true
                };
                var docs = web.Lists.GetByTitle("OCR");
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
            string subscriptionKey = LoadSubscriptionKey();
            OcrCore ocrCore = new OcrCore(subscriptionKey);

            using (Stream imageStream = new FileStream(imageFilePath, FileMode.Open))
            {
                string text = await ocrCore.ExtractTextFromImage(imageStream, "unk");
                return text;
            }

            throw new Exception("exception thrown from ExtractTextFromImage");
        }

        private string LoadSubscriptionKey()
        {
            return ConfigurationManager.AppSettings["OcrSubscriptionKey"];
        }
    }
}
