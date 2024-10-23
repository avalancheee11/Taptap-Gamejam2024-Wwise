using System;
using UnityEngine;

public static class GameObjectSetter
{
    //setPosition
    public static void setPosition(this MonoBehaviour behaviour, float x, float y)
    {
        behaviour.gameObject.setPosition(x, y);
    }

    public static void setPosition(this GameObject go, float x, float y)
    {
        go.setPosition(new Vector3(x, y));
    }
    public static void setPosition(this MonoBehaviour behaviour, Vector3 position)
    {
        behaviour.gameObject.setPosition(position);
    }

    public static void setPosition(this GameObject go, Vector3 position)
    {
        go.transform.localPosition = position;
    }

    //setScale
    public static void setScale(this MonoBehaviour behaviour, float s)
    {
        behaviour.gameObject.setScale(s);
    }

    public static void setScale(this GameObject go, float s)
    {
        go.setScale(new Vector3(s, s));
    }

    public static void setScale(this MonoBehaviour behaviour, float x, float y)
    {
        behaviour.gameObject.setScale(x, y);
    }

    public static void setScale(this GameObject go, float x, float y)
    {
        go.setScale(new Vector3(x, y));
    }

    public static void setScale(this MonoBehaviour behaviour, Vector3 scale)
    {
        behaviour.gameObject.setScale(scale);
    }

    public static void setScale(this GameObject go, Vector3 scale)
    {
        go.transform.localScale = scale;
    }

    public static void setScaleX(this MonoBehaviour behaviour, float scaleX)
    {
        behaviour.gameObject.setScaleX(scaleX);
    }

    public static void setScaleX(this GameObject go, float scaleX)
    {
        Vector3 scale = go.transform.localScale;
        go.transform.localScale = new Vector3(scaleX, scale.y, scale.z);
    }

    public static void setScaleY(this MonoBehaviour behaviour, float scaleY)
    {
        behaviour.gameObject.setScaleY(scaleY);
    }

    public static void setScaleY(this GameObject go, float scaleY)
    {
        Vector3 scale = go.transform.localScale;
        go.transform.localScale = new Vector3(scale.x, scaleY, scale.z);
    }

    //setRotation
    public static void setRotation(this MonoBehaviour behaviour, float rotation)
    {
        behaviour.gameObject.setRotation(rotation);
    }

    public static void setRotation(this GameObject go, float rotation)
    {
        go.transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }

    //setRadian
    public static void setRadian(this MonoBehaviour behaviour, float radian)
    {
        behaviour.gameObject.setRadian(radian);
    }

    public static void setRadian(this GameObject go, float radian)
    {
        go.transform.localRotation = Quaternion.Euler(0, 0, radian.toAngle());
    }

    public static void setActiveByCheck(this GameObject go, bool isActive)
    {
        if (go.activeSelf == isActive) {
            return;
        }
        
        go.SetActive(isActive);
    }
    
    public static void playAni(this Animator animator, string aniName, string path = "animator")
    {
        if (aniName.IsNullOrEmpty()) {
            return;
        }
        if (animator.runtimeAnimatorController != null) {
            var aniCtrl = animator.runtimeAnimatorController;
            if (aniCtrl.name == aniName) {
                return;
            }
        }

        //TODO:push
        var preAni = animator.runtimeAnimatorController;
        //TODO:pop
        var nextAni = DataUtils.Instance.getAnimator(aniName);
        if (nextAni == null) {
            Debug.LogError("没有找到对应的动画：" + aniName);
            return;
        }
        animator.runtimeAnimatorController = nextAni;
    }
    
    public static float playAni(this Animation animation, string aniName, bool replay = true)
    {
        if (aniName.IsNullOrEmpty()) {
            return 0;
        }

        if (!replay && animation.clip != null && animation.clip.name == aniName) {
            return 0;
        }
        animation.clip = null;

        if (!animation.Play(aniName)) {
            return 0;
        }

        return animation[aniName].length;
    }
}