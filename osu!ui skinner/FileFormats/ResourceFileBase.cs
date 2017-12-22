namespace osu_ui_skinner.FileFormats
{
    internal abstract class ResourceFileBase
    {
        public abstract string Category { get; }
        public abstract string FileExtension { get; }

        public abstract byte[] GetData();
    }
}
