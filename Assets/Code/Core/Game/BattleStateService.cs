using System;
using Code.Gameplay.DamageSystem;
using Code.Gameplay.Dragon.Health;
using Code.Gameplay.Player;
using Code.Infrastructure.Services;
using Code.UI.Windows;
using UnityEngine;

namespace Code.Core.Game
{
	public class BattleStateService : MonoBehaviour
	{
		public event Action OnPlayerWin;
		public event Action OnPlayerLose;

		private LivingEntity _player;
		private LivingEntity _dragon;

		private WindowService _windowService;

		private void Start() => 
			_windowService = ServiceLocator.Instance.WindowService;

		private void OnEnable() => 
			Subscribe();

		private void OnDisable() => 
			Unsubscribe();

		private void Subscribe()
		{
			_player = FindObjectOfType<PlayerHealth>();
			_dragon = FindObjectOfType<DragonHealth>();
			
			_player.OnDeath += OnPlayerDeath;
			_dragon.OnDeath += OnDragonDeath;
		}

		private void OnDragonDeath() => 
			OnPlayerWin?.Invoke();

		private void OnPlayerDeath()
		{
			OnPlayerLose?.Invoke();
			
			_windowService.Open(WindowType.LoseWindow);
		}

		private void Unsubscribe()
		{
			_player.OnDeath -= OnPlayerDeath;
			_dragon.OnDeath -= OnDragonDeath;
		}
	}
}
