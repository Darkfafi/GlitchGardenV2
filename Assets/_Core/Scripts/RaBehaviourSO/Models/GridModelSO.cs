using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/Models/GridModelSO")]
public class GridModelSO : ModelSOBase
{
	public GameGrid Grid
	{
		get; private set;
	}

	protected override void OnSetup()
	{
		Grid = Game.Grid;
	}

	protected override void OnStart()
	{

	}

	protected override void OnEnd()
	{
		Grid = null;
	}
}
