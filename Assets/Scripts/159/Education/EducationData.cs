using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EducationData
{
    public ItemDataRoot itemDataRoot => ItemDataHandler.Instance.dataRoot;
    public ItemConfigRoot itemConfigRoot => ItemDataHandler.Instance.configRoot;

    public ItemContainerDataRoot itemContainerDataRoot => ItemContainerDataHandler.Instance.dataRoot;
    public ItemContainerConfigRoot itemContainerConfigRoot => ItemContainerDataHandler.Instance.configRoot;
    
    public ItemFormulaConfigRoot itemFormulaConfigRoot => ItemFormulaConfigHandler.Instance.configRoot;

    public EducationData(EducationNode educationNode)
    {
        this.mainNode = educationNode;
        Instance = this;
    }

    public static EducationData Instance;

    public EducationNode mainNode { get; private set; }
    
    public void start()
    {
        DataBaseManager.Instance.start();
    }

    public void update(float dt)
    {
        DataBaseManager.Instance.update(dt);
    }

    public void stop()
    {
        DataBaseManager.Instance.stop();
        Instance = null;
    }

    public void cacheModule(string module)
    {
        DataBaseManager.Instance.cacheModule(module);
    }
}
