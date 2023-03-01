namespace Turing;

public static class Executor
{
    private static Dictionary<string, Action<TuringMachine, string[]>> allCommands;
    private static Parser _parser = new();

    static Executor()
    {
        allCommands = new();
        allCommands.Add("exit", (machine, args) => Environment.Exit(0));
        allCommands.Add("cls", (machine, args) => Console.Clear());
        allCommands.Add("step", (machine, args) =>
        {
            if (args.Length == 0)
            {
                machine.NextStep();
                return;
            }

            if (args.Length > 1)
                throw new ArgumentException("Wrong arguments count");
            int stepCount;
            if (!int.TryParse(args[0], out stepCount))
                throw new ArgumentException("Not integer steps count");
            if (stepCount < 0)
                throw new ArgumentException("Steps count must be non-negative");
            machine.DoSteps(stepCount);
        });
        allCommands.Add("settape", (machine, args) =>
        {
            if (args.Length != 1)
                throw new ArgumentException("Wrong arguments count");
            var tape = args[0];
            machine.InsertTape(tape);
        });
        allCommands.Add("setinitstate", (machine, args) =>
        {
            if (args.Length != 1)
                throw new ArgumentException("Wrong arguments count");
            var state = args[0];
            machine.SetInitialState(state);
        });
        allCommands.Add("show", (machine, args) =>
        {
            var clip = machine.TapeClipping(0, 10);
            Console.WriteLine(clip);
        });
        allCommands.Add("setrule", (machine, args) =>
        {
            var ruleExpression = String.Join(' ', args);
            Rule rule;
            if (!_parser.TryParse(ruleExpression, out rule))
                throw new Exception("Invalid rule syntax"); //TODO: заменить все исключения
            machine.UpdateRules(rule);
        });
        allCommands.Add("saveconfig", (machine, args) =>
        {
            string path = "machine-settings";
            if (args.Length > 0)
                path = args[0];
            var jsonString = machine.Serialize();
            File.WriteAllText($"{path}.json", jsonString);
        });
        allCommands.Add("loadconfig", (machine, args) =>
        {
            string path = "machine-settings";
            if (args.Length > 0)
                path = args[0];
            var jsonString = File.ReadAllText($"{path}.json");
            machine.Deserialize(jsonString);
        });
        allCommands.Add("loadtape", (machine, args) =>
        {
            string path = "tape-result.txt";
            if (args.Length > 0)
                path = args[0];
            var tape = File.ReadAllText($"{path}");
            machine.InsertTape(tape);
        });
        allCommands.Add("savetape", (machine, args) =>
        {
            string path = "tape.txt";
            if (args.Length > 0)
                path = args[0];
            File.WriteAllText($"{path}",
                machine.Tape); //TODO: переделать, сделать получение ленты в виде "ababab...(59)...ab"
        });
    }

    public static void Excecute(TuringMachine machine, string command)
    {
        var commandProperties = command.Split();
        var commandName = commandProperties[0];
        var commandArgs = commandProperties.Skip(1).ToArray();
        if (!allCommands.ContainsKey(commandName))
            throw new Exception("Unknown command.");
        allCommands[commandName].Invoke(machine, commandArgs);
    }
}