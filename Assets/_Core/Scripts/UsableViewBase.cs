using RaDataHolder;
using RaFlags;

namespace UI
{
	public abstract class UsableViewBase<T> : RaMonoDataHolderBase<T>
	{
		public RaFlagsTracker InUseFlags
		{
			get; private set;
		}

		public bool IsInUse
		{
			get; private set;
		}

		private bool _hasData = false;

		protected override void OnInitialization()
		{
			base.OnInitialization();
			InUseFlags = new RaFlagsTracker(IsEmptyChangedEvent);
		}

		protected override void OnDeinitialization()
		{
			if(InUseFlags != null)
			{
				InUseFlags.Dispose();
				InUseFlags = null;
			}

			base.OnDeinitialization();
		}

		protected override void OnSetData()
		{
			_hasData = true;
			RefreshUsingState();
		}

		protected override void OnClearData()
		{
			_hasData = false;
			RefreshUsingState();
		}

		private void IsEmptyChangedEvent(bool isEmpty, RaFlagsTracker tracker)
		{
			RefreshUsingState();
		}

		private void RefreshUsingState()
		{
			bool isInUse = _hasData && !InUseFlags.IsEmpty();
			if(isInUse != IsInUse)
			{
				if(IsInUse = isInUse)
				{
					OnStartUsing();
				}
				else
				{
					OnStopUsing();
				}
			}
		}

		protected abstract void OnStartUsing();
		protected abstract void OnStopUsing();
	}
}
