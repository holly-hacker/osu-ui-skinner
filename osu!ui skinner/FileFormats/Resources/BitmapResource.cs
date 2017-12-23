using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace osu_ui_skinner.FileFormats.Resources
{
    internal class BitmapResource : ResourceBase
    {
        public override string FileExtension => ".png";

        private readonly Bitmap _bmp;

        public BitmapResource(byte[] serialized)
        {
            _bmp = (Bitmap)new BinaryFormatter().Deserialize(new MemoryStream(serialized, false));
        }
        
        public override void SaveData(Stream s) => _bmp.Save(s, ImageFormat.Png);
    }
}
