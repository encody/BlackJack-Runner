using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Runner
{
    class Simulator
    {
        public struct Result
        {
            public Status status;
            public int score;
        };

        public struct Stat
        {
            public int rank;
            public int busts;
            public float percent;
            public int iterations;
        }

        public enum Status { STAND, BUST, BLACKJACK };

        public static void run (Deck deck, int iterations)
        {
            Console.WriteLine("");

            Stopwatch timer = Stopwatch.StartNew();

            IDictionary<int, Simulator.Stat> stats = Simulator.test(deck, iterations);

            timer.Stop();
            TimeSpan timespan = timer.Elapsed;

            Console.WriteLine("");
            Console.WriteLine("Simulation finished!");
            Console.WriteLine("");
            Console.WriteLine("Results:");
            Console.WriteLine("");
            Console.WriteLine("Rank\tStands\tBusts\tHands\tPercent");
            Console.WriteLine("");

            float avgAvgPercent = 0;
            int totalStands = 0, totalBusts = 0, totalIterations = 0;

            foreach (KeyValuePair<int, Simulator.Stat> kv in stats)
            {
                Simulator.Stat stat = kv.Value;
                Card placeholder = new Card(stat.rank, Suit.Spades);
                int stands = (stat.iterations - stat.busts),
                    busts = stat.busts;
                totalStands += stands;
                totalBusts += busts;
                totalIterations += iterations;
                avgAvgPercent += stat.percent;
                Console.WriteLine(placeholder.getLongRankName() + "\t" + stands + "\t" + busts + "\t" + stat.iterations + "\t" + stat.percent.ToString("p"));
            }

            avgAvgPercent /= stats.Count();

            Console.WriteLine("");
            Console.WriteLine("Total\t" + totalStands + "\t" + totalBusts + "\t" + totalIterations + "\t" + avgAvgPercent.ToString("p"));

            Console.WriteLine("");

            Console.WriteLine("Time elapsed: " + String.Format("{0:00}:{1:00}:{2:00}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10));
            Console.WriteLine("");
        }

        public static IDictionary<int, Stat> test (Deck deck, int iterations)
        {
            IDictionary<int, Stat> stats = new Dictionary<int, Stat>(13);

            Console.WriteLine("\t\t[" + new string(':', Console.WindowWidth - 32 - 2) + ']');

            int[] ranks = deck.ranks();

            for (int i = 0; i < ranks.Count(); i++)
            {
                Card c = new Card(ranks[i], Suit.Spades);
                Stat s = Simulator.testCard(c, deck, iterations);
                stats.Add(i + 1, s);
            }

            return stats;
        }

        public static Stat testCard(Card c, Deck deck, int iterations)
        {
            Stat s = new Stat
            {
                rank = c.getRank(),
                busts = 0,
                iterations = iterations,
            };

            Console.Write("Testing " + c.getLongRankName() + "\t");

            int lastNumDots = 0;

            for (int i = 0; i < iterations; i++)
            {
                int numDots = (int) (((i + 1) / (float) iterations) * (Console.WindowWidth - 32f));
                if (numDots > lastNumDots)
                {
                    //Console.Write("\rTesting " + c.getLongRankName() + "\t" + new string('#', numDots));
                    Console.Write(new string('#', numDots - lastNumDots));
                    lastNumDots = numDots;
                }

                Result o = Simulator.testOne(c, deck);
                if (o.status == Status.BUST)
                {
                    s.busts++;
                }
            }

            Console.WriteLine(" Complete!");

            s.percent = (float) s.busts / s.iterations;

            return s;
        }

        public static Result testOne(Card c, Deck d)
        {
            Deck deck = d.clone().shuffle();

            Hand h = new Hand();

            h.Add(deck.pick(c));

            do
            {
                h.Add(deck.draw());
                if (h.getScore() > 21)
                {
                    return new Result
                    {
                        score = h.getScore(),
                        status = Status.BUST,
                    };
                }
                else if (h.isBlackjack())
                {
                    return new Result
                    {
                        score = 21,
                        status = Status.BLACKJACK,
                    };
                }
            } while (h.getScore() < 17);

            return new Result
            {
                score = h.getScore(),
                status = Status.STAND,
            };
        }
    }
}
