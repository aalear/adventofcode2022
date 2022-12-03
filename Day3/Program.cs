var input = File.ReadAllLines("input.txt");

var partOneSum = 0;
foreach (var rucksack in input)
{
    var halfpoint = rucksack.Length / 2;
    var first = rucksack[0..halfpoint];
    var second = rucksack[halfpoint..];

    var overlap = first.Intersect(second).Single();
    partOneSum += GetPriority(overlap);
}

Console.WriteLine(partOneSum);

var partTwoSum = 0;
for (var i = 0; i < input.Length; i += 3)
{
    var groupLines = input.Skip(i).Take(3).ToList();

    var overlap = groupLines[0].Intersect(groupLines[1])
                               .Intersect(groupLines[2])
                               .Single();
    partTwoSum += GetPriority(overlap);
}

Console.WriteLine(partTwoSum);

// Lowercase item types a through z have priorities 1 through 26.
// Uppercase item types A through Z have priorities 27 through 52.
static int GetPriority(char item) => char.IsLower(item) ? item - '`' : item - 'A' + 27;