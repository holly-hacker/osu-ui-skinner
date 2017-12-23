using System.IO;
using dnlib.DotNet.Resources;

namespace osu_ui_skinner.FileFormats.Resources
{
    internal class Osz2Resource : ResourceBase
    {
        public override string FileExtension => ".osz2";

        private readonly byte[] _data;
        
        public Osz2Resource(byte[] bytes)
        {
            _data = bytes;
        }

        //TODO: parse and extract
        public override void Deserialize(Stream s) => s.Write(_data);
        public override IResourceData Serialize() => new BuiltInResourceData(ResourceTypeCode.ByteArray, _data);
    }
}
