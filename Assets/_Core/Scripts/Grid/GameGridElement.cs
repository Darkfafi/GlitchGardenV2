using RaDataHolder;
using System;
using UnityEngine;

public class GameGridElement : RaMonoDataHolderBase<GameGridElement.CoreData>
{
	public string ID
	{
		get; private set;
	}

	[field: SerializeField]
	public Vector2Int Position
	{
		get; private set;
	}


	[field: SerializeField]
	public Transform UnitSpotLocation
	{
		get; private set;
	}

	[field: SerializeField]
	public ElementUnitSpot HomeUnitSpot
	{
		get; private set;
	}

	[field: SerializeField]
	public ElementUnitSpot AwayUnitSpot
	{
		get; private set;
	}

	[Header("Buildable")]
	[SerializeField]
	private GameObject _notBuildableContainer = null;
	[SerializeField]
	private GameObject _buildableContainer = null;

	[Header("Preview")]
	[SerializeField]
	private GameObject _previewContainer = null;
	[SerializeField]
	private SpriteRenderer _previewRenderer = null;

	protected override void OnSetData()
	{
		ID = $"Tile ({Data.Position.x},{Data.Position.y})";
		
		Position = Data.Position;

		HomeUnitSpot = new ElementUnitSpot(this, UnitSpotLocation, Data.HomeSpotData);
		AwayUnitSpot = new ElementUnitSpot(this, UnitSpotLocation, Data.AwaySpotData);

		_notBuildableContainer.SetActive(!HomeUnitSpot.SpotData.IsBuildable);
		_buildableContainer.SetActive(HomeUnitSpot.SpotData.IsBuildable);

		HomeUnitSpot.PreviewChangedEvent += OnPreviewChangedEvent;
		OnPreviewChangedEvent(HomeUnitSpot);

		name = ID;
	}

	protected override void OnClearData()
	{
		if(HomeUnitSpot != null)
		{
			HomeUnitSpot.Dispose();
			HomeUnitSpot = null;
		}

		if(AwayUnitSpot != null)
		{
			AwayUnitSpot.Dispose();
			AwayUnitSpot = null;
		}

		OnPreviewChangedEvent(null);
	}

	private void OnPreviewChangedEvent(ElementUnitSpot unitSpot)
	{
		if(unitSpot != null && unitSpot.Preview != null)
		{
			_previewContainer.SetActive(true);
			_previewRenderer.sprite = unitSpot.Preview.Icon;
		}
		else
		{
			_previewContainer.SetActive(false);
		}
	}

	[Serializable]
	public struct CoreData
	{
		public Vector2Int Position;
		public ElementUnitSpot.CoreData HomeSpotData;
		public ElementUnitSpot.CoreData AwaySpotData;
	}
}
