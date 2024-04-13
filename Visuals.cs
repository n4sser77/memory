using MemoryGame;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace MemoryGame
{
   /// <summary>
   /// Visuals.cs contains the logic for printing the visuals of the game.
   /// </summary>
    public static class Visuals
    {
        /// <summary>
        /// Printing the board
        /// </summary>
        /// <param name="hiddenCards">Backside of the cards</param>
        /// <param name="currentIndex"> Index of the current highlighted card</param>
        /// <param name="tries"> Number of tries</param>
        public  static void DisplayBoardWithCursor(char[] hiddenCards, int currentIndex, int tries)
        {
            Console.Clear(); // Clear the console window
            Console.WriteLine("Attempts: " + tries);
            for (int i = 0; i < hiddenCards.Length; i++)
            {
                if (i == currentIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow; // Change background color for the selected card
                }

                Console.Write($"{hiddenCards[i]} ");

                if ((i + 1) % 4 == 0)
                {
                    Console.WriteLine();
                }

                Console.ResetColor(); // Reset background color
            }
        }
    }
}