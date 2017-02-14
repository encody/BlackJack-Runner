using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Runner
{
    public enum Suit { Clubs, Diamonds, Hearts, Spades };

    public class Card
    {
        private Suit suit;
        private int rank;
        private static String[] shortRankNames = {
            "A",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "J",
            "Q",
            "K",
        },
        longRankNames = {
            "Ace",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "Jack",
            "Queen",
            "King",
        };

        public Card(int rank, Suit suit)
        {
            this.rank = rank;
            this.suit = suit;
        }

        public override bool Equals(object c2)
        {
            Card c = (Card) c2;

            if (c == null)
            {
                return false;
            }

            return this.getRank() == c.getRank() && this.getSuit() == c.getSuit();
        }

        public override int GetHashCode()
        {
            return (this.getRank().GetHashCode() * 49) + (this.getSuit().GetHashCode());
        }

        public int getRank()
        {
            return rank;
        }

        public Suit getSuit()
        {
            return suit;
        }

        public String getShortRankName()
        {
            return shortRankNames[rank - 1];
        }

        public String getLongRankName()
        {
            return longRankNames[rank - 1];
        }

        public String getSuitName()
        {
            return suit.ToString();
        }

        public String getFullName()
        {
            return getLongRankName() + " of " + getSuitName();
        }
    }

    class Deck
    {
        private List<Card> cards;

        private static List<Card> createDefaultDeck()
        {
            List<Card> cards = new List<Card>(52);

            // Loop through suits first
            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                // Loop through every card rank
                for (int i = 0; i < 13; i++)
                {
                    Card c = new Card(i + 1, s);
                    cards.Add(c);
                }
            }

            return cards;
        }

        public Deck() : this(createDefaultDeck())
        {
        }

        public Deck(List<Card> cards)
        {
            this.cards = cards;
        }

        public Deck shuffle()
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            this.cards = this.cards.OrderBy(item => r.Next()).ToList();
            return this;
        }

        public List<Card> getCards()
        {
            return cards;
        }

        public Deck clone()
        {
            List<Card> newCards = new List<Card>(52);

            foreach (Card c in cards)
            {
                newCards.Add(new Card(c.getRank(), c.getSuit()));
            }

            return new Deck(newCards);
        }

        public Card draw()
        {
            Card c = cards[0];
            cards.Remove(c);
            return c;
        }

        public int[] ranks ()
        {
            List<int> ranks = new List<int>(13);

            foreach (Card c in cards)
            {
                if (ranks.IndexOf(c.getRank()) == -1)
                {
                    ranks.Add(c.getRank());
                }
            }

            ranks.Sort();

            return ranks.ToArray();
        }

        public Deck removeAll(int rank)
        {
            List<Card> newCards = new List<Card>(52);

            foreach (Card c in cards)
            {
                if (c.getRank() != rank)
                {
                    newCards.Add(c);
                }
            }

            this.cards = newCards;

            return this;
        }

        public Deck removeAll(Suit suit)
        {
            List<Card> newCards = new List<Card>(52);

            foreach (Card c in cards)
            {
                if (c.getSuit() != suit)
                {
                    newCards.Add(c);
                }
            }

            this.cards = newCards;

            return this;
        }

        public Card pick(Card c)
        {
            int index = cards.IndexOf(c);
            if (index == -1)
            {
                return null;
            }

            Card r = cards[index];
            cards.Remove(r);
            return r;
        }
    }
}
