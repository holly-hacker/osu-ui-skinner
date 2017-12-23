using System.IO;

namespace osu_ui_skinner.FileFormats.Resources
{
    internal abstract class ResourceBase
    {
        public abstract string FileExtension { get; }

        public virtual string FileName => null;
        
        public abstract void SaveData(Stream str);
    }
}
