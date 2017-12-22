using System.Linq;
using System.Text;

namespace osu_ui_skinner.FileFormats
{
    internal class TrueTypeFontResource : UnknownResource
    {
        public override string Category => "Fonts";
        public override string FileExtension => ".ttf";

        public TrueTypeFontResource(byte[] bytes) : base(bytes) { }

        public static bool Detect(ref byte[] bytes)
        {
            //the specified strings can only occur in the first 0x200 bytes
            byte[] firstPart = bytes.Take(0x200).ToArray();

            //check if all the strings occur in firstPart
            return new[] {"hmtx", "hhea", "cmap", "glyf", "head", "maxp", "name", "post"}
                        .Select(s => Encoding.ASCII.GetBytes(s))
                        .All(pattern => firstPart.HasPattern(pattern));
        }
    }
}
