// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;

namespace Turing
{
    class Program
    {
        static void Main(string[] args)
        {
            var machine = new TuringMachine();
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                Executor.Excecute(machine, line);
            }
        }
    }
}