namespace osu_ui_skinner
{
    internal static class GeneralHelper
    {
        public static bool BytesEqual(this byte[] bytesSrc, byte[] toFind, int offsetSrc = 0)
        {
            if (toFind.Length > bytesSrc.Length - offsetSrc) return false;

            for (int i = 0; i < toFind.Length; i++)
                if (bytesSrc[i + offsetSrc] != toFind[i])
                    return false;

            return true;
        }

        public static bool HasPattern(this byte[] bytesSrc, byte[] toFind)
        {
            for (int i = 0; i < bytesSrc.Length; i++)
                if (bytesSrc.BytesEqual(toFind, i))
                    return true;

            return false;
        }
    }
}
