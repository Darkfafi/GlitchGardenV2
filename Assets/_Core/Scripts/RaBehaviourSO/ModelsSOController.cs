using UnityEngine;

public class ModelsSOController : BehavioursSOControllerBase<ModelSOBase>
{
	[SerializeField]
	private Game _game;

	protected override void OnInit(ModelSOBase behaviourSO)
	{
		behaviourSO.SetData(_game);
	}

	protected override void OnDeinit(ModelSOBase behaviourSO)
	{
		behaviourSO.ClearData();
	}
}
