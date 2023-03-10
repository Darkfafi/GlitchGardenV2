using RaDataHolder;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
	public class UnitsHUD : RaMonoDataHolderBase<Player>
	{
		[Header("Display Requirements")]
		[SerializeField]
		private GridModelSO _gridModelSO = null;
		[SerializeField]
		private UnitHUDEntryUIElement _entryPrefab = null;

		[SerializeField]
		private RectTransform _container = null;

		[Header("Feature Requirements")]
		[SerializeField]
		private DraggingUnitElement _draggingUnitElement;

		private Dictionary<UnitConfig, UnitHUDEntryUIElement> _unitHUDEntryMap = new Dictionary<UnitConfig, UnitHUDEntryUIElement>();

		protected override void OnSetData()
		{
			_draggingUnitElement.Setup();

			for(int i = 0; i < Data.Model.Units.Length; i++)
			{
				var unitConfig = Data.Model.Units[i];
				CreateEntry(unitConfig);
			}
		}

		protected override void OnClearData()
		{
			UnitConfig[] keys = _unitHUDEntryMap.Keys.ToArray();
			for(int i = keys.Length - 1; i >= 0; i--)
			{
				RemoveEntry(keys[i]);
			}
		}

		private void CreateEntry(UnitConfig item)
		{
			if(_unitHUDEntryMap.ContainsKey(item))
			{
				return;
			}

			UnitHUDEntryUIElement itemView = Instantiate(_entryPrefab, _container);

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
			}
		}

		private void OnDragStartedEvent(UnitHUDEntryUIElement view)
		{
			view.SetGrabbed(true);
			_draggingUnitElement.SetData(view.Config, true);
			_gridModelSO.Grid.ShowUnitBuildabilityGrid(view.Config);
		}

		private void OnDraggingEvent(UnitHUDEntryUIElement view)
		{
			Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			newPos.z = _draggingUnitElement.transform.position.z;
			_draggingUnitElement.transform.position = newPos;
			_draggingUnitElement.RefreshPreviewTarget();
		}

		private void OnDragEndedEvent(UnitHUDEntryUIElement view)
		{
			_gridModelSO.Grid.ClearUnitBuildabilityGrid();

			_draggingUnitElement.TryCreateDraggingUnit();
			_draggingUnitElement.ClearData();

			view.SetGrabbed(false);
		}
	}
}
