using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class Casinox
    {
        internal string Name { get; set; }

        private Casinox(string name)
        {
            this.Name = name;
        }

        internal static IEnumerable<Casinox> GetCasinos()
        {
            return new List<Casinox>()
            {
                new Casinox("Flamingo"),
                new Casinox("The Cromwell"),
                new Casinox("Horseshoe"),
                new Casinox("Paris"),
                new Casinox("Planet Hollywood"),
                new Casinox("MGM Grand"),
                new Casinox("Mandalay Bay"),
                new Casinox("Luxor"),
                new Casinox("Excalibur"),
                new Casinox("New York New York"),
                new Casinox("Park MGM"),
                new Casinox("Aria"),
                new Casinox("Cosmopolitan"),
                new Casinox("Ballagio"),
                new Casinox("Caesars Palace"),
                new Casinox("Treasure Island"),
                new Casinox("Resorts World"),
                new Casinox("Circus Circus"),
                new Casinox("The Strat"),
                new Casinox("Sahara"),
                new Casinox("Fountainbleau"),
                new Casinox("Encore"),
                new Casinox("Wynn"),
                new Casinox("Palazzo"),
                new Casinox("Venetian"),
                new Casinox("Casino Royale"),
                new Casinox("Harrah's"),
                new Casinox("The Linq"),
            };
        }
    }
}
