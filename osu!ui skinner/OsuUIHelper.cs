using System;
using System.Diagnostics;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Resources;
using osu_ui_skinner.FileFormats.Resources;

namespace osu_ui_skinner
{
    internal static class OsuUIHelper
    {
        private const string Namespace = "osu_ui";
        private const string ResourceStore = "ResourcesStore";
        private const string ResourceStoreFull = Namespace + "." + ResourceStore;
        private const string Resources = ResourceStoreFull + ".resources";

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
                ResourceBase b = FileFormatHelper.ToFileFormat(element, out string category);

                if (b.GetType() == typeof(UnknownResource))
                    Logger.Warn("Unknown resource detected: " + element.Name);
                else
                    Logger.Debug("\tCreated " + b.GetType().Name);

                string catFolder = Path.Combine(outputDir, category);
                Directory.CreateDirectory(catFolder);

                using (FileStream fs = File.OpenWrite(Path.Combine(catFolder, (b.FileName ?? element.Name) + b.FileExtension)))
                    b.SaveData(fs);
            }
        }

        public static void Build(string fullPath, string outputPath)
        {
            var mod = new ModuleDefUser("osu!ui.dll", null, ModuleDefMD.Load(typeof(void).Module).Assembly.ToAssemblyRef()) {
                Kind = ModuleKind.Dll
            };
            
            var ass = new AssemblyDefUser("osu!ui", Version.Parse("1.0.0.0"));
            ass.Modules.Add(mod);

            //TODO: add bytes
            mod.Resources.Add(new EmbeddedResource(Resources, new byte[] {}, ManifestResourceAttributes.Private));

            //create store type
            TypeDef store = new TypeDefUser(Namespace, ResourceStore, mod.CorLibTypes.Object.TypeDefOrRef) {Attributes = TypeAttributes.Public | TypeAttributes.BeforeFieldInit};

            //add the type to the module
            mod.Types.Add(store);

            //add code
            BuildStore(mod, ref store);

            //write module
            mod.Write(Path.Combine(outputPath, "osu!ui-rebuilt.dll"));
        }

        private static void BuildStore(ModuleDef mod, ref TypeDef store)
        {
            //reused variables
            CilBody body;
            Instruction instr;
            var importer = new Importer(mod);
            
            var trType = importer.Import(typeof(System.Type));
            var trResourceManager = importer.Import(typeof(System.Resources.ResourceManager));

            TypeSig tsType = trType.ToTypeSig();
            TypeSig tsRuntimeTypeHandle = new ValueTypeSig(importer.Import(typeof(System.RuntimeTypeHandle)));
            TypeSig tsAssembly = importer.ImportAsTypeSig(typeof(System.Reflection.Assembly));
            TypeSig tsResourceManager = trResourceManager.ToTypeSig();
            TypeSig tsCultureInfo = importer.ImportAsTypeSig(typeof(System.Globalization.CultureInfo));

            //create fields
            /*
	            .field private static class [mscorlib]System.Resources.ResourceManager resourceMan
	            .field private static class [mscorlib]System.Globalization.CultureInfo resourceCulture
             */
            var fieldMan = new FieldDefUser("resourceMan", new FieldSig(tsResourceManager)) {
                Attributes = FieldAttributes.Private | FieldAttributes.Static
            };
            store.Fields.Add(fieldMan);

            var fieldCulture = new FieldDefUser("resourceCulture", new FieldSig(tsCultureInfo)) {
                Attributes = FieldAttributes.Private | FieldAttributes.Static
            };
            store.Fields.Add(fieldCulture);


            #region .ctor
            /*
                .method assembly hidebysig specialname rtspecialname 
	            instance void .ctor () cil managed   

             	IL_0000: ldarg.0
	            IL_0001: call      instance void [mscorlib]System.Object::.ctor()
	            IL_0006: ret
             */

            MethodDef ctor = new MethodDefUser(".ctor", MethodSig.CreateInstance(mod.CorLibTypes.Void))
            {
                Attributes = MethodAttributes.Assembly | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
            };

            ctor.Body = body = new CilBody();
            body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            body.Instructions.Add(OpCodes.Call.ToInstruction(new MemberRefUser(mod, ".ctor", MethodSig.CreateInstance(mod.CorLibTypes.Void), mod.CorLibTypes.Object.ToTypeDefOrRef())));
            body.Instructions.Add(OpCodes.Ret.ToInstruction());

            store.Methods.Add(ctor);
            #endregion

            #region get_ResourceManager
            /*
            	.method public hidebysig specialname static 
		        class [mscorlib]System.Resources.ResourceManager get_ResourceManager () cil managed 

                IL_0000: ldsfld    class [mscorlib]System.Resources.ResourceManager osu_ui.ResourcesStore::resourceMan
		        IL_0005: brtrue.s  IL_0025
		        IL_0007: ldstr     "osu_ui.ResourcesStore"
		        IL_000C: ldtoken   osu_ui.ResourcesStore
		        IL_0011: call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		        IL_0016: callvirt  instance class [mscorlib]System.Reflection.Assembly [mscorlib]System.Type::get_Assembly()
		        IL_001B: newobj    instance void [mscorlib]System.Resources.ResourceManager::.ctor(string, class [mscorlib]System.Reflection.Assembly)
		        IL_0020: stsfld    class [mscorlib]System.Resources.ResourceManager osu_ui.ResourcesStore::resourceMan
		        IL_0025: ldsfld    class [mscorlib]System.Resources.ResourceManager osu_ui.ResourcesStore::resourceMan
		        IL_002A: ret
             */

            //create method
            MethodDef getMan = new MethodDefUser("get_ResourceManager", MethodSig.CreateStatic(tsResourceManager)) {
                Attributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
            };

            getMan.Body = body = new CilBody();
            body.Instructions.Add(OpCodes.Ldsfld.ToInstruction(fieldMan));
            body.Instructions.Add(OpCodes.Brtrue_S.ToInstruction((Instruction)null));    //add operand later
            body.Instructions.Add(OpCodes.Ldstr.ToInstruction(ResourceStoreFull));
            body.Instructions.Add(OpCodes.Ldtoken.ToInstruction((ITokenOperand)store));
            body.Instructions.Add(OpCodes.Call.ToInstruction(new MemberRefUser(mod, "GetTypeFromHandle", MethodSig.CreateStatic(tsType, tsRuntimeTypeHandle), trType)));
            body.Instructions.Add(OpCodes.Callvirt.ToInstruction(new MemberRefUser(mod, "get_Assembly", MethodSig.CreateInstance(tsAssembly), trType)));
            body.Instructions.Add(OpCodes.Newobj.ToInstruction(new MemberRefUser(mod, ".ctor", MethodSig.CreateInstance(mod.CorLibTypes.Void, mod.CorLibTypes.String, tsAssembly), trResourceManager)));
            body.Instructions.Add(OpCodes.Stsfld.ToInstruction(fieldMan));
            body.Instructions.Add(instr = OpCodes.Ldsfld.ToInstruction(fieldMan));
            body.Instructions.Add(OpCodes.Ret.ToInstruction());

            body.UpdateInstructionOffsets();
            body.Instructions[1].Operand = instr;
            
            store.Methods.Add(getMan);
            #endregion

            #region get_Culture
            /*
                .method public hidebysig specialname static 
	            class [mscorlib]System.Globalization.CultureInfo get_Culture () cil managed

             	IL_0000: ldsfld    class [mscorlib]System.Globalization.CultureInfo osu_ui.ResourcesStore::resourceCulture
	            IL_0005: ret
             
             */

            MethodDef getCult = new MethodDefUser("get_Culture", MethodSig.CreateStatic(tsCultureInfo)) {
                Attributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
            };

            getCult.Body = body = new CilBody();
            body.Instructions.Add(OpCodes.Ldsfld.ToInstruction(fieldCulture));
            body.Instructions.Add(OpCodes.Ret.ToInstruction());

            store.Methods.Add(getCult);
            #endregion

            #region get_Culture
            /*
                .method public hidebysig specialname static 
	            void set_Culture (class [mscorlib]System.Globalization.CultureInfo 'value') cil managed    

             	IL_0000: ldarg.0
	            IL_0001: stsfld    class [mscorlib]System.Globalization.CultureInfo osu_ui.ResourcesStore::resourceCulture
	            IL_0006: ret
             */

            MethodDef setCult = new MethodDefUser("set_Culture", MethodSig.CreateStatic(mod.CorLibTypes.Void, tsCultureInfo)) {
                Attributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
            };
            setCult.Parameters[0].CreateParamDef();
            setCult.Parameters[0].ParamDef.Name = "value";

            setCult.Body = body = new CilBody();
            body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            body.Instructions.Add(OpCodes.Stsfld.ToInstruction(fieldCulture));
            body.Instructions.Add(OpCodes.Ret.ToInstruction());

            store.Methods.Add(setCult);
            #endregion
        }
    }
}
