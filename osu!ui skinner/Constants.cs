namespace osu_ui_skinner
{
    internal static class Constants
    {
        public const string OutputDir = "extracted";
        public static readonly string OutputDirOriginal = "Original";
        public static readonly string OutputDirEdit = "Changes";
        
        public const string ModuleName = "osu!ui.dll";
        public const string AssemblyName = "osu!ui";

        public const string Namespace = "osu_ui";
        public const string ResourceStore = "ResourcesStore";
        public const string ResourceStoreFull = Namespace + "." + ResourceStore;
        public const string Resources = ResourceStoreFull + ".resources";
    }
}
