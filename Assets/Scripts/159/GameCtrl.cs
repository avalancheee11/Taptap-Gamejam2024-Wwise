using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XHFrameWork;

public class GameCtrlStatus
{
    public const int None = 0;
    public const int Loading = 1;
    public const int Stay = 2;
    public const int Over = 3;
}

public class GameCtrl : MonoBehaviour
{
    public static GameCtrl instance;
    private EducationNode educationNode;
    private StatusObject _statusObject;
    [Header("UI层级")] 
    [SerializeField] public Camera uiCamera;
    [SerializeField] private Canvas noneCanvas;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas systemCanvas;
    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private Canvas debugCanvas;
    [SerializeField] private Canvas tipCanvas;

    private Dictionary<UICanvasType, Canvas> canvasMap;
    public Canvas getCanvas(UICanvasType type) => this.canvasMap[type];

    public IEnumerable<Canvas> getAllCanvas()
    {
        foreach (var kvp in this.canvasMap) {
            yield return kvp.Value;
        }
    }
    
    private int baseState
    {
        get => this._statusObject.status;
        set => this._statusObject.status = value;
    }
    protected Dictionary<int, Action> statusActions => _statusObject.statusActions;
    protected Dictionary<int, Action<float>> updateActions => _statusObject.updateActions;
    protected Dictionary<int, Action> leaveActions => _statusObject.leaveActions;
    private bool isStepComplete;

    private List<BasesLoadingObject> _loadingObjectList;
    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
#if ENABLE_TEST
        DOTween.debugMode = true;
#endif
        this.canvasMap = new Dictionary<UICanvasType, Canvas>();
        this.canvasMap[UICanvasType.None] = this.noneCanvas;
        this.canvasMap[UICanvasType.Main] = this.mainCanvas;
        this.canvasMap[UICanvasType.Popup] = this.popupCanvas;
        this.canvasMap[UICanvasType.System] = this.systemCanvas;
        this.canvasMap[UICanvasType.Debug] = this.debugCanvas;
        this.canvasMap[UICanvasType.Tip] = this.tipCanvas;
        foreach (var kvp in this.canvasMap) {
            kvp.Value.sortingOrder = (int) kvp.Key * 10;
        }
        
        this.educationNode = new EducationNode();
        CultureInfo.CurrentCulture = new CultureInfo("en");

        Application.runInBackground = true;
        instance = this;
        this._loadingObjectList = new List<BasesLoadingObject>();
        this._statusObject = new StatusObject();
        this._statusObject.ignoreSameState = true;
        this.statusActions[GameCtrlStatus.Loading] = this.runLoading;
        this.updateActions[GameCtrlStatus.Loading] = this.updateLoading;
        this.leaveActions[GameCtrlStatus.Loading] = this.leaveLoading;
        
        this.statusActions[GameCtrlStatus.Stay] = this.runStay;
        this.updateActions[GameCtrlStatus.Stay] = this.updateStay;
        this.leaveActions[GameCtrlStatus.Stay] = this.leaveStay;

        // var maxLevel = 100;
        // for (int level = 1; level <= maxLevel; level++) {
        //     var baseAtk = 2f * level;
        //     var baseHp = 6f;
        //     for (int i = 1; i < level; i++) {
        //         var v1 = Mathf.Clamp(Mathf.Log10(level), 0, 2);
        //         var v2 = 1 - v1 / 2f;
        //         var rate = Mathf.Clamp(v2, 0.08f, 0.6f);
        //         baseHp += Mathf.Max(128, baseHp * rate);
        //     }
        //     Debug.LogError($"level:{level}, atk:{baseAtk}, hp:{baseHp}");
        // }ss

        // HttpManager.Instance.PostRequest("s", (l, s) =>
        // {
        //     Debug.LogError("Bugly测试");
        //     BuglyAgent.ReportException("test", $"{l}_{s}", "test");
        // }, null, null);
    }

    private void Start()
    {
        Resources.UnloadUnusedAssets();

        this.baseState = 1;
    }
    
    private void OnDestroy()
    {
        instance = null;
    }

    void Update()
    {
        this._statusObject.update(Time.deltaTime);
        if (this.isStepComplete) {
            this.isStepComplete = false;
            this.baseState += 1;
        }

        XHFrameWork.UIManager.Instance.update(Time.deltaTime);
        AudioManager.Instance.update(Time.deltaTime);
    }

    void stepComplete()
    {
        this.isStepComplete = true;
    }

    private float totalLoadingCount;
    private float loadingDelay;
    
    void runLoading()
    {
        //创建要进行的步骤
        foreach (var className in CustomGlobalConfig.LoadingObjectSeq) {
            var o = DataUtils.Instance.getActivator<BasesLoadingObject>(className);
            o.start();
            this._loadingObjectList.Add(o);
        }

        this.loadingDelay = 1f;
        this.totalLoadingCount = this._loadingObjectList.Count;
    }

    void updateLoading(float dt)
    {
        if (this._loadingObjectList.Count == 0) {
            this.loadingDelay -= dt;
            if (this.loadingDelay <= 0) {
                this.stepComplete();
            }
            return;
        }
        
        var o =this._loadingObjectList[0];
        o.update(dt);
        if (o.isComplete) {
            o.stop();
            this._loadingObjectList.RemoveAt(0);
        }
    }

    void leaveLoading()
    {
        foreach (var o in this._loadingObjectList) {
            o.stop();
        }

        this._loadingObjectList.Clear();
    }

    void runStay()
    {
        Debug.Log("runStay");
        this.refreshCanvasScaler();
        this.educationNode.start();
    }

    void updateStay(float dt)
    {
        this.educationNode.update(dt);
    }

    void leaveStay()
    {
        this.educationNode.stop();
    }
    
    public void refreshCanvasScaler()
    {
        var width = Screen.width;
        var height = Screen.height;
        // var rate = height / (float)width;
        // var resolution = this.canvasScaler.referenceResolution;
        // this.canvasScaler.referenceResolution = new Vector2(resolution.x, Mathf.Max(2, rate) * resolution.x);
        foreach (var kvp in this.canvasMap) {
            var canvasScaler = kvp.Value.GetComponent<CanvasScaler>();
            var resolution = canvasScaler.referenceResolution;
            canvasScaler.referenceResolution = new Vector2(resolution.y * Mathf.Max(1920/1080f, width/ (float)height), resolution.y);
        }
    }
}
