using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RaDataHolder;

namespace UI
{
	public class UnitHUDEntryUIElement : RaMonoDataHolderBase<UnitConfig>, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public event Action<UnitHUDEntryUIElement> DragStartedEvent;
		public event Action<UnitHUDEntryUIElement> DraggingEvent;
		public event Action<UnitHUDEntryUIElement> DragEndedEvent;

		[SerializeField]
		private Image _iconImage = null;

		[SerializeField]
		private RaTweening.RaTweenerComponent _isGrabbedAnimation = null;

		[SerializeField]
		private CurrencyValueUIElement _costDisplay = null;

		public UnitConfig Config => Data;

		protected override void OnSetData()
		{
			SetGrabbed(false);
			_iconImage.sprite = Data.Icon;
			_costDisplay.SetData(Data, false);
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