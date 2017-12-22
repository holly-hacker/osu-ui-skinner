using System;
using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats;

namespace osu_ui_skinner
{
    internal static class FileFormatHelper
    {
        public static ResourceFileBase ToFileFormat(ResourceElement obj)
        {
            if (obj.ResourceData is BinaryResourceData br) {
                //we can cast this, probably
                if (br.TypeName.StartsWith("System.Drawing.Bitmap"))
                    return new BitmapResource(br.Data);
                else {
                    Logger.Warn("Unexpected BinaryResourceData");
                    return new UnknownResource(obj.ResourceData);
                }
            }
            else if (obj.ResourceData is BuiltInResourceData bi) {
                if (bi.Data is byte[] bytes)
                {
                    if (obj.Name.StartsWith("sh_"))
                        return new ShaderResource(obj.Name, bytes);

                    //fonts
                    if (OpenTypeFontResource.Detect(ref bytes))
                        return new OpenTypeFontResource(bytes);
                    if (TrueTypeFontResource.Detect(ref bytes))
                        return new TrueTypeFontResource(bytes);
                }
            }

            //byte[] bytes = obj.ResourceData.GetDataBytes();

            return new UnknownResource(obj.ResourceData);
        }

        public static byte[] GetDataBytes(this IResourceData obj)
        {
            switch (obj)
            {
                case BuiltInResourceData d1:
                    if (d1.Data is byte[] b)
                        return b;
                    else
                        throw new Exception("BuildInResourceData's data is not a byte[]");
                case BinaryResourceData d2:
                    return d2.Data;
                default:
                    throw new Exception("Unexpected type: " + obj.GetType());
            }
        }
    }
}
