using Code.Gameplay.Player.Movement;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Core.Reward
{
	public class Chest : MonoBehaviour
	{
		private RewardType _reward;

		private WindowService _windowService;
		private void Start() => 
			_windowService = ServiceLocator.Instance.WindowService;

		public void SetRewardInside(RewardType rewardType) => 
			_reward = rewardType;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out PlayerController player)) 
				OpenRewardWindow();
		}

		private void OpenRewardWindow()
		{
			_windowService.OpenRewardWindow(_reward);
			
			Destroy(gameObject);
		}
	}
}