using Code.Core.Reward;
using Code.Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows
{
	public class RewardWindow : WindowBase
	{
		private const string RareItemText = "Congratulations! You recieve a RARE item!";
		private const string CommonItemText = "You recieve a COMMON item! Better luck next time!";
		
		[SerializeField] private Image rewardIconImage;

		[SerializeField] private Sprite commonItemSprite;
		[SerializeField] private Sprite rareItemSprite;
		
		[SerializeField] private TMP_Text rewardText;

		[SerializeField] private Button playAgainButton;

		private RewardType _reward;

		private SceneLoaderService _sceneLoader;
		
		public void SetReward(RewardType rewardType) => 
			_reward = rewardType;

		protected override void OnAwake()
		{
			base.OnAwake();
			_sceneLoader = ServiceLocator.Instance.SceneLoaderService;
		}

		protected override void OnOpen()
		{
			base.OnOpen();
			
			SetRewardVisualData();
		}

		private void SetRewardVisualData()
		{
			SetText();
			SetIcon();
		}

		private void SetText()
		{
			string text = _reward == RewardType.RareItem ? RareItemText : CommonItemText;
			rewardText.text = text;
		}

		private void SetIcon()
		{
			Sprite icon = _reward == RewardType.RareItem ? rareItemSprite : commonItemSprite;
			rewardIconImage.sprite = icon;
		}

		protected override void SetListeners() => 
			playAgainButton.onClick.AddListener(_sceneLoader.LoadGame);

		protected override void RemoveListeners() => 
			playAgainButton.onClick.RemoveAllListeners();
	}
}