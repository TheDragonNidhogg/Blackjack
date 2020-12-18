using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{

    public class Deck
    {
        public List<Card> deckofcards = new List<Card>(); //Deck of Cards
        private int size; //Number of sets of cards that make up the deck
        private int cardnum; //Number of cards in deck
        public int Size
        {
            get { return this.size; }
        }
        public int DeckNum
        {
            get { return this.cardnum; }
        }
        public Deck(int x)
        {
            this.size = x;
            this.cardnum = x;
            for (; x > 0; x--)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        Card temp = new Card(j, i);
                        deckofcards.Add(temp);
                        //Console.WriteLine(temp.Name);
                        this.size++;
                    }
                }
            }
        }
        public Card RemoveTopCard()
        {
            Card temp = deckofcards.First();
            deckofcards.RemoveAt(0);
            this.cardnum--;
            return temp;
        }
        public void Shuffle()
        {
            var rnd = new Random();
            deckofcards = deckofcards.OrderBy(x => rnd.Next()).ToList();
        }
        public void AddCards(List<Card> usedcards)
        {
            usedcards.ForEach(delegate (Card temp)
            {
                deckofcards.Add(temp);
                cardnum++;
            });
        }
    }
    public class Player
    {
        public Hand player_hand;
        public Hand unused;
        private int chips;
        private string name;
        public bool inround;

        private int bet;
        private int splitbet;
        public int splitindex;
        public bool cansplit;
        public Hand split;
        public bool insplit;
        public bool donesplit;
        public Player()
        {
            this.player_hand = new Hand();
            this.chips = 350;
            this.inround = true;
            this.splitindex = 0;
            this.cansplit = false;
            this.split = new Hand();
            this.bet = 0;
            this.splitbet = 0;
            this.insplit = false;
            this.donesplit = false;
            this.unused = new Hand();
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public bool broke
        {
            get { return (this.chips > 0); }
        }
        public void AddChips(int n)
        {
            this.chips += n;
        }
        public void RemoveChips(int n)
        {
            if (n <= this.chips)
            {
                this.chips -= n;
            }
            else
            {
                Console.WriteLine("Error: Player Does have Enough Chips.");
            }

        }
        public int Chips
        {
            get { return this.chips; }
            set { this.chips = value; }
        }
        public void SetBet(int amount)
        {
            if (amount <= this.chips)
            {
                this.bet = amount;
                this.chips -= amount;
            }
            else
            {
                Console.WriteLine("Error: Player Does have Enough Chips.");
            }

        }

        
        public Deck Hit(Deck deck)
        {
            this.player_hand.AddCard(deck, 0);

            //Check for Split
            for(int i = 0; i<this.player_hand.HandOfCards.Count-2; i++)
            {
                if (this.split.HandOfCards.Count == 0 & (this.CheckSplit()))
                {
                    this.cansplit = true;
                    this.splitindex = this.player_hand.HandOfCards.Count - 1;
                }
            }
            
            Console.WriteLine(this.Name + " Hit");
            Console.Write(this.Name + ": ");
            this.player_hand.ReadHand(0, false);
            return deck;
        }
        public Deck HitSplit(Deck deck)
        {
            this.split.AddCard(deck, 0);
            Console.WriteLine(this.Name + " Hit");
            Console.Write(this.Name + ": ");
            this.split.ReadHand(0, false);
            return deck;
        }
        public void Stand()
        {
            this.inround = false;
            Console.WriteLine(this.Name + " Stand");

        }
        public void Split()
        {
            if (this.CheckSplit())
            {
                this.split.AddCard(this.player_hand.HandOfCards[splitindex]);
                this.player_hand.RemoveCard(splitindex);
                Console.WriteLine(this.Name + " Split with " + split.HandOfCards[0].Name);
                this.splitbet = this.bet;
                this.chips -= this.splitbet;
                this.cansplit = false;
                this.insplit = true;
            }
        }
        public Deck Double(Deck deck)
        {
            this.chips -= this.bet;
            this.bet = this.bet * 2;
            this.player_hand.AddCard(deck, 0);
            Console.WriteLine(this.Name + " Double");
            Console.Write(this.Name + ": ");
            this.player_hand.ReadHand(0, false);
            if (this.player_hand.GetValue(0) >= 21)
            {
                this.inround = false;


            }
            return deck;
        }
        public void ReadPlayer()
        {
            Console.WriteLine(this.Name + " --- " + this.chips + " Chips --- Bet: " + this.bet);
            Console.Write("    Hand: ");
            this.player_hand.ReadHand(0, false);
            if (this.split.HandOfCards.Count > 0)
            {
                Console.Write("    Split: ");
                this.split.ReadHand(0, false);
            }
        }

        public void PlayerResults(Hand dealer)
        {
            if (this.chips>0)
            {

                if ((this.player_hand.GetValue(0) <= 21 & this.player_hand.GetValue(0) > dealer.GetValue(0)) | dealer.GetValue(0)>21)
                {
                    Console.WriteLine(this.Name + ": Wins!");
                    this.chips += bet*2;
                    this.bet = 0;

                } else if (this.player_hand.GetValue(0) == dealer.GetValue(0))
                {
                    Console.WriteLine(this.Name + ": Pushes!");
                    this.chips += bet;
                    this.bet = 0;
                }
                else if (this.player_hand.GetValue(0) > 21)
                {
                    Console.WriteLine(this.Name + ": Busts!");
                    this.bet = 0;
                }
                else
                {
                    Console.WriteLine(this.Name + ": Loses!");
                    this.bet = 0;
                }
                if ((this.split.HandOfCards.Count() > 0 & this.split.GetValue(0) <= 21 & this.split.GetValue(0) > dealer.GetValue(0)) | (this.split.HandOfCards.Count() > 0 & dealer.GetValue(0)>21))
                {
                    Console.WriteLine("Split: Wins!");
                    this.chips += splitbet*2;
                    this.splitbet = 0;
                }
                else if (this.split.HandOfCards.Count() > 0 & this.split.GetValue(0) == dealer.GetValue(0))
                {
                    Console.WriteLine("Split: Pushes!");
                    this.chips += splitbet;
                    this.splitbet = 0;
                }
                else if (this.split.HandOfCards.Count() > 0 & this.split.GetValue(0) > 21)
                {
                    Console.WriteLine("Split: Busts!");
                    this.splitbet = 0;

                }
                else if (this.split.HandOfCards.Count() > 0)
                {
                    Console.WriteLine("Split: Loses!");
                    this.splitbet = 0;
                }
                this.ReadPlayer();

                //Console.Write(" Value = " + players[i].player_hand.GetValue(0));
                //Console.WriteLine();
            }
        }
        public void MakeBet(int n)
        {
            this.bet = n;
            this.chips -= n;
        }
        public void ResetPlayer()
        {
            this.player_hand = new Hand();
            this.inround = true;
            this.splitindex = 0;
            this.cansplit = true;
            this.split = new Hand();
            this.bet = 0;
            this.splitbet = 0;
            this.insplit = false;
            this.donesplit = false;
            this.unused = new Hand();
        }
        public bool CheckSplit()
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < this.player_hand.HandOfCards.Count; i++)
            {
                numbers.Add(this.player_hand.HandOfCards[i].Number);

            }
            for (int i = 2; i < 15; i++)
            {
                
                if (numbers.IndexOf(i) != numbers.LastIndexOf(i))
                {
                    
                    return true;
                }
            }
            return false;
        }
    }
    public class Hand
    {
        public List<Card> HandOfCards = new List<Card>();  //This is the cards in hand.

        private int cardnum;
        private int aces;

        public Hand()
        {
            this.cardnum = 0;
            this.aces = 0;
        }
        public void AddCard(Deck deck, int handnum)
        {
            Card temp = deck.RemoveTopCard();
            HandOfCards.Add(temp);
            this.cardnum++;
            if (temp.Value == 11)
            {
                this.aces++;
            }


        }

        public int GetValue(int handnum)
        {

            int handvalue = 0;
            int tempAces = this.aces;
            HandOfCards.ForEach(delegate (Card card)
            {
                handvalue += card.Value;
                //Console.WriteLine(card.Value);
                //Console.WriteLine(card.Name);
            });
            while (handvalue > 21)
            {
                if (tempAces > 0)
                {
                    tempAces--;
                    handvalue -= 10;
                }
                else
                {
                    break;
                }
            }
            return handvalue;

        }

        public List<Card> EmptyHand()
        {
            List<Card> temp = new List<Card>();
            for (int i = 0; i < cardnum; i++)
            {

                temp.Add(HandOfCards.First());
                HandOfCards.RemoveAt(0);
            }
            this.cardnum = 0;
            this.aces = 0;
            return temp;
        }

        public void ReadHand(int handnum, bool isdealeropening)
        {
            for (int i = 0; i < this.cardnum; i++)
            {
                if (!(isdealeropening && i == (this.cardnum - 1)))
                {
                    Console.Write(this.HandOfCards[i].Name + ", ");
                }
                else
                {
                    Console.Write("Faced-Down Card, ");
                }
            }
            if (!(isdealeropening))
            {
                Console.Write("Value: "+ this.GetValue(0));
            }
            Console.WriteLine();

        }
        public bool HasSecondEqual(Card card, int index)
        {
            for (int i = 0; i < HandOfCards.Count() - 1; i++)
            {
                if (i != index & card == this.HandOfCards[i])
                {
                    return true;
                }
            }
            return false;
        }
        public void RemoveCard(int i)
        {
            if (this.HandOfCards.ElementAt(i).Value == 11)
            {
                this.aces--;
            }
            this.HandOfCards.RemoveAt(i);
            this.cardnum--;
        }
        public void AddCard(Card c)
        {
            if(c.Value == 11)
            {
                this.aces++;
            }
            this.HandOfCards.Add(c);
            this.cardnum++;
        }
        
    }
    public class Card
    {
        private readonly int[] CardValue = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };
        private readonly int[] CardNumber = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        private readonly string[] CardName = new string[] { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };
        //private readonly string[] CardSuit = new string[] { "Spades", "Clubs", "Diamonds", "Hearts" };

        private string name;
        private int value;
        //private int suit;
        private int number;

        public Card(int n, int s)
        {
            this.name = CardName[n]; // + " of " + CardSuit[s];
            this.value = CardValue[n];
            //this.suit = s;
            this.number = CardNumber[n];
        }

        public string Name
        {
            get { return this.name; }
        }
        public int Value
        {
            get { return this.value; }
        }
      //  public int Suit
      //  {
            //get { return this.suit; }
       // }
        public int Number
        {
            get { return this.number; }
        }
        public bool Same(Card card)
        {
            if (this.value == card.value)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        class Program
        {
            static void Main(string[] args)
            {

                bool loop = true;
                int userinput = 0;
                while (loop)
                {
                    userinput = Menu();
                    switch (userinput)
                    {
                        case 1:
                            int playernum = 0;
                            while (playernum < 1 | playernum > 5)
                            {
                                Console.WriteLine("How many people are playing? Enter a number between 1 and 5:");
                                while (playernum < 1 | playernum > 5)
                                {
                                    string temp = Console.ReadLine();
                                    int x;
                                    if (int.TryParse(temp,out x))
                                    {
                                        if (x>0 & x<5)
                                        {
                                            playernum = x;
                                        } else
                                        {
                                            Console.WriteLine("Invalid Selection. Try Again.");
                                        }
                                        
                                        

                                    } else
                                    {
                                        Console.WriteLine("Invalid Selection. Try Again.");
                                    }
                                    
                                }
                            }
                            Console.WriteLine("------------------------------------------------------------------------");
                            Console.WriteLine("Starting Game");
                            Main_Game(playernum);
                            /*
                            Hand testhand = new Hand();
                            Deck testdeck = new Deck(4);
                            testdeck.Shuffle();
                            testhand.AddCard(testdeck, 0);
                            Console.Write("Hand: ");
                            testhand.ReadHand(0, false);
                            Console.WriteLine(testhand.GetValue(0));
                            testhand.AddCard(testdeck, 0);
                            Console.Write("Hand: ");
                            testhand.ReadHand(0, false);
                            Console.WriteLine(testhand.GetValue(0));
                            */

                            break;
                        case 2:
                            Instructions();
                            break;
                        case 3:
                            loop = false;
                            break;
                    }
                }



            }
            //This is the Function Designed to Run the initial Menu to Start a game
            static int Menu()
            {

                Console.WriteLine("Welcome to Blakcjack");
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("<1> Play game ");
                Console.WriteLine("<2> Instuctions");
                Console.WriteLine("<3> Quit");
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("Enter a Selection from 1 to 3:");
                bool correct_selection = false;
                int userinput = 0;
                while (!correct_selection)
                {
                    string temp = Console.ReadLine();
                    int x;
                    if (int.TryParse(temp, out x))
                    {
                        if (x > 0 & x < 4)
                        {
                            userinput = x;
                            correct_selection = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Selection. Try Again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Selection. Try Again.");
                    }
                }
                Console.WriteLine("------------------------------------------------------------------------");
                return userinput;
            }

            static void Instructions()
            {
                Console.WriteLine("Instructions");
                Console.WriteLine("------------------------------------------------------------------------");
            }

            //This function starts the game with the potential for eventually enabling multiple players
            //Each player starts with 350 chips and are asked how many they would like to bet
            //Two Cards are dealt to each Player and the Dealer
            //The Game continues to call Player_Turn for each player until they all Stand or Bust
            //Then the game has the dealer draw cards until the hand has a value of 17 or greater.
            //Each Player can Bust, Lose, Beat, or Tie the Dealers hand The game then puts all the cards in the discard Card list and clears the hands.
            //If the Deck is at 50% or less it is shuffled
            //The game then asks if it should go to another round. If so it does and if not it returns to the menu.
            
            
            
            static void Main_Game(int numofplayers)
            {
                List<Card> usedcards = new List<Card>();
                bool playagain = true;
                string userinput = "";
                bool correctinput = false;
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("Game Setup");
                Console.WriteLine("------------------------------------------------------------------------");
                List<Player> players = new List<Player>();
                Deck deck = new Deck(4);

                deck.Shuffle();

                for (int i = 0; i < numofplayers; i++)
                {
                    Console.WriteLine("Creating Player " + i);
                    players.Add(new Player());
                    Console.WriteLine("Enter a name for Player " + i + ":");
                    players[i].Name = Console.ReadLine();
                }
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("Player List:");
                for (int i = 0; i < numofplayers; i++)
                {
                    Console.WriteLine("Player " + i + ": " + players[i].Name);
                }
                //This is the Loop that checks the decisions of each player until they all bust, get blackjack, or stand.
                while (playagain)
                {
                    if (deck.DeckNum <= deck.Size * 52) //If the Deck is halve empty it adds all used cards into the List of Cards and Randomly shuffles them
                    {
                        deck.AddCards(usedcards);
                        deck.Shuffle();
                    }


                    players = Each_Game(numofplayers, userinput, correctinput, players, deck);

                    

                    Console.WriteLine("Do you want to play another round? <Y/N>:");
                    userinput = Console.ReadLine();
                    correctinput = false;
                    while (correctinput)
                    {
                        if (userinput == "Y" | userinput == "y" | userinput == "N" | userinput == "n")
                        {
                            correctinput = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input. Please Enter Y or N:");
                            userinput = Console.ReadLine();
                        }
                    }
                    if (userinput == "N" | userinput == "n")
                    {
                        Console.WriteLine("Returning to Menu");
                        Console.WriteLine("------------------------------------------------------------------------");
                        playagain = false;
                    }
                    else
                    {
                        Console.WriteLine("Preparing Next Game...");
                        Console.WriteLine("------------------------------------------------------------------------");
                    }
                    for (int i = 0; i < numofplayers; i++)
                    {
                        usedcards.AddRange(players[i].player_hand.EmptyHand());
                        usedcards.AddRange(players[i].split.EmptyHand());
                    }
                    usedcards.AddRange(players[0].unused.EmptyHand());
                    for (int i = 0; i< numofplayers; i++)
                    {
                        players[i].ResetPlayer();
                    }
                }
                //This function give displays the interface.
                //The Interface will display the first Dealer card, two cards for the player whose turn it is and then give the player a selection from four possible options: Stand, Hit, Split, and Double
                //The program will move onto one of those functions(aside from Stand) and then retun a bool of if the player is finished to Main_Game
            }
            static List<Player> Each_Game(int numofplayers, string userinput, bool correctinput, List<Player> players, Deck deck)
            {
                Hand dealer = new Hand();

                Console.WriteLine("Game Start");
                int playersinround = 0;
                for (int i = 0; i < numofplayers; i++) //This sets the number of players in the round and check to make sure they have Chips to Bet.
                {
                    if (players[i].Chips > 0)
                    {
                        players[i].inround = true;
                        playersinround++;

                    }
                    else
                    {
                        players[i].inround = false;
                    }
                }
                Console.WriteLine("Dealing Cards");
                for (int m = 0; m < 2; m++) //Loop to Deal out the Cards
                {
                    dealer.AddCard(deck, 0); //Adds a Card to the dealer
                    for (int i = 0; i < numofplayers; i++)//Adds a Card to each player in the round
                    {
                        if (players[i].inround)
                        {
                            //deck.deckofcards[0] = new Card(12, 0);  //Used for Testing Split
                            players[i].player_hand.AddCard(deck, 0);


                        }


                    }
                }


                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("Place Your Bets!");
                Console.WriteLine();
                for (int i = 0; i < numofplayers; i++)
                {
                    if (players[i].inround)
                    {

                        int tempbet = 0;
                        Console.WriteLine(players[i].Name + ": You may bet up to " + players[i].Chips + ".");
                        while (tempbet <= 0 | tempbet >= players[i].Chips)
                        {
                            string temp = Console.ReadLine();
                            int x;
                            if (int.TryParse(temp, out x))
                            {
                                if (x > 0 & x <= players[i].Chips)
                                {
                                    tempbet = x;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Selection. Try Again.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Selection. Try Again.");
                            }
                        }
                        players[i].MakeBet(tempbet);
                        Console.WriteLine(players[i].Name + " bet " + tempbet);
                        Console.WriteLine();

                    }
                }
                Console.WriteLine("Showing Hands");
                if (dealer.GetValue(0) == 21)
                {
                    for (int i = 0; i < numofplayers; i++)
                    {
                        if (!(players[i].broke))
                        {

                            players[i].PlayerResults(dealer);


                            //Console.Write(" Value = " + players[i].player_hand.GetValue(0));
                            //Console.WriteLine();
                        }
                    }
                }
                else
                {
                    Console.Write("Dealer: ");
                    dealer.ReadHand(0, true);
                    for (int i = 0; i < numofplayers; i++)
                    {
                        if (players[i].inround)
                        {

                            players[i].ReadPlayer();
                            //Console.Write(" Value = " + players[i].player_hand.GetValue(0));
                            //Console.WriteLine();
                        }
                    }
                    Console.WriteLine("------------------------------------------------------------------------");
                    int finished = 0;
                    int round = 0;
                    List<int> activeplayerindex = new List<int>();
                    for (int i = 0; i < numofplayers; i++)
                    {
                        if (players[i].inround)
                        {
                            activeplayerindex.Add(i);
                        }
                    }
                    while (activeplayerindex.Count > 0)
                    {
                        round++;


                        activeplayerindex.ForEach(delegate (int i)
                        {

                            if (players[i].player_hand.GetValue(0) >= 21)
                            {
                                players[i].inround = false;
                                finished++;
                            }
                            if (players[i].inround & players[i].player_hand.GetValue(0) < 21)
                            {
                                Console.Write("Dealer: ");
                                dealer.ReadHand(0, true);
                                Console.WriteLine("------------------------------------------------------------------------");
                                players[i].ReadPlayer();
                                players[i].CheckSplit();
                                Console.WriteLine(players[i].Name);
                                Console.Write("Options: <1>Hit <2>Stand");
                                
                                
                                if (round == 1)
                                {
                                    if (players[i].CheckSplit())
                                    {
                                        Console.Write(" <3>Split");
                                    }
                                    Console.WriteLine(" <4>Double:");
                                }
                                else
                                {
                                    Console.WriteLine(":");
                                }

                                string choice = "0";
                                choice = Console.ReadLine();
                                bool inputincorrect = true;
                                while (inputincorrect)
                                {
                                    switch (choice)
                                    {
                                        case "1":
                                            deck = players[i].Hit(deck);
                                            if (players[i].player_hand.GetValue(0) >= 21)
                                            {
                                                players[i].inround = false;
                                                finished++;
                                            }

                                            inputincorrect = false;
                                            Console.WriteLine("------------------------------------------------------------------------");
                                            break;
                                        case "2":
                                            players[i].inround = false;
                                            finished++;
                                            Console.WriteLine(players[i].Name + " Stand");
                                            Console.WriteLine("------------------------------------------------------------------------");
                                            inputincorrect = false;
                                            break;

                                        case "3":
                                            if (players[i].CheckSplit() & players[i].split.HandOfCards.Count == 0)
                                            {
                                                players[i].Split();
                                                inputincorrect = false;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid Selection. Try Again.");
                                                choice = Console.ReadLine();
                                                break;
                                            }
                                            break;

                                        case "4":
                                            if (round == 1)
                                            {
                                                deck = players[i].Double(deck);
                                                inputincorrect = false;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid Selection. Try Again.");
                                                choice = Console.ReadLine();
                                                break;
                                            }
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Selection. Try Again.");
                                            choice = Console.ReadLine();
                                            break;
                                    }
                                }

                                Console.WriteLine("------------------------------------------------------------------------");
                            }
                        });









                        activeplayerindex = new List<int>();
                        for (int i = 0; i < numofplayers; i++)
                        {
                            if (players[i].inround)
                            {
                                activeplayerindex.Add(i);
                            }
                        }

                    }


                    Console.WriteLine("Split Hands");
                    Console.WriteLine("------------------------------------------------------------------------");
                    //Split Hands

                    activeplayerindex = new List<int>();
                    for (int i = 0; i < numofplayers; i++)
                    {
                        if (players[i].insplit)
                        {
                            activeplayerindex.Add(i);
                        }
                    }
                    while (activeplayerindex.Count > 0)
                    {
                        round++;


                        activeplayerindex.ForEach(delegate (int i)
                        {

                            if (players[i].split.GetValue(0) >= 21)
                            {
                                players[i].insplit = false;
                                players[i].donesplit = true;
                                //finished++;
                            }
                            if (players[i].insplit & players[i].split.GetValue(0) < 21)
                            {
                                Console.Write("Dealer: ");
                                dealer.ReadHand(0, true);
                                Console.WriteLine("------------------------------------------------------------------------");
                                players[i].ReadPlayer();
                                Console.WriteLine(players[i].Name);
                                Console.WriteLine("Options: <1>Hit <2>Stand:");

                                string choice = "0";
                                choice = Console.ReadLine();
                                bool inputincorrect = true;
                                while (inputincorrect)
                                {
                                    switch (choice)
                                    {
                                        case "1":
                                            deck = players[i].HitSplit(deck);
                                            if (players[i].split.GetValue(0) >= 21)
                                            {
                                                players[i].insplit = false;
                                                players[i].donesplit = true;
                                                //finished++;
                                            }
                                            inputincorrect = false;
                                            Console.WriteLine("------------------------------------------------------------------------");
                                            break;
                                        case "2":
                                            players[i].insplit = false;
                                            players[i].donesplit = true;
                                            //finished++;
                                            Console.WriteLine(players[i].Name + " Stand");
                                            Console.WriteLine("------------------------------------------------------------------------");
                                            inputincorrect = false;
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Selection. Try Again.");
                                            choice = Console.ReadLine();
                                            break;
                                    }
                                }
                                Console.WriteLine("------------------------------------------------------------------------");
                            }
                        });
                        activeplayerindex = new List<int>();
                        for (int i = 0; i < numofplayers; i++)
                        {
                            if (players[i].insplit)
                            {
                                activeplayerindex.Add(i);
                            }
                        }
                    }


                    while (dealer.GetValue(0) < 17)
                    {
                        dealer.AddCard(deck, 0);
                        Console.Write("Dealer: ");
                        dealer.ReadHand(0, false);
                        Console.WriteLine("------------------------------------------------------------------------");
                    }

                    Console.WriteLine("Results");
                    Console.WriteLine("------------------------------------------------------------------------");
                    Console.Write("Dealer: ");
                    dealer.ReadHand(0, false);
                    players.ForEach(delegate (Player player)
                    {
                        player.PlayerResults(dealer);
                        
                    });


                    Console.WriteLine("------------------------------------------------------------------------");
                    Console.WriteLine("Game Finish");
                    Console.WriteLine("------------------------------------------------------------------------");

                    

                    players[0].unused = dealer;
                }
                return players;
            }

        
        }
    }
}
