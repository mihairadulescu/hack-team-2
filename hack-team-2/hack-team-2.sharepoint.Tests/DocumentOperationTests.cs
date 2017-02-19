using NUnit.Framework;

namespace hack_team_2.sharepoint.Tests
{
    [TestFixture]
    public class DocumentOperationTests
    {
        [Test]
        public async void Given_an_image_with_text_calling_UploadImageAsWordDocument_will_add_file_to_Sharepoint()
        {
            var documentOperation = new DocumentOperation();
            await documentOperation.UploadImageAsWordDocument(@"Data\\test.jpg");
        }
    }
}
