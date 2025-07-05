using Mbrit.Vegas.Games;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace Mbrit.Vegas.Utility
{
    internal class CrapsFoo
    {
        internal void DoMagic()
        {
            var rand = new Random();

            const int seed = 1500;

            const int runs = 25000;

            var bankrolls = new List<int>();
            for (var index = 0; index < runs; index++)
            {
                var bankroll = this.PlayCraps(seed, rand, runs == 1);
                bankrolls.Add(bankroll);
            }

            Console.WriteLine("==========================");
            Console.WriteLine(bankrolls.Average());

            Func<int, string> getPercentage = (count) =>
            {
                return ((decimal)count / (decimal)runs) * 100 + "%";
            };

            Console.WriteLine($"Num losses --> " + getPercentage(bankrolls.Where(v => v <= 0).Count()));
            Console.WriteLine($"Num evens --> " + getPercentage(bankrolls.Where(v => v < seed).Count()));
            Console.WriteLine($"Num wins over {seed} (2x) --> " + getPercentage(bankrolls.Where(v => v >= seed).Count()));
            Console.WriteLine($"Num wins over {seed * 1.5} (2.5x) --> " + getPercentage(bankrolls.Where(v => v >= seed * 1.5).Count()));
            Console.WriteLine($"Num wins over {seed * 2} (3x) --> " + getPercentage(bankrolls.Where(v => v >= seed * 2).Count()));
            Console.WriteLine($"Num wins over {seed * 3} (4x) --> " + getPercentage(bankrolls.Where(v => v >= seed * 3).Count()));
        }

        private int PlayCraps(int bankroll, Random rand, bool trace)
        {
            if (trace)
                Console.WriteLine("Playing craps with " + bankroll);

            var places = new Dictionary<int, int>();
            for (var index = 2; index <= 12; index++)
                places[index] = 0;

            //var target = bankroll * 2;

            var betAmount = (int)Math.Floor((decimal)bankroll / 6);
            if (bankroll >= betAmount * 6) // Ensure we can cover all place bets
            {
                for (var index = 2; index <= 12; index++)
                {
                    if (index == 4 || index == 5 || index == 6 || index == 8 || index == 9 || index == 10)
                    {
                        places[index] = betAmount;
                        bankroll -= betAmount;
                    }
                }
            }

            if (trace)
                Console.Write("Rolling --> ");

            while (true)
            {
                var dice = new Dice(2);
                var score = dice.Roll(rand);

                if (trace)
                    Console.WriteLine("Rolled --> " + score.Score);

                if (score.Score == 7)
                    break;

                if (places[score.Score] != 0)
                {
                    GameOdds odds = null;
                    if (score.Score == 4 || score.Score == 10)
                        odds = Craps.Place4Or10;
                    else if (score.Score == 5 || score.Score == 9)
                        odds = Craps.Place5Or9;
                    else if (score.Score == 6 || score.Score == 8)
                        odds = Craps.Place6Or8;
                    else
                        throw new NotSupportedException($"Cannot handle '{score.Score}'.");

                    var win = Math.Floor(odds.GetWin(places[score.Score])) - places[score.Score];

                    if(trace)
                        Console.WriteLine("Won --> " + win);

                    bankroll += (int)Math.Floor(win);
                }
            }

            if (trace)
            {
                Console.WriteLine();
                Console.WriteLine("Craps winnings --> " + bankroll);
            }

            return bankroll;
        }
    }
}