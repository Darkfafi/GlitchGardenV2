using RaDataHolder;
using RaTweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battle
{

	public class BattleGameGrid : RaMonoDataHolderBase<BattleGameGrid.CoreData>
	{
		public event Action DirtyEvent;

		public delegate void LoopHandler(Vector2Int position, GameGridElement element);

		public CoreData GridData => Data;

		[SerializeField]
		private GameGridElement _prefab = null;

		[SerializeField]
		private SpriteRenderer _gridHighlightRenderer = null;

		private Dictionary<Vector2Int, GameGridElement> _elements = new Dictionary<Vector2Int, GameGridElement>();

		public SpriteRenderer GridHighlightRenderer => _gridHighlightRenderer;

		public void ShowUnitBuildabilityGrid(UnitModel unitModel)
		{
			RaTweenBase.StopGroup(_gridHighlightRenderer);
			_gridHighlightRenderer.TweenColorA(0.4f, 0.25f)
				.SetGroup(_gridHighlightRenderer);

			ForEach((pos, element) => element.SetUnitBuildabilityPreview(unitModel));
		}

		public void ClearUnitBuildabilityGrid()
		{
			RaTweenBase.StopGroup(_gridHighlightRenderer);
			_gridHighlightRenderer.TweenColorA(0f, 0.25f)
				.SetGroup(_gridHighlightRenderer);

			ForEach((pos, element) => element.SetUnitBuildabilityPreview(null));
		}

		public Vector2Int ToGridPos(Vector3 worldPosition)
		{
			Vector3 localWorldPosition = worldPosition - transform.position;
			return new Vector2Int(Mathf.FloorToInt(localWorldPosition.x), Mathf.FloorToInt(localWorldPosition.y));
		}

		public Vector3 ToWorldPos(Vector2Int position)
		{
			Vector3 returnValue = transform.position;
			returnValue.x += position.x;
			returnValue.y += position.y;
			return returnValue;
		}

		public GameGridElement GetElement(Vector2Int position)
		{
			if(TryGetElement(position, out GameGridElement element))
			{
				return element;
			}
			return default;
		}

		public bool TryGetElement(Vector2Int position, out GameGridElement element)
		{
			return _elements.TryGetValue(position, out element);
		}

		public int GetPlayerColumn(BattlePlayer.Type playerType)
		{
			switch(playerType)
			{
				case BattlePlayer.Type.Home:
					return 0;
				case BattlePlayer.Type.Away:
					return GridData.Size.x - 1;
			}
			return -1;
		}

		public ElementBattleUnitSpot GetUnitSpot(BattleUnit unit)
		{
			if(TryGetUnitSpot(unit, out ElementBattleUnitSpot spot))
			{
				return spot;
			}

			return null;
		}

		public ElementBattleUnitSpot GetUnitSpot(Vector2Int position, BattlePlayer owner)
		{
			if(TryGetUnitSpot(position, owner, out ElementBattleUnitSpot spot))
			{
				return spot;
			}

			return null;
		}

		public bool TryGetUnitSpot(Vector2Int position, BattlePlayer owner, out ElementBattleUnitSpot spot)
		{
			spot = default;
			return TryGetElement(position, out GameGridElement element) && element.TryGetElementUnitSpot(owner, out spot);
		}

		public bool TryGetUnitSpot(BattleUnit unit, out ElementBattleUnitSpot spot)
		{
			spot = default;
			return TryGetElement(unit.Position, out GameGridElement element) && element.TryGetElementUnitSpot(unit.Owner, out spot);
		}

		protected override void OnSetDataResolved()
		{
			ForEach((pos, element) =>
			{
				element.Resolve();
				element.DirtyEvent += MarkDirty;
			});

			MarkDirty();
		}

		protected override void OnSetData()
		{
			ForEach((position, element) =>
			{
				element = Instantiate(_prefab, transform);
				element.SetData(new GameGridElement.CoreData()
				{
					Position = position,
					HomeSpotData = new ElementBattleUnitSpot.CoreData()
					{
						IsBuildable = (Data.HomeBuildableColumns - 1) >= position.x,
					},
					AwaySpotData = new ElementBattleUnitSpot.CoreData()
					{
						IsBuildable = position.x >= (GridData.Size.x - Data.AwayBuildableColumns),
					}
				}, false);

				_elements[element.Position] = element;
				element.transform.position = ToWorldPos(position);
			});
		}

		protected override void OnClearData()
		{
			ClearUnitBuildabilityGrid();

			ForEach((position, element) =>
			{
				if(element != null)
				{
					element.DirtyEvent -= MarkDirty;
					Destroy(element.gameObject);
				}
			});
		}

		public void ForEach(LoopHandler action)
		{
			for(int x = 0; x < GridData.Size.x; x++)
			{
				for(int y = 0; y < GridData.Size.y; y++)
				{
					Vector2Int position = new Vector2Int(x, y);
					_elements.TryGetValue(position, out GameGridElement element);
					action(position, element);
				}
			}
		}

		private void MarkDirty()
		{
			DirtyEvent?.Invoke();
		}

		[Serializable]
		public struct CoreData
		{
			public Vector2Int Size;

			[Header("Home")]
			public int HomeBuildableColumns;

			[Header("Away")]
			public int AwayBuildableColumns;
		}
	}
}