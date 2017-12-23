using System.IO;
using System.Text;

namespace osu_ui_skinner.FileFormats
{
    internal class ShaderResource : ResourceFileBase
    {
        public override string Category => "Shaders";

        public override string FileExtension { get; } = ".glsl";
        public override string FileName { get; }

        private readonly string _text;

        public ShaderResource(string name, byte[] data)
        {
            var splitted = name.Split('_');
            if (splitted.Length == 3) {
                FileExtension = "." + splitted[2];
                FileName = splitted[1];
            }

            _text = Encoding.UTF8.GetString(data);
        }

        public override void SaveData(Stream s) => s.Write(Encoding.UTF8.GetBytes(_text));
    }
}
