using System;
using System.Diagnostics;

namespace osu_ui_skinner
{
    internal static class Logger
    {
        public static void Info(string text)
        {
            WriteWithColor(text, ConsoleColor.White);
        }

        public static void Warn(string text)
        {
            WriteWithColor(text, ConsoleColor.Yellow);
        }

        public static void Error(string text)
        {
            WriteWithColor(text, ConsoleColor.Red);
        }

        [Conditional("DEBUG")]
        public static void Debug(string text)
        {
            WriteWithColor("[D] " + text, ConsoleColor.DarkGray);
        }


        private static void WriteWithColor(string text, ConsoleColor c)
        {
            var orig = Console.ForegroundColor;
            Console.ForegroundColor = c;
            Console.WriteLine(text);
            Console.ForegroundColor = orig;
        }
    }
}
