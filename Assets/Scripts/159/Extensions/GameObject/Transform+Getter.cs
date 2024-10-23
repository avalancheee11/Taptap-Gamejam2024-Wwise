using System;
using UnityEngine;

public static class TransformGetter
{
    //getPosition
    public static Vector3 getPosition(this Transform tf)
    {
        return tf.localPosition;
    }

    //getScale
    public static Vector3 getScale(this Transform tf)
    {
        return tf.localScale;
    }

    public static float getScaleX(this Transform tf)
    {
        return tf.localScale.x;
    }

    public static float getScaleY(this Transform tf)
    {
        return tf.localScale.y;
    }

    //getRotation
    public static float getRotation(this Transform tf)
    {
        return tf.localRotation.FromQ2().z;
    }

    //getRadian
    public static float getRadian(this Transform tf)
    {
        return tf.getRotation().toRadian();
    }
}

