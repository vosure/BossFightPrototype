using UnityEngine;

namespace Code.Gameplay.DamageSystem
{
	public abstract class LivingEntity : MonoBehaviour, IDamageable, IHealth
	{
		[SerializeField] private int startingHealth;

		private int _currentHealth;

		public int CurrentHealth
		{
			get => _currentHealth;
			set
			{
				_currentHealth = value;

				if (CurrentHealth == startingHealth) // HACK(vlad): To Not Invoke Event While Initialization
					return;
				
				OnHealthChanged?.Invoke(_currentHealth);
				
				if (CurrentHealth <= 0 && !IsDead)
					Die();
			}
		}
		
		public int StartingHealth => startingHealth;
		public bool IsDead { get; set; }
		
		public event System.Action OnDeath;
		public event System.Action<int> OnHealthChanged;

		private void Start() => 
			InitializeFields();

		private void InitializeFields() => 
			CurrentHealth = startingHealth;

		public virtual void TakeDamage(int damage) => 
			CurrentHealth -= damage;

		public void Die()
		{
			IsDead = true;
			OnDie();
			OnDeath?.Invoke();
		}

		protected abstract void OnDie();
	}
}