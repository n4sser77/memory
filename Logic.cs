using MemoryGame;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace MemoryGame
{
    /// <summary>
    /// Logic.cs contains the logic for the game.
    /// </summary>
    public static class Logic
    {
        /// <summary>
        /// Logic for Selecting a card.
        /// </summary>
        /// <param name="hiddenCards"> the array of hidden cards (front side of the cards) </param>
        /// <param name="cards"> the array of cards (backside of the cards)</param>
        /// <param name="currentIndex"> the index of the current highlighted card </param>
        /// <param name="isFlipped"> the array of whether the card is flipped or not </param>
        /// <param name="tries"> the number of tries </param>
        /// <param name="message"> Controls the message displayed to the user</param>
        /// <returns> the index of the selected card </returns>
        public static int SelectCard(char[] hiddenCards, char[] cards, int currentIndex, bool[] isFlipped, int tries, string message)
        {
            bool waitingForChoice = true;
            int SelectedCardIndex = -1;

            while (waitingForChoice)
            {
                Visuals.DisplayBoardWithCursor(hiddenCards, currentIndex, tries);
                Console.WriteLine(message);
                ConsoleKeyInfo keyInfo = Console.ReadKey(); // Read the user's key press

                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        currentIndex = Math.Max(currentIndex - 1, 0); // Move the cursor to the left
                        break;
                    case ConsoleKey.RightArrow:
                        currentIndex = Math.Min(currentIndex + 1, hiddenCards.Length - 1); // Move the cursor to the right
                        break;
                    case ConsoleKey.UpArrow:
                        currentIndex = Math.Max(currentIndex - 4, 0); // Move the cursor up by 4 cards
                        break;
                    case ConsoleKey.DownArrow:
                        currentIndex = Math.Min(currentIndex + 4, hiddenCards.Length - 1); // Move the cursor down by 4 cards
                        break;
                    case ConsoleKey.Enter:

                        if (isFlipped[currentIndex]) // Check if the card is not already isFlipped
                        {
                            Console.WriteLine("The card is already isFlipped.");
                            Thread.Sleep(500);
                            waitingForChoice = true;
                        }
                        else
                        {
                            FlipCard(hiddenCards, cards, currentIndex); // Flip the card
                            isFlipped[currentIndex] = true; // Mark the card as isFlipped
                            SelectedCardIndex = currentIndex;
                            waitingForChoice = false;
                            return SelectedCardIndex; // Return the result
                        }
                        break;


                }

            }
            return SelectedCardIndex;
        }
       /// <summary>
       /// Logic for Shuffling the cards using the Fisher-Yates algorithm
       /// </summary>
       /// <param name="cards"> the array of cards </param>
        public static void ShuffleCards(char[] cards)
        {
            Random random = new Random();

            for (int i = cards.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // Swap cards[i] with cards[j]
                char temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }
        /// <summary>
        /// Logic for Flipping the cards back to the original state
        /// </summary>
         /// <param name="hiddenCards"> the array of hidden cards (front side of the cards) </param>
        /// <param name="cards"> the array of cards (backside of the cards)</param>
        /// <param name="previousIndex"></param>
        /// <param name="secondIndex"></param>
        public static void FlipCardsBack(char[] hiddenCards, char[] cards, int previousIndex, int secondIndex)
        {
            hiddenCards[previousIndex] = '*';
            hiddenCards[secondIndex] = '*';

        }
    
       /// <summary>
       /// Logic for Flipping a card
       /// </summary>
       /// <param name="hiddenCards"></param>
       /// <param name="cards"></param>
       /// <param name="currentIndex"></param>
        public static void FlipCard(char[] hiddenCards, char[] cards, int currentIndex)
        {
            hiddenCards[currentIndex] = cards[currentIndex];

        }

    }

}
