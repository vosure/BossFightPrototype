using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.Services
{
	public class SceneLoaderService : MonoBehaviour
	{
		private const string GameSceneName = "Game";
		private const string MenuSceneName = "Menu";

		public void LoadMenu() => 
			LoadScene(MenuSceneName);

		public void LoadGame() => 
			LoadScene(GameSceneName);

		private void LoadScene(string sceneName) => 
			SceneManager.LoadScene(sceneName);
	}
}