using System.Threading.Tasks;

namespace Chatbot.Services.SpeechToText
{
    public interface ISpeechRecognizer
    {
        Task<string> RecognizeSpeechAsync(string audioUrl);
    }
}
