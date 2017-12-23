using System;
using System.IO;
using dnlib.DotNet.Resources;

namespace osu_ui_skinner
{
    internal static class GeneralHelper
    {
        public static byte ToByte(this bool b) => b ? (byte)1 : (byte)0;

        public static bool MatchBytes(this byte[] bytesSrc, byte[] toFind, int offsetSrc = 0)
        {
            if (toFind.Length > bytesSrc.Length - offsetSrc) return false;

            for (int i = 0; i < toFind.Length; i++)
                if (bytesSrc[i + offsetSrc] != toFind[i])
                    return false;

            return true;
        }

        public static bool HasPattern(this byte[] bytesSrc, byte[] toFind)
        {
            for (int i = 0; i < bytesSrc.Length - (toFind.Length - 1); i++)
                if (bytesSrc.MatchBytes(toFind, i))
                    return true;

            return false;
        }

        public static void Write(this Stream s, byte[] bytes) => s.Write(bytes, 0, bytes.Length);

        public static byte[] ReadAllBytes(this Stream s)
        {
            using (var ms = new MemoryStream())
            {
                s.CopyTo(ms);
                return ms.ToArray();
            }
        }
        
        public static byte[] GetBytes(this IResourceData obj)
        {
            switch (obj)
            {
                case BuiltInResourceData d1:
                    return d1.Data as byte[] ?? throw new Exception("BuildInResourceData's data is not a byte[]");
                case BinaryResourceData d2:
                    return d2.Data;
                default:
                    throw new Exception("Unexpected type: " + obj.GetType());
            }
        }
    }
}
