using UnityEngine;

public abstract class ConfigBase<T> : ConfigBase
{
	[SerializeField]
	private T _data = default;

	public T Data => _data;
}

public abstract class ConfigBase : ScriptableObject
{

}