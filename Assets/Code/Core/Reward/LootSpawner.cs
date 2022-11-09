using Code.Core.Game;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Core.Reward
{
	public class LootSpawner : MonoBehaviour
	{
		private const RewardType FailReward = RewardType.CommonItem;
		private const RewardType SuccessReward = RewardType.RareItem;
		
		[SerializeField] private Transform lootSpawnPoint;

		private RewardService _rewardService;
		private GameFactory _gameFactory;
		private BattleStateService _battleStateService;
		private StaticDataService _staticDataService;
		private PlayerProgressService _playerProgress;

		private void Start() => 
			Initialize();

		private void OnDisable() => 
			Unsubscribe();

		private void Initialize()
		{
			GetServices();
			Subscribe();
		}

		private void GetServices()
		{
			_battleStateService = FindObjectOfType<BattleStateService>();
			_gameFactory = ServiceLocator.Instance.GameFactory;
			_staticDataService = ServiceLocator.Instance.StaticDataService;
			_playerProgress = ServiceLocator.Instance.PlayerProgress;
			_rewardService = new RewardService(FailReward, SuccessReward);
		}

		private void Subscribe() => 
			_battleStateService.OnPlayerWin += SpawnLoot;

		private void SpawnLoot()
		{
			Chest chest = _gameFactory.SpawnChest(lootSpawnPoint.position, lootSpawnPoint.rotation);
			RewardType rewardType = _rewardService.GetReward(_staticDataService.GetDataForReward(),
				_playerProgress.Progress.FailAttemptsToGetRareReward);

			chest.SetRewardInside(rewardType);
			UpdatePlayerAttempts(rewardType);
		}

		private void UpdatePlayerAttempts(RewardType rewardType)
		{
			if (rewardType == RewardType.RareItem)
				_playerProgress.ResetAttempts();
			else
				_playerProgress.IncrementAttempts();
		}

		private void Unsubscribe() => 
			_battleStateService.OnPlayerWin -= SpawnLoot;
	}
}
