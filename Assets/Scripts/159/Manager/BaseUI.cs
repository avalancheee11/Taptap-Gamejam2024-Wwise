//BaseUI基本UI面板类，所有UI继承自这里（数据逻辑都继承自BaseModule，UI逻辑都继承自BaseUI）

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XHFrameWork
{
    public interface IUIData
    {
        
    }
    
    public abstract class BaseUI : MonoBehaviour
    {
        public virtual UICanvasType uiCanvasType => UICanvasType.Main;
        public virtual bool canRepeatOpen => true;
        public bool deleteLater { get; private set; }
        public bool isOver { get; private set; }
        protected virtual bool needStatusObject => false;
        private StatusObject _statusObject;
        protected int baseState
        {
            get => _statusObject.status;
            set
            {
                if (this._statusObject == null) {
                    return;
                }
                _statusObject.status = value;
            }
        }
        
        protected Dictionary<int, Action> statusActions => _statusObject.statusActions;
        protected Dictionary<int, Action<float>> updateActions => _statusObject.updateActions;
        protected Dictionary<int, Action> leaveActions => _statusObject.leaveActions;
        
        public event Action onDestroyAction;
        
        //弹窗界面所用的主节点，用于确定显示位置和播放弹出动画（从prefab挂入）
        public Transform UIbox;
        public EducationNode mainNode => EducationNode.Instance;
        public EducationData mainData => this.mainNode.mainData;
        public string key { get; private set; }
        public CanvasGroup canvasGroup { get; private set; }
        private HashSet<RectTransform> gamePadNavigationRtList;
        protected IUIData uiData;
        public virtual void start(IUIData uiData)
        {
            this.uiData = uiData;
            foreach (var obj in  this.gameObject.transform.GetComponentsInChildren<CustomUIComponent>(true)) {
                if (obj == null) {
                    continue;
                }
                if (obj.unused) {
                    //这里会连带子节点下不需要的Node一起销毁
                    GameObject.DestroyImmediate(obj.gameObject);
                }
            }
            foreach (var obj in  this.gameObject.transform.GetComponentsInChildren<CustomUIComponent>(true)) {
                obj.startComponent();
            }
            this.gamePadNavigationRtList = new HashSet<RectTransform>();
            this.key = this.GetType().toString() + int.MaxValue.toCCRandomIndex().toString();
            if (this.needStatusObject) {
                this._statusObject = new StatusObject();
            }
            
            this.canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
            if (this.canvasGroup  == null) {
                this.canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
            }
            
            this.OnPlayOpenUIAudio();
        }

        public virtual void update(float dt)
        {
            if (this.needStatusObject) {
                this._statusObject.update(dt);
            }
            OnUpdate(dt);

        }
        
        //关闭UI并释放
        public void Release()
        {
            this.isOver = true;
            OnRelease();
            this.stop();
            GameObject.Destroy(this.gameObject);
        }
        
        //真Update-Update时伴随的行为加在这里
        protected virtual void OnUpdate(float deltaTime)
        {

        }

        //释放时伴随的行为加在这里
        protected virtual void OnRelease()
        {
            this.OnPlayCloseUIAudio();
        }

        protected virtual void stop()
        {
            if (this.needStatusObject) {
                this._statusObject.clearStatus();
                this._statusObject.clearAction();
                this._statusObject = null;
            }

            if (this.canvasGroup != null) {
                GameObject.Destroy(this.canvasGroup);
                this.canvasGroup = null;
            }
            
            var uiComponents = this.gameObject.transform.GetComponentsInChildren<CustomUIComponent>(true);
            foreach (var obj in uiComponents) {
                obj.stopComponent();
            }
            
            this.onDestroyAction?.Invoke();
            this.onDestroyAction = null;
        }

        //播放打开界面音乐
        protected virtual void OnPlayOpenUIAudio()
        {
            
        }

        // 播放关闭界面音乐
        protected virtual void OnPlayCloseUIAudio()
        {
            //AudioManager.Instance.PlaySound(EnumSoundType.BadMsg);
        }

        public void closeUI()
        {
            this.deleteLater = true;
        }

        public void addGamePadNavigation(RectTransform rt)
        {
            this.gamePadNavigationRtList.Add(rt);
        }

        public void removeGamePadNavigation(RectTransform rt)
        {
            this.gamePadNavigationRtList.Add(rt);
        }
    }
}