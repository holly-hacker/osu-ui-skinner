using System;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Resources;

namespace osu_ui_skinner
{
    internal static class OsuUIHelper
    {
        public static void Extract(string fullPath)
        {
            var mod = ModuleDefMD.Load(fullPath);
            var res = mod.Resources;

            Debug.Assert(res.Count == 1);
            Debug.Assert(res[0] is EmbeddedResource);
            Debug.Assert(res[0].Name == "osu_ui.ResourcesStore.resources");

            var r = (EmbeddedResource)res[0];

            foreach (var element in ResourceReader.Read(mod, r.Data).ResourceElements) {
                Console.WriteLine(element.Name);
            }
        }
    }
}
