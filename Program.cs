using System;
using System.Diagnostics;
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
                Console.WriteLine("pressany key to start...");
                Console.ReadKey();

                Stopwatch timer = Stopwatch.StartNew();
                while (matchesFound < cards.Length / 2)
                {
                    waitingForChoice = true;
                    while (waitingForChoice)
                    {
                        Console.WriteLine("Attempts: " + tries);
                        previousIndex = SelectCardWithArrowKeys1(hiddenCards, cards, currentIndex, isFlipped, tries);
                        waitingForChoice = false;

                    }
                    isFlipped[previousIndex] = true;


                    waitingForChoice = true;

                    while (waitingForChoice)
                    {
                        Console.WriteLine("Attempts: " + tries);
                        secondIndex = SelectCardWithArrowKeys2(hiddenCards, cards, currentIndex, isFlipped, previousIndex, tries);
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
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("No match");
                        Thread.Sleep(600);
                        FlipCardsBack(hiddenCards, cards, previousIndex, secondIndex);
                        isFlipped[previousIndex] = false;
                        isFlipped[secondIndex] = false;
                        DisplayBoardWithCursor(hiddenCards, currentIndex, tries);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();

                    }
                }
                timer.Stop();

                // Calculate and display elapsed time
                TimeSpan elapsedTime = timer.Elapsed;
                string minute = (elapsedTime.Minutes > 1) ? "minutes" : "minute";

                Console.WriteLine($"Congratulations! You've matched all pairs in {elapsedTime.Minutes} {minute} and {elapsedTime.Seconds} seconds!");
                Console.WriteLine("Attempts: " + tries);
                string input = "";


                Console.WriteLine("Would you like to play again? (y/n)");
                input = Console.ReadLine();
                if (input == "n" || input == "N") playAgain = false;
                else if (input == "y" || input == "Y") playAgain = true;


            }
        }

        // Selects a card using arrow keys, displays the board, and allows the user to make a choice. Returns the index of the selected card.
        static int SelectCardWithArrowKeys1(char[] hiddenCards, char[] cards, int currentIndex, bool[] isFlipped, int tries)
        {
            bool waitingForChoice = true;
            int previousIndex = -1;
            while (waitingForChoice)
            {
                Console.Clear();
                DisplayBoardWithCursor(hiddenCards, currentIndex, tries);
                Console.WriteLine("Choose a card");
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
                        // Check if the card is not already isFlipped
                        if (isFlipped[currentIndex]) // Check if the card is not already isFlipped
                        {
                            Console.WriteLine("The card is already flipped.");
                            Thread.Sleep(500);
                            Console.ReadKey();
                            waitingForChoice = true;
                        }
                        else
                        {
                            FlipCard(hiddenCards, cards, currentIndex); // Flip the card
                            previousIndex = currentIndex;
                            return previousIndex; // Return the result
                        }
                        break;

                }

            }
            return previousIndex; // Return the result
        }

        // Selects a card using arrow keys, displays the board with a cursor. 
        static int SelectCardWithArrowKeys2(char[] hiddenCards, char[] cards, int currentIndex, bool[] isFlipped, int previousIndex, int tries)
        {
            bool waitingForChoice = true;
            int secondIndex = -1;

            while (waitingForChoice)
            {
                Console.Clear();
                DisplayBoardWithCursor(hiddenCards, currentIndex, tries);
                Console.WriteLine("Choose a second card");
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
                            Console.ReadKey();
                            waitingForChoice = true;
                        }
                        else
                        {
                            FlipCard(hiddenCards, cards, currentIndex); // Flip the card

                            isFlipped[currentIndex] = true; // Mark the card as isFlipped
                            secondIndex = currentIndex;


                            Console.WriteLine("Checking for match");
                            waitingForChoice = false;
                            return secondIndex; // Return the result
                        }
                        break;


                }

            }
            return secondIndex;
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
