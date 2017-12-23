namespace osu_ui_skinner.FileFormats.Resources
{
    internal class WAVResource : UnknownResource
    {
        public override string FileExtension => ".wav";

        public WAVResource(byte[] bytes) : base(bytes) { }
    }
}
