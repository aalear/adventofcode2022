var input = File.ReadAllLines("input.txt");

var fullyContainedCount = 0;
var overlapCount = 0;
foreach (var line in input)
{
    var ranges = line.Split(',');

    var rangeA = new Range(ranges[0]);
    var rangeB = new Range(ranges[1]);

    if (rangeA.IsContainedIn(rangeB) || rangeB.IsContainedIn(rangeA))
    {
        fullyContainedCount++;
    }

    if (rangeA.OverlapsWith(rangeB))
    {
        overlapCount++;
    }
}

Console.WriteLine(fullyContainedCount);
Console.WriteLine(overlapCount);

record Range
{
    public Range(string definition)
    {
        var boundaries = definition.Split('-').Select(i => int.Parse(i)).ToList();
        Start = boundaries[0];
        End = boundaries[1];
    }

    public int Start { get; init; }
    public int End { get; init; }

    public bool IsContainedIn(Range other) => Start >= other.Start && End <= other.End;

    public bool OverlapsWith(Range other) => (Start >= other.Start && Start <= other.End)
                                          || (End >= other.Start && End <= other.End)
                                          || (other.Start >= Start && other.Start <= End)
                                          || (other.End >= Start && other.End <= End);
}