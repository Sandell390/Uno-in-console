using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno_Muliplayer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool power = true;
            while (power)
            {
                Console.Clear();
                Console.WriteLine("Skriv antal Players: ");
                int playerCount = Convert.ToInt32(Console.ReadLine());
                Table table = new Table(playerCount);

                table.createDeck();

                table.shuffleDesk();

                //table.showDeck();

                for (int i = 0; i < playerCount; i++)
                {
                    table.givPlayerCards(7, i);
                }

                

                bool stillGaming = true;
                while (stillGaming) 
                {
                    table.NextRoud();
                    if (table.winner != null) 
                    {
                        stillGaming = false;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Winner: Playernumber {table.winner.playerNumber}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }

        }
    }
}
