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
            for (; x > 0; x--){
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
        public void Shuffle ()
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
        private int chips;
        private string name;
        public bool inround;
        public bool broke;

        public Player()
        {
            this.player_hand = new Hand();
            this.chips = 350;
            this.inround = true;
            this.broke = false;

        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public void AddChips(int n)
        {
            this.chips += n;
        }
        public void RemoveChips(int n)
        {
            this.chips -= n;
        }
        public int Chips
        {
            get { return chips; }
            set { chips = value; }
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
            if(temp.Value == 11)
            {
                this.aces++;
            }
            

        }
        public void SplitHand(int handnum)
        {

        }
        public int GetValue(int handnum)
        {
            
            int handvalue = 0;
            int tempAces = 0;
            HandOfCards.ForEach(delegate (Card card)
            {
                handvalue += card.Value;
                //Console.WriteLine(card.Value);
                //Console.WriteLine(card.Name);
            });
            while(tempAces < this.aces & handvalue > 21)
            {
                tempAces++;
                handvalue -= 10;
            }
            return handvalue;
            
        }

        public List<Card> EmptyHand()
        {
            List<Card> temp = new List<Card>();
            for (int i= 0; i<cardnum; i++)
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
            for(int i = 0; i< this.cardnum; i++)
            {
                if (!(isdealeropening && i==(this.cardnum-1)))
                {
                    Console.Write(this.HandOfCards[i].Name + ", ");
                } else
                {
                    Console.Write("Faced-Down Card, ");
                }
            }
            if (!(isdealeropening))
            {
                Console.Write(this.GetValue(0));
            }  
            Console.WriteLine();
            
        }
    }
    public class Card
    {
        private readonly int[] CardValue = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };
        private readonly int[] CardNumber = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        private readonly string[] CardName = new string[] { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };
        private readonly string[] CardSuit = new string[] { "Spades", "Clubs", "Diamonds", "Hearts" };
        
        private string name;
        private int value;
        private int suit;
        private int number;
        
        public Card(int n, int s)
        {
            this.name = CardName[n] + " of " + CardSuit[s];
            this.value = CardValue[n];
            this.suit = s;
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
        public int Suit
        {
            get { return this.suit; }
        }
        public int Number
        {
            get { return number; }
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
                            playernum = Convert.ToInt32(Console.ReadLine());
                            if (playernum < 1 | playernum > 5)
                            {
                                Console.WriteLine("Invalid Input. Please Enter a Number between 1 and 5:");

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
            Console.WriteLine("1. Play game ");
            Console.WriteLine("2. Instuctions");
            Console.WriteLine("3. Quit");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("Enter a Selection from 1 to 3:");
            bool correct_selection = false;
            int userinput = 0;
            while (!correct_selection)
            {   
                userinput = Convert.ToInt32(Console.ReadLine());
                if (userinput > 0 | userinput < 4)
                {
                    correct_selection = true;
                } else
                {
                    Console.WriteLine("Invalid Selection. Please Enter a Number between 1 and 3:");
                    
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
            bool playagain = true;
            string userinput = "";
            bool correctinput;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("Game Setup");
            Console.WriteLine("------------------------------------------------------------------------");
            List<Player> players = new List<Player>();
            Hand dealer = new Hand();
            Deck deck = new Deck(4);
            List<Card> usedcards = new List<Card>();
            deck.Shuffle();

            for(int i = 0; i<numofplayers; i++)
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
            while (playagain)
            {
                if (deck.DeckNum <= deck.Size * 52)
                {
                    deck.AddCards(usedcards);
                    deck.Shuffle();
                }
                Console.WriteLine("Game Start");
                int playersinround = 0;
                for (int i = 0; i < numofplayers; i++)
                {
                    if (players[i].Chips > 0)
                    {
                        players[i].inround = true;
                        playersinround++;

                    } else
                    {
                        players[i].inround = false;
                    }
                }
                Console.WriteLine("Dealing Cards");
                dealer.AddCard(deck, 0);
                for (int i = 0; i < numofplayers; i++)
                {
                    if (players[i].inround)
                    { 
                        players[i].player_hand.AddCard(deck, 0);
                    }
                }
                dealer.AddCard(deck, 0);
                for (int i = 0; i < numofplayers; i++)
                {
                    if (players[i].inround)
                    {
                        players[i].player_hand.AddCard(deck, 0);
                    }
                }
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("Showing Hands");
                if (dealer.GetValue(0) == 21)
                {
                    Console.Write("Dealer: ");
                    dealer.ReadHand(0, false);
                    playersinround = 0;
                    for (int i = 0; i < numofplayers; i++)
                    {
                        if (players[i].inround)
                        {
                            Console.Write(players[i].Name + ": ");
                            players[i].player_hand.ReadHand(0, false);
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
                        Console.Write(players[i].Name + ": ");
                        players[i].player_hand.ReadHand(0, false);
                        //Console.Write(" Value = " + players[i].player_hand.GetValue(0));
                        //Console.WriteLine();
                        }
                    }
                    Console.WriteLine("------------------------------------------------------------------------");
                    /*
                    while (playersinround>0)
                    {
                        for (int i = 0; i < numofplayers; i++)
                        {
                            if (players[i].player_hand.GetValue(0) >= 21)
                            {
                                players[i].inround = false;
                            }
                            if (players[i].inround & players[i].player_hand.GetValue(0) < 21)
                            {
                                Console.WriteLine(players[i].Name + "- <1>Hit <2>Stand:");
                                int choice = Convert.ToInt32(Console.ReadLine());
                                bool inputincorrect = true;
                                while (inputincorrect)
                                {
                                    switch (choice)
                                    {
                                        case 1:
                                            players[i].player_hand.AddCard(deck, 0);
                                            Console.WriteLine(players[i].Name + " Hit");
                                            Console.Write(players[i].Name + ": ");
                                            players[i].player_hand.ReadHand(0, false);
                                            if (players[i].player_hand.GetValue(0)>=21)
                                            {
                                                players[i].inround = false;
                                                playersinround--;
                                            }
                                            inputincorrect = false;
                                            Console.WriteLine("------------------------------------------------------------------------");
                                            break;
                                        case 2:
                                            players[i].inround = false;
                                            playersinround--;
                                            Console.WriteLine(players[i].Name + " Stand");
                                            Console.WriteLine("------------------------------------------------------------------------");
                                            inputincorrect = false;
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Selection. Try Again.");
                                            choice = Convert.ToInt32(Console.ReadLine());
                                            break;
                                    }
                                }
                            }


                        }
                    }*/
                    bool continues = true;
                    int finished = 0;
                    while (continues)
                    {
                        for (int i = 0; i < numofplayers; i++)
                        {

                            if (players[i].player_hand.GetValue(0) >= 21)
                            {
                                players[i].inround = false;
                                finished++;
                            }
                            if (players[i].inround & players[i].player_hand.GetValue(0) < 21)
                            {
                                Console.Write(players[i].Name + ": ");
                                players[i].player_hand.ReadHand(0, false);
                                Console.WriteLine(players[i].Name + "- <1>Hit <2>Stand:");
                                int choice = Convert.ToInt32(Console.ReadLine());
                                bool inputincorrect = true;
                                while (inputincorrect)
                                {
                                    switch (choice)
                                    {
                                        case 1:
                                            players[i].player_hand.AddCard(deck, 0);
                                            Console.WriteLine(players[i].Name + " Hit");
                                            Console.Write(players[i].Name + ": ");
                                            players[i].player_hand.ReadHand(0, false);
                                            if (players[i].player_hand.GetValue(0) >= 21)
                                            {
                                                players[i].inround = false;
                                                finished++;
                                            }
                                            inputincorrect = false;
                                            Console.WriteLine("------------------------------------------------------------------------");
                                            break;
                                        case 2:
                                            players[i].inround = false;
                                            finished++;
                                            Console.WriteLine(players[i].Name + " Stand");
                                            Console.WriteLine("------------------------------------------------------------------------");
                                            inputincorrect = false;
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Selection. Try Again.");
                                            choice = Convert.ToInt32(Console.ReadLine());
                                            break;
                                    }
                                }
                            }
                            if (finished == playersinround)
                            {
                                break;
                            }
                        }
                    int dealervalue = dealer.GetValue(0);
                    while (dealervalue < 17)
                    {
                        dealer.AddCard(deck,0);
                        Console.Write("Dealer: ");
                        dealer.ReadHand(0, false);
                        dealervalue = dealer.GetValue(0);
                        Console.WriteLine("------------------------------------------------------------------------");
                    }

                    Console.WriteLine("Results");
                    Console.WriteLine("------------------------------------------------------------------------");
                    Console.Write("Dealer: ");
                    dealer.ReadHand(0, false);
                    dealervalue = dealer.GetValue(0);
                    for (int i = 0; i < numofplayers; i++)
                    {
                        if (!(players[i].broke))
                        {

                            if (players[i].player_hand.GetValue(0) <= 21 & players[i].player_hand.GetValue(0) > dealer.GetValue(0))
                            {
                                Console.WriteLine(players[i].Name + ": Wins!");
                            }
                            else if (players[i].player_hand.GetValue(0) == dealer.GetValue(0))
                            {
                                Console.WriteLine(players[i].Name + ": Pushes!");
                            }
                            else if (players[i].player_hand.GetValue(0) > 21)
                            {
                                Console.WriteLine(players[i].Name + ": Busts!");
                            }
                            else
                            {
                                Console.WriteLine(players[i].Name + ": Loses!");
                            }
                            Console.Write(players[i].Name + ": ");
                            players[i].player_hand.ReadHand(0, false);
                            //Console.Write(" Value = " + players[i].player_hand.GetValue(0));
                            //Console.WriteLine();
                        }
                    }


                }
                




                for (int i = 0; i < numofplayers; i++)
                {
                    usedcards.AddRange(players[i].player_hand.EmptyHand());
                }
                usedcards.AddRange(dealer.EmptyHand());
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("Game Finish");
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("Do you want to play another round? <Y/N>:");
                userinput = Console.ReadLine();
                correctinput = false;
                while (correctinput) {
                    if(userinput == "Y" | userinput == "y" | userinput == "N" | userinput == "n")
                    {
                        correctinput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input. Please Enter Y or N:");
                        userinput = Console.ReadLine();
                    }
                }
                if(userinput == "N" | userinput == "n")
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
                
            }
        }
        //This function give displays the interface.
        //The Interface will display the first Dealer card, two cards for the player whose turn it is and then give the player a selection from four possible options: Stand, Hit, Split, and Double
        //The program will move onto one of those functions(aside from Stand) and then retun a bool of if the player is finished to Main_Game
        static int Player_Turn(int playernum, int turnnum, Player player, Deck deck)
        {
            int move = 0;



            return move;
        }

        static void Hit(int playernum, int turnnum, Hand player, Deck deck)
        {

        }
        static void Split(int playernum, int turnnum, Hand player, Deck deck)
        {

        }
        static void Double(int playernum, int turnnum, Hand player, Deck deck)
        {

        }
        static void Double(Deck deck)
        {

        }
    }
}
