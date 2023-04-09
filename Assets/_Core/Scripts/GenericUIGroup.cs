using RaTweening;
using UnityEngine;
using RaDataHolder;

namespace UI
{
	public abstract class GenericUIGroup<TData> : GenericUIGroup, IRaDataHolder<TData>
	{
		protected TData Data
		{
			get; private set;
		}

		public bool HasData
		{
			get; private set;
		}

		public bool IsReplacingData
		{
			get; private set;
		}

		private bool _resolved = false;

		protected override void OnDeinitialization()
		{
			Data = default;
			HasData = default;
			IsReplacingData = default;
			_resolved = default;
			base.OnDeinitialization();
		}

		public IRaDataClearResolver ClearData(bool resolve = true)
		{
			if(!HasData)
			{
				return this;	
			}

			_resolved = false;
			OnClearData();
			if(resolve)
			{
				((IRaDataClearResolver)this).Resolve();
			}
			return this;
		}
		public void ReplaceData(TData data, bool ignoreOnEqual)
		{
			if(ignoreOnEqual)
			{
				if(Data == null && data == null)
				{
					return;
				}

				if(Data != null && Data.Equals(data))
				{
					return;
				}
			}

			IsReplacingData = true;
			{
				ClearData();
				SetData(data);
			}
			IsReplacingData = false;
		}

		public IRaDataSetResolver Resolve()
		{
			if(!_resolved)
			{
				_resolved = true;
				OnResolveData();
			}
			return this;
		}

		public IRaDataSetResolver SetData(TData data, bool resolve = true)
		{
			if(HasData)
			{
				return this;
			}

			_resolved = false;
			Data = data;
			HasData = true;
			OnSetData();
			if(resolve)
			{
				Resolve();
			}

			return this;
		}

		public IRaDataSetResolver SetRawData(object data, bool resolve = true)
		{
			if(data is TData castedData)
			{
				SetData(castedData);
			}
			return this;
		}

		IRaDataClearResolver IRaDataClearResolver.Resolve()
		{
			return this;
		}

		protected abstract void OnSetData();
		protected abstract void OnClearData();
		protected abstract void OnResolveData();
	}

	public class GenericUIGroup : UsableElementBase
	{
		[SerializeField]
		private CanvasGroup _content = null;

		private RaTweenBase _tween = null;

		protected override void OnInitialization()
		{
			_content.gameObject.SetActive(false);
			_content.alpha = 0;
		}

		protected override void OnUseStarted()
		{
			if(_tween != null)
			{
				_tween.Stop();
				_tween = null;
			}

			_tween = _content.TweenAlpha(1f, 0.4f).OnStart(() => { _content.gameObject.SetActive(true); });
		}

		protected override void OnUseEnded()
		{
			if(_tween != null)
			{
				_tween.Stop();
				_tween = null;
			}

			_tween = _content.TweenAlpha(0f, 0.4f).OnComplete(() => { _content.gameObject.SetActive(false); });
		}

		protected override void OnDeinitialization()
		{
			if(_tween != null)
			{
				_tween.Stop();
				_tween = null;
			}

			_content.gameObject.SetActive(false);
			_content.alpha = 0;
		}
	}
}