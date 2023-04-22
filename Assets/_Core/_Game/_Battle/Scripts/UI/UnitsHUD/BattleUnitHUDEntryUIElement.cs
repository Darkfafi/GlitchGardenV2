using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RaDataHolder;

namespace Game.Battle.UI
{
	public class BattleUnitHUDEntryUIElement : RaMonoDataHolderBase<UnitModel>, IBeginDragHandler, IDragHandler, IEndDragHandler
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

		public UnitModel UnitModel => Data;
		public BattleUnitConfigData BattleUnitData => UnitModel.BattleUnitData;

		protected override void OnSetData()
		{
			SetGrabbed(false);
			_iconImage.sprite = Data.Config.Icon;
			_costDisplay.SetData(BattleUnitData, false);
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