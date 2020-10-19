using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno_Muliplayer
{
    class player
    {
        public static int playerCount { get; set; } = 1;
        public  int playerNumber { get; set; }
        public List<cards> playerCards { get; set; }
        public State playerState { get; set; }

        public enum State
        {
            ACTIVE,
            DONE
        }

        public player() 
        {
            playerCards = new List<cards>();
            playerNumber = playerCount;
            playerCount++;
        }
 
        public void addCard(cards card) 
        {
            //Adds cards to the player
            playerCards.Add(card);
        }

        public void removeCard(cards card)
        {
            //remove cards from the player
            playerCards.Remove(card);
        }

        public string playerAction() 
        {
            
            Console.WriteLine("Your Cards:");
            //Show cards to player
            ShowPlayerCards();

            Console.WriteLine("Options: ");
            Console.WriteLine("Type 'p' for picking up a card");
            Console.WriteLine("Type 'd' for end round");
            //Let the player choose card


            Console.WriteLine("Choose a card or an option: ");
            string playerChoice = Console.ReadLine();

            return playerChoice;
        }

        void ShowPlayerCards() 
        {
            Console.WriteLine($"Player Number: {playerNumber}");

            foreach (var card in playerCards)
            {
                card.showCard();
            }
            Console.WriteLine();
            for (int i = 0; i < playerCards.Count; i++)
            {
                
                if (i >= 10) //If the card has 2 charaters 
                {
                    Console.Write($"  {i + 1} ");
                }
                else if(playerCards[i].extraSpace) //If the player have over 10 cards
                {
                    Console.Write($"   {i + 1}  ");
                }
                else //If the player have under 10 cards
                {
                    Console.Write($"  {i + 1}  ");
                }
                
            }
            Console.WriteLine();
        }

        public int switchColor() 
        {
            Console.WriteLine("Which color would you like?");

            Console.Write("1.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Red");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("2.");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Blue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("3.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Green");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("4.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Yellow");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("");

            int playerChoice = Convert.ToInt32(Console.ReadLine()) - 1;

            return playerChoice;
        }
    }
}
