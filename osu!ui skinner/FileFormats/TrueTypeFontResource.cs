using System;
using System.Linq;

namespace osu_ui_skinner.FileFormats
{
    internal class TrueTypeFontResource : UnknownResource
    {
        public override string Category => "Fonts";
        public override string FileExtension => ".ttf";

        public TrueTypeFontResource(byte[] bytes) : base(bytes) { }

        public static bool Detect(ref byte[] bytes)
        {
            string[] toDetect = {"hmtx", "hhea", "cmap", "glyf", "head", "maxp", "name", "post"};

            foreach (string s in toDetect) {
                //check if it exists
                var success = false;

                //the specified strings can only occur in the first 0x200 bytes
                for (int i = 0; i < Math.Min(bytes.Length, 0x200); i++) {
                    byte b = bytes[i];
                    if (b == s[0]) {
                        if (i + s.Length - 1 >= bytes.Length) continue;    //we're at the end and this isn't it

                        //need to speed this up, badly
                        //TODO
                        var candidate = new string(bytes.Skip(i).Take(s.Length).Select(a => (char)a).ToArray());
                        if (candidate == s) {
                            success = true;
                            break;  //it exists, go to next one
                        }
                    }
                }

                //didn't find it :(
                if (!success)
                    return false;
            }

            //all check passed, w00t!
            return true;
        }
    }
}
