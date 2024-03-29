﻿using RaDataHolder;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle
{
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
		public ElementBattleUnitSpot HomeUnitSpot
		{
			get; private set;
		}

		[field: SerializeField]
		public ElementBattleUnitSpot AwayUnitSpot
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
		private BattlePlayer.Type _playerType = BattlePlayer.Type.Home;
		[SerializeField]
		private BattleUnitsMechanicSO _unitsMechanicSO = null;

		[Header("References")]
		[SerializeField]
		[FormerlySerializedAs("_playersModelSO")]
		private BattlePlayerSidesReferenceSO _playersReferenceSO = null;

		private UnitModel _unitBuildabilityPreview = null;

		protected override void OnSetData()
		{
			ID = $"Tile ({Data.Position.x},{Data.Position.y})";

			Position = Data.Position;

			HomeUnitSpot = new ElementBattleUnitSpot(this, UnitSpotLocation, Data.HomeSpotData);
			AwayUnitSpot = new ElementBattleUnitSpot(this, UnitSpotLocation, Data.AwaySpotData);

			name = ID;

			_previewContainer.SetActive(false);
		}

		protected override void OnSetDataResolved()
		{
			base.OnSetDataResolved();

			TryGetElementUnitSpot(_playersReferenceSO.GetPlayer(_playerType), out ElementBattleUnitSpot spot);

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

		protected override void OnDeinitialization()
		{
			DirtyEvent = null;
		}

		public void SetUnitBuildabilityPreview(UnitModel unitModel)
		{
			_unitBuildabilityPreview = unitModel;
			MarkDirty();
		}

		public bool TryGetUnit(BattlePlayer.Type playerType, out BattleUnit unit)
		{
			if(HomeUnitSpot.Unit != null && HomeUnitSpot.Unit.GetAlignment(playerType))
			{
				unit = HomeUnitSpot.Unit;
				return true;
			}

			if(AwayUnitSpot.Unit != null && AwayUnitSpot.Unit.GetAlignment(playerType))
			{
				unit = AwayUnitSpot.Unit;
				return true;
			}

			unit = default;
			return false;
		}

		public bool IsOccupied(out BattleUnit homeUnit, out BattleUnit awayUnit)
		{
			homeUnit = HomeUnitSpot.Unit;
			awayUnit = AwayUnitSpot.Unit;
			return homeUnit != null || awayUnit != null;
		}

		public bool TryGetElementUnitSpot(BattlePlayer player, out ElementBattleUnitSpot elementUnitSpot)
		{
			return TryGetElementUnitSpot(player.PlayerType, out elementUnitSpot);
		}

		public bool TryGetElementUnitSpot(BattlePlayer.Type playerType, out ElementBattleUnitSpot elementUnitSpot)
		{
			switch(playerType)
			{
				case BattlePlayer.Type.Home:
					elementUnitSpot = HomeUnitSpot;
					return true;
				case BattlePlayer.Type.Away:
					elementUnitSpot = AwayUnitSpot;
					return true;
				default:
					elementUnitSpot = default;
					return false;
			}
		}

		private void OnUnitChangedEvent(ElementBattleUnitSpot spot)
		{
			MarkDirty();
		}

		private void OnPreviewChangedEvent(ElementBattleUnitSpot unitSpot)
		{
			if(TryGetElementUnitSpot(_playerType, out ElementBattleUnitSpot focussedSpot) && focussedSpot == unitSpot)
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
			bool isBuildable = false;

			if(_unitsMechanicSO.IsInitialized && _unitBuildabilityPreview != null)
			{
				isBuildable = _unitsMechanicSO.CanCreateUnit(
					new BattleUnit.CoreData()
					{
						Owner = _playersReferenceSO.GetPlayer(_playerType),
						UnitModel = _unitBuildabilityPreview
					}, Position, includeCost: false).IsSuccess;
			}
			else if(TryGetElementUnitSpot(_playerType, out var unitSpot))
			{
				isBuildable = unitSpot.SpotData.IsBuildable;
			}

			_notBuildableContainer.SetActive(!isBuildable);
			_buildableContainer.SetActive(isBuildable);
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
			public ElementBattleUnitSpot.CoreData HomeSpotData;
			public ElementBattleUnitSpot.CoreData AwaySpotData;
		}
	}
}