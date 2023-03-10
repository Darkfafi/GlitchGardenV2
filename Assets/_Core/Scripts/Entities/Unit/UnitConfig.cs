using UnityEngine;

[CreateAssetMenu(menuName = "Configs/UnitConfig", fileName = "UnitConfig")]
public class UnitConfig : ScriptableObject
{
	[field: SerializeField]
	public Unit UnitPrefab
	{
		get; private set;
	}

	[field: SerializeField]
	public int HealthPoints
	{
		get; private set;
	} = 10;

	[field: SerializeField]
	public Color Color
	{
		get; private set;
	} = Color.white;

	[field: SerializeField]
	public Sprite Icon
	{
		get; private set;
	}

	[field: SerializeField]
	public CurrencyValue Cost
	{
		get; private set;
	}

	[field: SerializeField]
	public bool FirstColumnUnit
	{
		get; private set;
	}
}
