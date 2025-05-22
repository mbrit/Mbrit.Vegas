
using BootFX.Common;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Security.Principal;

namespace Mbrit.Vegas.Utility
{
    internal class UthFoo
    {
        internal void DoMagic()
        {
            const int bankroll = 1500;
            const int numPlays = 10000;
            const int totalHands = 30;
            const int ante = 25;

            var winTarget = (int)(.8 * (8 * ante));       // ante + blind + 4x raise...

            var cashOutAt = bankroll + winTarget;
            var quitAt = 0; //  bankroll - 500;

            var results = new List<BankrollResults>();
            for(var index = 0; index < numPlays; index++)
            {
                var result = this.Play(bankroll, ante, totalHands, cashOutAt, quitAt);
                results.Add(result);
            }

            /*
            var table = new ConsoleTable();
            foreach (var result in results)
                table.AddRow(result.Bankroll, result.MaxBankroll, result.MinBankroll, result.Busted);
            table.Render();
            */

            Console.WriteLine("====================================");
            Console.WriteLine("Bankroll: " + bankroll);
            Console.WriteLine("Average: " + results.Select(v => v.Bankroll).Average());

            var averageWager = results.Select(v => v.TotalWager).Average();
            Console.WriteLine("Average wager: " + averageWager);
            Console.WriteLine("Tier points: " + averageWager / 25);

            // wins...
            var winLevel = 1M - (bankroll / winTarget);
            var wins = results.Where(v => v.Bankroll >= cashOutAt).ToList();

            var numWon = wins.Count();
            var percentWon = 0M;
            if (numWon > 0)
                percentWon = (decimal)numWon / (decimal)numPlays;

            var averageWinHands = 0;
            if (wins.Any())
                averageWinHands = (int)Math.Ceiling(wins.Average(v => v.HandsPlayed));

            Console.WriteLine($"Num won (↑${cashOutAt}): " + numWon + " of " + numPlays + ", " + (percentWon * 100).ToString("n2") + $"%, hands until stop: {averageWinHands}");

            // losses...
            var losses = results.Where(v => v.Bankroll - winTarget <= quitAt);

            var numWasted = losses.Count();
            var percentWasted = 0M;
            if (numWasted > 0)
                percentWasted = (decimal)numWasted / (decimal)numPlays;

            var averageStopHands = 0;
            if(losses.Any())
                averageStopHands = (int)Math.Ceiling(losses.Average(v => v.HandsPlayed));

            Console.WriteLine($"Num lost (↓${quitAt}): " + numWasted + " of " + numPlays + ", " + (percentWasted * 100).ToString("n2") + $"%, hands until stop: {averageStopHands}");

            // others...
            var others = new List<BankrollResults>();
            foreach(var result in results)
            {
                if (!(wins.Contains(result)) && !(losses.Contains(result)))
                    others.Add(result);
            }

            var numOthers = others.Count();
            var percentOthers = 0M;
            if (numOthers > 0)
                percentOthers = (decimal)numOthers / (decimal)numPlays;

            //            Console.WriteLine($"Num other: " + others.Count + " of " + numPlays + ", " + (percentOthers * 100).ToString("n2") + $"%, average -- $" + others.Average(v => v.Bankroll).ToString("n0"));
        }

        internal BankrollResults Play(int bankroll, int ante, int totalHands, int winTarget, int stopTarget, bool trace = false)
        {
            //Console.WriteLine(">>> " + bankroll + ", " + winTarget + ", " + stopTarget);

            Deck deck = null;
            IEnumerator<Card> enumerator = null;

            Func<Card> draw = () =>
            {
                if (deck == null || enumerator == null || !(enumerator.MoveNext()))
                {
                    deck = new Deck();
                    enumerator = deck.GetEnumerator();
                    enumerator.MoveNext();
                }

                return enumerator.Current;
            };

            Func<int, List<Card>> drawN = (count) =>
            {
                var cards = new List<Card>();
                for (var index = 0; index < count; index++)
                    cards.Add(draw());

                // sort by rank...
                cards.Sort(new CardComparer());

                return cards;
            };

            var blind = ante;

            var num4x = 0;
            var num2x = 0;
            var num1x = 0;
            var numFolds = 0;

            var numPlayerWins = 0;
            var numDealerWins = 0;
            var numPushes = 0;

            var comparer = new HandComparer();

            var bankrollMin = bankroll;
            var bankrollMax = bankroll;

            var totalWager = 0;
            var handsPlayed = 0;

            for (var index = 0; index < totalHands; index++)
            {
                var player = drawN(2);
                var dealer = drawN(2);
                var community = drawN(5);

                if(trace)
                    ConsoleHelper.WriteBanner(index.ToString() + " -- $" + bankroll);

                var bankrollBefore = bankroll;

                var antePlusBlind = ante + blind;
                totalWager += ante + blind;

                // can we bet?
                if (bankroll - (antePlusBlind + (4 * ante)) < stopTarget)
                {
                    //Console.WriteLine(" -- quit, " + bankroll);
                    return new BankrollResults(bankroll, bankrollMin, bankrollMax, totalWager, handsPlayed);
                }

                bankroll -= antePlusBlind;

                if (trace)
                    Console.WriteLine("Player --> " + this.Format(player));

                if(trace)
                    Console.WriteLine("Play -- $" + antePlusBlind);

                if(trace)
                    Console.WriteLine("Bankroll after play -- $" + bankroll);

                // first street...
                var p1 = player[0];
                var p2 = player[1];

                var betUnits = 0;

                // get first three community cards cards...
                var c1 = community[0];
                var c2 = community[1];
                var c3 = community[2];

                var flop = community.Take(3).ToList();
                var river = community.TakeLast(2).ToList();

                if (((p1.Rank == p2.Rank) && p1.Rank >= 3) ||
                    p1.Rank == 14 ||
                    p1.Rank == 13 && (p2.Rank >= 5) ||
                    p1.Rank == 12 && (p2.Rank >= 8) ||
                    p1.Rank == 11 && (p2.Rank == 10))
                {
                    if (trace)
                        Console.WriteLine($"4x on first street...");

                    betUnits = 4;
                    num4x++;

                    if (trace)
                        Console.WriteLine("Community --> " + this.Format(community));
                }
                else
                {
                    if (trace)
                        Console.WriteLine("Flop --> " + this.Format(flop));

                    // now if we have a hidden pair, do 2x...
                    var hiddens = this.GetHiddenPairs(player, flop);
                    if (hiddens.Count() == 2 || (hiddens.Count() == 1 && hiddens.First().First().Rank != 2))
                    {
                        if (trace)
                            Console.WriteLine("Bet 2x on second street...");

                        betUnits = 2;
                        num2x++;
                    }
                    else
                    {
                        if (trace)
                            Console.WriteLine("River --> " + this.Format(river));

                        hiddens = this.GetHiddenPairs(player, community);
                        if (hiddens.Any())
                        {
                            if (trace)
                                Console.WriteLine("Bet 1x on third street...");

                            betUnits = 1;
                            num1x++;
                        }
                        else
                        {
                            if (trace)
                                Console.WriteLine("Fold...");

                            //Console.Write("f");
                            numFolds++;
                        }
                    }
                }

                // remove the bet...
                if (betUnits > 0)
                {
                    var bet = betUnits * ante;
                    totalWager += bet;

                    bankroll -= bet;

                    if (trace)
                        Console.WriteLine("Bet -- $" + bet);

                    if (trace)
                        Console.WriteLine("Bankroll after bet -- $" + bankroll);

                    // check...
                    var playerCombined = new List<Card>(player);
                    playerCombined.AddRange(community);
                    playerCombined.Sort(new CardComparer());

                    var dealerCombined = new List<Card>(dealer);
                    dealerCombined.AddRange(community);
                    dealerCombined.Sort(new CardComparer());

                    if (trace)
                        Console.WriteLine("Dealer --> " + this.Format(dealer));

                    var result = comparer.CompareHands(playerCombined, dealerCombined);

                    if (trace)
                    {
                        Console.WriteLine("Player position --> " + this.Format(playerCombined) + ", " + result.PlayerHand.ToString());
                        Console.WriteLine("Dealer position --> " + this.Format(dealerCombined) + ", " + result.DealerHand.ToString());
                    }

                    var dealerQualifies = (int)result.DealerHand.Rank >= (int)PokerHandRank.OnePair;
                    if (!(dealerQualifies))
                    {
                        if (trace)
                            Console.WriteLine("Dealer doesn't qualify...");

                        bankroll = bankrollBefore;

                        //Console.Write("-");
                        numPushes++;
                    }
                    else
                    {
                        if (trace)
                            Console.WriteLine("Winner -- > " + result.Winner);

                        if (result.Winner == Actor.Player)
                        {
                            //Console.Write("w");

                            // give the bets back...
                            bankroll = bankrollBefore;

                            // give the 1:1 on the play...
                            var profit = bet;

                            if (trace)
                                Console.WriteLine("Win on play --> $" + profit);

                            // if the dealer qualifies, add the base bet in again...
                            if (dealerQualifies)
                                profit += bet;

                            // what happened?
                            var odds = 0M;
                            var rank = result.PlayerHand.Rank;
                            if (rank == PokerHandRank.Straight)
                                odds = 1M;
                            else if (rank == PokerHandRank.Flush)
                                odds = 3M / 2M;
                            else if (rank == PokerHandRank.FullHouse)
                                odds = 3M;
                            else if (rank == PokerHandRank.FourOfAKind)
                                odds = 10M;
                            else if (rank == PokerHandRank.StraightFlush)
                                odds = 50M;
                            else if (rank == PokerHandRank.RoyalFlush)
                                odds = 500M;

                            var blindWin = 0;
                            if (odds > 0)
                            {
                                blindWin = (int)(odds * (decimal)blind);

                                if (trace)
                                    Console.WriteLine("Win on blinds --> $" + blindWin);

                                profit += (int)blindWin;
                            }

                            if (trace)
                                Console.WriteLine("Profit -- $" + profit);

                            bankroll += profit;

                            numPlayerWins++;
                        }
                        else
                        {
                            if (result.Winner == Actor.Dealer)
                            {
                                //Console.Write("l");
                                numDealerWins++;
                            }
                            else
                            {
                                //Console.Write("f");
                                numFolds++;
                            }
                        }
                    }
                    
                    if (trace)
                        Console.WriteLine("Bankroll now -- $" + bankroll);

                    if (bankroll > bankrollMax)
                        bankrollMax = bankroll;

                    if (bankroll < bankrollMin)
                        bankrollMin = bankroll;
                }

                //Console.WriteLine("\t" + bankroll);

                handsPlayed++;
                if (bankroll > winTarget)
                    break;
            }

            if (trace)
            {
                Console.WriteLine("================");
                Console.WriteLine("4x bets -- " + num4x);
                Console.WriteLine("2x bets -- " + num2x);
                Console.WriteLine("1x bets -- " + num1x);
                Console.WriteLine("Folds -- " + numFolds);
                Console.WriteLine("Player wins -- " + numPlayerWins);
                Console.WriteLine("Dealer wins -- " + numDealerWins);
                Console.WriteLine("Pushes -- " + numPushes);
                Console.WriteLine(bankroll);
            }

            //Console.WriteLine(" -- ok, " + bankroll + ", " + handsPlayed);
            return new BankrollResults(bankroll, bankrollMin, bankrollMax, totalWager, handsPlayed);
        }

        private IEnumerable<IEnumerable<Card>> GetHiddenPairs(IEnumerable<Card> player, IEnumerable<Card> community)
        {
            var hiddens = new List<IEnumerable<Card>>();
            foreach(var p in player)
            {
                foreach(var c in community)
                {
                    if (p.Rank == c.Rank)
                        hiddens.Add(new List<Card>() { p, c });
                }
            }

            return hiddens;
        }

        private string Format(IEnumerable<Card> player) => string.Join(", ", player.Select(v => v.ToString()));
    }
}