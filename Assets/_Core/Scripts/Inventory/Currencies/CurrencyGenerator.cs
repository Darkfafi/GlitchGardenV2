using RaDataHolder;
using UnityEngine;

public class CurrencyGenerator : RaMonoDataHolderBase<CurrencyGenerator.CoreData>
{
	public const int INFINITE_RESOURCE = -1;

	[SerializeField]
	private CurrencyValue _amountToGenerate = default;

	[SerializeField]
	private float _durationUntilGenerate = 1f;

	[SerializeField]
	private int _maxResounceAmount = INFINITE_RESOURCE;

	private bool _isRunning = false;

	public float Timer
	{
		get; private set;
	}

	public CurrencyValue AmountToGenerate => _amountToGenerate;

	public float Duration => _durationUntilGenerate;

	public float NormalizedTime => Mathf.Clamp01(Timer / Duration);

	public bool IsRunning => _isRunning && HasData && Data.Wallet != null && HasResourcesRemaining;

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

		if(Data.MaxResources.HasValue)
		{
			_maxResounceAmount = Data.MaxResources.Value;
		}

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
				Data.Wallet.Earn(_amountToGenerate);
				
				if(HasMaxResourcesEnabled)
				{
					ResourcesRemaining = Mathf.Max(ResourcesRemaining - _amountToGenerate.Amount, 0);
				}
			}
		}
	}

	public int GetBudget()
	{
		int budget = Data.Wallet.GetAmount(AmountToGenerate.Currency);

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

	public struct CoreData
	{
		public Wallet Wallet;
		public int? MaxResources;
	}
}
