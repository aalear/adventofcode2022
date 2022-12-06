using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

var endOfStacksIndex = Array.IndexOf(input, input.Single(line => string.IsNullOrEmpty(line))) - 1;
var numStacks = int.Parse(input[endOfStacksIndex].Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());

var stacksPartOne = new Dictionary<int, List<string>>();
var stacksPartTwo = new Dictionary<int, List<string>>();
for (var i = 1; i <= numStacks; i++) 
{
    stacksPartOne.Add(i, new List<string>());
    stacksPartTwo.Add(i, new List<string>());
}

// Populate the stacks...
for (var stackLineIndex = 0; stackLineIndex < endOfStacksIndex; stackLineIndex++)
{
    var line = input[stackLineIndex];

    for (var c = 0; c < line.Length; c += 4)
    {
        // Only need 3 to test with
        var col = Regex.Replace(line.Substring(c, 3), @"\[|\]", "");
        
        if (string.IsNullOrWhiteSpace(col))
            continue;

        stacksPartOne[c / 4 + 1].Insert(0, col);
        stacksPartTwo[c / 4 + 1].Insert(0, col);
    }
}

// And process the moves...
var moveRegex = new Regex(@"move (?<numToMove>\d+) from (?<from>\d+) to (?<to>\d+)", RegexOptions.Compiled);
for (var i = endOfStacksIndex + 2; i < input.Length; i++)
{
    var parsedInstruction = moveRegex.Match(input[i]);
    var numToMove = int.Parse(parsedInstruction.Groups["numToMove"].Value);
    var from = int.Parse(parsedInstruction.Groups["from"].Value);
    var to = int.Parse(parsedInstruction.Groups["to"].Value);

    // Part 1 - move one crate at a time
    for (var j = 0; j < numToMove; j++)
    {
        var fromStackP1 = stacksPartOne[from];
        stacksPartOne[to].Add(fromStackP1.Last());
        fromStackP1.RemoveAt(fromStackP1.Count - 1);
    }

    // Part 2 - move X crates at once in the same order
    var fromStackP2 = stacksPartTwo[from];
    var cratesToMove = fromStackP2.Skip(fromStackP2.Count - numToMove).Take(numToMove);
    stacksPartTwo[to].AddRange(cratesToMove);
    fromStackP2.RemoveRange(fromStackP2.Count - numToMove, numToMove);
}

foreach (var key in stacksPartOne.Keys.OrderBy(k => k))
{
    Console.Write(stacksPartOne[key].Last());
}
Console.WriteLine();

foreach (var key in stacksPartTwo.Keys.OrderBy(k => k))
{
    Console.Write(stacksPartTwo[key].Last());
}
Console.WriteLine();