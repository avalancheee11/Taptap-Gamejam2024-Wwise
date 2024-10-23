/*DontDestroyOnLoad的单例类，协程控制器继承自该单例*/
/*因为单例Instance是静态的，所以只有一个，避免出现多个协程控制器进行操作*/
/*Instance的作用：初始化Instance后，可在其他脚本中直接通过声明Instance变量所在类的名称来访问Instance对象，
   而不需要实例化该类。本案中CoroutineController继承了DDOLSingleton的Instance，因此可直接使用
   CoroutineController.Instance来操作使用CoroutineController中的公共属性或方法。*/

using UnityEngine;
using System.Collections;

public abstract class DDOLSingleton<T> : MonoBehaviour where T : DDOLSingleton<T>
{
	protected static T _Instance = null;
	
	public static T Instance
	{
		get{
			if (null == _Instance)
			{
				GameObject go = GameObject.Find("DDOLGameObject");
				if (null == go)
				{
					go = new GameObject("DDOLGameObject");
					//切换场景时go对象不被释放
                    DontDestroyOnLoad(go);
				}
                //给go对象增加一个T脚本
				_Instance = go.AddComponent<T>();

			}
			return _Instance;
		}
	}

    //程序退出时重置_Instance为空
	private void OnApplicationQuit ()
	{
		_Instance = null;
	}
}

