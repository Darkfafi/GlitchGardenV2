using System;
using UnityEngine;

[Serializable]
public class ElementUnitSpot : IDisposable
{
	public event Action<ElementUnitSpot> UnitChangedEvent;
	public event Action<ElementUnitSpot> PreviewChangedEvent;

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
	public Unit Unit
	{
		get; private set;
	}

	[field: SerializeField]
	public UnitConfig Preview
	{
		get; private set;
	}

	public ElementUnitSpot(GameGridElement element, Transform unitSpotLocation, CoreData spotData)
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

	public void SetUnit(Unit unit)
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
