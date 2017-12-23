using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Factories;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner
{
    internal static class FileFormatHelper
    {
        private static readonly FactoryBase[] Factories = {
            new ImageResourceFactory(),
            new ShaderResourceFactory(),
            new FontResourceFactory(),
            new BeatmapResourceFactory(),
            new AudioResourceFactory()
        };
        private static readonly FactoryBase DefaultFactory = new UnknownResourceFactory();

        public static ResourceBase ToFileFormat(ResourceElement obj, out string s)
        {
            foreach (FactoryBase factory in Factories)
                if (factory.Detect(obj, out byte type))
                    return factory.CreateResource(obj, type, out s);
            
            return DefaultFactory.CreateResource(obj, 0, out s);
        }
    }
}
