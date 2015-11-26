using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Cards;

namespace Bullets.Logic
{
	// http://poker.srv.ualberta.ca/preflop (call)
	public static class EvaluationAfterOpponentsCall
	{
		private const int MaxCardTypeValue = 14;
		
		private static readonly float[,] Raise =
			{
				{ 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f }, // AA AKs AQs AJs ATs A9s A8s A7s A6s A5s A4s A3s A2s
                { 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.8772f, 0.6048f }, // AKo KK KQs KJs KTs K9s K8s K7s K6s K5s K4s K3s K2s
                { 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.8328f, 0.5753f }, // AQo KQo QQ QJs QTs Q9s Q8s Q7s Q6s Q5s Q4s Q3s Q2s
                { 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.9235f, 0.6947f, 0.5167f }, // AJo KJo QJo JJ JTs J9s J8s J7s J6s J5s J4s J3s J2s
                { 1.0000f, 1.0000f, 0.9972f, 0.9948f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.7528f, 0.7099f, 0.5500f, 0.3808f }, // ATo KTo QTo JTo TT T9s T8s T7s T6s T5s T4s T3s T2s
                { 1.0000f, 0.9859f, 0.8491f, 0.8554f, 0.9773f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.8626f, 0.4807f, 0.4021f, 0.2028f }, // A9o K9o Q9o J9o T9o 99 98s 97s 96s 95s 94s 93s 92s
                { 1.0000f, 0.7759f, 0.6450f, 0.6327f, 0.7209f, 0.8242f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.6933f, 0.2071f, 0.0391f }, // A8o K8o Q8o J8o T8o 98o 88 87s 86s 85s 84s 83s 82s
                { 0.9999f, 0.7727f, 0.4445f, 0.4214f, 0.5185f, 0.5897f, 0.7605f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.4937f, 0.0000f }, // A7o K7o Q7o J7o T7o 97o 87o 77 76s 75s 74s 73s 72s
                { 0.7815f, 0.7124f, 0.5057f, 0.2837f, 0.3184f, 0.3836f, 0.5100f, 0.7116f, 1.0000f, 1.0000f, 1.0000f, 0.7735f, 0.2608f }, // A6o K6o Q6o J6o T6o 96o 86o 76o 66 65s 64s 63s 62s
                { 1.0000f, 0.6066f, 0.4731f, 0.3662f, 0.1543f, 0.1663f, 0.2978f, 0.4721f, 0.6113f, 1.0000f, 1.0000f, 1.0000f, 0.4895f }, // A5o K5o Q5o J5o T5o 95o 85o 75o 65o 55 54s 53s 52s
                { 0.7683f, 0.4164f, 0.3760f, 0.2748f, 0.0991f, 0.0000f, 0.0000f, 0.2112f, 0.4046f, 0.5617f, 1.0000f, 0.9563f, 0.4217f }, // A4o K4o Q4o J4o T4o 94o 84o 74o 64o 54o 44 43s 42s
                { 0.5613f, 0.2484f, 0.2254f, 0.0975f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.0194f, 0.2863f, 0.2024f, 1.0000f, 0.2793f }, // A3o K3o Q3o J3o T3o 93o 83o 73o 63o 53o 43o 33 32s
                { 0.3425f, 0.0430f, 0.0217f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.1579f } // A2o K2o Q2o J2o T2o 92o 82o 72o 62o 52o 42o 32o 22
            };

		private static readonly float[,] Call =
			{
				{ 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f }, // AA AKs AQs AJs ATs A9s A8s A7s A6s A5s A4s A3s A2s
                { 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.1228f, 0.3952f }, // AKo KK KQs KJs KTs K9s K8s K7s K6s K5s K4s K3s K2s
                { 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.1672f, 0.4247f }, // AQo KQo QQ QJs QTs Q9s Q8s Q7s Q6s Q5s Q4s Q3s Q2s
                { 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0765f, 0.3053f, 0.4833f }, // AJo KJo QJo JJ JTs J9s J8s J7s J6s J5s J4s J3s J2s
                { 0.0000f, 0.0000f, 0.0028f, 0.0052f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.2472f, 0.2901f, 0.4500f, 0.6192f }, // ATo KTo QTo JTo TT T9s T8s T7s T6s T5s T4s T3s T2s
                { 0.0000f, 0.0141f, 0.1509f, 0.1446f, 0.0227f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.1374f, 0.5193f, 0.5979f, 0.7972f }, // A9o K9o Q9o J9o T9o 99 98s 97s 96s 95s 94s 93s 92s
                { 0.0000f, 0.2241f, 0.3550f, 0.3673f, 0.2791f, 0.1758f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.3067f, 0.7929f, 0.9609f }, // A8o K8o Q8o J8o T8o 98o 88 87s 86s 85s 84s 83s 82s
                { 0.0001f, 0.2273f, 0.5555f, 0.5786f, 0.4815f, 0.4103f, 0.2395f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.5063f, 1.0000f }, // A7o K7o Q7o J7o T7o 97o 87o 77 76s 75s 74s 73s 72s
                { 0.2185f, 0.2876f, 0.4943f, 0.7163f, 0.6816f, 0.6164f, 0.4900f, 0.2884f, 0.0000f, 0.0000f, 0.0000f, 0.2265f, 0.7392f }, // A6o K6o Q6o J6o T6o 96o 86o 76o 66 65s 64s 63s 62s
                { 0.0000f, 0.3934f, 0.5269f, 0.6338f, 0.8457f, 0.8337f, 0.7022f, 0.5279f, 0.3887f, 0.0000f, 0.0000f, 0.0000f, 0.5105f }, // A5o K5o Q5o J5o T5o 95o 85o 75o 65o 55 54s 53s 52s
                { 0.2317f, 0.5836f, 0.6240f, 0.7252f, 0.9009f, 1.0000f, 1.0000f, 0.7888f, 0.5954f, 0.4383f, 0.0000f, 0.0437f, 0.5783f }, // A4o K4o Q4o J4o T4o 94o 84o 74o 64o 54o 44 43s 42s
                { 0.4387f, 0.7516f, 0.7746f, 0.9025f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.9806f, 0.7137f, 0.7976f, 0.0000f, 0.7207f }, // A3o K3o Q3o J3o T3o 93o 83o 73o 63o 53o 43o 33 32s
                { 0.6575f, 0.9570f, 0.9783f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.8421f } // A2o K2o Q2o J2o T2o 92o 82o 72o 62o 52o 42o 32o 22
            };

		public static float RaisePercentage(Card firstCard, Card secondCard)
		{
			var value = firstCard.Suit == secondCard.Suit
						  ? (firstCard.Type > secondCard.Type
								 ? Raise[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
								 : Raise[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
						  : (firstCard.Type > secondCard.Type
								 ? Raise[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]
								 : Raise[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]);

			return value;
		}
	}
}
