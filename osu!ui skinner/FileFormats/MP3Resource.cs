using System.Text;

namespace osu_ui_skinner.FileFormats
{
    internal class MP3Resource : UnknownResource
    {
        public override string Category => "Audio";
        public override string FileExtension => ".mp3";

        public MP3Resource(byte[] bytes) : base(bytes) { }

        public static bool Detect(ref byte[] bytes)
        {
            return DetectBasic(ref bytes) || DetectID3v2(ref bytes);
        }
        
        //Sadly, there is no better way without parsing the entire thing
        private static bool DetectBasic(ref byte[] bytes) => bytes[0] == 0xFF && (bytes[1] & 0b11100000) == 0b11100000;

        //Starts with ID3 magic
        //ReSharper disable once InconsistentNaming
        private static bool DetectID3v2(ref byte[] bytes) => bytes.MatchBytes(Encoding.ASCII.GetBytes("ID3"));
    }
}
