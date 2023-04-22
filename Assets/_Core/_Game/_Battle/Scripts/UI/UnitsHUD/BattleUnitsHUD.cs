using RaDataHolder;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle.UI
{
	public class BattleUnitsHUD : RaMonoDataHolderBase<BattlePlayer>
	{
		[Header("References")]
		[SerializeField]
		[FormerlySerializedAs("_gridModelSO")]
		private BattleGridReferenceSO _gridReferenceSO = null;

		[Header("Requirements")]
		[SerializeField]
		private BattleUnitHUDEntryUIElement _entryPrefab = null;
		[SerializeField]
		private RectTransform _optionsContainer = null;

		[Header("External")]
		[SerializeField]
		private DraggingBattleUnitElement _draggingUnitElement;

		private Dictionary<UnitModel, BattleUnitHUDEntryUIElement> _unitHUDEntryMap = new Dictionary<UnitModel, BattleUnitHUDEntryUIElement>();

		private BattleUnitHUDEntryUIElement _currentDraggingHUDEntry = null;

		public bool IsInteractable
		{
			get; private set;
		}

		public void SetInteractable(bool isInteractable)
		{
			IsInteractable = isInteractable && HasData;

			if(!isInteractable)
			{
				ReleaseDraggingUnit(false);
			}
		}

		protected override void OnSetData()
		{
			_draggingUnitElement.Setup();

			List<UnitModel> units = Data.GetUnits();
			for(int i = 0; i < units.Count; i++)
			{
				var unitModel = units[i];
				CreateEntry(unitModel);
			}
		}

		protected override void OnClearData()
		{
			UnitModel[] keys = _unitHUDEntryMap.Keys.ToArray();
			for(int i = keys.Length - 1; i >= 0; i--)
			{
				RemoveEntry(keys[i]);
			}

			if(_gridReferenceSO.Grid != null)
			{
				_gridReferenceSO.Grid.DirtyEvent -= OnGridDirtyEvent;
			}

			SetInteractable(false);
		}

		private void CreateEntry(UnitModel item)
		{
			if(_unitHUDEntryMap.ContainsKey(item))
			{
				return;
			}

			BattleUnitHUDEntryUIElement itemView = Instantiate(_entryPrefab, _optionsContainer);

			_unitHUDEntryMap[item] = itemView;

			var resolver = itemView.SetData(item, false);
			{
				itemView.DragStartedEvent += OnDragStartedEvent;
				itemView.DraggingEvent += OnDraggingEvent;
				itemView.DragEndedEvent += OnDragEndedEvent;
			}
			resolver.Resolve();
		}

		private void RemoveEntry(UnitModel item)
		{
			if(_unitHUDEntryMap.TryGetValue(item, out BattleUnitHUDEntryUIElement itemView))
			{
				itemView.DragStartedEvent -= OnDragStartedEvent;
				itemView.DraggingEvent -= OnDraggingEvent;
				itemView.DragEndedEvent -= OnDragEndedEvent;
				itemView.ClearData();

				Destroy(itemView.gameObject);
				_unitHUDEntryMap.Remove(item);
			}
		}

		private void OnDragStartedEvent(BattleUnitHUDEntryUIElement view)
		{
			if(_currentDraggingHUDEntry != null)
			{
				return;
			}

			if(IsInteractable)
			{
				_currentDraggingHUDEntry = view;
				_gridReferenceSO.Grid.DirtyEvent += OnGridDirtyEvent;
				view.SetGrabbed(true);
				_draggingUnitElement.SetData(new DraggingBattleUnitElement.CoreData { Player = Data, UnitModel = view.UnitModel }, true);
				_gridReferenceSO.Grid.ShowUnitBuildabilityGrid(view.UnitModel);
			}
		}

		private void OnDraggingEvent(BattleUnitHUDEntryUIElement view)
		{
			if(view != _currentDraggingHUDEntry)
			{
				return;
			}

			if(IsInteractable)
			{
				Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				newPos.z = _draggingUnitElement.transform.position.z;
				_draggingUnitElement.transform.position = newPos;
				_draggingUnitElement.RefreshPreviewTarget();
			}
		}

		private void OnDragEndedEvent(BattleUnitHUDEntryUIElement view)
		{
			if(view != _currentDraggingHUDEntry)
			{
				return;
			}

			if(IsInteractable)
			{
				ReleaseDraggingUnit(true);
			}
		}

		private void ReleaseDraggingUnit(bool placeUnit)
		{
			if(_currentDraggingHUDEntry != null)
			{
				_gridReferenceSO.Grid.DirtyEvent -= OnGridDirtyEvent;
				_gridReferenceSO.Grid.ClearUnitBuildabilityGrid();

				if(placeUnit)
				{
					_draggingUnitElement.TryCreateDraggingUnit();
				}

				_draggingUnitElement.ClearData();

				_currentDraggingHUDEntry.SetGrabbed(false);
				_currentDraggingHUDEntry = null;
			}
		}

		private void OnGridDirtyEvent()
		{
			if(HasData)
			{
				if(_draggingUnitElement != null)
				{
					_draggingUnitElement.RefreshPreviewTarget();
				}
			}
		}
	}
}