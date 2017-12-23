using System.IO;

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

        public override void SaveData(Stream s) => s.Write(_data);
    }
}
