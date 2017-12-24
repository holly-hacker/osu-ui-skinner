using System.Diagnostics;
using System.IO;
using System.Text;
using dnlib.DotNet.Resources;

namespace osu_ui_skinner.FileFormats.Resources
{
    internal class ShaderResource : ResourceBase
    {
        public override string FileExtension { get; }
        public override string FileName { get; }
        public override string ResourceName { get; }

        private readonly string _text;

        public ShaderResource(ResourceElement e)
        {
            string[] splitted = (ResourceName = e.Name).Split('_');

            FileName = splitted[1];
            FileExtension = "." + splitted[2];

            _text = Encoding.UTF8.GetString(e.ResourceData.GetBytes());
        }

        public ShaderResource(string filename, Stream inputStream)
        {
            string[] splitted = filename.Split('.');
            Debug.Assert(splitted.Length == 2, "Shader file name contained more than one period");
            ResourceName = $"sh_{splitted[0]}_{splitted[1]}";
            _text = new StreamReader(inputStream).ReadToEnd();

            //for the unlikely case we save it again
            FileName = filename;
            FileExtension = "." + splitted[1];
        }

        public override void Deserialize(Stream s) => s.Write(Encoding.UTF8.GetBytes(_text));
        public override IResourceData Serialize() => new BuiltInResourceData(ResourceTypeCode.ByteArray, Encoding.UTF8.GetBytes(_text));
    }
}
