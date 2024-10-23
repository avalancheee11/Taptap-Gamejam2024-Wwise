#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(CustomContentSizeFitter), true)]
    [CanEditMultipleObjects]
    public class CustomContentSizeFitterEditor : ContentSizeFitterEditor
    {
        SerializedProperty m_MaxWidth;
        SerializedProperty m_MaxHeight;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_MaxWidth = serializedObject.FindProperty("m_maxWidth");
            m_MaxHeight = serializedObject.FindProperty("m_maxHeight");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_MaxWidth, true);
            EditorGUILayout.PropertyField(m_MaxHeight, true);
            serializedObject.ApplyModifiedProperties();

        }
    }
}
#endif