using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner.FileFormats.Factories
{
    internal class UnknownResourceFactory : FactoryBase
    {
        public override string Category => "Unknown";
        public override byte Detect(ResourceElement element) => 1;

        public override ResourceBase CreateResource(ResourceElement element, byte type) => new UnknownResource(element.ResourceData.GetBytes());
    }
}
