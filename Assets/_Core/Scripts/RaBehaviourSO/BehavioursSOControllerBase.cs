using RaBehaviourSO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class BehavioursSOControllerBase<T> : MonoBehaviour
	where T : RaBehaviourSOBase
{
	[SerializeField]
	private T[] _behaviours = null;

	[SerializeField]
	private InitType _initializationType = InitType.OnAwake;

	private RaBehaviourSOController<T> _controller = null;

#if UNITY_EDITOR
	protected void OnValidate()
	{
		string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
		T[] behaviours = new T[guids.Length];
		for(int i = 0; i < guids.Length; i++)
		{
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			behaviours[i] = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
		}
		_behaviours = behaviours;
	}
#endif

	public void Initialize()
	{
		if(_controller == null)
		{
			_controller = new RaBehaviourSOController<T>(_behaviours, OnInit, OnDeinit);
		}
		_controller.Register(this);
	}

	public void Deinitialize()
	{
		if(_controller != null)
		{
			_controller.Unregister(this);
		}
	}

	protected void Awake()
	{
		if(_initializationType == InitType.OnAwake)
		{
			Initialize();
		}
	}

	protected void Start()
	{
		if(_initializationType == InitType.OnStart)
		{
			Initialize();
		}
	}

	protected void OnDestroy()
	{
		Deinitialize();
	}

	protected abstract void OnInit(T behaviourSO);
	protected abstract void OnDeinit(T behaviourSO);

	public enum InitType
	{
		OnAwake,
		OnStart,
		Manually,
	}
}
