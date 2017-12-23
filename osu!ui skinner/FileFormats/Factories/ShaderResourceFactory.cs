using System.IO;
using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner.FileFormats.Factories
{
    internal class ShaderResourceFactory : FactoryBase
    {
        public override string Category => "Shaders";

        public override byte Detect(ResourceElement element) => (element.Name.StartsWith("sh_") && element.Name.Split('_').Length == 3).ToByte();

        public override ResourceBase CreateResource(ResourceElement element, byte type) => new ShaderResource(element);
        public override ResourceBase ReadResource(FileStream fs) => new ShaderResource(Path.GetFileName(fs.Name), fs);
    }
}
