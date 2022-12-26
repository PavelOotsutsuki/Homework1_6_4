using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1_6_4
{
    class Program
    {
        const string CaseTakeCard = "1";
        const string CaseAddCardInTable = "2";
        const string CaseShowAllCardsInHand = "3";
        const string CaseEndTurn = "4";
        const string CaseExit = "5";

        static void Main(string[] args)
        {
            Player player = new Player();
            bool isWork = true;

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine(CaseTakeCard + ". Взять карту");
                Console.WriteLine(CaseAddCardInTable + ". Выставить карту на стол");
                Console.WriteLine(CaseShowAllCardsInHand + ". Показать все карты в руке");
                Console.WriteLine(CaseEndTurn + ". Закончить ход");
                Console.WriteLine(CaseExit + ". Закончить игру");

                player.ShowMana();
                Console.Write("Введите команду: ");
                string command = Console.ReadLine();

                switch (command)
                {
                    case CaseTakeCard:
                        player.TakeCard();
                        break;
                    case CaseAddCardInTable:
                        player.AddCardInTable();
                        break;
                    case CaseShowAllCardsInHand:
                        player.ShowAllCardsInHand();
                        break;
                    case CaseEndTurn:
                        player.EndTurn();
                        break;
                    case CaseExit:
                        isWork = false;
                        break;
                    default:
                        Console.WriteLine("Введена неверная команда");
                        break;
                }

                Console.ReadKey();
            }

            Console.Write("Все взятые из колоды карты:");
            player.ShowAllTakenCards();
            Console.ReadKey();
        }
    }

    class Player
    {
        private int _maxCardsInHand;
        private int _maxMana;
        private int _mana;
        private int _manaInTurn;
        private Deck _deck = new Deck();
        private List<Card> _hand = new List<Card>();
        private List<Card> _burnedCards = new List<Card>();
        private Table _table = new Table();
        private Stack <Card> _allTakenCards = new Stack<Card>();


        public Player(int startCardsInHand=4, int maxCardsInHand=10, int mana=1, int maxMana=10)
        {
            _maxCardsInHand = maxCardsInHand;
            _mana = mana;
            _maxMana = maxMana;
            _manaInTurn = _mana;

            for (int i = 0; i < startCardsInHand; i++)
            {
                TakeCard();
            }
        }

        public void TakeCard()
        {
            if (_deck.IsEmptyDeck() == false)
            {
                _allTakenCards.Push(_deck.TakeCard());

                if (_hand.Count < _maxCardsInHand)
                {
                    _hand.Add(_allTakenCards.Peek());
                }
                else
                {
                    _burnedCards.Add(_allTakenCards.Peek());
                }
            }
            else
            {
                Console.WriteLine("Карт больше нет!");
            }
        }

        public void ShowAllCardsInHand()
        {
            ShowAllCardsStats(_hand);
        }

        private void ShowAllCardsStats(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Console.WriteLine("\nКарта " + (i+1) + ":\n");
                Console.WriteLine("Стоимость: " + cards[i].CostMana + " маны");
                Console.WriteLine("Атака: " + cards[i].Attack);
                Console.WriteLine("Здоровье: " + cards[i].Health);
                Console.WriteLine("Тип существа: " + cards[i].Minion);
            }
        }

        private void ShowAllCardsStats(Stack<Card> cards)
        {
            int cardNumber = 1;

            foreach (var card in cards)
            {
                Console.WriteLine("\nКарта " + cardNumber + ":\n");
                Console.WriteLine("Стоимость: " + card.CostMana + " маны");
                Console.WriteLine("Атака: " + card.Attack);
                Console.WriteLine("Здоровье: " + card.Health);
                Console.WriteLine("Тип существа: " + card.Minion);

                cardNumber++;
            }
        }

        public void EndTurn()
        {
            if (_mana<_maxMana)
            {
                _mana++;
            }

            _manaInTurn = _mana;
        }

        public void AddCardInTable()
        {
            Console.Write("Введите номер карты, которую хотите разыграть: ");
            int cardNumber = Convert.ToInt32(Console.ReadLine());

            if (cardNumber >= 0 && cardNumber <= _hand.Count)
            {
                if (_manaInTurn >= _hand[cardNumber - 1].CostMana)
                {
                    if (_table.AddCardInTable(_hand[cardNumber - 1]))
                    {
                        _manaInTurn -= _hand[cardNumber - 1].CostMana;
                        _hand.RemoveAt(cardNumber - 1);
                        Console.WriteLine("Карта на столе!");
                    }
                    else
                    {
                        Console.WriteLine("Стол переполнен");
                    }
                }
                else
                {
                    Console.WriteLine("У вас не хватает маны");
                }
            }
            else
            {
                Console.WriteLine("Введен неверный номер карты");
            }
        }

        public void ShowAllTakenCards()
        {
            ShowAllCardsStats(_allTakenCards);
        }

        public void ShowMana()
        {
            Console.WriteLine("Мана: "+_manaInTurn +"/" + _mana);
        }
    }

    class Deck
    {
        private int _countCards;
        private Stack<Card> _cardsInDeck = new Stack<Card>();

        public Deck(int countCards=30)
        {
            _countCards = countCards;
            Random random = new Random();

            while (_cardsInDeck.Count<_countCards)
            {
                int minCostManaCard = 0;
                int maxCostManaCard = 10;
                int minHealthCard = 1;
                int maxHealthCard = 10;
                int minAttackCard = 1;
                int maxAttackCard = 10;
                int minIndexTypeMinion = 0;
                int maxIndexTypeMinion = 2;

                int costManaCard = random.Next(minCostManaCard, maxCostManaCard+1);
                int healthCard = random.Next(minHealthCard, maxHealthCard+1);
                int attackCard = random.Next(minAttackCard, maxAttackCard+1);
                Minion minionCard = (Minion)random.Next(minIndexTypeMinion, maxIndexTypeMinion+1);

                _cardsInDeck.Push(new Card(costManaCard,healthCard,attackCard,minionCard));
            }
        }

        public Card TakeCard()
        {
            return _cardsInDeck.Pop();
        }

        public bool IsEmptyDeck()
        {
            if (_cardsInDeck.Count>0)
            {
                return false;
            }

            return true;
        }
    }

    enum Minion
    {
        Beast,
        Demon,
        Dragon
    }

    class Card
    {
        public int CostMana { get; private set; }
        public int Health { get; private set; }
        public int Attack { get; private set; }
        public Minion Minion { get; private set; }

        public Card(int costMana, int health, int attack, Minion minion)
        {
            CostMana = costMana;
            Health = health;
            Attack = attack;
            Minion = minion;
        }
    }

    class Table
    {
        private int _maxSpots;
        private List<Card> _cardsInTable = new List<Card>();

        public Table(int maxSpots=7)
        {
            _maxSpots = maxSpots;
        }

        public bool AddCardInTable(Card card)
        {
            if (_cardsInTable.Count<_maxSpots)
            {
                _cardsInTable.Add(card);
                return true;
            }

            return false;
        }
    }
}
