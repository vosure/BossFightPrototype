using Code.Infrastructure.Data.StaticData;
using Code.Utility;
using Random = UnityEngine.Random;

// NOTE(vlad): In this implementation the event's chance increases every time it does not occur, 
// but is lower in the first place as compensation. 
// This results in the effects occurring more consistently.
// The probability of an effect to occur on the N-th test since the last successful proc is given by C * N.
// For each instance which could trigger the effect but does not, the probability of the effect happening for the next instance by a constant C. 
// This constant, which is also the initial probability, is lower than the listed probability of the effect it is shadowing. 
// Once the effect occurs, the counter is reset.
// NOTE: On top of that, we have parameters that control the minimum and maximum number of attempts, if we need to tightly control the player when receiving a reward
namespace Code.Core.Reward
{
	public class RewardService
	{
		private readonly RewardType _failReward;
		private readonly RewardType _successReward;

		public RewardService(RewardType failReward, RewardType successReward)
		{
			_failReward = failReward;
			_successReward = successReward;
		}

		public RewardType GetReward(RewardStaticData rewardStaticData, int attemptsBefore)
		{
			int currentAttempts = attemptsBefore + 1;

			if (IsMinAttemptsNotReached(currentAttempts, rewardStaticData.MinAttempts))
				return _failReward;
			if (IsMaxAttemptsPassed(currentAttempts, rewardStaticData.MaxAttempts))
				return _successReward;

			double chanceToGetRareItem = CalculateCurrentChance(rewardStaticData.NominalChanceToGetRareItem, currentAttempts);

			return HitChance(chanceToGetRareItem) ? _successReward : _failReward;
		}

		private bool HitChance(double chance) => 
			Random.value <= chance;

		private double CalculateCurrentChance(double nominalChance, int attempts)
		{
			double constant = (double) MathUtils.GetProbabilityConstantForNominalChance((decimal) nominalChance);
			return constant * attempts;
		}

		private bool IsMinAttemptsNotReached(int current, int min) =>
			current < min;

		private bool IsMaxAttemptsPassed(int current, int max) =>
			current >= max;
	}
}