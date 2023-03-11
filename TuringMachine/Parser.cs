using System.Text.RegularExpressions;

namespace Turing;

public sealed class Parser
{
    public bool TryParse(string expression, out Rule rule)
    {
        rule = new Rule();
        var expressionRegex = new Regex(@"((\w+\d*\s.{1})|(\w+\d*))\s*->\s*((\w+\d*\s.{1})|(\w+\d*))\s[LRN]");
        if (!expressionRegex.IsMatch(expression))
        {
            Console.WriteLine("Invalid expression");
            return false;
        }

        var conditions = expression
            .Trim()
            .Split("->", StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        var opCondition = conditions[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var opState = opCondition[0];
        var opLetter = opCondition.Length == 2 ? opCondition[1][0] : '\u0000';

        var clCondition = conditions[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var clState = clCondition[0];
        var clLetter = clCondition.Length == 3 ? clCondition[1][0] : '\u0000';
        var action = clCondition.Length == 3 ? clCondition[2][0] : clCondition[1][0];

        rule = new Rule(opState, clState, opLetter, clLetter, action);
        return true;
    }
}