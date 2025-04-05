using BootFX.Common;

namespace Mbrit.Vegas.Utility
{
    internal class RandomFoo
    {
        internal void DoMagic()
        {
            var rand = new RandomWrapper();

            var results = new Dictionary<int, int>();

            for(var index = 0; index < 10000; index++)
            {
                var value = rand.Next(10, 20);

                if (!(results.ContainsKey(value)))
                    results[value] = 0;
                results[value]++;
            }

            var keys = new List<int>(results.Keys);
            keys.Sort();

            var table = new ConsoleTable();
            foreach (var key in keys)
                table.AddRow(key, results[key]);

            table.Render();
        }
    }
}