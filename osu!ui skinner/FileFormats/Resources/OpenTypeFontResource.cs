namespace osu_ui_skinner.FileFormats.Resources
{
    internal class OpenTypeFontResource : UnknownResource
    {
        public override string FileExtension => ".otf";

        public OpenTypeFontResource(byte[] bytes) : base(bytes) { }
    }
}
