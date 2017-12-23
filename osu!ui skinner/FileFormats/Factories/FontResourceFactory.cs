using System;
using System.Linq;
using System.Text;
using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner.FileFormats.Factories
{
    internal class FontResourceFactory : FactoryBase
    {
        private const byte TypeTTF = 1;
        private const byte TypeOTF = 2;

        public override string Category => "Fonts";

        public override byte Detect(ResourceElement element)
        {
            var bytes = element.ResourceData.GetBytes();
            if (DetectTTF(ref bytes)) return TypeTTF;
            if (DetectOTF(ref bytes)) return TypeOTF;
            return 0;
        }

        public override ResourceBase CreateResource(ResourceElement element, byte type)
        {
            var bytes = element.ResourceData.GetBytes();

            switch (type)
            {
                case TypeTTF: return new TrueTypeFontResource(bytes);
                case TypeOTF: return new OpenTypeFontResource(bytes);
                default: throw new ArgumentOutOfRangeException(nameof(type));
            }
        }


        private static bool DetectTTF(ref byte[] bytes)
        {
            //the specified strings can only occur in the first 0x200 bytes
            byte[] firstPart = bytes.Take(0x200).ToArray();

            //check if all the strings occur in firstPart
            return new[] { "hmtx", "hhea", "cmap", "glyf", "head", "maxp", "name", "post" }
                .Select(s => Encoding.ASCII.GetBytes(s))
                .All(pattern => firstPart.HasPattern(pattern));
        }


        //check if the first 5 bytes are equal to the magic
        private static bool DetectOTF(ref byte[] bytes) => bytes.MatchBytes(new byte[] { 0x4F, 0x54, 0x54, 0x4F, 0x00 });
    }
}
