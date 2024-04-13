using MemoryGame;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace MemoryGame
{
    public class Game
    {
    /// <summary>
    /// Index of the current highlighted card
    /// </summary>
     public int currentIndex { get; set; }   = 0;
    /// <summary>
    /// The index of the first selected card
    /// </summary>
     public int previousIndex { get; set; }   = -1;
    /// <summary>
    /// The index of the second selected card
    /// </summary>
     public int secondIndex { get; set; }   = -1;
   /// <summary>
   /// Keeps track of whether the cards are flipped
   /// </summary>
     public bool[] isFlipped { get; set; }   = new bool[16];
     public bool waitingForChoice { get; set; }   = true;
     /// <summary>
     /// The array of cards after the user has flipped them.
     /// </summary>
     public char [] cards { get; set; }   =  { 'a', 'a', 'b', 'b', 'c', 'c', 'd', 'd', 'e', 'e', 'f', 'f', 'g', 'g', 'h', 'h' };
    /// <summary>
    /// the array of hidden cards. These are the cards that are displayed Before the user flips them.
    /// </summary>
     public char[] hiddenCards { get; set; }   = new char[16];
    ///
     public int tries { get; set; }   = 0;
     public bool playAgain { get; set; }   = true;

       /// <summary>
       /// Starts and controls the flow of the game.
       /// </summary>
        public void StartGame()
        {
            while (playAgain)
            {
                Array.Fill(hiddenCards, '*');

                int matchesFound = 0; // Number of matched pairs
                Logic.ShuffleCards(cards); // Shuffle the cards
                Console.WriteLine("Use arrows to select cards. Press Enter to flip cards.");
                Console.WriteLine("press any key to start...");
                Console.ReadKey();

                Stopwatch timer = Stopwatch.StartNew();
                while (matchesFound < cards.Length / 2)
                {
                    waitingForChoice = true;
                    while (waitingForChoice)
                    {
                        Console.WriteLine("Attempts: " + tries);
                        previousIndex = Logic.SelectCard(hiddenCards, cards, currentIndex, isFlipped, tries, "Choose a first card");
                        waitingForChoice = false;

                    }
                    isFlipped[previousIndex] = true;


                    waitingForChoice = true;

                    while (waitingForChoice)
                    {
                        Console.WriteLine("Attempts: " + tries);
                        secondIndex = Logic.SelectCard(hiddenCards, cards, currentIndex, isFlipped, tries, "Choose a second card");
                        waitingForChoice = false;
                        tries++;
                    }

                    Visuals.DisplayBoardWithCursor(hiddenCards, currentIndex, tries); 
                    if (cards[previousIndex] == cards[secondIndex]) 
                    {
                        Console.WriteLine("Match!");
                        Thread.Sleep(600);
                        isFlipped[previousIndex] = true; 
                        isFlipped[secondIndex] = true;
                        matchesFound++;
                    }
                    else
                    {
                        Console.WriteLine("No match");
                        Thread.Sleep(600);
                        Logic.FlipCardsBack(hiddenCards, cards, previousIndex, secondIndex);
                        isFlipped[previousIndex] = false;
                        isFlipped[secondIndex] = false;
                        Visuals.DisplayBoardWithCursor(hiddenCards, currentIndex, tries);


                    }
                }
                timer.Stop();

                // Calculate and display elapsed time
                TimeSpan elapsedTime = timer.Elapsed;
                string minute = (elapsedTime.Minutes > 1) ? "minutes" : "minute";

                Console.WriteLine($"Congratulations! You've matched all pairs in {elapsedTime.Minutes} {minute} and {elapsedTime.Seconds} seconds!");
                Console.WriteLine("Attempts: " + tries);
                string input = "";

                for (int i = 0; i < isFlipped.Length; i++) isFlipped[i] = false; ;
               while (playAgain)
               {
                   Console.WriteLine("Would you like to play again? (y/n)");

                   
                   input = Console.ReadLine();
               
                   if (input != null && (input.Equals("n", StringComparison.OrdinalIgnoreCase) || input.Equals("no", StringComparison.OrdinalIgnoreCase)))
                   {
                       playAgain = false;
                   }
                   else if (input != null && (input.Equals("y", StringComparison.OrdinalIgnoreCase) || input.Equals("yes", StringComparison.OrdinalIgnoreCase)))
                   {
                       playAgain = true;
                       break;
                   }
                   else if (input == null)
                   {
                       Console.WriteLine("Invalid input");
                   }


               }


            }
            Console.WriteLine();
            Console.WriteLine("Thanks for playing, Goodbye!");
        }
    }
}