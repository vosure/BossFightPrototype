using Code.Gameplay.Player;
using Code.Infrastructure.Data.StaticData.Weapons;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.WeaponSystem
{
	public class WeaponController : MonoBehaviour
	{
		[SerializeField] private WeaponTypeID weaponToEquip;
		[SerializeField] private Weapon weapon;

		private InputService _input;
		private StaticDataService _staticData;
		private VFXFactory _vfxFactory;
		private GameFactory _gameFactory;

		private void Awake() =>
			InitializeFields();

		public Weapon GetCurrentWeapon() => weapon;

		private WeaponStaticData GetWeaponData()
		{
			_staticData = ServiceLocator.Instance.StaticDataService;
			WeaponStaticData weaponStaticData = _staticData.GetDataForWeapon(weaponToEquip);
			return weaponStaticData;
		}

		private void InitializeFields()
		{
			_input = ServiceLocator.Instance.InputService;
			_vfxFactory = ServiceLocator.Instance.VfxFactory;
			_gameFactory = ServiceLocator.Instance.GameFactory;
			
			PlayerAim playerAim = GetComponent<PlayerAim>();
			
			weapon.Initialize(GetWeaponData(), _gameFactory, playerAim, _vfxFactory);
			
			Subscribe();
		}

		private void OnDisable() => 
			Unsubscribe();


		private void Reload()
		{
			if (HasWeapon())
				weapon.Reload();
		}

		private void OnTriggerHold()
		{
			if (HasWeapon() && IsAiming())
				weapon.OnTriggerHold();
		}

		private void OnTriggerRelease()
		{
			if (HasWeapon() && IsAiming())
				weapon.OnTriggerRelease();
		}

		private bool HasWeapon() => 
			weapon != null;

		private bool IsAiming() => 
			_input.IsAiming;

		private void Subscribe()
		{
			_input.OnTriggerHold += OnTriggerHold;
			_input.OnTriggerReleased += OnTriggerRelease;
			_input.OnReloaded += Reload;
		}

		private void Unsubscribe()
		{
			_input.OnTriggerHold -= OnTriggerHold;
			_input.OnTriggerReleased -= OnTriggerRelease;
			_input.OnReloaded -= Reload;
		}
	}
}