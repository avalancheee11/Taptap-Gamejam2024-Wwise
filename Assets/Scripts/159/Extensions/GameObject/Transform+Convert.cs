using System;
using UnityEngine;

public static class TransformConvert
{
    //convert
    public static Vector3 convertToNodeSpace(this Transform tf, float x, float y)
    {
        return tf.convertToNodeSpace(new Vector3(x, y));
    }

    public static Vector3 convertToNodeSpace(this Transform tf, Vector3 vector)
    {
        return tf.InverseTransformPoint(vector);
    }

    public static Vector3 convertToWorldSpace(this Transform tf, float x, float y)
    {
        return tf.convertToWorldSpace(new Vector3(x, y));
    }

    public static Vector3 convertToWorldSpace(this Transform tf, Vector3 vector)
    {
        return tf.TransformPoint(vector);
    }
}