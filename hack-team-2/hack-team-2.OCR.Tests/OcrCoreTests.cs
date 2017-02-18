using System.IO;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace hack_team_2.OCR.Tests
{
    [TestFixture]
    public class OcrCoreTests
    {
        private string subscriptionKey;

        [TestFixtureSetUp]
        public void Setup()
        {
            subscriptionKey = File.ReadAllText("Data\\TestSubscriptionKey.txt");
        }
        [Test]
        public async void Given_an_image_with_text_the_text_is_extracted()
        {
            Stream image = new FileStream("Data\\test.jpg", FileMode.Open);

            var ocrCore = new OcrCore(subscriptionKey);
            string actual = await ocrCore.ExtractTextFromImage(image, "unk");

            string expected = File.ReadAllText("Data\\testExpected.txt");

            Assert.AreEqual(expected, actual);
        }
    }
}
