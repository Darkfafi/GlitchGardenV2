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

		public UnitConfig Config => Data;

		protected override void OnSetData()
		{
			SetGrabbed(false);
			_iconImage.sprite = Data.Icon;
		}

		protected override void OnClearData()
		{
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