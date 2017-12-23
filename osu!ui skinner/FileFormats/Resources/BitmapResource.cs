using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using dnlib.DotNet.Resources;

namespace osu_ui_skinner.FileFormats.Resources
{
    internal class BitmapResource : ResourceBase
    {
        public override string FileExtension => ".png";

        private readonly Bitmap _bmp;

        public BitmapResource(byte[] serialized)
        {
            _bmp = (Bitmap)new BinaryFormatter().Deserialize(new MemoryStream(serialized, false));
            Logger.Debug("\timage file format: " + _bmp.RawFormat);
        }

        public BitmapResource(Bitmap image)
        {
            _bmp = image;
        }
        
        public override void Deserialize(Stream s) => _bmp.Save(s, new ImageFormat(System.Guid.Parse("{b96b3caf-0728-11d3-9d7b-0000f81ef32e}")));
        public override IResourceData Serialize()
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, _bmp);
                bytes = ms.ToArray();
            }


            return new BinaryResourceData(new UserResourceType(typeof(Bitmap).AssemblyQualifiedName, ResourceTypeCode.UserTypes), bytes);
        }
    }
}
