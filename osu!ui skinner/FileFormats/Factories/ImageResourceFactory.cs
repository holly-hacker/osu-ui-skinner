using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner.FileFormats.Factories
{
    internal class ImageResourceFactory : FactoryBase
    {
        public override string Category => "Images";

        public override byte Detect(ResourceElement element) => IsBitmap(element).ToByte();

        public override ResourceBase CreateResource(ResourceElement element, byte type) 
            => new BitmapResource(((BinaryResourceData)element.ResourceData).Data);


        private static bool IsBitmap(ResourceElement obj) => obj.ResourceData is BinaryResourceData br 
                                                          && br.TypeName.StartsWith("System.Drawing.Bitmap");
    }
}
