
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSlicing : MonoBehaviour
{
	[Header("玩家")]
	public Transform _player;
	[Header("生成地形的父对象")]
	public Transform _terrParent;
	[Header("生成地形的大小(单个预制体 长/宽)")]
	public Vector2 _terrainSize=new Vector2(10,10);
	[Header("固定场景中的已有地形的位置")]
	public StructData[] datas;
	[Header("超出已设置的地形范围后，随机生成的地形")]
	public List<GameObject> _terrainObjs;
	[Header("离开地形区域后地形自动隐藏的时间")]
	public float _timer = 3f;

	private Vector2 terrainSize;
	private Dictionary<Vector2, GameObject> _terrainLoadedFixed;
	private Dictionary<(int x, int y), GameObject> _terrainLoaded;
	private Dictionary<(int x, int y), GameObject> _dictTemp;
	private Dictionary<(int x, int y), GameobjAndCoroutine> _unloadTerrCountDown;
	private Stack<GameObject> _terrainPool;
	private List<GameObject> _terrainLoadObjs;
	private (int x, int y) _lastPos = (0, 0);

	struct GameobjAndCoroutine
	{
		public GameObject Go;
		public Coroutine Cor;
	}

	[System.Serializable]
	public struct StructData
	{
		public Vector2 key;
		public GameObject value;
	}

	private void Awake()
	{
		terrainSize = new Vector2(_terrainSize.x*10, _terrainSize.y*10);
		_terrainLoadedFixed = new Dictionary<Vector2, GameObject>();
		_terrainLoaded = new Dictionary<(int x, int y), GameObject>();
		_dictTemp = new Dictionary<(int x, int y), GameObject>();
		_unloadTerrCountDown = new Dictionary<(int x, int y), GameobjAndCoroutine>();
		_terrainPool = new Stack<GameObject>();
		_terrainLoadObjs = new List<GameObject>();
		InitData();
	}

	private void Start()
	{
		FirstLoadTerrain();
	}

	private void FixedUpdate()
	{
		LoadTerrain();
	}


	void InitData()
	{
		for (int i = 0; i < datas.Length; i++)
		{
			if (_terrainLoadedFixed.ContainsKey(datas[i].key))
				return;
			_terrainLoadedFixed.Add(datas[i].key, datas[i].value);
		}
	}


	private void FirstLoadTerrain()
	{
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				if (_terrainLoaded.TryGetValue((i, j), out GameObject terr))
				{
					_dictTemp.Add((i, j), terr);
					_terrainLoaded.Remove((i, j));
					terr.transform.position = new Vector3(i * terrainSize.x, 0f, j * terrainSize.y);
					terr.SetActive(true);

				}
				else
				{
					if (_unloadTerrCountDown.TryGetValue((i, j), out GameobjAndCoroutine val))
					{
						StopCoroutine(val.Cor);
						_dictTemp.Add((i, j), val.Go);
						_unloadTerrCountDown.Remove((i, j));
						val.Go.transform.position = new Vector3(i * terrainSize.x, 0f, j * terrainSize.y);
						val.Go.SetActive(true);
					}
					else
					{
						//var newTerr = GetTerrain();
						var newTerr = GetTerrainNew(i, j);
						_dictTemp.Add((i, j), newTerr);
						newTerr.transform.position = new Vector3(i * terrainSize.x, 0f, j * terrainSize.y);
						newTerr.SetActive(true);
					}
				}
			}
		}
		(_terrainLoaded, _dictTemp) = (_dictTemp, _terrainLoaded);
	}


	private void LoadTerrain()
    {
		if (_player != null)
		{
			(int x, int y) pos = (Mathf.RoundToInt(_player.position.x / terrainSize.x), Mathf.RoundToInt(_player.position.z / terrainSize.y));
			if (!(pos == _lastPos))
			{
				_lastPos = pos;
				_dictTemp.Clear();
				
				for (int i = pos.x - 1; i < pos.x + 2; i++)
				{
					for (int j = pos.y - 1; j < pos.y + 2; j++)
					{
						if (_terrainLoaded.TryGetValue((i, j), out GameObject terr))
						{
							_dictTemp.Add((i, j), terr);
							_terrainLoaded.Remove((i, j));
							terr.transform.position = new Vector3(i * terrainSize.x, 0f, j * terrainSize.y);
							terr.SetActive(true);
						}
						else
						{
							if (_unloadTerrCountDown.TryGetValue((i, j), out GameobjAndCoroutine val))
							{
								StopCoroutine(val.Cor);
								_dictTemp.Add((i, j), val.Go);
								_unloadTerrCountDown.Remove((i, j));
								val.Go.transform.position = new Vector3(i * terrainSize.x, 0f, j * terrainSize.y);
								val.Go.SetActive(true);
							}
							else
							{
								var newTerr = GetTerrainNew(i, j);
								_dictTemp.Add((i, j), newTerr);
								newTerr.transform.position = new Vector3(i * terrainSize.x, 0f, j * terrainSize.y);
								newTerr.SetActive(true);
							}
						}
					}
				}

				foreach (var item in _terrainLoaded)
				{
					_unloadTerrCountDown.Add(item.Key, new GameobjAndCoroutine
					{
						Cor = StartCoroutine(RemoveTerrDelay(item.Key)),
						Go = item.Value,
					});
				}
				_terrainLoaded.Clear();
				(_terrainLoaded, _dictTemp) = (_dictTemp, _terrainLoaded);
			}
		}
	}
	

	private IEnumerator RemoveTerrDelay((int x, int y) pos)
	{
		yield return new WaitForSeconds(_timer);
		if (_unloadTerrCountDown.TryGetValue(pos, out var v))
		{
			RecycleTerrain(v.Go);
			_unloadTerrCountDown.Remove(pos);
		}
	}

	private void RecycleTerrain(GameObject t)
	{
		t.SetActive(false);
	}


	private GameObject GetTerrainNew(int x, int y)
	{
		if (_terrainLoadedFixed.TryGetValue(new Vector2(x,y), out GameObject terr))
		{
			if (!_terrainLoadObjs.Contains(terr))
			{
				_terrainLoadObjs.Add(terr);
				return Instantiate(terr, _terrParent); ;
			}
			return Instantiate(terr, _terrParent); ;
		}

		int randomTer = Random.Range(0, _terrainObjs.Count);
		return Instantiate(_terrainObjs[randomTer], _terrParent);
	}
}

