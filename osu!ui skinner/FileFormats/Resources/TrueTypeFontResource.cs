namespace osu_ui_skinner.FileFormats.Resources
{
    internal class TrueTypeFontResource : UnknownResource
    {
        public override string FileExtension => ".ttf";

        public TrueTypeFontResource(byte[] bytes) : base(bytes) { }
    }
}
