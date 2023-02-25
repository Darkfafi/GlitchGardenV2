using UnityEngine;
using System;

[Serializable]
public class PlayerModel
{
	[field: SerializeField]
	public UnitConfig[] Units
	{
		get; private set;
	}
}
