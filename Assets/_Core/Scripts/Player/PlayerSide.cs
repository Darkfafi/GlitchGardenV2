using RaDataHolder;
using UnityEngine;
using static Player;

public class PlayerSide : RaMonoDataHolderBase<PlayerModel>
{
	[field: SerializeField]
	public Type SideType
	{
		get; private set;
	}

	[field: SerializeField]
	public Player Player
	{
		get; private set;
	}

	[field: SerializeField]
	public CurrencyGenerator ResourceGenerator
	{
		get; private set;
	}

	protected override void OnSetData()
	{
		Player.SetData(new CoreData() { PlayerModel = Data, Type = SideType }, false);
		ResourceGenerator.SetData(Player.Wallet, false);
	}

	protected override void OnSetDataResolved()
	{
		Player.Resolve();
		ResourceGenerator.Resolve();
	}

	protected override void OnClearData()
	{
		ResourceGenerator.ClearData();
		Player.ClearData();
	}
}
