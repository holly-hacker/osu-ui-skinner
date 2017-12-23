using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner.FileFormats.Factories
{
    internal class BeatmapResourceFactory : FactoryBase
    {
        public override string Category => "Beatmaps";

        public override byte Detect(ResourceElement element) => element.ResourceData.GetBytes().MatchBytes(new byte[] {0xEC, 0x48, 0x4F}).ToByte();

        public override ResourceBase CreateResource(ResourceElement element, byte type) => new Osz2Resource(element.ResourceData.GetBytes());
    }
}
