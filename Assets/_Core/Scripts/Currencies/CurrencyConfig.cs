using UnityEngine;


[CreateAssetMenu(menuName = "Configs/CurrencyConfig", fileName = "CurrencyConfig")]
public class CurrencyConfig : ScriptableObject
{
	[SerializeField, HideInInspector]
	private string _id;

	[field: SerializeField]
	public Sprite Icon
	{
		get; private set;
	}

	public string Id => _id;

	protected void Awake()
	{
		InitializeId();
	}

#if UNITY_EDITOR
	protected void OnValidate()
	{
		InitializeId();
	}
#endif

	private void InitializeId()
	{
		if(string.IsNullOrEmpty(_id))
		{
			_id = System.Guid.NewGuid().ToString();
		}
	}
}
