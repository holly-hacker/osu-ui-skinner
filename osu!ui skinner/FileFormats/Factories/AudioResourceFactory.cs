using System;
using System.Text;
using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner.FileFormats.Factories
{
    internal class AudioResourceFactory : FactoryBase
    {
        private const byte TypeWAV = 1;
        private const byte TypeMP3 = 2;

        public override string Category => "Audio";

        public override byte Detect(ResourceElement element)
        {
            byte[] bytes = element.ResourceData.GetBytes();
            if (DetectWAV(ref bytes)) return TypeWAV;
            if (DetectMP3(ref bytes)) return TypeMP3;
            return 0;
        }

        public override ResourceBase CreateResource(ResourceElement element, byte type)
        {
            byte[] bytes = element.ResourceData.GetBytes();
            switch (type)
            {
                case TypeWAV: return new WAVResource(bytes);
                case TypeMP3: return new MP3Resource(bytes);
                default: throw new ArgumentOutOfRangeException(nameof(type));
            }
        }


        private static bool DetectWAV(ref byte[] bytes) => bytes.MatchBytes(Encoding.ASCII.GetBytes("RIFF")) 
                                                        || bytes.MatchBytes(Encoding.ASCII.GetBytes("WAVEfmt"));
        
        private static bool DetectMP3(ref byte[] bytes) => bytes[0] == 0xFF && (bytes[1] & 0b11100000) == 0b11100000 
                                                        || bytes.MatchBytes(Encoding.ASCII.GetBytes("ID3"));
    }
}
