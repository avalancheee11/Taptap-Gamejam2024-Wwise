#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BuildTools
{
    public static string rootPath = Path.Combine(Application.dataPath, "AssetBundleResource");
    
    private static List<AssetBundleBuild> _assetBundleBuilds = new List<AssetBundleBuild>();

    [MenuItem("Tools/BuildTool/CreateAssetBundles")]
    public static void BuildAssetBundles()
    {
        _assetBundleBuilds.Clear();
        if (!Directory.Exists(rootPath)) {
            Debug.LogError("CreateAssetBundles Error AssetBundleResource文件夹缺失");
            return;
        }

        var targetPath = string.Format("{0}/{1}/{2}/{3}", Application.streamingAssetsPath, "mods", CustomGlobalConfig.ModName, "assetsBundles");
        //删除已经生成的
        if (Directory.Exists(targetPath)) {
            Directory.Delete(targetPath, true);
        }
        //设置对应文件夹内的资源
        DirectoryInfo rootDirectory = new DirectoryInfo(rootPath);
        createAssetBundlesByDir(rootDirectory);

        Directory.CreateDirectory(targetPath);
        
        var builds = _assetBundleBuilds.ToArray();
        
        BuildPipeline.BuildAssetBundles(targetPath, builds, BuildAssetBundleOptions.ChunkBasedCompression|BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();  
        
        PackManagerPath.packPath();
    }

    static void createAssetBundlesByDir(DirectoryInfo directoryInfo)
    {
        if (!directoryInfo.FullName.Equals(rootPath)) {
            var assetsNameList = new List<string>();
            var files = directoryInfo.GetFiles();
            foreach (var fileInfo in files) {
                if (CheckFileSuffixNeedIgnore(fileInfo.Name)) {
                    continue;
                }

                var p = fileInfo.FullName.Substring(rootPath.Length + 1);
                var pp = String.Format("{0}/{1}/{2}", "Assets", "AssetBundleResource", p);
                assetsNameList.Add(pp);
            }

            if (assetsNameList.Count > 0) {
                var p = directoryInfo.FullName.Substring(rootPath.Length + 1);
                AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
                assetBundleBuild.assetBundleName = p;
                assetBundleBuild.assetNames = assetsNameList.ToArray();
                _assetBundleBuilds.Add(assetBundleBuild);
            }
        }
        
        var directories = directoryInfo.GetDirectories();

        foreach (var dirInfo in directories) {
            createAssetBundlesByDir(dirInfo);
        }
    }
    
    public static bool CheckFileSuffixNeedIgnore(string fileName)
    {
        if(fileName.EndsWith(".meta") || fileName.EndsWith(".DS_Store") || 
           fileName.EndsWith(".cs") || fileName.EndsWith(".lua") || 
           fileName.EndsWith(".manifest") || fileName.EndsWith(".plist"))
            return true;
        if (fileName.StartsWith("."))
            return true;
        return false;
    }
}
#endif