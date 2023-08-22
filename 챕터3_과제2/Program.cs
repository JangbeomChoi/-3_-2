namespace 챕터3_과제2
{
    
    public enum Shape { Diamonds, Clubs, Spades, Hearts }
    public enum Number { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen , King, Ace }

    public class Card
    {
        public Shape Shape { get; private set; }
        public Number Number { get; private set; }

        public Card(Shape s, Number n)
        {
            Shape = s;
            Number = n;
        }

        //블랙잭 점수 계산
        public int GetValue()
        {
            if ((int)Number <= 10) //number가 아니라 shape을 넣어둬서 점수계산이 똑바로 안되는 오류가 있었음
            {
                return (int)Number; //number가 아니라 shape을 넣어둬서 점수계산이 똑바로 안되는 오류가 있었음
            }
            else if ((int)Number <= 13) //number가 아니라 shape을 넣어둬서 점수계산이 똑바로 안되는 오류가 있었음.
            {
                return 10;
            }
            else
            {
                return 11;
            }
        }
        public override string ToString()
        {
            return $"{Shape} of {Number}";
        }
    }
    public class Deck
    {
        private List<Card> cards;
        public Deck()
        {
            cards = new List<Card>();

            foreach (Shape s in Enum.GetValues(typeof(Shape)))
            {
                foreach (Number n in Enum.GetValues(typeof(Number)))
                {
                    cards.Add(new Card(s, n));
                }
            }
            Shuffle();
        }
        public void Shuffle()
        {
            Random rand = new Random();

            for (int i = 0; i < cards.Count; i++)
            {
                int j = rand.Next(i, cards.Count);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }
        public Card DrawCard()
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }
    // 패를 표현하는 클래스
    public class Hand
    {
        private List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int GetTotalValue()
        {
            int total = 0;
            int aceCount = 0;

            foreach (Card card in cards)
            {
                if (card.Number == Number.Ace)
                {
                    aceCount++;
                }
                total += card.GetValue();
            }

            while (total > 21 && aceCount > 0)
            {
                total -= 10;
                aceCount--;
            }

            return total;
        }
    }

    // 플레이어를 표현하는 클래스
    public class Player
    {
        public Hand Hand { get; private set; }

        public Player()
        {
            Hand = new Hand();
        }

        public Card DrawCardFromDeck(Deck deck)
        {
            Card drawnCard = deck.DrawCard();
            Hand.AddCard(drawnCard);
            return drawnCard;
        }
    }

    // 여기부터는 학습자가 작성
    // 딜러 클래스를 작성하고, 딜러의 행동 로직을 구현하세요.
    public class Dealer : Player
    {
        // 코드를 여기에 작성하세요
        public void KeepDrawCards(Deck deck)
        {
            while (Hand.GetTotalValue() < 17)
            {
                Card drawnCard = DrawCardFromDeck(deck);
                Console.WriteLine("딜러는 {0}을 뽑았습니다. 현재 총합은 {1}점입니다.", drawnCard, Hand.GetTotalValue());
            }
        }
    }

    // 블랙잭 게임을 구현하세요. 
    public class Blackjack
    {
        // 코드를 여기에 작성하세요
        private Dealer dealer;
        private Player player;
        private Deck deck;

        public void PlayGame()
        {
            deck = new Deck();
            player = new Player();
            dealer = new Dealer();

            //게임시작, 두장의 카드를 드로우 (딜러,플레이어)
            for (int i = 0; i<2; i++)
            {
                player.DrawCardFromDeck(deck);
                dealer.DrawCardFromDeck(deck);
            }

            Console.WriteLine("게임을 시작합니다!");
            Console.WriteLine("플레이어의 초기 카드 합: " + player.Hand.GetTotalValue());
            Console.WriteLine("딜러의 초기카드 합: " + dealer.Hand.GetTotalValue());

            Console.WriteLine();

            //플레이어, 21점 전까지 카드를 계속 뽑을수 있다
            Console.WriteLine("플레이어의 차례입니다.");
            while(player.Hand.GetTotalValue() < 21)
            {
                Console.Write("카드를 더 뽑는다. (Y/N): ");
                string input = Console.ReadLine();

                if (input.ToUpper() == "Y") // 대문자로 받기
                {
                    Card drawnCard = player.DrawCardFromDeck(deck);
                    Console.WriteLine("{0}을(를) 뽑았습니다. 현재 총합은 {1}점입니다.", drawnCard, player.Hand.GetTotalValue());
                }
                else
                {
                    break;
                }
                Console.WriteLine();
            }
            Console.WriteLine("딜러의 차례입니다.");
            dealer.KeepDrawCards(deck);
            Console.WriteLine("딜러의 총합은{0}점 입니다.", dealer.Hand.GetTotalValue());

            //승리조건
            if(player.Hand.GetTotalValue() > 21)
            {
                Console.WriteLine("플레이어의 카드 합이 21점을 초과했습니다.딜러의 승리입니다.");
            }
            else if (dealer.Hand.GetTotalValue() > 21)
            {
                Console.WriteLine("딜러의 카드 합이 21점을 초과했습니다. 플레이어의 승리입니다.");
            }
            else if (player.Hand.GetTotalValue() > dealer.Hand.GetTotalValue()) 
            {
                Console.WriteLine("플레이어의 카드합이 더 높습니다. 플레이어의 승리입니다.");
            }
            else
            {
                Console.WriteLine("딜러의 카드 합이 더높거나 같습니다. 딜러의 승리입니다");
            }

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 블랙잭 게임을 실행하세요
            Blackjack game= new Blackjack();
            game.PlayGame();
        }
    }
}
//git commit test