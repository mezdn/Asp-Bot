using Chatbot.Services.CloudConvert;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Chatbot.Services.SpeechToText
{
    public class SpeechRecognizer : ISpeechRecognizer
    {
        private ICloudConvert CloudConvert;
        private SpeechRecognizerSettings Settings;

        public SpeechRecognizer(SpeechRecognizerSettings settings, ICloudConvert cloudConvert)
        {
            Settings = settings;
            CloudConvert = cloudConvert;
        }

        public async Task<string> RecognizeSpeechAsync(string audioUrl)
        {
            var audioName = await CloudConvert.ConvertAudioToWavAsync(audioUrl);
            var config = SpeechConfig.FromSubscription(Settings.SubscriptionKey, Settings.SubscriptionRegion);
            var textConverted = "";

            using (var audioInput = AudioConfig.FromWavFileInput(audioName))
            using (var recognizer = new Microsoft.CognitiveServices.Speech.SpeechRecognizer(config, audioInput))
            {
                var result = await recognizer.RecognizeOnceAsync();

                switch (result.Reason)
                {
                    case ResultReason.NoMatch:
                        textConverted = "Sorry, I couldn't understand what you said.";
                        break;
                    case ResultReason.RecognizedSpeech:
                        textConverted = result.Text;
                        break;
                    default:
                        break;
                }
            }

            File.Delete(audioName);
            return textConverted;
        }
    }
}
