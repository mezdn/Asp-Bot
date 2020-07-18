namespace Chatbot.Services.CloudConvert.Dtos
{
    public class PostConversionDto
    {
        public string input { get; set; }
        public string file { get; set; }
        public string outputformat { get; set; }
        public string filename { get; set; }
        public ConversionOptions converteroptions { get; set; }
    }

    public class ConversionOptions
    {
        public int audio_bitrate { get; set; }
        public string audio_channels { get; set; }
        public int audio_frequency { get; set; }
    }
}
