using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Factories;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner
{
    internal static class FileFormatHelper
    {
        private static readonly FactoryBase[] Factories = {
            new ImageResourceFactory(),
            new ShaderResourceFactory(),
            new FontResourceFactory(),
            new BeatmapResourceFactory(),
            new AudioResourceFactory(),
            new UnknownResourceFactory()
        };

        public static ResourceBase ToFileFormat(ResourceElement obj, out string s)
        {
            foreach (FactoryBase factory in Factories)
                if (factory.Detect(obj, out byte type))
                    return factory.CreateResource(obj, type, out s);
            throw new Exception("No fallback factory present");
        }

        public static IEnumerable<ResourceElement> GetResourceElements(string fullPath)
        {
            //loop through all folders
            foreach (string directory in Directory.GetDirectories(fullPath))
            {
                //get factory for this folder
                string d = directory.Split(Path.DirectorySeparatorChar).Last();
                FactoryBase b = Factories.Single(f => f.Category == d);

                Logger.Info($"Packing {b.Category} files...");

                //loop through files
                foreach (string filePath in Directory.GetFiles(directory))
                {
                    //read the file from disk
                    string fileName = Path.GetFileName(filePath);
                    Logger.Debug($"Reading resource {fileName}");
                    ResourceBase res = b.ReadResource(File.OpenRead(filePath));

                    //if ResourceName is not set, take the file name and remove the extensions
                    //then, serialize it back into an IResourceData and return it
                    yield return new ResourceElement {
                        Name = res.ResourceName ?? fileName.Remove(fileName.Length - res.FileExtension.Length),
                        ResourceData = res.Serialize()
                    };
                }
            }
        }
    }
}
