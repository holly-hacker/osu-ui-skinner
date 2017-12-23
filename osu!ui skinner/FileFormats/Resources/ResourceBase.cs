using dnlib.DotNet.Resources;
using System.IO;

namespace osu_ui_skinner.FileFormats.Resources
{
    internal abstract class ResourceBase
    {
        public abstract string FileExtension { get; }

        public virtual string FileName => null;
        public virtual string ResourceName => null;
        
        public abstract void Deserialize(Stream s);
        public abstract IResourceData Serialize();
    }
}
