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
            string pathOrig = Path.Combine(fullPath, Constants.OutputDirOriginal);
            string pathEdit = Path.Combine(fullPath, Constants.OutputDirEdit);

            //loop through all folders
            foreach (string directory in Directory.GetDirectories(pathOrig))
            {
                //get factory for this folder
                string cat = directory.Split(Path.DirectorySeparatorChar).Last();
                FactoryBase b = Factories.Single(f => f.Category == cat);

                Logger.Info($"Packing {b.Category} files...");

                //loop through files and read them from disk
                foreach (string filePath in Directory.GetFiles(directory))
                {
                    string fileName = Path.GetFileName(filePath);
                    Logger.Debug($"Reading resource {fileName}");

                    //get change path to edit folder if possible
                    string edit = filePath.Replace(pathOrig, pathEdit);

                    //read the file using the approperiate name
                    ResourceBase res = b.ReadResource(File.OpenRead(File.Exists(edit) ? edit : filePath));

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
