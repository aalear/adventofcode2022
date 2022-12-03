var input = File.ReadAllLines("input.txt");

var sumsByElf = new List<int> { 0 };
foreach (var line in input)
{
    if (string.IsNullOrEmpty(line))
    {
        sumsByElf.Add(0);
        continue;
    }

    sumsByElf[^1] += int.Parse(line);
}

Console.WriteLine(sumsByElf.Max());
Console.WriteLine(sumsByElf.OrderByDescending(s => s).Take(3).Sum());