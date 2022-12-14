var input = File.ReadAllLines("input.txt");

var rowCount = input.Length;
var colCount = input.First().Length;

// Process into an array
var treeMap = new int[rowCount, colCount];

for (var r = 0; r < rowCount; r++)
{
    var line = input[r];
    for (var c = 0; c < colCount; c++)
    {
        treeMap[r, c] = line[c] - '0';
    }
}

// Parts 1 & 2
var visibleTreeCount = 0;
var maxScenicScore = 0;
for (var r = 0; r < rowCount; r++)
{
    for (var c = 0; c < colCount; c++)
    {
        var currentTreeHeight = treeMap[r, c];
        var left = Enumerable.Range(0, c).Select(col => treeMap[r, col]).Reverse();
        var right = Enumerable.Range(c + 1, colCount - c - 1).Select(col => treeMap[r, col]);
        var top = Enumerable.Range(0, r).Select(row => treeMap[row, c]).Reverse();
        var bottom = Enumerable.Range(r + 1, rowCount - r - 1).Select(row => treeMap[row, c]);

        var isEdgeTree = !left.Any() || !right.Any() || !top.Any() || !bottom.Any();

        if (isEdgeTree
            || left.All(h => h < currentTreeHeight)
            || right.All(h => h < currentTreeHeight)
            || top.All(h => h < currentTreeHeight)
            || bottom.All(h => h < currentTreeHeight))
        {
            visibleTreeCount++;
        }

        var currentScenicScore = CalculateScenicScore(currentTreeHeight, left, right, top, bottom);
        maxScenicScore = currentScenicScore > maxScenicScore ? currentScenicScore : maxScenicScore;
    }
}

Console.WriteLine(visibleTreeCount);
Console.WriteLine(maxScenicScore);

int CalculateScenicScore(int currentTreeHeight, IEnumerable<int> left, IEnumerable<int> right, IEnumerable<int> top, IEnumerable<int> bottom)
{
    var leftScore = CalculateDirectionScore(currentTreeHeight, left);
    var rightScore = CalculateDirectionScore(currentTreeHeight, right);
    var topScore = CalculateDirectionScore(currentTreeHeight, top);
    var bottomScore = CalculateDirectionScore(currentTreeHeight, bottom);

    return leftScore * rightScore * topScore * bottomScore;
}

int CalculateDirectionScore(int currentTreeHeight, IEnumerable<int> otherTrees)
{
    var score = 0;

    foreach (var treeHeight in otherTrees)
    {
        score++;
        if (treeHeight >= currentTreeHeight)
            break;
    }

    return score;
}