using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class HandComparer
    {
        internal HandComparisonResult CompareHands(IEnumerable<Card> player, IEnumerable<Card> dealer)
        {
            var ps = player.ToList();
            var ds = dealer.ToList();

            if (ps.Count != ds.Count)
                throw new InvalidOperationException("Hand size mismatches.");

            var playerEval = EvaluateHand(ps);
            var dealerEval = EvaluateHand(ds);

            int cmp = CompareHandRanks(playerEval, dealerEval);
            Actor winner = cmp > 0 ? Actor.Player : Actor.Dealer; // player wins if cmp > 0, dealer wins otherwise (including draw)

            return new HandComparisonResult(winner, playerEval, dealerEval);
        }

        private PokerHand EvaluateHand(List<Card> hand)
        {
            var ordered = hand.OrderByDescending(c => c.Rank == 1 ? 14 : c.Rank).ToList();
            var rankGroups = ordered.GroupBy(c => c.Rank == 1 ? 14 : c.Rank)
                                    .OrderByDescending(g => g.Count())
                                    .ThenByDescending(g => g.Key)
                                    .ToList();
            bool isFlush = hand.All(c => c.Suit == hand[0].Suit);
            var ranks = ordered.Select(c => c.Rank == 1 ? 14 : c.Rank).ToList();
            bool isStraight = ranks.Zip(ranks.Skip(1), (a, b) => a - b).All(d => d == 1) ||
                              (ranks.SequenceEqual(new List<int> { 14, 5, 4, 3, 2 })); // Wheel straight

            if (isFlush && ranks.SequenceEqual(new List<int> { 14, 13, 12, 11, 10 }))
                return new PokerHand(PokerHandRank.RoyalFlush, new List<int> { 14 }, new List<int>());
            if (isFlush && isStraight)
                return new PokerHand(PokerHandRank.StraightFlush, new List<int> { ranks.Max() }, new List<int>());
            if (rankGroups[0].Count() == 4)
                return new PokerHand(PokerHandRank.FourOfAKind, new List<int> { rankGroups[0].Key }, new List<int> { rankGroups[1].Key });
            if (rankGroups[0].Count() == 3 && rankGroups[1].Count() == 2)
                return new PokerHand(PokerHandRank.FullHouse, new List<int> { rankGroups[0].Key, rankGroups[1].Key }, new List<int>());
            if (isFlush)
                return new PokerHand(PokerHandRank.Flush, ranks, new List<int>());
            if (isStraight)
                return new PokerHand(PokerHandRank.Straight, new List<int> { ranks.Max() }, new List<int>());
            if (rankGroups[0].Count() == 3)
                return new PokerHand(PokerHandRank.ThreeOfAKind, new List<int> { rankGroups[0].Key }, rankGroups.Skip(1).Select(g => g.Key).ToList());
            if (rankGroups[0].Count() == 2 && rankGroups[1].Count() == 2)
                return new PokerHand(PokerHandRank.TwoPair, new List<int> { rankGroups[0].Key, rankGroups[1].Key }, new List<int> { rankGroups[2].Key });
            if (rankGroups[0].Count() == 2)
                return new PokerHand(PokerHandRank.OnePair, new List<int> { rankGroups[0].Key }, rankGroups.Skip(1).Select(g => g.Key).ToList());
            return new PokerHand(PokerHandRank.HighCard, ranks, new List<int>());
        }

        private int CompareHandRanks(PokerHand a, PokerHand b)
        {
            if (a.Rank != b.Rank)
                return a.Rank.CompareTo(b.Rank);
            for (int i = 0; i < Math.Max(a.RankValues.Count, b.RankValues.Count); i++)
            {
                int av = i < a.RankValues.Count ? a.RankValues[i] : 0;
                int bv = i < b.RankValues.Count ? b.RankValues[i] : 0;
                if (av != bv)
                    return av.CompareTo(bv);
            }
            for (int i = 0; i < Math.Max(a.Kickers.Count, b.Kickers.Count); i++)
            {
                int av = i < a.Kickers.Count ? a.Kickers[i] : 0;
                int bv = i < b.Kickers.Count ? b.Kickers[i] : 0;
                if (av != bv)
                    return av.CompareTo(bv);
            }
            return 0; // Draw
        }
    }
}
