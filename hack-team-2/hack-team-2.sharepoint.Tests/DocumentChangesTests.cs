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
    public class DocumentChangesTests
    {
        [Test]
        public void Given_an_document_library_changes_can_be_detected()
        {
            string documentLibraryTitle = "OCR";
            var documentChanges = new DocumentChanges(documentLibraryTitle);
            documentChanges.GetAddedDocuments();
        }
    }
}
