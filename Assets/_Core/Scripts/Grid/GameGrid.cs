using RaDataHolder;
using RaTweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : RaMonoDataHolderBase<GameGrid.CoreData>
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

	public void ShowUnitBuildabilityGrid(UnitConfig config)
	{
		RaTweenBase.StopGroup(_gridHighlightRenderer);
		_gridHighlightRenderer.TweenColorA(0.4f, 0.25f)
			.SetGroup(_gridHighlightRenderer);

		ForEach((pos, element) => element.SetUnitBuildabilityPreview(config));
	}

	public void ClearUnitBuildabilityGrid()
	{
		RaTweenBase.StopGroup(_gridHighlightRenderer);
		_gridHighlightRenderer.TweenColorA(0f, 0.25f)
			.SetGroup(_gridHighlightRenderer);

		ForEach((pos, element) => element.SetUnitBuildabilityPreview(null));
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

	public ElementUnitSpot GetUnitSpot(Unit unit)
	{
		if(TryGetUnitSpot(unit, out ElementUnitSpot spot))
		{
			return spot;
		}

		return null;
	}

	public ElementUnitSpot GetUnitSpot(Vector2Int position, Player owner)
	{
		if(TryGetUnitSpot(position, owner, out ElementUnitSpot spot))
		{
			return spot;
		}

		return null;
	}

	public bool TryGetUnitSpot(Vector2Int position, Player owner, out ElementUnitSpot spot)
	{
		spot = default;
		return TryGetElement(position, out GameGridElement element) && element.TryGetElementUnitSpot(owner, out spot);
	}

	public bool TryGetUnitSpot(Unit unit, out ElementUnitSpot spot)
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
				HomeSpotData = new ElementUnitSpot.CoreData()
				{
					IsBuildable = (Data.HomeBuildableColumns - 1) >= position.x,
				},
				AwaySpotData = new ElementUnitSpot.CoreData()
				{
					IsBuildable = position.x >= (GridData.Size.x - Data.AwayBuildableColumns),
				}
			}, false);

			_elements[element.Position] = element;
			element.transform.localPosition = new Vector3(position.x, position.y, 0);
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
