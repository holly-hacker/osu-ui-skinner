namespace osu_ui_skinner.FileFormats
{
    internal class OpenTypeFontResource : UnknownResource
    {
        public override string Category => "Fonts";
        public override string FileExtension => ".otf";

        public OpenTypeFontResource(byte[] bytes) : base(bytes) { }

        public static bool Detect(ref byte[] bytes)
        {
            byte[] magic = {0x4F, 0x54, 0x54, 0x4F, 0x00};

            for (int i = 0; i < magic.Length; i++) {
                if (bytes[i] != magic[i])
                    return false;
            }
            return true;
        }
    }
}
