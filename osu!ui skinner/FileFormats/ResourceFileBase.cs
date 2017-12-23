using System.IO;

namespace osu_ui_skinner.FileFormats
{
    internal abstract class ResourceFileBase
    {
        public abstract string Category { get; }
        public abstract string FileExtension { get; }

        public virtual string FileName => null;

        public abstract void SaveData(Stream str);
    }
}
