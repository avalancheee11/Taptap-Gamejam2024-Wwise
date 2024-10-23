using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CustomJson;
using UnityEngine;

public partial class CacheManager
{
    public Dictionary<string, string> assetBundlePaths { get; private set; }

    //通过AB包模块加载出来的
    private Dictionary<string, (AssetBundle, Dictionary<string, object>)> assetBundles;
    //没有通过ab包模块加载出来的
    private Dictionary<string, Dictionary<string, object>> assetBundleResources;

    void initAssetBundle()
    {
        this.assetBundles = new Dictionary<string, (AssetBundle, Dictionary<string, object>)>();
        this.assetBundleResources = new Dictionary<string, Dictionary<string, object>>();
        this.assetBundlePaths = new Dictionary<string, string>();
    }

    void clearAssetBundle()
    {
        this.assetBundleResources.Clear();
    }

    public void loadAssetBundleModAsync(Action callBack)
    {
        var path = Path.Combine(CustomGlobalConfig.streamingAssetBasePath, "mods", CustomGlobalConfig.ModName, "packpath.json");
        FileUtils.Instance.readFileStringAsync(path, (str) =>
        {       
            var dic = MiniJson.JsonDecode(str);
            foreach (var kvp in dic.toDictionary()) {
                this.assetBundlePaths[kvp.Key] = kvp.Value.toString();
            }
            callBack?.Invoke();
        });

    }

    public string getAssetBundleMod(string fileName)
    {
        return this.assetBundlePaths.objectValue(fileName);
    }
    
    public void loadModuleByAssetBundleAysnc(string module, Action<AssetBundle> callBack)
    {
        if (this.assetBundles.containsKey(module)) {
            callBack?.Invoke(this.assetBundles[module].Item1);
            return ;
        }

        CoroutineUtils.Instance.StartCoroutine(load(module, callBack));

        IEnumerator load(string module, Action<AssetBundle> callBack)
        {
            var path = Path.Combine(CustomGlobalConfig.streamingAssetBasePath, "mods", CustomGlobalConfig.ModName, "assetsBundles", module);
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(path);
            yield return bundleLoadRequest;
            
            var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                callBack?.Invoke(null);
                yield break;
            }
            
            this.assetBundles[module] = (myLoadedAssetBundle, new Dictionary<string, object>());
            callBack?.Invoke(myLoadedAssetBundle);
        }
    }
    
    public void unLoadModuleByAssetBundle(string module)
    {
        if (!this.assetBundles.containsKey(module)) {
            return;
        }

        var item = this.assetBundles[module];
        item.Item1.Unload(false);
        this.assetBundles.Remove(module);
    }

    public T loadPrefabByAssetBundle<T>(string fileName) where T : MonoBehaviour
    {
        var o = this.loadResourceByAssetBundle<GameObject>(fileName + ".prefab");
        return o.GetComponent<T>();
    }

    public Sprite loadSpriteByAssetBundle(string fileName)
    {
        return this.loadResourceByAssetBundle<Sprite>(fileName + ".png");
    }

    
    public AudioClip loadAudioClipByAssetBundle(string fileName)
    {
        return this.loadResourceByAssetBundle<AudioClip>(fileName + ".ogg");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName">带后缀</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T loadResourceByAssetBundle<T>(string fileName) where T : UnityEngine.Object
    {
        var module = this.getAssetBundleMod(fileName);
        if (module.IsNullOrEmpty()) {
            return null;
        }

#if UNITY_EDITOR
        var path = Path.Combine("Assets", "AssetBundleResource", module, fileName);
        return path.loadResource<T>();
#endif
        
        if (this.assetBundles.containsKey(module)) {
            var item = this.assetBundles[module];
            var o = item.Item2.objectValue(fileName);
            if (o != null) {
                return o as T;
            }

            var t = item.Item1.LoadAsset<T>(fileName);
            item.Item2[fileName] = t;
            return t;
        }

        Debug.LogWarning($"资源对应的ab包mod没有提前加载,这里做同步加载的处理 mod:{module}, fileName:{fileName}");
        var dic = this.assetBundleResources.objectValue(module);
        if (dic == null) {
            dic = new Dictionary<string, object>();
            this.assetBundleResources[module] = dic;
        }
        var obj = dic.objectValue(fileName);
        if (obj == null) {
            var o = AssetBundle.LoadFromFile(Path.Combine(CustomGlobalConfig.streamingAssetBasePath, "mods", CustomGlobalConfig.ModName, "assetsBundles", module));
            if (o == null) {
                Debug.LogError(string.Format("资源加载失败 module : {0}  fileName : {1}", module, fileName));
                return null;
            }
            
            obj = o.LoadAsset<T>(fileName);
            o.Unload(false);

            dic[fileName] = obj;
        }

        return obj as T;
    }
}
