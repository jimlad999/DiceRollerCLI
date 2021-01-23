using System;

namespace DiceRollerCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            DiceRoller.RollDice();

            Console.WriteLine("Press any key to quit");
            Console.ReadLine();
        }
    }
}
