using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RaDataHolder;

namespace Game.Battle.UI
{
	public class BattleUnitHUDEntryUIElement : RaMonoDataHolderBase<UnitConfig>, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public event Action<BattleUnitHUDEntryUIElement> DragStartedEvent;
		public event Action<BattleUnitHUDEntryUIElement> DraggingEvent;
		public event Action<BattleUnitHUDEntryUIElement> DragEndedEvent;

		[SerializeField]
		private Image _iconImage = null;

		[SerializeField]
		private RaTweening.RaTweenerComponent _isGrabbedAnimation = null;

		[SerializeField]
		private CurrencyValueUIElement _costDisplay = null;

		public UnitConfig Config => Data;
		public BattleUnitConfigData BattleUnitConfigData => Config.BattleUnitConfigData;

		protected override void OnSetData()
		{
			SetGrabbed(false);
			_iconImage.sprite = Data.Icon;
			_costDisplay.SetData(BattleUnitConfigData, false);
		}

		protected override void OnSetDataResolved()
		{
			_costDisplay.Resolve();
		}

		protected override void OnClearData()
		{
			_costDisplay.ClearData();
			_iconImage.sprite = null;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			DragStartedEvent?.Invoke(this);
		}

		public void OnDrag(PointerEventData eventData)
		{
			DraggingEvent?.Invoke(this);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			DragEndedEvent?.Invoke(this);
		}

		public void SetGrabbed(bool isGrabbed)
		{
			_iconImage.gameObject.SetActive(!isGrabbed);

			if(isGrabbed)
			{
				_isGrabbedAnimation.Play();
			}
		}
	}
}