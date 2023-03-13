using System.Text;
using System.Text.Json;
using ConsoleTables;

namespace Turing;

public sealed class TuringMachine
{
    private const int TapeMaxLenght = 65536;

    private Rules _rules;

    private string _currentState;
    private int _pointer;
    private char[] _tape;

    public TuringMachine()
    {
        _currentState = null;
        _pointer = 0;
        _tape = new char[TapeMaxLenght];
        _rules = new();
    }

    private void Move(int offset)
    {
        _pointer = (_pointer + offset) % TapeMaxLenght;
    }

    private void MoveLeft() => Move(-1);
    private void MoveRight() => Move(1);

    public string Tape
    {
        get => new string(_tape).Replace('\u0000', (char)0); //TODO: доделать
    }

    private char CurrentCell
    {
        get => _tape[_pointer];
        set => _tape[_pointer] = value;
    }

    public MachineSettings Settings
    {
        get => new MachineSettings()
        {
            CurrentState = _currentState,
            Rule = _rules,
            Tape = new string(_tape),
            Position = _pointer
        };
        private set
        {
            _currentState = value.CurrentState;
            _rules = value.Rule;
            _tape = value.Tape.ToCharArray();
            _pointer = value.Position;
        }
    }

    public void SetInitialState(string state)
    {
        _currentState ??= state;
    }

    public void InsertTape(string tapeClip)
    {
        for (int i = 0; i < tapeClip.Length; i++)
            _tape[(_pointer + i) % TapeMaxLenght] = tapeClip[i];
    }

    public void UpdateRules(Rule rule)
    {
        Update(rule);

        _rules.Add(rule);
    }

    public void UpdateRules(IEnumerable<Rule> rules)
    {
        foreach (var rule in rules)
            UpdateRules(rule);
    }

    private void Update(Rule rule)
    {
        _currentState ??= rule.OpeningState;
        //TODO: переписать или убрать(инициализировать в момент первого шага)
    }

    public void NextStep()
    {
        if (_currentState is null)
            throw new Exception("Current state is undefined");
        var rule = _rules[_currentState, CurrentCell];
        _currentState = rule.ClosingState;
        CurrentCell = rule.ClosingLetter;
        switch (rule.Action)
        {
            case 'L':
                MoveLeft();
                break;
            case 'R':
                MoveRight();
                break;
            case 'N':
                break;
            default:
                throw new Exception("Invalid exception");
        }
    }

    public void DoSteps(int count)
    {
        for (int i = 0; i < count; i++)
            NextStep();
    }

    public string TapeClipping(int lenght) => TapeClipping(_pointer, _pointer + lenght);

    public string TapeClipping(int startPosition, int endPosition)
    {
        if (startPosition > endPosition)
            throw new ArgumentException("");
        var builder = new StringBuilder();
        for (int i = startPosition; i < endPosition; i++)
        {
            builder.Append(_tape[i % TapeMaxLenght]);
        }

        return builder.ToString() + "\n" + (
            startPosition <= _pointer && _pointer <= endPosition
                ? new string(' ', endPosition - startPosition).Insert(_pointer - startPosition, "^")
                : new string(' ', endPosition - startPosition + 1)
        );
    }

    public string Serialize() =>
        JsonSerializer.Serialize(Settings);

    public void Deserialize(string json) //TODO: должно возвращать TuringMachine?
    {
        Settings = JsonSerializer.Deserialize<MachineSettings>(json) ?? throw new Exception("Settings error");
    }

    public void PrintRules()
    {
        var alphbet = _rules.Alphabet;
        var states = _rules.States;

        var columns = new List<string>() { "State\\Symbol" };
        columns.AddRange(alphbet.Select(c => c.ToString()));
        var table = new ConsoleTable(columns.ToArray());
        foreach (var state in states)
        {
            var row = new List<string>() { state };
            row.AddRange(alphbet.Select(alp =>
            {
                var rule = _rules[state, alp];
                if (rule is null)
                    return " ";
                return $"{rule.ClosingState} {rule.ClosingLetter} {rule.Action}";
            }));
            table.AddRow(row.ToArray());
        }

        table.Write();
    }
}