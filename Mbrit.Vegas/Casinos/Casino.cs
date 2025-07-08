using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    public class Casino
    {
        public string Name { get; set; }

        private static IEnumerable<Casino> Casinos { get; } =  new List<Casino>()
            {
                new Casino("Flamingo"),
                new Casino("The Cromwell"),
                new Casino("Horseshoe"),
                new Casino("Paris"),
                new Casino("Planet Hollywood"),
                new Casino("MGM Grand"),
                new Casino("Mandalay Bay"),
                new Casino("Luxor"),
                new Casino("Excalibur"),
                new Casino("New York New York"),
                new Casino("Park MGM"),
                new Casino("Aria"),
                new Casino("Cosmopolitan"),
                new Casino("Ballagio"),
                new Casino("Caesars Palace"),
                new Casino("Treasure Island"),
                new Casino("Resorts World"),
                new Casino("Circus Circus"),
                new Casino("The Strat"),
                new Casino("Sahara"),
                new Casino("Fountainbleau"),
                new Casino("Encore"),
                new Casino("Wynn"),
                new Casino("Palazzo"),
                new Casino("Venetian"),
                new Casino("Casino Royale"),
                new Casino("Harrah's"),
                new Casino("The Linq"),
            };

        private Casino(string name)
        {
            this.Name = name;
        }

        public static IEnumerable<Casino> GetCasinos() => Casinos;
    }
}
