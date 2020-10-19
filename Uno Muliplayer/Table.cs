﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Uno_Muliplayer
{
    class Table
    {
        public List<player> players { get; set; }
        public List<cards> playedDeck { get; set; }
        public List<cards> currentDeck { get; set; }
        public List<cards> ActionDeck { get; set; }
        public cards currentCard { get; set; }
        public player winner { get; set; }
        public int stackedCards { get; set; }

        Random random;

        public Table(int playerCount) 
        {
            players = new List<player>();
            for (int i = 0; i < playerCount; i++)
            {
                players.Add(new player());
            }
            currentDeck = new List<cards>();
            playedDeck = new List<cards>();
            ActionDeck = new List<cards>();
            random = new Random();
            stackedCards = 0;
            winner = null;
        }

        public void createDeck() 
        {
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int j = 1; j < 10; j++)
                    {
                        currentDeck.Add(new cards((cards.colorState)i, cards.cardType.NUMBER, j)); //Number 1-9 Card
                    }

                    currentDeck.Add(new cards((cards.colorState)i, cards.cardType.SKIP)); //Skip card
                    currentDeck.Add(new cards((cards.colorState)i, cards.cardType.REVERSE)); //Reverse Card
                    currentDeck.Add(new cards((cards.colorState)i, cards.cardType.PLUS2)); //Plus 2 Card
                    
                }
                currentDeck.Add(new cards((cards.colorState)i, cards.cardType.NUMBER, 0)); //Number 0 Card
                currentDeck.Add(new cards(cards.colorState.NULL, cards.cardType.SWICTH_COLOR)); //Switch Color Card
                currentDeck.Add(new cards(cards.colorState.NULL, cards.cardType.PLUS4)); //Plus 4 Card
            }

        }

        public void shuffleDesk() 
        {

            var shuffledcards = currentDeck.OrderBy(a => Guid.NewGuid()).ToList(); //Shuffle the cards

            currentDeck = shuffledcards;

            playedDeck.Add(currentDeck[0]); //Place the first card on the table ^^
            currentDeck.Remove(currentDeck[0]);

            currentCard = playedDeck[0];

            if (currentCard.ColorState == cards.colorState.NULL) 
            {
                currentCard.ColorState = (cards.colorState)random.Next(0, 3);
            }
        }

        public void showDeck() 
        {
            foreach (var card in currentDeck)
            {
                card.showCard();
            }
        }

        public void givPlayerCards(int amount, int playerNumber) 
        {

            if (amount > currentDeck.Count) 
            {
                currentDeck.AddRange(playedDeck.GetRange(1,playedDeck.Count - 1));
                playedDeck.RemoveRange(0, playedDeck.Count - 1);
            }
            for (int i = 0; i < amount; i++)
            {
                players[playerNumber].addCard(currentDeck[0]);
                currentDeck.RemoveAt(0);
            }
        }

        public void NextRoud()
        {
            Console.Clear();

            bool completeRound = false;

            while (!completeRound && players[0].playerState != player.State.DONE)
            {
                Console.Clear();

                Console.WriteLine($"current Deck: {currentDeck.Count}");
                Console.WriteLine($"Played deck: {playedDeck.Count}");

                Console.WriteLine("");
                Console.WriteLine("");
                playedDeck[playedDeck.Count - 1].showCard();
                Console.WriteLine("");
                Console.WriteLine("");

                string playerChoiceString = players[0].playerAction(); //Returns the player chooses and want play
                
                if (playerChoiceString.ToLower() == "p") //Checks if player can picks up cards
                {
                    givPlayerCards(1,0);
                }
                else if (playerChoiceString.ToLower() == "d" && ActionDeck.Count > 0) //Checks if player can end the round
                {
                    putPlayerLastInList();


                    for (int i = 0; i < ActionDeck.Count; i++)
                    {
                        cardAction(ActionDeck[i]);
                        
                    }
                    ActionDeck.Clear();

                    completeRound = true;
                }
                else if (int.TryParse(playerChoiceString, out int playerChoiceInt)) //Checks if player have choose a number
                {
                    processingRound(playerChoiceInt);

                }
                else //Player have to play a card or an option
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You have to play a card before you can end the round");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(900);
                }
            }
        }

        void processingRound(int playerChoiceInt) 
        {
            playerChoiceInt -= 1;
            if (playerChoiceInt >= 0 && playerChoiceInt <= players[0].playerCards.Count)
            {
                if (players[0].playerCards[playerChoiceInt].ColorState == currentCard.ColorState || players[0].playerCards[playerChoiceInt].ColorState == cards.colorState.NULL || players[0].playerCards[playerChoiceInt].number == currentCard.number)
                {
                    playedDeck.Add(players[0].playerCards[playerChoiceInt]);

                    ActionDeck.Add(players[0].playerCards[playerChoiceInt]);

                    currentCard = playedDeck[playedDeck.Count - 1];

                    players[0].removeCard(players[0].playerCards[playerChoiceInt]);

                    if (players[0].playerCards.Count == 0)
                    {
                        players[0].playerState = player.State.DONE;

                        if (winner == null)
                        {
                            winner = players[0];
                        }
                    }
                }
                else //If the player dont play the right card then an error shows to the player
                {

                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You can not play the card");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Please play another card");
                    Thread.Sleep(900);
                }
            }
            else //If the player dont do the right thing then an error shows to the player
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("That card does are not in your deck");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Please play another card");
                Thread.Sleep(900);
            }
        }
        void putPlayerLastInList() 
        {
            players.Add(players[0]);
            players.RemoveAt(0);
        }

        void cardAction(cards card) 
        {
            switch (card.CardType)
            {
                case cards.cardType.PLUS2:
                    stackedCards += 2;
                    break;
                case cards.cardType.PLUS4:
                    stackedCards += 4;
                    currentCard.ColorState = (cards.colorState)players[0].switchColor();
                    break;
                case cards.cardType.SWICTH_COLOR:
                    currentCard.ColorState = (cards.colorState)players[0].switchColor();
                    break;
                case cards.cardType.REVERSE:
                    if (players.Count == 2) players.Reverse(0, players.Count);

                    else players.Reverse(0, players.Count - 1);
                    break;
                case cards.cardType.SKIP:
                    putPlayerLastInList();
                    break;
                default:
                    Console.WriteLine("Can not do any actions on the card");
                    break;
            }
        }

        void stackCard(cards card)  //Virker ikke, Skal lige have tænkt det godt igennem
        {
            bool canPlayerStack = false;
            string playerChoice;

            for (int i = 0; i < players[0].playerCards.Count; i++)
            {
                if (players[0].playerCards[i].CardType == card.CardType) 
                {
                    canPlayerStack = true; 
                    
                }
            }

            if (canPlayerStack) 
            {
                playerChoice = players[0].playerStackCard(card);

                if (playerChoice.ToLower() == "d") 
                {
                    givPlayerCards(stackedCards,0);
                }
                else if (int.TryParse(playerChoice, out int playerChoiceInt)) //Checks if player have choose a number
                {
                    processingRound(playerChoiceInt);

                }
            }
        }

    }
}
