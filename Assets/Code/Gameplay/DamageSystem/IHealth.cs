namespace Code.Gameplay.DamageSystem
{
	public interface IHealth
	{
		public int CurrentHealth { get; set; }
		public bool IsDead { get; set; }

		public void Die();
	}
}