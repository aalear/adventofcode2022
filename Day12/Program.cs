// Huge thanks to Nicholas Swift for breaking down the A* algorithm
// in a way that really made sense to my brain:
// https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2

var input = File.ReadAllLines("input.txt");

var mapWidth = input.First().Length;
var mapHeight = input.Length;
var map = new char[mapHeight, mapWidth];

var possibleStartNodes = new List<Node>();
int endX = 0, endY = 0;

for (var r = 0; r < mapHeight; r++)
{
    for (var c = 0; c < mapWidth; c++)
    {
        var cell = input[r][c];

        map[r, c] = cell == 'E' ? 'z' : cell;

        if (cell == 'a' || cell == 'S')
        {
            possibleStartNodes.Add(new Node
            {
                X = c,
                Y = r,
                Value = 'a',
                Part1StartNode = cell == 'S'
            });
        }
        if (cell == 'E')
        {
            endX = c;
            endY = r;
        }
    }
}

// Part 1
Console.WriteLine(GetPathLength(possibleStartNodes.Single(n => n.Part1StartNode)));

// Part 2. This takes a hot minute, but gets the job done.
Console.WriteLine(possibleStartNodes.Min(n => GetPathLength(n)));

int? GetPathLength(Node startingNode)
{
    var open = new List<Node> { startingNode };
    var closed = new List<Node>();

    while (open.Any())
    {
        // Let currentNode be the node with the smallest F value
        var currentNode = open.OrderBy(n => n.F).First();

        open.Remove(currentNode);
        closed.Add(currentNode);

        if (currentNode.X == endX && currentNode.Y == endY)
        {
            // We're done!
            return currentNode.StepCountFromStart();
        }

        var candidates = new List<Node>
        {
            new Node { X = currentNode.X - 1, Y = currentNode.Y },
            new Node { X = currentNode.X + 1, Y = currentNode.Y },
            new Node { X = currentNode.X, Y = currentNode.Y - 1 },
            new Node { X = currentNode.X, Y = currentNode.Y + 1 }
        };
        // Remove all nodes beyond the bounds of the map
        candidates.RemoveAll(n => n.X < 0 || n.X >= mapWidth || n.Y < 0 || n.Y >= mapHeight);
        // Populate node values based on the map
        candidates.ForEach(n => n.Value = map[n.Y, n.X]);
        // Remove all nodes we're prohibited from moving to
        candidates.RemoveAll(n => n.Value - currentNode.Value > 1);

        foreach (var candidate in candidates)
        {
            if (closed.Contains(candidate))
                continue;

            // current.G + distance between candidate and current. In this case, the distance is always 1 step.
            candidate.G = currentNode.G + 1;
            // Distance from candidate to end
            candidate.H = Math.Abs(endX - currentNode.X) + Math.Abs(endY - currentNode.Y);
            candidate.F = candidate.G + candidate.H;

            if (open.Contains(candidate))
            {
                var existingCandidate = open.Where(n => n.X == candidate.X && n.Y == candidate.Y).Single();
                if (candidate.G > existingCandidate.G)
                    continue;

                existingCandidate.Parent = currentNode;
                existingCandidate.G = existingCandidate.StepCountFromStart();
                existingCandidate.F = existingCandidate.G + existingCandidate.H;
            }
            else
            {
                candidate.Parent = currentNode;
                open.Add(candidate);
            }
        }
    }

    // Path Not Found. This will not happen under the puzzle constraints, probably.
    return null;
}

public class Node : IEquatable<Node>
{
    public int X { get; init; }
    public int Y { get; init; }

    public char Value { get; set; }

    public int F { get; set; }
    public int G { get; set; }
    public int H { get; set; }

    public bool Part1StartNode { get; init; }

    public Node Parent { get; set; }

    public bool Equals(Node? other)
    {
        return other != null && other.X == X && other.Y == Y;
    }

    public int StepCountFromStart()
    {
        var temp = this;
        var stepCount = -1;
        while (temp != null)
        {
            stepCount++;
            temp = temp.Parent;
        }
        return stepCount;
    }
}