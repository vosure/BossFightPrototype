namespace Code.Gameplay.Dragon.Attack.AttackSystem
{
	public interface IAttack
	{
		public AttackType AttackType { get; }

		public float GetAttackTime();
		public void PerformAttack();
	}
}