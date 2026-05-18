using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;

namespace AD.Editor
{
    public static class ADEditorDefs
    {
        private static readonly Def[] Defs = new Def[]
        {
            new Def("ADToolBar", "EXIST_AD_TOOLBAR")
        };

        private static List<string> defines = new List<string>();

        [InitializeOnLoadMethod]
        private static void InitADEditor()
        {
            CompilationPipeline.compilationStarted += OnCompilationStart;
            // CompilationPipeline.RequestScriptCompilation(); += KeepSymbol;

            KeepSymbol();
        }

        private static void OnCompilationStart(object obj) => KeepSymbol();

        private static void KeepSymbol()
        {
            if (BuildPipeline.isBuildingPlayer) return;
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            defines.Clear();
            defines.AddRange(PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';'));
            bool update = false;
            foreach (var def in Defs)
            {
                int indexOf;
                if (IsAssemblyExist(def.assemblyName) ^ ((indexOf = defines.IndexOf(def.symbol)) >= 0))
                {
                    if (indexOf >= 0) defines.RemoveAt(indexOf);
                    else defines.Add(def.symbol);
                    update = true;
                }
            }

            if (update)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", defines));
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }

        private static bool IsAssemblyExist(string assemblyName)
        {
            var assemblies = CompilationPipeline.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.name == assemblyName)
                    return true;
            }

            return false;
        }

        private class Def
        {
            public string assemblyName;
            public string symbol;

            public Def(string assemblyName, string symbol)
            {
                this.assemblyName = assemblyName;
                this.symbol = symbol;
            }
        }
    }
}