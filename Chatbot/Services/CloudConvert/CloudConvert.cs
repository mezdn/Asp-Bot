using Chatbot.Services.CloudConvert.Dtos;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Services.CloudConvert
{
    public class CloudConvert : ICloudConvert
    {
        private HttpClient Client;

        public CloudConvert(CloudConvertSettings settings)
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);
            Client.BaseAddress = new Uri(settings.EndpointUrl);
        }

        public async Task<string> ConvertAudioToWavAsync(string audioUrl)
        {
            //Start the process
            var postProcessDto = new PostProcessDto
            {
                inputformat = "oga",
                outputformat = "wav"
            };
            var response = await Client.PostAsync("process",
                new StringContent(JsonConvert.SerializeObject(postProcessDto), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            var getProcessJson = await response.Content.ReadAsStringAsync();
            var process = JsonConvert.DeserializeObject<GetProcessDto>(getProcessJson);

            //Start conversion
            var postConversionDto = new PostConversionDto
            {
                input = "download",
                file = audioUrl,
                outputformat = "wav",
                filename = "input.oga",
                converteroptions = new ConversionOptions
                {
                    audio_bitrate = 16,
                    audio_channels = "mono",
                    audio_frequency = 16000
                }
            };
            var urlString = "https://" + process.Host + "/";
            var token = Client.DefaultRequestHeaders.Authorization.Parameter;
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(urlString);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var conversionResponse = await httpClient.PostAsync($"process/{process.Id}",
                new StringContent(JsonConvert.SerializeObject(postConversionDto), Encoding.UTF8, "application/json"));
            var getConversionJson = await conversionResponse.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var conversion = JsonConvert.DeserializeObject<GetConversionDto>(getConversionJson);

            //Download output File
            string returnedUri;
            var outputResponse = await httpClient.GetAsync($"process/{process.Id}");
            var getoutputJson = await outputResponse.Content.ReadAsStringAsync();
            var output = JsonConvert.DeserializeObject<GetConversionDto>(getoutputJson);
            returnedUri = output.output.url;
            
            var guid = Guid.NewGuid().ToString();
            var webClient = new WebClient();
            await webClient.DownloadFileTaskAsync("https:" + returnedUri, $"{guid}.wav");
            return $"{guid}.wav";
        }
    }
}
