using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Runner
{
    class Hand : List<Card>
    {
        public decimal bet = 0;
        private int score;
        public bool set = false;

        public new void Add(Card c)
        {
            base.Add(c);
            score = getScore(this);
        }

        public int getScore()
        {
            return score;
        }

        public bool isBlackjack()
        {
            if (this.Count != 2)
                return false;

            bool hasAce = false,
                hasTen = false;

            foreach (Card c in this)
            {
                if (c.getRank() == 1)
                    hasAce = true;
                else if (c.getRank() >= 10)
                    hasTen = true;
            }

            return hasAce && hasTen;
        }

        private static int getScore(List<Card> cards)
        {
            int score = 0,
                aceCount = 0;

            for (int i = 0; i < cards.Count; i++)
            {
                int rank = cards[i].getRank();
                if (rank == 1)
                {
                    aceCount++;
                    score += 11;
                }
                else if (rank >= 10)
                {
                    score += 10;
                }
                else
                {
                    score += rank;
                }
            }

            while (score > 21)
            {
                if (aceCount > 0)
                {
                    score -= 10;
                    aceCount--;
                }
                else
                {
                    break;
                }
            }

            return score;
        }
    }
}
