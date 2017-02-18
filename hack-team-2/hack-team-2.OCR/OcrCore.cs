using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

// ReSharper disable InconsistentNaming

namespace hack_team_2.OCR
{
    public class OcrCore
    {
        private readonly string subscriptionKey;

        public OcrCore(string subscriptionKey)
        {
            this.subscriptionKey = subscriptionKey;
        }

        public async Task<string> ExtractTextFromImage(Stream imageStream, string language)
        {
            var visionServiceClient = new VisionServiceClient(subscriptionKey);

            OcrResults ocrResult = await visionServiceClient.RecognizeTextAsync(imageStream, language);

            string textResult = AggregateOcrResults(ocrResult);

            return textResult;
        }

        private string AggregateOcrResults(OcrResults ocrResults)
        {
            var result = new StringBuilder();

            if (ocrResults != null && ocrResults.Regions != null)
            {
                foreach (var item in ocrResults.Regions)
                {
                    foreach (var line in item.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            result.Append(word.Text);
                            result.Append(" ");
                        }
                        result.AppendLine();
                    }
                    result.AppendLine();
                }
            }
            return result.ToString();
        }
    }
}
