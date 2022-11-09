using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.Data.StaticData;
using Code.Infrastructure.Data.StaticData.Weapons;
using UnityEngine;

namespace Code.Infrastructure.Services
{
	public class StaticDataService : MonoBehaviour
	{
		private const string WeaponsStaticDataPath = "StaticData/Weapons";
		private const string RewardStaticDataPath = "StaticData/Rewards/ChestRewardData";

		private Dictionary<WeaponTypeID, WeaponStaticData> _weapons;
		private RewardStaticData _rewardStaticData;

		private void LoadWeapons()
		{
			_weapons = Resources
				.LoadAll<WeaponStaticData>(WeaponsStaticDataPath)
				.ToDictionary(x => x.TypeID, x => x);
		}

		private void LoadRewardStaticData() =>
			_rewardStaticData = Resources.Load<RewardStaticData>(RewardStaticDataPath);

		public WeaponStaticData GetDataForWeapon(WeaponTypeID typeID)
		{
			if (_weapons == null)
				LoadWeapons();

			return _weapons.TryGetValue(typeID, out WeaponStaticData staticData)
				? staticData
				: null;
		}

		public RewardStaticData GetDataForReward()
		{
			if (_rewardStaticData == null)
				LoadRewardStaticData();

			return _rewardStaticData != null ? _rewardStaticData : null;
		}
	}
}