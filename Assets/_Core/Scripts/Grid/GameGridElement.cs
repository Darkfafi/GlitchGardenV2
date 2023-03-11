using RaDataHolder;
using System;
using UnityEngine;

public class GameGridElement : RaMonoDataHolderBase<GameGridElement.CoreData>
{
	public event Action DirtyEvent;

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

	[Header("Mechanic")]
	[SerializeField]
	private Player.Type _playerType = Player.Type.Home;
	[SerializeField]
	private PlayersModelSO _playersModelSO = null;
	[SerializeField]
	private UnitsMechanicSO _unitsMechanicSO = null;

	private UnitConfig _unitBuildabilityPreview = null;

	protected override void OnSetData()
	{
		ID = $"Tile ({Data.Position.x},{Data.Position.y})";
		
		Position = Data.Position;

		HomeUnitSpot = new ElementUnitSpot(this, UnitSpotLocation, Data.HomeSpotData);
		AwayUnitSpot = new ElementUnitSpot(this, UnitSpotLocation, Data.AwaySpotData);

		name = ID;

		_previewContainer.SetActive(false);
	}

	protected override void OnSetDataResolved()
	{
		base.OnSetDataResolved();

		TryGetElementUnitSpot(_playersModelSO.GetPlayer(_playerType), out ElementUnitSpot spot);

		HomeUnitSpot.PreviewChangedEvent += OnPreviewChangedEvent;
		HomeUnitSpot.UnitChangedEvent += OnUnitChangedEvent;

		AwayUnitSpot.PreviewChangedEvent += OnPreviewChangedEvent;
		AwayUnitSpot.UnitChangedEvent += OnUnitChangedEvent;

		MarkDirty();
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

		_previewContainer.SetActive(false);
	}

	protected override void OnDispose()
	{
		DirtyEvent = null;
	}

	public void SetUnitBuildabilityPreview(UnitConfig config)
	{
		_unitBuildabilityPreview = config;
		MarkDirty();
	}

	public bool IsOccupied(out Unit homeUnit, out Unit awayUnit)
	{
		homeUnit = HomeUnitSpot.Unit;
		awayUnit = AwayUnitSpot.Unit;
		return homeUnit != null || awayUnit != null;
	}

	public bool TryGetElementUnitSpot(Player player, out ElementUnitSpot elementUnitSpot)
	{
		return TryGetElementUnitSpot(player.PlayerType, out elementUnitSpot);
	}

	public bool TryGetElementUnitSpot(Player.Type playerType, out ElementUnitSpot elementUnitSpot)
	{
		switch(playerType)
		{
			case Player.Type.Home:
				elementUnitSpot = HomeUnitSpot;
				return true;
			case Player.Type.Away:
				elementUnitSpot = AwayUnitSpot;
				return true;
			default:
				elementUnitSpot = default;
				return false;
		}
	}

	private void OnUnitChangedEvent(ElementUnitSpot spot)
	{
		MarkDirty();
	}

	private void OnPreviewChangedEvent(ElementUnitSpot unitSpot)
	{
		if(TryGetElementUnitSpot(_playerType, out ElementUnitSpot focussedSpot) && focussedSpot == unitSpot)
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
		MarkDirty();
	}

	private void RefreshBuildability()
	{
		if(_unitsMechanicSO.IsInitialized)
		{
			bool isBuildable = _unitsMechanicSO.CanCreateUnit(
				new Unit.CoreData() { 
					Owner = _playersModelSO.GetPlayer(_playerType), 
					Config = _unitBuildabilityPreview 
				}, Position, includeCost: false).IsSuccess;

			_notBuildableContainer.SetActive(!isBuildable);
			_buildableContainer.SetActive(isBuildable);
		}
	}

	private void MarkDirty()
	{
		RefreshBuildability();
		DirtyEvent?.Invoke();
	}

	[Serializable]
	public struct CoreData
	{
		public Vector2Int Position;
		public ElementUnitSpot.CoreData HomeSpotData;
		public ElementUnitSpot.CoreData AwaySpotData;
	}
}
