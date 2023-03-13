using System.Collections;

namespace Turing;

public sealed class Rules : ICollection<Rule>
{
    public List<Rule> data { get; set; }

    //public IEnumerable<Rule> Data => data;

    public Rules()
    {
        data = new();
    }

    public Rules(IEnumerable<Rule> rules) => data = rules.ToList();

    public void Add(Rule rule) => data.Add(rule);
    public void Clear() => data.Clear();
    public bool Contains(Rule item) => data.Contains(item);
    public void CopyTo(Rule[] array, int arrayIndex) => data.CopyTo(array, arrayIndex);
    public bool Remove(Rule item) => data.Remove(item);

    public int Count
    {
        get => data.Count;
    }

    public bool IsReadOnly
    {
        get => false;
    }

    public char[] Alphabet
    {
        get => data
            .SelectMany(r => new[] { r.OpeningLetter, r.ClosingLetter })
            .Distinct()
            .ToArray();
    }

    public string[] States
    {
        get => data
            .SelectMany(r => new[] { r.OpeningState, r.ClosingState })
            .Distinct()
            .ToArray();
    }

    public Rule? this[string state, char letter]
    {
        get
        {
            var result = data
                .FirstOrDefault(rule => rule.OpeningState == state && rule.OpeningLetter == letter);
            return result;
        }
    }

    public void AddRange(IEnumerable<Rule> rules) =>
        data.AddRange(rules);

    public IEnumerator<Rule> GetEnumerator()
    {
        foreach (var rule in data)
            yield return rule;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}