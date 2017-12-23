using System.IO;

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
        public override void SaveData(Stream s) => s.Write(_data);
    }
}
