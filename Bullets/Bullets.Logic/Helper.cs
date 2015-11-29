using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldem.Logic.Cards;
using TexasHoldem.Logic.Helpers;

namespace Bullets.Logic
{
	internal static class Helper
	{
		internal static TypePair GetTypePair(ICollection<Card> community, ICollection<Card> cards)
		{
			var hand = Helpers.GetHandRank(cards);
			if (hand == TexasHoldem.Logic.HandRankType.Pair)
			{
				var cardType = cards.FirstOrDefault().Type;
			}

			throw new NotImplementedException();
		}

		internal enum TypePair
		{
			UnderPair = 0,
			BottomPair = 1,
			MiddlePair = 2,
			TopPair = 3,
			OverPair = 4
		}
	}
}
