using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RaTweening;

namespace UI
{
	public class UnitsHUD : UsableViewBase<Player>
	{
		[Header("Display Requirements")]
		[SerializeField]
		private GridModelSO _gridModelSO = null;
		[SerializeField]
		private UnitHUDEntryUIElement _entryPrefab = null;

		[SerializeField]
		private RectTransform _content = null;

		[SerializeField]
		private RectTransform _optionsContainer = null;

		[Header("Feature Requirements")]
		[SerializeField]
		private DraggingUnitElement _draggingUnitElement;

		private Dictionary<UnitConfig, UnitHUDEntryUIElement> _unitHUDEntryMap = new Dictionary<UnitConfig, UnitHUDEntryUIElement>();

		protected override void OnInitialization()
		{
			base.OnInitialization();
			_content.gameObject.SetActive(false);
			_content.anchoredPosition = Vector2.down * _content.rect.height;
		}

		protected override void OnStartUsing()
		{
			_draggingUnitElement.Setup();

			for(int i = 0; i < Data.Model.Units.Length; i++)
			{
				var unitConfig = Data.Model.Units[i];
				CreateEntry(unitConfig);
			}

			_content.TweenAnchorPosY(0f, 1.5f)
				.SetEasing(RaEasingType.OutBack)
				.OnStart(() => { _content.gameObject.SetActive(true); });
		}

		protected override void OnStopUsing()
		{
			_content.TweenAnchorPosY(-_content.rect.height, 1f)
				.SetEasing(RaEasingType.InBack)
				.OnEnd(() => 
				{
					UnitConfig[] keys = _unitHUDEntryMap.Keys.ToArray();
					for(int i = keys.Length - 1; i >= 0; i--)
					{
						RemoveEntry(keys[i]);
					}

					if(_gridModelSO.Grid != null)
					{
						_gridModelSO.Grid.DirtyEvent -= OnGridDirtyEvent;
					}

					_content.gameObject.SetActive(false);
				});
		}

		private void CreateEntry(UnitConfig item)
		{
			if(_unitHUDEntryMap.ContainsKey(item))
			{
				return;
			}

			UnitHUDEntryUIElement itemView = Instantiate(_entryPrefab, _optionsContainer);

			_unitHUDEntryMap[item] = itemView;

			var resolver = itemView.SetData(item, false);
			{
				itemView.DragStartedEvent += OnDragStartedEvent;
				itemView.DraggingEvent += OnDraggingEvent;
				itemView.DragEndedEvent += OnDragEndedEvent;
			}
			resolver.Resolve();
		}

		private void RemoveEntry(UnitConfig item)
		{
			if(_unitHUDEntryMap.TryGetValue(item, out UnitHUDEntryUIElement itemView))
			{
				itemView.DragStartedEvent -= OnDragStartedEvent;
				itemView.DraggingEvent -= OnDraggingEvent;
				itemView.DragEndedEvent -= OnDragEndedEvent;
				itemView.ClearData();

				Destroy(itemView.gameObject);
				_unitHUDEntryMap.Remove(item);
			}
		}

		private void OnDragStartedEvent(UnitHUDEntryUIElement view)
		{
			if(IsInUse)
			{
				_gridModelSO.Grid.DirtyEvent += OnGridDirtyEvent;
				view.SetGrabbed(true);
				_draggingUnitElement.SetData(new DraggingUnitElement.CoreData { Player = Data, UnitConfig = view.Config }, true);
				_gridModelSO.Grid.ShowUnitBuildabilityGrid(view.Config);
			}
		}

		private void OnDraggingEvent(UnitHUDEntryUIElement view)
		{
			if(IsInUse)
			{
				Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				newPos.z = _draggingUnitElement.transform.position.z;
				_draggingUnitElement.transform.position = newPos;
				_draggingUnitElement.RefreshPreviewTarget();
			}
		}

		private void OnDragEndedEvent(UnitHUDEntryUIElement view)
		{
			if(IsInUse)
			{
				_gridModelSO.Grid.DirtyEvent -= OnGridDirtyEvent;
				_gridModelSO.Grid.ClearUnitBuildabilityGrid();

				_draggingUnitElement.TryCreateDraggingUnit();
				_draggingUnitElement.ClearData();

				view.SetGrabbed(false);
			}
		}

		private void OnGridDirtyEvent()
		{
			if(IsInUse)
			{
				if(_draggingUnitElement != null)
				{
					_draggingUnitElement.RefreshPreviewTarget();
				}
			}
		}
	}
}
