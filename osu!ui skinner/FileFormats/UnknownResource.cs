using dnlib.DotNet.Resources;

namespace osu_ui_skinner.FileFormats
{
    internal class UnknownResource : ResourceFileBase
    {
        public override string FileExtension => ".bin";
        public override string Category => "unknown";

        private readonly byte[] _data;

        public UnknownResource(IResourceData obj) : this(obj.GetDataBytes()) { }
        public UnknownResource(byte[] bytes)
        {
            Logger.Debug("\tCreated " + nameof(UnknownResource));
            _data = bytes;
        }

        public override byte[] GetData() => _data;
    }
}
