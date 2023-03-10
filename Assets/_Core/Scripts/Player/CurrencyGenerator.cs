using RaDataHolder;
using UnityEngine;

public class CurrencyGenerator : RaMonoDataHolderBase<Wallet>
{
	[SerializeField]
	private CurrencyValue _amountToGenerate = default;

	[SerializeField]
	private float _durationUntilGenerate = 1f;

	private bool _isRunning = false;

	public float Timer
	{
		get; private set;
	}

	public float Duration => _durationUntilGenerate;

	public float NormalizedTime => Mathf.Clamp01(Timer / Duration);

	public bool IsRunning => _isRunning && Data != null;

	protected override void OnSetData()
	{
		Timer = 0f;
	}

	protected override void OnSetDataResolved()
	{
		_isRunning = true;
	}

	protected override void OnClearData()
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
			}
		}
	}
}
