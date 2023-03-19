using System;
using UnityEngine;

namespace Game.Battle
{
	[Serializable]
	public class ElementBattleUnitSpot : IDisposable
	{
		public event Action<ElementBattleUnitSpot> UnitChangedEvent;
		public event Action<ElementBattleUnitSpot> PreviewChangedEvent;

		public GameGridElement Element
		{
			get; private set;
		}

		// Readonly
		[field: SerializeField]
		public Transform UnitSpotLocation
		{
			get; private set;
		}

		[field: SerializeField]
		public CoreData SpotData
		{
			get; private set;
		}

		[field: SerializeField]
		public BattleUnit Unit
		{
			get; private set;
		}

		[field: SerializeField]
		public UnitConfig Preview
		{
			get; private set;
		}

		public ElementBattleUnitSpot(GameGridElement element, Transform unitSpotLocation, CoreData spotData)
		{
			Element = element;
			SpotData = spotData;
			UnitSpotLocation = unitSpotLocation;
		}

		public Vector3 GetUnitLocation() => UnitSpotLocation.position;

		public void SetPreview(UnitConfig config)
		{
			if(Preview != config)
			{
				Preview = config;
				PreviewChangedEvent?.Invoke(this);
			}
		}

		public void SetUnit(BattleUnit unit)
		{
			if(Unit != unit)
			{
				Unit = unit;
				UnitChangedEvent?.Invoke(this);
			}
		}

		[Serializable]
		public struct CoreData
		{
			public bool IsBuildable;
		}

		public void Dispose()
		{
			Unit = null;
			SpotData = default;
			Element = default;

			PreviewChangedEvent = null;
			UnitChangedEvent = null;
		}
	}
}