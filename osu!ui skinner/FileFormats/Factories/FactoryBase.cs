using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner.FileFormats.Factories
{
    internal abstract class FactoryBase
    {
        public abstract string Category { get; }

        public abstract byte Detect(ResourceElement element);
        public abstract ResourceBase CreateResource(ResourceElement element, byte type);

        //helper methods
        public bool Detect(ResourceElement obj, out byte type) => (type = Detect(obj)) > 0;

        public ResourceBase CreateResource(ResourceElement element, byte type, out string category)
        {
            category = Category;
            return CreateResource(element, type);
        }
    }
}
