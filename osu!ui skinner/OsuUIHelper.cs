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
        private const string Namespace = "osu_ui";
        private const string ResourceStore = Namespace + ".ResourcesStore";
        private const string Resources = ResourceStore + ".resources";

        public static void Extract(string fullPath, string outputDir)
        {
            Logger.Info($"Creating output directory {outputDir}...");
            Directory.CreateDirectory(outputDir);

            Logger.Info("Loading module...");
            var mod = ModuleDefMD.Load(fullPath);
            var res = mod.Resources;

            Debug.Assert(mod.Types.Count == 2, "Too many/little types");
            Debug.Assert(mod.Types[1].Namespace == Namespace, "Invalid namespace for resource type");
            Debug.Assert(res.Count == 1, "Invalid amount of resources");
            Debug.Assert(res[0] is EmbeddedResource, "Resource is not embedded");
            Debug.Assert(res[0].Name == Resources, "Invalid resource name");

            if (res.Count != 1 || !(res[0] is EmbeddedResource r) || r.Name != Resources)
                throw new Exception("Invalid dll");
            
            Logger.Info("Saving resources...");
            foreach (var element in ResourceReader.Read(mod, r.Data).ResourceElements) {
                Logger.Debug(element.ToString());

                //save
                ResourceFileBase b = FileFormatHelper.ToFileFormat(element);

                if (b.GetType() == typeof(UnknownResource))
                    Logger.Warn("Unknown resource detected: " + element.Name);
                else
                    Logger.Debug("\tCreated " + b.GetType().Name);

                string catFolder = Path.Combine(outputDir, b.Category);
                Directory.CreateDirectory(catFolder);
                File.WriteAllBytes(Path.Combine(catFolder, (b.FileName ?? element.Name) + b.FileExtension), b.GetData());
            }
        }
    }
}
