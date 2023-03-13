namespace Turing;

public class MachineSettings
{
    public string CurrentState { get; set; }
    public Rules Rule { get; set; }
    public string Tape { get; set; }
    public int Position { get; set; }
}