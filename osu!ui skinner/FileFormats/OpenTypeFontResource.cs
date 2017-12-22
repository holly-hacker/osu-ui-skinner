namespace osu_ui_skinner.FileFormats
{
    internal class OpenTypeFontResource : UnknownResource
    {
        public override string Category => "Fonts";
        public override string FileExtension => ".otf";

        public OpenTypeFontResource(byte[] bytes) : base(bytes) { }

        //check if the first 5 bytes are equal to the magic
        public static bool Detect(ref byte[] bytes) => bytes.MatchBytes(new byte[] { 0x4F, 0x54, 0x54, 0x4F, 0x00 });
    }
}
