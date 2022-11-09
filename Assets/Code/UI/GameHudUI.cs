using Code.Gameplay.DamageSystem;
using Code.Gameplay.Dragon.Health;
using Code.Gameplay.Player;
using Code.Gameplay.WeaponSystem;
using Code.Infrastructure;
using Code.Infrastructure.Services;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
	public class GameHudUI : MonoBehaviour
	{
		[SerializeField] private TMP_Text bulletsText;
		[SerializeField] private Image playerHealthBar;
		[SerializeField] private Image dragonHealthBar;
		[SerializeField] private Image crosshair;

		private int _bulletsRemainingInMagazine;
		private int _bulletsPerMagazine;
		
		private int _maxPlayerHealth;
		private int _maxDragonHealth;
		
		private InputService _input;
		private Weapon _weapon;
		
		private LivingEntity _playerHealth;
		private LivingEntity _dragonHealth;
	
		private void Start() =>
			InitializeFields();
	
		private void InitializeFields()
		{
			_input = ServiceLocator.Instance.InputService;
			_weapon = FindObjectOfType<WeaponController>().GetCurrentWeapon();
			_playerHealth = FindObjectOfType<PlayerHealth>();
			_dragonHealth = FindObjectOfType<DragonHealth>();
			
			SetUpWeaponUI();
			SetUpPlayerHealthUI();
			
			Subscribe();
		}

		private void SetUpWeaponUI()
		{
			_bulletsPerMagazine = _weapon.GetBulletsPerMagazine();
			bulletsText.text = GetBulletsText(_bulletsPerMagazine, _bulletsPerMagazine);
		}

		private void SetUpPlayerHealthUI()
		{
			_maxPlayerHealth = _playerHealth.StartingHealth;
			playerHealthBar.fillAmount = 1.0f;

			_maxDragonHealth = _dragonHealth.StartingHealth;
			dragonHealthBar.fillAmount = 1.0f;
		}

		private void OnDisable() =>
			Unsubscribe();

		private void AnimateCrosshairFade(float targetFade)
		{
			crosshair.DOFade(targetFade, 0.5f);
		}

		private void UpdateBulletsInMagazineText(int amount) => 
			bulletsText.text = GetBulletsText(amount, _bulletsPerMagazine);

		private void UpdatePlayerHealthBar(int value) => 
			UpdateHealthBar(playerHealthBar, value, _maxPlayerHealth);

		private void UpdateDragonHealthBar(int value) => 
			UpdateHealthBar(dragonHealthBar, value, _maxDragonHealth);

		private void UpdateHealthBar(Image target, int value, int maxValue)
		{
			float remainingHealthPercent = ((float)value / maxValue);

			target.DOFillAmount(remainingHealthPercent, 1.0f);
		}

		private void Subscribe()
		{
			_input.OnAimStarted += OnAimStarted;
			_input.OnAimEnded += OnAimEnded;
			
			_weapon.OnBulletsInMagazineChanged += UpdateBulletsInMagazineText;
			
			_playerHealth.OnHealthChanged += UpdatePlayerHealthBar;
			_dragonHealth.OnHealthChanged += UpdateDragonHealthBar;
		}


		private void OnAimStarted() => 
			AnimateCrosshairFade(1);

		private void OnAimEnded() => 
			AnimateCrosshairFade(0);

		private string GetBulletsText(int amount, int max) => 
			amount + " / " + max;

		private void Unsubscribe()
		{
			_input.OnAimStarted -= OnAimStarted;
			_input.OnAimEnded -= OnAimEnded;
			
			_weapon.OnBulletsInMagazineChanged -= UpdateBulletsInMagazineText;
			
			_playerHealth.OnHealthChanged -= UpdatePlayerHealthBar;
			_dragonHealth.OnHealthChanged -= UpdateDragonHealthBar;
		}
	}
}
