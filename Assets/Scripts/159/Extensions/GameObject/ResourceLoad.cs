using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ResourceLoad
{
#if UNITY_EDITOR
    /// <summary>
    /// 只能加载assets下除StreamingAssets的文件
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T loadResource<T>(this string path) where T : UnityEngine.Object
    {
        T o = AssetDatabase.LoadAssetAtPath<T>(path);
        return o;
    }
#endif
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">带后缀</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T loadResourceByAssetBundle<T>(this string name) where T : UnityEngine.Object
    {
        T o = CacheManager.Instance.loadResourceByAssetBundle<T>(name);
        return o;
    }
    
    public static Texture2D loadTextureByAssetBundle(this string name)
    {
        return name.loadResourceByAssetBundle<Texture2D>();
    } 
    
    public static AudioClip loadAudioClipByAssetBundle(this string name)
    {
        return name.loadResourceByAssetBundle<AudioClip>();;
    } 
    
    public static Animation loadAnimationByAssetBundle(this string name)
    {
        return name.loadResourceByAssetBundle<Animation>();
    } 
    
    public static Animator loadAnimatorByAssetBundle(this string name)
    {
        return name.loadResourceByAssetBundle<Animator>();
    } 
}