using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarMaint.Services   // <-- THIS FIXES YOUR ERROR
{
    public class TranslationService
    {
        private readonly string key = "YOUR_AZURE_TRANSLATOR_KEY";
        private readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";
        private readonly string region = "YOUR_RESOURCE_REGION";

        public async Task<string> DetectLanguageAsync(string text)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", region);

                var body = new object[] { new { Text = text } };
                var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);

                var response = await client.PostAsync(
                    $"{endpoint}/detect?api-version=3.0",
                    new StringContent(requestBody, Encoding.UTF8, "application/json")
                );

                var result = await response.Content.ReadAsStringAsync();

                // Parse as JObject because detect returns an object
                var json = JObject.Parse(result);

                // If Azure returned an error, handle it
                if (json["error"] != null)
                {
                    string message = json["error"]["message"].ToString();
                    throw new Exception("Azure Translator Error: " + message);
                }

                // If Azure returned language info
                if (json["language"] != null)
                {
                    return json["language"].ToString();
                }

                throw new Exception("Unexpected Azure response: " + result);
            }
        }



        public async Task<string> TranslateAsync(string text, string toLang)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", region);

                var body = new object[] { new { Text = text } };
                var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);

                var response = await client.PostAsync(
                    $"{endpoint}/translate?api-version=3.0&to={toLang}",
                    new StringContent(requestBody, Encoding.UTF8, "application/json")
                );

                var result = await response.Content.ReadAsStringAsync();
                var json = JArray.Parse(result);

                return json[0]["translations"][0]["text"].ToString();
            }
        }
    }
}
