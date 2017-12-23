using System.IO;
using dnlib.DotNet.Resources;

namespace osu_ui_skinner.FileFormats.Resources
{
    internal class UnknownResource : ResourceBase
    {
        public override string FileExtension => string.Empty;

        private readonly byte[] _data;

        public UnknownResource(byte[] bytes)
        {
            _data = bytes;
        }

        public override void Deserialize(Stream s) => s.Write(_data);
        public override IResourceData Serialize() => new BuiltInResourceData(ResourceTypeCode.ByteArray, _data);
    }
}
