using RaDataHolder;
using UnityEngine;

public class CurrencyGenerator : RaMonoDataHolderBase<Wallet>
{
	[SerializeField]
	private CurrencyValue _amountToGenerate = default;

	[SerializeField]
	private float _durationUntilGenerate = 1f;

	[SerializeField]
	private int _maxResounceAmount = -1;

	private bool _isRunning = false;

	public float Timer
	{
		get; private set;
	}

	public CurrencyValue AmountToGenerate => _amountToGenerate;

	public float Duration => _durationUntilGenerate;

	public float NormalizedTime => Mathf.Clamp01(Timer / Duration);

	public bool IsRunning => _isRunning && Data != null && HasResourcesRemaining;

	public int MaxResourceAmount => _maxResounceAmount;
	
	public int ResourcesRemaining 
	{ 
		get; 
		private set; 
	}

	public bool HasMaxResourcesEnabled => ResourcesRemaining >= 0;

	public bool HasResourcesRemaining => !HasMaxResourcesEnabled || ResourcesRemaining >= _amountToGenerate.Amount;

	protected override void OnSetData()
	{
		Timer = 0f;
		ResourcesRemaining = _maxResounceAmount;
	}

	protected override void OnClearData()
	{
		_isRunning = false;
	}

	public void Run()
	{
		Timer = 0f;
		_isRunning = true;
	}

	public void Stop()
	{
		_isRunning = false;
	}

	protected void Update()
	{
		if(IsRunning)
		{
			Timer += Time.deltaTime;
			if(Mathf.Approximately(NormalizedTime, 1f))
			{
				Timer = 0f;
				Data.Earn(_amountToGenerate);
				
				if(HasMaxResourcesEnabled)
				{
					ResourcesRemaining = Mathf.Max(ResourcesRemaining - _amountToGenerate.Amount, 0);
				}
			}
		}
	}

	public int GetBudget()
	{
		int budget = Data.GetAmount(AmountToGenerate.Currency);

		if(HasMaxResourcesEnabled)
		{
			budget += ResourcesRemaining;
		}
		else
		{
			budget = int.MaxValue;
		}

		return budget;
	}
}
