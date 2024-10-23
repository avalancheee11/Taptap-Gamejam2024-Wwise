using System;
using UnityEngine;

public static class GameObjectConvert
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="behaviour"></param>
    /// <param name="vector">WorldPosition</param>
    /// <returns></returns>
    public static Vector3 convertToNodeSpace(this MonoBehaviour behaviour, Vector3 vector)
    {
        return behaviour.gameObject.convertToNodeSpace(vector);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <param name="vector">WorldPosition</param>
    /// <returns></returns>
    public static Vector3 convertToNodeSpace(this GameObject go, Vector3 vector)
    {
        return go.transform.InverseTransformPoint(vector);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="behaviour"></param>
    /// <param name="vector">LocalPostion</param>
    /// <returns></returns>
    public static Vector3 convertToWorldSpace(this MonoBehaviour behaviour, Vector3 vector)
    {
        return behaviour.gameObject.convertToWorldSpace(vector);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <param name="vector">LocalPoint</param>
    /// <returns></returns>
    public static Vector3 convertToWorldSpace(this GameObject go, Vector3 vector)
    {
        return go.transform.TransformPoint(vector);
    }
    
    public static Texture2D convertToTexture2D(this Sprite sprite)
    {
        // var targetTex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        // var pixels = sprite.texture.GetPixels();
        // targetTex.SetPixels(pixels);
        // return targetTex;
        return sprite.texture;
    }
}
