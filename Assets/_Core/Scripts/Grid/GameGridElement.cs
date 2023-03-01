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

	[Header("Mechanic")]
	[SerializeField]
	private Player.Type _playerType = Player.Type.Home;
	[SerializeField]
	private PlayersModelSO _playersModelSO = null;
	[SerializeField]
	private UnitsMechanicSO _unitsMechanicSO = null;

	private bool _isResolved = false;

	protected override void OnSetData()
	{
		ID = $"Tile ({Data.Position.x},{Data.Position.y})";
		
		Position = Data.Position;

		HomeUnitSpot = new ElementUnitSpot(this, UnitSpotLocation, Data.HomeSpotData);
		AwayUnitSpot = new ElementUnitSpot(this, UnitSpotLocation, Data.AwaySpotData);

		name = ID;
	}

	public void Resolve()
	{
		if(_isResolved)
		{
			return;
		}

		_isResolved = true;
		TryGetElementUnitSpot(_playersModelSO.GetPlayer(_playerType), out ElementUnitSpot spot);
		spot.PreviewChangedEvent += OnPreviewChangedEvent;
		spot.UnitChangedEvent += OnUnitChangedEvent;

		OnPreviewChangedEvent(HomeUnitSpot);
		RefreshBuildability();
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
		_isResolved = false;
	}

	public bool TryGetElementUnitSpot(Player player, out ElementUnitSpot elementUnitSpot)
	{
		switch(player.PlayerType)
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
		RefreshBuildability();
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
		RefreshBuildability();
	}

	private void RefreshBuildability()
	{
		if(_unitsMechanicSO.IsInitialized)
		{
			bool isBuildable = _unitsMechanicSO.CanCreateUnit(new Unit.CoreData() { Owner = _playersModelSO.GetPlayer(_playerType) }, Position);
			_notBuildableContainer.SetActive(!isBuildable);
			_buildableContainer.SetActive(isBuildable);
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
