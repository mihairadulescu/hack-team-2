using hack_team_2.sharepoint;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hack_team_2.sharepoint.Tests
{
    [TestFixture]
    public class DocumentOperationTests
    {
        [Test]
        public void Given_an_word_document_the_upload_is_successfull()
        {
            var documentOperation = new DocumentOperation();
            documentOperation.UploadDocumentToSharepoint(@"Data\test.docx", "A GOAL WITHOUT");
        }

        [Test]
        public void Given_an_image_document_the_upload_is_successfull()
        {
            var documentOperation = new DocumentOperation();
            documentOperation.UploadDocumentToSharepoint(@"Data\test.jpg", "A GOAL WITHOUT");
        }

        [Test]
        public void Given_an_image_document_the_download_is_successfull()
        {
            var documentOperation = new DocumentOperation();
            documentOperation.DownloadFile("test.jpg");
        }
    }
}
