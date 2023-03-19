using RaDataHolder;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battle.UI
{
	public class DraggingBattleUnitElement : RaMonoDataHolderBase<DraggingBattleUnitElement.CoreData>
	{
		[SerializeField]
		private BattleUnitsMechanicSO _unitsMechanic = null;

		[SerializeField]
		private SpriteRenderer _iconRenderer = null;

		[SerializeField]
		private TrailRenderer _trailRenderer = null;

		private List<GameGridElement> _gridElements = new List<GameGridElement>();
		private ElementBattleUnitSpot _previewTarget = null;

		public bool TryGetPlacementTarget(out ElementBattleUnitSpot target)
		{
			target = _previewTarget;
			return target != null;
		}

		protected override void OnSetData()
		{
			gameObject.SetActive(true);
			_iconRenderer.sprite = Data.UnitConfig.Icon;
			SetTrailColor(Data.UnitConfig.Color);

			Data.Player.Wallet.ValueChangedEvent += OnWalletChangedEvent;
		}

		protected override void OnClearData()
		{
			Data.Player.Wallet.ValueChangedEvent -= OnWalletChangedEvent;

			gameObject.SetActive(false);
			_iconRenderer.sprite = null;
			SetTrailColor(Color.white);
			_gridElements.Clear();
			RefreshPreviewTarget();
		}

		public void Setup()
		{
			gameObject.SetActive(false);
		}

		public bool TryCreateDraggingUnit()
		{
			if(TryGetPlacementTarget(out ElementBattleUnitSpot spot))
			{
				return _unitsMechanic.CreateUnit(new BattleUnit.CoreData() { Owner = Data.Player, Config = Data.UnitConfig }, spot.Element.Position).IsSuccess;
			}
			return false;
		}

		public void RefreshPreviewTarget()
		{
			float itemDistance = float.MaxValue;
			ElementBattleUnitSpot closestItem = null;
			for(int i = 0; i < _gridElements.Count; i++)
			{
				var element = _gridElements[i];
				float currentDistance = Vector3.Distance(element.transform.position, transform.position);
				if(currentDistance < itemDistance)
				{
					var response = _unitsMechanic.CanCreateUnit(
						new BattleUnit.CoreData { Owner = Data.Player, Config = Data.UnitConfig }, 
						element.Position, 
						out ElementBattleUnitSpot currentItem);

					if(response.IsSuccess)
					{
						closestItem = currentItem;
						itemDistance = currentDistance;
					}
				}
			}

			if(_previewTarget != closestItem)
			{
				if(_previewTarget != null)
				{
					_previewTarget.SetPreview(null);
				}

				_previewTarget = closestItem;

				if(_previewTarget != null)
				{
					_previewTarget.SetPreview(Data.UnitConfig);
				}
			}
		}

		protected void OnTriggerEnter2D(Collider2D collision)
		{
			if(HasData)
			{
				if(collision.gameObject.TryGetComponent(out GameGridElement element))
				{
					_gridElements.Add(element);
					RefreshPreviewTarget();
				}
			}
		}

		protected void OnTriggerExit2D(Collider2D collision)
		{
			if(HasData)
			{
				if(collision.gameObject.TryGetComponent(out GameGridElement element))
				{
					_gridElements.Remove(element);
					RefreshPreviewTarget();
				}
			}
		}

		private void SetTrailColor(Color color)
		{
			_trailRenderer.startColor = color;
			color.a = _trailRenderer.endColor.a;
			_trailRenderer.endColor = color;
		}

		private void OnWalletChangedEvent(CurrencyConfig currency, int newValue, int oldValue)
		{
			if(currency == Data.BattleUnitConfigData.Cost.Currency)
			{
				RefreshPreviewTarget();
			}
		}

		[System.Serializable]
		public struct CoreData
		{
			public UnitConfig UnitConfig;
			public BattlePlayer Player;

			public BattleUnitConfigData BattleUnitConfigData => UnitConfig.BattleUnitConfigData;
		}
	}
}