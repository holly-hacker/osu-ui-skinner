using System.Text;

namespace osu_ui_skinner.FileFormats
{
    internal class WAVResource : UnknownResource
    {
        public override string Category => "Audio";
        public override string FileExtension => ".wav";

        public WAVResource(byte[] bytes) : base(bytes) { }

        public static bool Detect(ref byte[] bytes) => bytes.BytesEqual(Encoding.ASCII.GetBytes("RIFF")) || bytes.BytesEqual(Encoding.ASCII.GetBytes("WAVEfmt"));
    }
}
