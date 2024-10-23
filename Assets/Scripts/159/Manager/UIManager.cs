//UI管理器，单例类型

using System;
using System.Collections.Generic;
using UnityEngine;

namespace XHFrameWork
{

	public enum UICanvasType
	{
		None,
		Main,
		System,
		Popup,
		Debug,
		Tip,
	}

	public class UIManager : Singleton<UIManager>
	{
		public event Action<BaseUI> onOpenUIAction;
		public event Action<BaseUI> onCloseUIAction;
		public List<BaseUI> uiList { get; private set; }
		private List<BaseUI> tempUIList;
		private List<BaseUI> tempUIList2;

		//初始化
		public override void Init ()
		{
			this.uiList = new List<BaseUI>();
			this.tempUIList = new List<BaseUI>();
			this.tempUIList2 = new List<BaseUI>();
		}

		public void update(float dt)
		{
			this.tempUIList.Clear();
			this.tempUIList2.Clear();
			this.tempUIList2.AddRange(this.uiList);
			foreach (var ui in this.tempUIList2) {
				ui.update(dt);
				if (ui.deleteLater) {
					this.onCloseUIAction?.Invoke(ui);
					ui.Release();
					this.tempUIList.Add(ui);
				}
			}

			if (this.tempUIList.Count > 0) {
				foreach (var ui in this.tempUIList) {
					this.uiList.RemoveAll(x => x == ui);
				}
			}
		}

		public void clear()
		{
			foreach (var ui in this.uiList) {
				ui.Release();
			}
			this.uiList.Clear();
		}

		//使用UI类型来获取UI对象（约束T继承自BaseUI）
        public T GetUI<T>() where T : BaseUI
		{
			foreach (var ui in this.uiList) {
				if (ui is T) {
					return ui as T;
				}
			}

			return null;
		}

        public T OpenUI<T>(IUIData uiData = null) where T : BaseUI
		{
			var pui = this.uiList.Find(x => x.GetType() == typeof(T));
			if (pui != null && !pui.canRepeatOpen) {
				return pui as T;
			}
			string _path = UIManager.GetUIPathByType(typeof(T));
			var prefabObj = Resources.Load(_path);
			//转UI prefab资源为游戏UI对象
			var uiObject = MonoBehaviour.Instantiate(prefabObj, GameCtrl.instance.getCanvas(UICanvasType.None).transform) as GameObject;
			//取该UI对象的脚本
			var ui = uiObject.GetComponent<T>();
			if (ui == null)
			{
				ui = uiObject.AddComponent<T>() as T;
			}
			
			ui.transform.SetParent(GameCtrl.instance.getCanvas(ui.uiCanvasType).transform);
			ui.start(uiData);
			this.uiList.Add(ui);
			this.onOpenUIAction?.Invoke(ui);
			return ui;
		}

        public BaseUI OpenUIByType(string typeName, IUIData uiData = null)
        {
	        var uiType = Type.GetType(typeName);
	        var pui = this.uiList.Find(x => x.GetType() == uiType);
	        if (pui != null && !pui.canRepeatOpen) {
		        return pui;
	        }
	        string _path = UIManager.GetUIPathByType(uiType);
	        var prefabObj = Resources.Load(_path);
	        //转UI prefab资源为游戏UI对象
	        var uiObject = MonoBehaviour.Instantiate(prefabObj, GameCtrl.instance.getCanvas(UICanvasType.None).transform) as GameObject;
	        //取该UI对象的脚本
	        var ui = uiObject.GetComponent<BaseUI>();
	        if (ui == null)
	        {
		        ui = uiObject.AddComponent(uiType) as BaseUI;
	        }
	        
	        ui.transform.SetParent(GameCtrl.instance.getCanvas(ui.uiCanvasType).transform);
	        ui.start(uiData);
	        this.uiList.Add(ui);
	        this.onOpenUIAction?.Invoke(ui);
	        return ui;
        }
        
        private static Dictionary<Type, string> UIPathsByType = new Dictionary<Type, string>()
        {
	        
        };
        
        public static string GetUIPathByType(Type type)
        {
	        return UIPathsByType.objectValue(type, $"Prefabs/UI/{type.toString()}/{type.toString()}");
        }
	}
}

