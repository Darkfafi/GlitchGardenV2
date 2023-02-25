using RaDataHolder;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : RaMonoDataHolderBase<GameGrid.CoreData>
{
	public delegate void LoopHandler(Vector2Int position, GameGridElement element);

	public CoreData GridData => Data;

	[SerializeField]
	private GameGridElement _prefab = null;

	private Dictionary<Vector2Int, GameGridElement> _elements = new Dictionary<Vector2Int, GameGridElement>();

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
			});

			_elements[element.Position] = element;
			element.transform.localPosition = new Vector3(position.x, position.y, 0);
		});
	}

	protected override void OnClearData()
	{
		ForEach((position, element) =>
		{
			if(element != null)
			{
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
