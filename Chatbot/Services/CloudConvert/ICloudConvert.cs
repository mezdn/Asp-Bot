using System.Threading.Tasks;

namespace Chatbot.Services.CloudConvert
{
    public interface ICloudConvert
    {
        /// <param name="audioUrl"></param>
        /// <returns>returns the audio name in the root level folder</returns>
        Task<string> ConvertAudioToWavAsync(string audioUrl);
    }
}
