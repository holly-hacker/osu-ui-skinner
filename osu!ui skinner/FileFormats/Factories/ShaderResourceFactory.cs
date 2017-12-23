using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner.FileFormats.Factories
{
    internal class ShaderResourceFactory : FactoryBase
    {
        public override string Category => "Shaders";

        public override byte Detect(ResourceElement element) => element.Name.StartsWith("sh_").ToByte();

        public override ResourceBase CreateResource(ResourceElement element, byte type) 
            => new ShaderResource(element.Name, element.ResourceData.GetBytes());
    }
}
