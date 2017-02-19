using System;
using System.IO;
using System.Threading.Tasks;
using hack_team_2.OCR;
using hack_team_2.sharepoint.Config;
using Microsoft.SharePoint.Client;
using Novacode;
using File = System.IO.File;
using Image = System.Drawing.Image;

// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable RedundantArgumentNameForLiteralExpression

namespace hack_team_2.sharepoint
{
    public class DocumentOperation
    {
        public async void UploadImageAsWordDocument(string imageFilePath)
        {
            string wordDocumentFilePath = CreateWordDocumentWithImage(imageFilePath);
            string textFromImage = await ExtractTextFromImage(imageFilePath);

            UploadDocumentToSharepoint(wordDocumentFilePath, textFromImage);
        }

        private string CreateWordDocumentWithImage(string imageFilePath)
        {
            var imageFileName = Path.GetFileNameWithoutExtension(imageFilePath);
            string documentPath = $@"Document\\{imageFileName}.docx";
            using (var document = DocX.Create(documentPath))
            {
                using (var memoryStream = new MemoryStream())
                {
                    var image = Image.FromFile(imageFilePath);

                    image.Save(memoryStream, image.RawFormat);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    Novacode.Image documentPicture = document.AddImage(memoryStream);

                    Paragraph paragraph = document.InsertParagraph();
                    Picture picture = documentPicture.CreatePicture();

                    paragraph.InsertPicture(picture, index: 0);

                    document.Save();
                }
            }
            return documentPath;
        }

        private async Task<string> ExtractTextFromImage(string imageFilePath)
        {
            string subscriptionKey = CognitiveServicesConnectionData.SubscriptionKey;
            var ocrCore = new OcrCore(subscriptionKey);

            using (Stream imageStream = new FileStream(imageFilePath, FileMode.Open))
            {
                string text = await ocrCore.ExtractTextFromImage(imageStream, "unk");
                return text;
            }

            throw new Exception("exception thrown from ExtractTextFromImage");
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
                    Url = Path.GetFileName(wordDocumentFilePath)
                };
                var documents = web.Lists.GetByTitle("OCR");
                Microsoft.SharePoint.Client.File addedDocument = documents.RootFolder.Files.Add(newFile);
                var item = addedDocument.ListItemAllFields;
                item["OCR"] = textFromImage;
                item.Update();

                context.ExecuteQuery();
            }
        }

        public byte[] DownloadFile(string fileName)
        {
            byte[] fileAsByte;
            Uri filename = new Uri(string.Format(@"{0}ocr/{1}", SharepointConnectionData.WebFullUrl, fileName));
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
    }
}
