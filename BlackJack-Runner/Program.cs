using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            int iterations = 10000;

            Console.WriteLine("Welcome to the BlackJack Dealer Bust Odds Simulator.");
            Console.WriteLine("");
            Console.WriteLine("Initializing simulation (iterations: " + iterations + ")...");

            Stopwatch timer = Stopwatch.StartNew();

            Simulator.run(new Deck().shuffle(), iterations);

            Console.WriteLine("Removing all 5's from the deck...");
            Simulator.run(new Deck().removeAll(5).shuffle(), iterations);

            Console.WriteLine("Removing all 4's and 5's from the deck...");
            Simulator.run(new Deck().removeAll(5).removeAll(4).shuffle(), iterations);

            timer.Stop();
            TimeSpan timespan = timer.Elapsed;

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Total time elapsed: " + String.Format("{0:00}:{1:00}:{2:00}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10));
            Console.WriteLine("");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
