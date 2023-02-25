public class Health
{
	public int HP
	{
		get; private set;
	}

	public int MaxHP
	{
		get; private set;
	}

	public Health(int amount)
	{
		MaxHP = HP = amount;
	}
}
