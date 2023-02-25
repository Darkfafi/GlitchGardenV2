using RaDataHolder;
using System;
using UnityEngine;

public class Unit : RaMonoDataHolderBase<Unit.CoreData>
{
	[SerializeField]
	private SpriteRenderer _renderer = null;

	public CoreData UnitData => Data;

	public Player Owner
	{
		get; private set;
	}

	public Health Health
	{
		get; private set;
	}

	public Vector2Int Position
	{
		get; private set;
	}

	protected override void OnSetData()
	{
		Owner = Data.Owner;

		_renderer.sprite = Data.Config.Icon;
		
		Health = new Health(Data.Config.HealthPoints);
	}

	protected override void OnClearData()
	{

	}

	public void SetPosition(Vector2Int position)
	{
		Position = position;
	}

	[Serializable]
	public struct CoreData
	{
		public Player Owner;
		public UnitConfig Config;
	}
}
