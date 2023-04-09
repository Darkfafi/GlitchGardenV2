using RaCollection;
using System;
using UnityEngine;

public abstract class ConfigBase<T> : ConfigBase
{
	[SerializeField]
	private T _data = default;

	public T Data => _data;
}

public abstract class ConfigBase : ScriptableObject, IRaCollectionElement
{
	[field: SerializeField, HideInInspector]
	public string Id
	{
		get; private set;
	}

	protected void Awake()
	{
		SetInitialValues();
	}

#if UNITY_EDITOR
	protected void OnValidate()
	{
		SetInitialValues();
	}
#endif

	protected virtual void SetInitialValues()
	{
		if(string.IsNullOrEmpty(Id))
		{
			Id = Guid.NewGuid().ToString();
		}
	}
}