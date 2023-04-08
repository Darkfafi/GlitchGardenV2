using RaDataHolder;
using RaTweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Battle
{
	public class HealthInteractionView : RaMonoDataHolderBase<Health>
	{
		public UnityEvent DamagedEvent;
		public UnityEvent HealedEvent;
		public UnityEvent KilledEvent;

		[Header("Character")]
		[SerializeField]
		private SpriteRenderer _renderer = null;

		[Header("Health Bar")]
		[SerializeField]
		private CanvasGroup _healthBarGroup = null;

		[SerializeField]
		private Image _healthBarFill = null;

		private bool _displayingHealthBar = false;

		protected override void OnInitialization()
		{
			ResetBarVisuals();
		}

		protected override void OnSetData()
		{
			Data.HealthChangedEvent += OnHealthChangedEvent;
		}

		protected override void OnClearData()
		{
			Data.HealthChangedEvent -= OnHealthChangedEvent;
			RaTweenBase.CompleteGroup(_healthBarGroup);
			ResetBarVisuals();
		}

		private void OnHealthChangedEvent(Health health, int delta)
		{
			if(delta == 0)
			{
				return;
			}

			if(!_displayingHealthBar)
			{
				ResetBarVisuals();
				_healthBarGroup.gameObject.SetActive(true);
				_displayingHealthBar = true;
				_healthBarGroup.TweenAlpha(1f, 0.1f);
			}

			RaTweenBase.CompleteGroup(_healthBarGroup);

			RaTweenSequence healthHitSequence = RaTweenSequence.Create().SetGroup(_healthBarGroup);

			healthHitSequence.Append(_renderer.TweenColor(Color.red, 0.2f).SetModifier(RaModifierType.AbsSin).ToSequenceEntry(0f));
			healthHitSequence.Append(_healthBarFill.TweenFill(health.NormalizedHealth, 0.1f).ToSequenceEntry());

			if(!health.IsAlive)
			{
				healthHitSequence.Append(_healthBarGroup.TweenAlpha(0f, 0.1f).ToSequenceEntry());
			}

			if(delta < 0)
			{
				DamagedEvent.Invoke();
			}
			else
			{
				HealedEvent.Invoke();
			}

			if(!health.IsAlive)
			{
				KilledEvent.Invoke();
			}
		}

		private void ResetBarVisuals()
		{
			_displayingHealthBar = false;
			_healthBarGroup.alpha = 0;
			_healthBarFill.fillAmount = Data != null ? Data.NormalizedHealth : 0f;
			_healthBarGroup.gameObject.SetActive(false);
		}
	}
}