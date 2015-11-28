using TexasHoldem.Logic.Cards;

namespace Bullets.Logic.TablesForGoodCards
{
	public static class HandStrengthValuation
	{
		private const int MaxCardTypeValue = 14;

		private static readonly int[,] StartingHandRecommendationsNormalMoney =
			{
				{ 2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, // AA AKs AQs AJs ATs A9s A8s A7s A6s A5s A4s A3s A2s
                { 1, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, // AKo KK KQs KJs KTs K9s K8s K7s K6s K5s K4s K3s K2s
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, // AQo KQo QQ QJs QTs Q9s Q8s Q7s Q6s Q5s Q4s Q3s Q2s
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, // AJo KJo QJo JJ JTs J9s J8s J7s J6s J5s J4s J3s J2s
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, // ATo KTo QTo JTo TT T9s T8s T7s T6s T5s T4s T3s T2s
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, // A9o K9o Q9o J9o T9o 99 98s 97s 96s 95s 94s 93s 92s
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, // A8o K8o Q8o J8o T8o 98o 88 87s 86s 85s 84s 83s 82s
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, // A7o K7o Q7o J7o T7o 97o 87o 77 76s 75s 74s 73s 72s
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, // A6o K6o Q6o J6o T6o 96o 86o 76o 66 65s 64s 63s 62s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, // A5o K5o Q5o J5o T5o 95o 85o 75o 65o 55 54s 53s 52s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, // A4o K4o Q4o J4o T4o 94o 84o 74o 64o 54o 44 43s 42s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, // A3o K3o Q3o J3o T3o 93o 83o 73o 63o 53o 43o 33 32s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 } // A2o K2o Q2o J2o T2o 92o 82o 72o 62o 52o 42o 32o 22
            };

		private static readonly int[,] StartingHandRecommendationsLessThan500Money =
			{
				{ 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, // AA AKs AQs AJs ATs A9s A8s A7s A6s A5s A4s A3s A2s
                { 1, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // AKo KK KQs KJs KTs K9s K8s K7s K6s K5s K4s K3s K2s
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, // AQo KQo QQ QJs QTs Q9s Q8s Q7s Q6s Q5s Q4s Q3s Q2s
                { 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, // AJo KJo QJo JJ JTs J9s J8s J7s J6s J5s J4s J3s J2s
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, // ATo KTo QTo JTo TT T9s T8s T7s T6s T5s T4s T3s T2s
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, // A9o K9o Q9o J9o T9o 99 98s 97s 96s 95s 94s 93s 92s
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, // A8o K8o Q8o J8o T8o 98o 88 87s 86s 85s 84s 83s 82s
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, // A7o K7o Q7o J7o T7o 97o 87o 77 76s 75s 74s 73s 72s
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, // A6o K6o Q6o J6o T6o 96o 86o 76o 66 65s 64s 63s 62s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, // A5o K5o Q5o J5o T5o 95o 85o 75o 65o 55 54s 53s 52s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // A4o K4o Q4o J4o T4o 94o 84o 74o 64o 54o 44 43s 42s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // A3o K3o Q3o J3o T3o 93o 83o 73o 63o 53o 43o 33 32s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } // A2o K2o Q2o J2o T2o 92o 82o 72o 62o 52o 42o 32o 22
            };

		private static readonly int[,] StartingHandRecommendationsGreaterThan500Money =
			{
				{ 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // AA AKs AQs AJs ATs A9s A8s A7s A6s A5s A4s A3s A2s
                { 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // AKo KK KQs KJs KTs K9s K8s K7s K6s K5s K4s K3s K2s
                { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // AQo KQo QQ QJs QTs Q9s Q8s Q7s Q6s Q5s Q4s Q3s Q2s
                { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // AJo KJo QJo JJ JTs J9s J8s J7s J6s J5s J4s J3s J2s
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, // ATo KTo QTo JTo TT T9s T8s T7s T6s T5s T4s T3s T2s
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, // A9o K9o Q9o J9o T9o 99 98s 97s 96s 95s 94s 93s 92s
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, // A8o K8o Q8o J8o T8o 98o 88 87s 86s 85s 84s 83s 82s
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, // A7o K7o Q7o J7o T7o 97o 87o 77 76s 75s 74s 73s 72s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // A6o K6o Q6o J6o T6o 96o 86o 76o 66 65s 64s 63s 62s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // A5o K5o Q5o J5o T5o 95o 85o 75o 65o 55 54s 53s 52s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // A4o K4o Q4o J4o T4o 94o 84o 74o 64o 54o 44 43s 42s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // A3o K3o Q3o J3o T3o 93o 83o 73o 63o 53o 43o 33 32s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } // A2o K2o Q2o J2o T2o 92o 82o 72o 62o 52o 42o 32o 22
            };

		public static CardValuationType PreFlopNormal(Card firstCard, Card secondCard)
		{
			var value = firstCard.Suit == secondCard.Suit
						  ? (firstCard.Type > secondCard.Type
								 ? StartingHandRecommendationsNormalMoney[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
								 : StartingHandRecommendationsNormalMoney[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
						  : (firstCard.Type > secondCard.Type
								 ? StartingHandRecommendationsNormalMoney[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]
								 : StartingHandRecommendationsNormalMoney[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]);

			switch (value)
			{
				case 0:
					return CardValuationType.Unplayable;
				case 1:
					return CardValuationType.Playable;
				case 2:
					return CardValuationType.BesthHand;
				default:
					return CardValuationType.Unplayable;
			}
		}

		public static CardValuationType PreFlopGreaterThan500Money(Card firstCard, Card secondCard)
		{
			var value = firstCard.Suit == secondCard.Suit
						  ? (firstCard.Type > secondCard.Type
								 ? StartingHandRecommendationsGreaterThan500Money[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
								 : StartingHandRecommendationsGreaterThan500Money[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
						  : (firstCard.Type > secondCard.Type
								 ? StartingHandRecommendationsGreaterThan500Money[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]
								 : StartingHandRecommendationsGreaterThan500Money[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]);

			switch (value)
			{
				case 0:
					return CardValuationType.Unplayable;
				case 1:
					return CardValuationType.Playable;
				case 2:
					return CardValuationType.BesthHand;
				default:
					return CardValuationType.Unplayable;
			}
		}

		public static CardValuationType PreFlopLessThan500Money(Card firstCard, Card secondCard)
		{
			var value = firstCard.Suit == secondCard.Suit
						  ? (firstCard.Type > secondCard.Type
								 ? StartingHandRecommendationsLessThan500Money[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
								 : StartingHandRecommendationsLessThan500Money[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
						  : (firstCard.Type > secondCard.Type
								 ? StartingHandRecommendationsLessThan500Money[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]
								 : StartingHandRecommendationsLessThan500Money[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]);

			switch (value)
			{
				case 0:
					return CardValuationType.Unplayable;
				case 1:
					return CardValuationType.Playable;
				case 2:
					return CardValuationType.BesthHand;
				default:
					return CardValuationType.Unplayable;
			}
		}
	}
}
