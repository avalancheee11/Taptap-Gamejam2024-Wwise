using System;
using UnityEngine;
using UnityEngine.UI;

public static class GameObjectGetter
{
    //getPosition
    public static Vector3 getLocalPosition(this MonoBehaviour behaviour)
    {
        return behaviour.gameObject.getLocalPosition();
    }

    public static Vector3 getLocalPosition(this GameObject go)
    {
        return go.transform.localPosition;
    }

    public static Vector3 getWorldPosition(this GameObject go)
    {
        return go.transform.position;
    }
    
    //getScale
    public static Vector3 getScale(this MonoBehaviour behaviour)
    {
        return behaviour.gameObject.getScale();
    }

    public static Vector3 getScale(this GameObject go)
    {
        return go.transform.localScale;
    }

    public static float getScaleX(this MonoBehaviour behaviour)
    {
        return behaviour.gameObject.getScaleX();
    }

    public static float getScaleX(this GameObject go)
    {
        return go.transform.localScale.x;
    }

    public static float getScaleY(this MonoBehaviour behaviour)
    {
        return behaviour.gameObject.getScaleY();
    }

    public static float getScaleY(this GameObject go)
    {
        return go.transform.localScale.y;
    }

    //getRotation
    public static float getRotation(this MonoBehaviour behaviour)
    {
        return behaviour.gameObject.getRotation();
    }

    public static float getRotation(this GameObject go)
    {
        return go.transform.localRotation.FromQ2().z;
    }

    //getRadian
    public static float getRadian(this MonoBehaviour behaviour)
    {
        return behaviour.gameObject.getRadian();
    }

    public static float getRadian(this GameObject go)
    {
        return go.getRotation().toRadian();
    }
    
    public static Vector2 getTextSize(this Text text, string desc, int width = 512, int height = 0, 
        VerticalWrapMode verticalWrapMode = VerticalWrapMode.Overflow, HorizontalWrapMode horizontalWrapMode = HorizontalWrapMode.Wrap)
    {
        TextGenerationSettings generationSettings = new TextGenerationSettings
        {
            textAnchor = TextAnchor.UpperLeft,
            font = text.font,
            fontSize = text.fontSize,
            color = Color.black,
            scaleFactor = 1.0f,
            lineSpacing = text.lineSpacing,
            richText = false,
            fontStyle = text.fontStyle,
            resizeTextForBestFit = false,
            verticalOverflow = verticalWrapMode,
            horizontalOverflow = horizontalWrapMode,
            updateBounds = true,
            generationExtents = new Vector2(width, height) // 生成的文本可以占用的最大尺寸
        };

        TextGenerator textGenerator = new TextGenerator();
        textGenerator.Populate(desc, generationSettings);

        // 获取文本的实际尺寸
        Rect rect = textGenerator.rectExtents;
        return new Vector2(rect.width, rect.height);
    }
    
    public static string getFullPath(this Transform tr)
    {
        if (tr.parent == null) {
            return tr.name;
        }

        return $"{tr.parent.getFullPath()}/{tr.name}";
    }
    
    public static string getFullPath(this GameObject o)
    {
        return o.transform.getFullPath();
    }
}
