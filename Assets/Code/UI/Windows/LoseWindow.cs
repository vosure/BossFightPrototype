using Code.Infrastructure.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows
{
	public class LoseWindow : WindowBase
	{
		[SerializeField] private Button playAgainButton;

		private SceneLoaderService _sceneLoader;

		protected override void OnAwake()
		{
			base.OnAwake();
			_sceneLoader = ServiceLocator.Instance.SceneLoaderService;
		}
		protected override void SetListeners() => 
			playAgainButton.onClick.AddListener(_sceneLoader.LoadGame);

		protected override void RemoveListeners() => 
			playAgainButton.onClick.RemoveAllListeners();
	}
}