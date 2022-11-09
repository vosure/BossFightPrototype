using System;
using UnityEngine;

namespace Code.Infrastructure.Services
{
	public class PlayerProgressService : MonoBehaviour
	{
		private const string FailAttemptsPlayerPrefsKey = "FailAttemptsToGetRareReward";
		
		private PlayerProgressData _progress;

		public PlayerProgressData Progress => _progress;
		private void Start() => 
			Load();

		public void IncrementAttempts()
		{
			_progress.FailAttemptsToGetRareReward++;
			
			Save();
		}

		public void ResetAttempts()
		{
			_progress.FailAttemptsToGetRareReward = 0;
			
			Save();
		}

		private void Save()
		{
			PlayerPrefs.SetInt(FailAttemptsPlayerPrefsKey, _progress.FailAttemptsToGetRareReward);
			PlayerPrefs.Save();
		}

		private void Load()
		{
			int attempts = PlayerPrefs.GetInt(FailAttemptsPlayerPrefsKey, 0);

			_progress = new PlayerProgressData(attempts);
		}

		private void OnApplicationQuit() => 
			Save();
	}

	public class PlayerProgressData
	{
		public int FailAttemptsToGetRareReward;

		public PlayerProgressData(int failAttemptsToGetRareReward) => 
			FailAttemptsToGetRareReward = failAttemptsToGetRareReward;
	}
}