// using System;
// using System.Collections;
// using System.Collections.Generic;
// using DG.Tweening;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class GameObjectActionTag
// {
//     public const string Rotate = "rotate";
//     public const string Fade = "fade";
// }
//
// public static class GameObjectAction
// {
//     public static Sequence createSequence(this GameObject obj, bool setUpdate = false)
//     {
//         Sequence sequence = DOTween.Sequence();
//         sequence.SetTarget(obj); 
//         sequence.SetRecyclable();
//         sequence.SetUpdate(setUpdate);
//         return sequence;
//     }
//     
//     public static Tweener moveTo(this GameObject gobject, Vector3 endValue, float duration, bool setUpdate = false)
//         {
//             return DOTween.To(() => gobject.transform.position, x => gobject.transform.position = x, endValue, duration)
//                 .SetUpdate(setUpdate)
//                 .SetRecyclable()
//                 .SetTarget(gobject)
//                 .SetUpdate(UpdateType.Late);
//         }
//
//     public static Tweener moveLocalPosTo(this GameObject gobject, Vector3 endValue, float duration, bool setUpdate = false)
//     {
//         return DOTween.To(() => gobject.transform.localPosition, x => gobject.transform.localPosition = x, endValue, duration)
//             .SetUpdate(setUpdate)
//             .SetRecyclable()
//             .SetTarget(gobject)
//             .SetUpdate(UpdateType.Late);
//     }
//     
//     public static Tweener moveLocalPosXTo(this GameObject gobject, float endValue, float duration, bool setUpdate = false)
//     {
//         return DOTween.To(() => gobject.transform.localPosition.x, x => gobject.transform.localPosition = new Vector3(x, gobject.transform.localPosition.y, gobject.transform.localPosition.z), endValue, duration)
//             .SetUpdate(setUpdate)
//             .SetRecyclable()
//             .SetTarget(gobject)
//             .SetUpdate(UpdateType.Late);
//     }
//     
//     public static Tweener moveAnchoredPositionTo(this RectTransform gobject, Vector2 endValue, float duration, bool setUpdate = false)
//     {
//         return DOTween.To(() => gobject.anchoredPosition, x => gobject.anchoredPosition = x, endValue, duration)
//             .SetUpdate(setUpdate)
//             .SetRecyclable()
//             .SetTarget(gobject)
//             .SetUpdate(UpdateType.Late);
//     }
//
//     public static Tweener scaleTo(this GameObject gobject, Vector3 endValue, float duration, bool setUpdate = false)
//         {
//             return DOTween.To(() => gobject.transform.localScale, x => gobject.transform.localScale = x, endValue, duration)
//                 .SetUpdate(setUpdate)
//                 .SetRecyclable()
//                 .SetTarget(gobject)
//                 .SetUpdate(UpdateType.Late);
//         }
//
//     public static Tweener rotateTo(this GameObject gobject, Vector3 endValue, float duration, bool setUpdate = false)
//     {
//         return DOTween.To(() => gobject.transform.localEulerAngles, x => gobject.transform.localEulerAngles = x, endValue, duration)
//             .SetUpdate(setUpdate)
//             .SetRecyclable()
//             .SetTarget(gobject)
//             .SetUpdate(UpdateType.Late);
//     }
//     
//     public static Tweener rotateZTo(this GameObject gobject, float endValue, float duration, bool setUpdate = false)
//     {
//         if (gobject == null) {
//             return null;
//         }
//         return DOTween.To(() => gobject.transform.localEulerAngles.x, x => gobject.transform.localEulerAngles = new Vector3(gobject.transform.localEulerAngles.x, gobject.transform.localEulerAngles.y, x), endValue, duration)
//             .SetUpdate(setUpdate)
//             //.SetRecyclable()
//             .SetTarget(gobject)
//             .SetUpdate(UpdateType.Late);
//     }
//
//     public static Tweener barValueTo(this Image gobject, float endValue, float duration, bool setUpdate = false)
//     {
//         return DOTween.To(() => gobject.fillAmount, x => gobject.fillAmount = x, endValue, duration)
//             .SetUpdate(setUpdate)
//             //.SetRecyclable()
//             .SetTarget(gobject)
//             .SetUpdate(UpdateType.Late);
//     }
//
//     public static Tweener fadeTo(this CanvasGroup gobject, float endValue, float duration, bool setUpdate = false)
//     {
//         return DOTween.To(() => gobject.alpha, x => gobject.alpha = x, endValue, duration)
//             .SetUpdate(setUpdate)
//             .SetRecyclable()
//             .SetTarget(gobject)
//             .SetUpdate(UpdateType.Late);
//     }
//     
//     public static Tweener fadeTo(this Text gobject, Color endValue, float duration, bool setUpdate = false)
//     {
//         return DOTween.To(() => gobject.color, x => gobject.color = x, endValue, duration)
//             .SetUpdate(setUpdate)
//             .SetRecyclable()
//             .SetTarget(gobject)
//             .SetUpdate(UpdateType.Late);
//     }
//     
//     public static Sequence runDelay(this GameObject gobject, float duration, Action callBack,bool setUpdate = false)
//     {
//         var seq = gobject.createSequence().SetUpdate(setUpdate);
//         seq.AppendInterval(duration);
//         seq.OnComplete(() => callBack?.Invoke());
//         return seq;
//     } 
//     
//     public static void stopAllActions(this GameObject gobject, bool complete = false)
//     {
//         DOTween.Kill(gobject, complete);
//     }
//     
//     public static void stopAllActions(this object gobject, bool complete = false)
//     {
//         DOTween.Kill(gobject, complete);
//     }
//
//
//     public static void stopActionByTag<T>(this GameObject gobject, T tag, bool complete = false)
//     {
//         var list = DOTween.TweensByTarget(gobject);
//         if (list == null) {
//             return;
//         }
//         foreach (var t in list) {
//             if (t == null || t.id == null || !t.id.Equals(tag)) {
//                 continue;
//             }
//             if (t.IsActive()) {
//                 t.Kill(complete);
//             }
//         }
//     }
// }
