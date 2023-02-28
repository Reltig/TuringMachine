namespace Turing;

public class MachineSettings
{
    public HashSet<char> Alphabet { get; set; }
    public HashSet<string> States { get; set; }
    public string CurrentState { get; set; }
    public Rules Rule { get; set; }
    public string Tape { get; set; }
}