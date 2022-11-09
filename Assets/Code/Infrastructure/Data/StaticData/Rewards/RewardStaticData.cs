using UnityEngine;

namespace Code.Infrastructure.Data.StaticData
{
	[CreateAssetMenu(fileName = "RewardData", menuName = "StaticData/RewardStaticData", order = 0)]
	public class RewardStaticData : ScriptableObject
	{
		public double NominalChanceToGetRareItem;
		public int MinAttempts;
		public int MaxAttempts;
	}
}