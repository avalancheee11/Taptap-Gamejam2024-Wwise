using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomContentSizeFitter : ContentSizeFitter
{
    [SerializeField] private float m_maxWidth = 500;
    [SerializeField] private float m_maxHeight = 500;
    
    public override void SetLayoutHorizontal()
    {
        base.SetLayoutHorizontal();
        if (this.horizontalFit == FitMode.PreferredSize) {
            var rectTr = this.GetComponent<RectTransform>();
            rectTr.SetSizeWithCurrentAnchors((RectTransform.Axis)0, Mathf.Min(LayoutUtility.GetPreferredSize(rectTr, 0), this.m_maxWidth));
        }
    }

    public override void SetLayoutVertical()
    {
        base.SetLayoutVertical();
        if (this.verticalFit == FitMode.PreferredSize) {
            var rectTr = this.GetComponent<RectTransform>();
            rectTr.SetSizeWithCurrentAnchors((RectTransform.Axis)1, Mathf.Min(LayoutUtility.GetPreferredSize(rectTr, 1), this.m_maxHeight));
        }
    }
}