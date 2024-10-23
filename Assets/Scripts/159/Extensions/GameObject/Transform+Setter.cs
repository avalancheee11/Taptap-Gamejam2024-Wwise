using System;
using UnityEngine;

public static class TransformSetter
{
    //setPosition
    public static void setPosition(this Transform tf, float x, float y)
    {
        tf.setPosition(new Vector3(x, y));
    }

    public static void setPosition(this Transform tf, Vector3 position)
    {
        tf.localPosition = position;
    }

    //setScale
    public static void setScale(this Transform tf, float s)
    {
        tf.setScale(new Vector3(s, s));
    }

    public static void setScale(this Transform tf, float x, float y)
    {
        tf.setScale(new Vector3(x, y));
    }

    public static void setScale(this Transform tf, Vector3 scale)
    {
        tf.localScale = scale;
    }

    public static void setScaleX(this Transform tf, float scaleX)
    {
        Vector3 scale = tf.getScale();
        tf.localScale = new Vector3(scaleX, scale.y, scale.z);
    }

    public static void setScaleY(this Transform tf, float scaleY)
    {
        Vector3 scale = tf.getScale();
        tf.localScale = new Vector3(scale.x, scaleY, scale.z);
    }

    //setRotation
    public static void setRotation(this Transform tf, float rotation)
    {
        tf.localRotation = Quaternion.Euler(0, 0, rotation);
    }

    public static void setRadian(this Transform tf, float radian)
    {
        tf.localRotation = Quaternion.Euler(0, 0, radian.toAngle());
    }
}

