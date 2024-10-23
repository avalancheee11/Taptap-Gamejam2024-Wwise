using System.Collections;
using System.Collections.Generic;
using System.IO;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

public partial class EducationNode
{
    public static EducationNode Instance;
    public EducationData mainData { get; private set; }
    public Camera mainCamera { get; private set; }

    
    public EducationNode()
    {
        EducationNode.Instance = this;
        this.mainData = new EducationData(this);
    }

    public void start()
    {
        this.mainCamera = Camera.main;
        var cacheManager = CacheManager.Instance;
        this.mainData.start();
        XHFrameWork.UIManager.Instance.OpenUI<ProductRoomWindow>();
    }

    public void update(float dt)
    {
        this.mainData.update(dt);
    }

    public void stop()
    {
        CacheManager.Instance.clear();
        EducationNode.Instance = null;
    }
}
