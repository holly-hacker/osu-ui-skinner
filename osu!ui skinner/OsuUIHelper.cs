using System;
using System.Diagnostics;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats;

namespace osu_ui_skinner
{
    internal static class OsuUIHelper
    {
        public static void Extract(string fullPath, string outputDir)
        {
            Logger.Info($"Creating output directory {outputDir}...");
            Directory.CreateDirectory(outputDir);

            Logger.Info("Loading module...");
            var mod = ModuleDefMD.Load(fullPath);
            var res = mod.Resources;

            Debug.Assert(res.Count == 1, "Invalid amount of resources");
            Debug.Assert(res[0] is EmbeddedResource, "Resource is not embedded");
            Debug.Assert(res[0].Name == "osu_ui.ResourcesStore.resources", "Invalid resource name");

            if (res.Count != 1 || !(res[0] is EmbeddedResource r) || r.Name != "osu_ui.ResourcesStore.resources")
                throw new Exception("Invalid osu!ui.dll");
            
            Logger.Info("Iterating through resources...");
            foreach (var element in ResourceReader.Read(mod, r.Data).ResourceElements) {
                Logger.Debug(element.ToString());

                //save
                ResourceFileBase b = FileFormatHelper.ToFileFormat(element);
                string catFolder = Path.Combine(outputDir, b.Category);
                Directory.CreateDirectory(catFolder);
                File.WriteAllBytes(Path.Combine(catFolder, element.Name + b.FileExtension), b.GetData());
            }
        }
    }
}
