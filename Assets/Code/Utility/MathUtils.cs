using System;

namespace Code.Utility
{
	public static class MathUtils
	{
		// NOTE: https://gaming.stackexchange.com/questions/161430/calculating-the-constant-c-in-dota-2-pseudo-random-distribution#comment217055_162047
		public static decimal GetProbabilityConstantForNominalChance( decimal p )
		{
			decimal Cupper = p;
			decimal Clower = 0m;
			decimal Cmid;
			decimal p1;
			decimal p2 = 1m;
			while(true)
			{
				Cmid = ( Cupper + Clower ) / 2m;
				p1 = PfromC( Cmid );
				if ( Math.Abs( p1 - p2 ) <= 0m ) break;

				if ( p1 > p )
				{
					Cupper = Cmid;
				}
				else
				{
					Clower = Cmid;
				}

				p2 = p1;
			}

			return Cmid;
		}

		private static decimal PfromC( decimal C )
		{
			decimal pProcOnN = 0m;
			decimal pProcByN = 0m;
			decimal sumNpProcOnN = 0m;

			int maxFails = (int)Math.Ceiling( 1m / C );
			for (int N = 1; N <= maxFails; ++N)
			{
				pProcOnN = Math.Min( 1m, N * C ) * (1m - pProcByN);
				pProcByN += pProcOnN;
				sumNpProcOnN += N * pProcOnN;
			}

			return ( 1m / sumNpProcOnN );
		}
	}
}