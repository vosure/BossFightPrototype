using System.Linq;
using Code.Core.Reward;
using Code.UI.Windows;
using UnityEngine;

namespace Code.Infrastructure.Services
{
	public class WindowService : MonoBehaviour
	{
		[SerializeField] private WindowBase[] windows;

		public void OpenRewardWindow(RewardType rewardType)
		{
			if (Get(WindowType.RewardWindow) is RewardWindow rewardWindow)
			{
				rewardWindow.SetReward(rewardType);
				rewardWindow.Open();
			}
		}

		public void Open(WindowType windowType) =>
			Get(windowType).Open();

		public void Close(WindowType windowType) =>
			Get(windowType).Close();

		private WindowBase Get(WindowType windowType) =>
			windows.First(w => w.WindowType == windowType);
	}
}