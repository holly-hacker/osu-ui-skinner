using System;
using System.IO;

namespace osu_ui_skinner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintHeader();
            
            //clueless user ran our program, tell them no to
            if (args.Length == 0) {
                Console.WriteLine("This is a commandline application, but you can simply drag 'n' drop either an osu!ui.dll or extracted folder on me too, and I will work my magic!.");
                Console.ReadLine();
                return;
            }

            string path = Path.GetFullPath(args[0]);

            if (Directory.Exists(path)) {
                throw new NotImplementedException("Re-compiling osuui.dll is not yet implemented.");
            } else if (File.Exists(path)) {
                OsuUIHelper.Extract(path);
            }
            else {
                Console.WriteLine("Please pass me an existing file or directory as parameter.");
                return;
            }
        }

        private static void PrintHeader()
        {
#if DEBUG
            const string versionText = "(DEBUG BUILD)";
#else
            const string versionText = "v.indev";
#endif
            const string headerText = "osu!ui skinner" + " " + versionText;
            int len = Math.Max(headerText.Length, 20);


            Console.WriteLine("+" + new string('-', len + 2) + "+");
            Console.WriteLine($"| {headerText.PadRight(len)} |");
            Console.WriteLine("+" + new string('-', len + 2) + "+");
            Console.WriteLine();
        }
    }
}
