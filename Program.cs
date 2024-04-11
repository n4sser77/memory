using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace MemoryGame
{
    class Program
    {
        static void Main(string[] args)
        {
            int currentIndex = 0; // Index of the current selected card for swaping
            int previousIndex = -1; // Index of the previously selected card (choice one)
            int secondIndex = -1; // Index of the second card (choice two)
            bool[] isFlipped = new bool[16]; // Array to track if a card is isFlipped
            bool waitingForChoice = true;
            char[] cards = { 'a', 'a', 'b', 'b', 'c', 'c', 'd', 'd', 'e', 'e', 'f', 'f', 'g', 'g', 'h', 'h' };
            char[] hiddenCards = new char[16]; // Array to represent hidden cards
            int tries = 0;
            bool playAgain = true;

            while (playAgain)
            {
                Array.Fill(hiddenCards, '*');

                int matchesFound = 0; // Number of matched pairs
                shuffleCards(cards); // Shuffle the cards
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
                        previousIndex = SelectCard(hiddenCards, cards, currentIndex, isFlipped, tries, "Choose a first card");
                        waitingForChoice = false;

                    }
                    isFlipped[previousIndex] = true;


                    waitingForChoice = true;

                    while (waitingForChoice)
                    {
                        Console.WriteLine("Attempts: " + tries);
                        secondIndex = SelectCard(hiddenCards, cards, currentIndex, isFlipped, tries, "Choose a second card");
                        waitingForChoice = false;
                        tries++;
                    }

                    // check for match
                    Console.Clear();
                    DisplayBoardWithCursor(hiddenCards, currentIndex, tries); // Display the board with the cursor
                    if (cards[previousIndex] == cards[secondIndex]) // Check if the cards match
                    {
                        Console.WriteLine("Match!");
                        Thread.Sleep(600);
                        isFlipped[previousIndex] = true; // Set the isFlipped flags
                        isFlipped[secondIndex] = true;
                        matchesFound++;
                    }
                    else
                    {
                        Console.WriteLine("No match");
                        Thread.Sleep(600);
                        FlipCardsBack(hiddenCards, cards, previousIndex, secondIndex);
                        isFlipped[previousIndex] = false;
                        isFlipped[secondIndex] = false;
                        DisplayBoardWithCursor(hiddenCards, currentIndex, tries);


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
                   else
                   {
                       Console.WriteLine("Invalid input");
                   }
               }


            }
            Console.WriteLine("Thanks for playing, Goodbye!");
        }


          static int SelectCard(char[] hiddenCards, char[] cards, int currentIndex, bool[] isFlipped, int tries, string message)
        {
            bool waitingForChoice = true;
            int SelectedCardIndex = -1;

            while (waitingForChoice)
            {
                Console.Clear();
                DisplayBoardWithCursor(hiddenCards, currentIndex, tries);
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


        // Displays the game board with a cursor at the current index.
        static void DisplayBoardWithCursor(char[] hiddenCards, int currentIndex, int tries)
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

        // Flips the card att the current index.
        static void FlipCard(char[] hiddenCards, char[] cards, int currentIndex)
        {
            hiddenCards[currentIndex] = cards[currentIndex];

        }

        // Flips the selected cards back.
        static void FlipCardsBack(char[] hiddenCards, char[] cards, int previousIndex, int secondIndex)
        {
            hiddenCards[previousIndex] = '*';
            hiddenCards[secondIndex] = '*';

        }

        // Shuffles the cards using the Fisher-Yates algorithm.
        static void shuffleCards(char[] cards)
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

    }
}
