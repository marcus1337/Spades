namespace Spades
{
    public enum Rank
    {
        two = 2,
        three = 3,
        four = 4,
        five = 5,
        six = 6,
        seven = 7,
        eight = 8,
        nine = 9,
        ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    };
    public enum Suit { Clubs, Diamonds, Hearts, Spades }

    public class Card
    {
        private Rank rank;
        private Suit suit;

        public Card(Rank rank, Suit suit)
        {
            this.rank = rank;
            this.suit = suit;
        }

        public Suit getSuit()
        {
            return suit;
        }

        public Rank getRank()
        {
            return rank;
        }

        public override string ToString()
        {
            return "|" + rank + "\t" + suit + "|";
        }


        public override bool Equals(System.Object obj)
        {
            if (obj == null)
                return false;

            Card p = obj as Card;
            if ((System.Object)p == null)
                return false;

            return (rank == p.rank) && (suit == p.suit);
        }

        public bool Equals(Card p)
        {
            if ((object)p == null)
                return false;

            return (rank == p.rank) && (suit == p.suit);
        }

        public override int GetHashCode()
        {
            return rank.GetHashCode() ^ suit.GetHashCode();
        }

        public static bool operator ==(Card a, Card b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;

            if ((object)a == null || (object)b == null)
                return false;

            return a.rank == b.rank && a.suit == b.suit;
        }

        public static bool operator !=(Card a, Card b)
        {
            return !(a == b);
        }

    }
}