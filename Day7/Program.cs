using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

// Build the directory tree
var root = new Node() { Name = "/" };
var currentNode = root;

var fileRegex = new Regex(@"(?<size>\d+) (?<name>.+)", RegexOptions.Compiled);
for (var i = 1; i < input.Length; i++)
{
    var line = input[i];
    if (line == "$ ls")
    {
        i++;
        while (i < input.Length && !(line = input[i]).StartsWith("$"))
        {
            if (line.StartsWith("dir"))
            {
                currentNode.Children.Add(new Node { Parent = currentNode, Name = line[4..] });
            }
            else
            {
                var parsedLine = fileRegex.Match(line);
                var fileSize = int.Parse(parsedLine.Groups["size"].Value);
                currentNode.Children.Add(new Node
                {
                    Parent = currentNode,
                    FileSize = fileSize,
                    Name = parsedLine.Groups["name"].Value
                });
            }
            i++;
        }
    }

    if (line == "$ cd ..")
    {
        currentNode = currentNode.Parent;
        continue;
    }
    if (line.StartsWith("$ cd"))
    {
        currentNode = currentNode.Children.Where(c => c.Name == line[5..]).Single();
        continue;
    }
}

// Part 1
Console.WriteLine(GetSmallDirectories(root).Where(dir => dir.GetSize() < 100000).Sum(dir => dir.GetSize()));

// Part 2
const int TOTAL_DISK_SIZE = 70000000;
const int MIN_DISK_SIZE = 30000000;
var neededSpace = MIN_DISK_SIZE - (TOTAL_DISK_SIZE - root.GetSize());
Console.WriteLine(GetDeletionCandidates(root, neededSpace).Min(dir => dir.GetSize()));

static IEnumerable<Node> GetSmallDirectories(Node root)
{
    var directories = new List<Node>();

    if (root.GetSize() < 100000)
        directories.Add(root);

    foreach(var dir in root.Children.Where(c => c.IsDirectory))
    {
        directories.AddRange(GetSmallDirectories(dir));
    }

    return directories;
}

static IEnumerable<Node> GetDeletionCandidates(Node root, int minSize)
{
    var directories = new List<Node>();

    if (root.GetSize() >= minSize)
        directories.Add(root);

    foreach (var dir in root.Children.Where(c => c.IsDirectory))
    {
        directories.AddRange(GetDeletionCandidates(dir, minSize));
    }

    return directories;
}

class Node
{
    public Node Parent { get; init; }
    public List<Node> Children { get; init; } = new List<Node>(0);

    public bool IsDirectory => FileSize == null;

    public int GetSize() => FileSize == null ? Children.Sum(c => c.GetSize()) : FileSize.Value;
    public int? FileSize { get; init; } = null;
    public string? Name { get; init; }
}