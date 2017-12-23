namespace osu_ui_skinner.FileFormats.Resources
{
    internal class MP3Resource : UnknownResource
    {
        public override string FileExtension => ".mp3";

        public MP3Resource(byte[] bytes) : base(bytes) { }
    }
}
