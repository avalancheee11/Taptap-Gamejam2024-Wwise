using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Image))]
    public class ImageAnim : MonoBehaviour
    {
        [System.Serializable]
        public class Data
        {
            public string name;
            public UnityEngine.Rendering.ShaderPropertyType type;
        }

        public List<Data> Property = new List<Data>();

        private Image m_Image;
        [SerializeField] private Material m_ImageMat;
        private MeshRenderer m_MeshRenderer;
        private MaterialPropertyBlock m_MaterialPropertyBlock;

        private void Awake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_MaterialPropertyBlock = new MaterialPropertyBlock();
            m_Image = GetComponent<Image>();
            m_ImageMat = Instantiate(m_Image.material);
            m_Image.material = m_ImageMat;
        }

        private void LateUpdate()
        {
            if (m_MeshRenderer.HasPropertyBlock()) {
                m_MeshRenderer.GetPropertyBlock(m_MaterialPropertyBlock);
                foreach (var item in Property) {
                    SetValue(item.name, item.type);
                }
            }
        }

        void SetValue(string name, UnityEngine.Rendering.ShaderPropertyType type)
        {
            switch (type) {
                case UnityEngine.Rendering.ShaderPropertyType.Color:
                    m_Image.color = m_MaterialPropertyBlock.GetColor(name);
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Range:
                case UnityEngine.Rendering.ShaderPropertyType.Float:
                    m_Image.materialForRendering.SetFloat(name, m_MaterialPropertyBlock.GetFloat(name));
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Vector:
                    m_Image.materialForRendering.SetVector(name, m_MaterialPropertyBlock.GetVector(name));
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Texture:
                    m_Image.materialForRendering.SetVector($"{name}_ST", m_MaterialPropertyBlock.GetVector($"{name}_ST"));
                    break;
                default:
                    break;
            }

        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ImageAnim))]
    public class MyTestEditor : Editor
    {
        ImageAnim myTest;
        MeshRenderer meshrenderer;

        private void OnEnable()
        {
            myTest = (target as ImageAnim);
            meshrenderer = myTest.GetComponent<MeshRenderer>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("refresh")) {
                myTest.Property = new List<ImageAnim.Data>();
                Shader shader = meshrenderer.sharedMaterial.shader;
                int count = shader.GetPropertyCount();
                for (int i = 0; i < count; i++) {
                    myTest.Property.Add(new ImageAnim.Data() {name = shader.GetPropertyName(i), type = shader.GetPropertyType(i)});
                }
            }
        }
    }
#endif
