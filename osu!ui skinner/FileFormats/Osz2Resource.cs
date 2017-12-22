namespace osu_ui_skinner.FileFormats
{
    internal class Osz2Resource : ResourceFileBase
    {
        public override string Category => "Beatmaps";
        public override string FileExtension => ".osz2";

        private readonly byte[] _data;
        
        public Osz2Resource(byte[] bytes)
        {
            _data = bytes;
        }

        //TODO: parse and extract
        public override byte[] GetData() => _data;

        public static bool Detect(ref byte[] bytes) => bytes.MatchBytes(new byte[] {0xEC, 0x48, 0x4F});
    }
}
