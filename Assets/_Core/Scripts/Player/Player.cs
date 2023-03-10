using RaDataHolder;
using UnityEngine;

public class Player : RaMonoDataHolderBase<Player.CoreData>
{
	public Type PlayerType => Data.Type;
	public PlayerModel Model => Data.PlayerModel;

	public Wallet Wallet
	{
		get; private set;
	}

	protected override void OnSetData()
	{
		Wallet = new Wallet();
	}

	protected override void OnClearData()
	{
		Wallet.Clear();
	}

	public Vector3 GetOrientation(Vector3 value)
	{
		value.x = GetOrientation(value.x);
		return value;
	}

	public Vector2 GetOrientation(Vector2 value)
	{
		value.x = GetOrientation(value.x);
		return value;
	}

	public float GetOrientation(float value)
	{
		if(PlayerType == Type.Away)
		{
			value *= -1;
		}
		return value;
	}

	public enum Type
	{
		Home, 
		Away
	}

	public struct CoreData
	{
		public PlayerModel PlayerModel;
		public Type Type;
	}
}
