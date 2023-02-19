namespace Turing;

public static class Executor
{
    private static Dictionary<string, Action<TuringMachine, string[]>> allCommands;

    static Executor()
    {
        allCommands = new();
        allCommands.Add("exit", (machine, args) => Environment.Exit(0));
        allCommands.Add("cls", (machine, args) => Console.Clear());
        allCommands.Add("step", (machine, args) => machine.NextStep());
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
            if (!new Parser().TryParse(ruleExpression, out rule))
                throw new Exception("Invalid rule syntax");//TODO: заменить все исключения
            machine.UpdateRules(rule);
        });
        allCommands.Add("saveconfig", (machine, args) =>
        {
            string path = null;
            if (args.Length > 0)
                path = args[0];
            File.WriteAllText("machine-settings.json", machine.Serialize());
        });
    }

    public static void Excecute(TuringMachine machine, string command)
    {
        var commandProperties = command.Split();//TODO: rename
        var commandName = commandProperties[0];
        var commandArgs = commandProperties.Skip(1).ToArray();
        if (!allCommands.ContainsKey(commandName))
            throw new Exception("Unknown command.");
        allCommands[commandName].Invoke(machine, commandArgs);
    }
}