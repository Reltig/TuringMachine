// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Turing
{
    class Program
    {
        static void Main(string[] args)
        {
            /*JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new CustomConverter());
                return settings;
            };*/
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