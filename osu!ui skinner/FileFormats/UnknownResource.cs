using System.IO;
using dnlib.DotNet.Resources;

namespace osu_ui_skinner.FileFormats
{
    internal class UnknownResource : ResourceFileBase
    {
        public override string FileExtension => string.Empty;
        public override string Category => "Unknown";

        private readonly byte[] _data;

        public UnknownResource(IResourceData obj) : this(obj.GetDataBytes()) { }
        public UnknownResource(byte[] bytes)
        {
            _data = bytes;
        }

        public override void SaveData(Stream s) => s.Write(_data);
    }
}
