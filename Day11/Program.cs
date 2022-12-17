var input = File.ReadAllLines("input.txt").Where(l => !string.IsNullOrWhiteSpace(l));

var monkeysP1 = new List<Monkey>();
var monkeysP2 = new List<Monkey>();

for (var i = 0; i < input.Count(); i += 6)
{
    var instructions = input.Skip(i).Take(6).ToList();

    var monkey = new Monkey
    {
        Items = instructions[1].Replace("  Starting items: ", "").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList(),
        Op = instructions[2].Contains('+') ? (x, y) => x + y : (x, y) => x * y,
        OpValue = int.TryParse(instructions[2].Split(" ").Last(), out int val) ? val : null,
        Divisor = int.Parse(instructions[3].Split(" ").Last()),
        TrueMonkeyId = int.Parse(instructions[4].Split(" ").Last()),
        FalseMonkeyId = int.Parse(instructions[5].Split(" ").Last()),
    };

    monkeysP1.Add(monkey);

    monkeysP2.Add(new Monkey
    {
        Items = new List<long>(monkey.Items),
        Op = monkey.Op,
        OpValue = monkey.OpValue,
        Divisor = monkey.Divisor,
        TrueMonkeyId = monkey.TrueMonkeyId,
        FalseMonkeyId = monkey.FalseMonkeyId,
    });
}

// Part 1
for (var round = 0; round < 20; round++)
{
    foreach (var monkey in monkeysP1)
    {
        foreach (var item in monkey.Items)
        {
            var (targetMonkey, worry) = monkey.Inspect(item, (w) => w / 3);
            monkeysP1[targetMonkey].Items.Add(worry);
        }
        monkey.Items.Clear();
    }
}
Console.WriteLine(monkeysP1.OrderByDescending(m => m.ItemsInspected).Take(2).Aggregate(1L, (agg, m) => agg * m.ItemsInspected));

// Part 2
var divisorsProduct = monkeysP2.Select(m => m.Divisor).Aggregate(1L, (agg, d) => agg * d);
for (var round = 0; round < 10000; round++)
{
    foreach (var monkey in monkeysP2)
    {
        foreach (var item in monkey.Items)
        {
            var (targetMonkey, worry) = monkey.Inspect(item, (w) => w % divisorsProduct);
            monkeysP2[targetMonkey].Items.Add(worry);
        }
        monkey.Items.Clear();
    }
}
Console.WriteLine(monkeysP2.OrderByDescending(m => m.ItemsInspected).Take(2).Aggregate(1L, (agg, m) => agg * m.ItemsInspected));

public class Monkey
{
    public List<long> Items { get; init; } = new List<long>();
    public Func<long, long, long> Op { get; set; }
    public int? OpValue { get; init; }
    public int Divisor { get; init; }
    public int TrueMonkeyId { get; init; }
    public int FalseMonkeyId { get; init; }
    public int ItemsInspected { get; set; } = 0;

    public (int, long) Inspect(long item, Func<long, long> worryReducer)
    {
        ItemsInspected++;

        // Worry level increases.
        var worry = Op(item, OpValue ?? item);

        // Monkey is bored.
        worry = worryReducer(worry);

        // Monkey passes the item on to another.
        return (worry % Divisor == 0 ? TrueMonkeyId : FalseMonkeyId, worry);
    }
}
