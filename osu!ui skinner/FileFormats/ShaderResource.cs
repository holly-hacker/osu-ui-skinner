using System;
using System.Text;

namespace osu_ui_skinner.FileFormats
{
    internal class ShaderResource : ResourceFileBase
    {
        public override string Category => "Shaders";
        public override string FileExtension => ".glsl";

        private readonly string _text;

        public ShaderResource(byte[] data)
        {
            _text = Encoding.UTF8.GetString(data);
        }

        public override byte[] GetData() => Encoding.UTF8.GetBytes(_text);
    }
}
