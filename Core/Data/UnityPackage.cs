﻿using System;
using System.IO;
using System.Linq;
using ThunderKit.Common;
using ThunderKit.Core.Attributes;
using ThunderKit.Core.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThunderKit.Core.Data
{
    using static ScriptableHelper;
    [Flags]
    public enum IncludedSettings
    {
        AudioManager = 1,
        ClusterInputManager = 2,
        DynamicsManager = 4,
        EditorBuildSettings = 8,
        EditorSettings = 16,
        GraphicsSettings = 32,
        InputManager = 64,
        NavMeshAreas = 128,
        NetworkManager = 256,
        Physics2DSettings = 512,
        PresetManager = 1024,
        ProjectSettings = 2048,
        QualitySettings = 4096,
        TagManager = 8192,
        TimeManager = 16384,
        UnityConnectSettings = 32768,
        VFXManager = 65536,
        XRSettings = 131072
    }

    public class UnityPackage : ScriptableObject
    {
        const string ExportMenuPath = Constants.ThunderKitContextRoot + "Compile " + nameof(UnityPackage);

        [EnumFlag]
        public IncludedSettings IncludedSettings;
        [EnumFlag]
        public ExportPackageOptions exportPackageOptions;

        public Object[] AssetFiles;

        [MenuItem(Constants.ThunderKitContextRoot + nameof(UnityPackage), false, priority = Constants.ThunderKitMenuPriority)]
        public static void Create()
        {
            SelectNewAsset<UnityPackage>();
        }

        //[MenuItem(ExportMenuPath, true, priority = Constants.ThunderKitMenuPriority)]
        //public static bool CanExport() => Selection.activeObject is UnityPackage;

        //[MenuItem(ExportMenuPath, false, priority = Constants.ThunderKitMenuPriority)]
        //public static void Export()
        //{
        //    if (!(Selection.activeObject is UnityPackage redist)) return;
        //    redist.Export("Deployments");
        //}

        public void Export(string path)
        {
            var assetPaths = AssetFiles.Select(af => AssetDatabase.GetAssetPath(af));

            string[] assetPathNames = assetPaths.ToArray();
            string fileName = Path.Combine(path, $"{name}.unityPackage");
            string metaFileName = Path.Combine(path, $"{name}.unityPackage.meta");
            if (File.Exists(fileName)) File.Delete(fileName);
            if (File.Exists(metaFileName)) File.Delete(metaFileName);
            AssetDatabase.ExportPackage(assetPathNames, fileName, exportPackageOptions);
        }
    }
}