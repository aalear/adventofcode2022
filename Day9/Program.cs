var input = File.ReadAllLines("input.txt").Select(i => i.Split(" "));

var p1rope = new List<Coordinate> 
{
    new Coordinate(), // head
    new Coordinate(), // tail
};

var p2rope = new List<Coordinate>();
for (var i = 0; i < 10; i++) {
    p2rope.Add(new Coordinate());
}

List<Coordinate> GetTailVisits(List<Coordinate> rope) 
{
    // Everything overlaps at the start and that counts.
    var positionsVisited = new List<Coordinate> { new Coordinate() };
    foreach (var instruction in input)
    {
        var dir = instruction[0];
        var steps = int.Parse(instruction[1]);

        for (var i = 0; i < steps; i++)
        {
            rope[0].Move(dir);

            for (var segment = 1; segment < rope.Count; segment++)
            { 
                if (!rope[segment].IsAdjacent(rope[segment - 1]))
                {
                    var pos = rope[segment].MoveTowards(rope[segment - 1]);

                    if (segment == rope.Count - 1)
                    {
                        positionsVisited.Add(pos);
                    }
                }
            }
        }
    }

    return positionsVisited;
}

Console.WriteLine(GetTailVisits(p1rope).Distinct(Coordinate.Comparer).Count());
Console.WriteLine(GetTailVisits(p2rope).Distinct(Coordinate.Comparer).Count());

public class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }

    public bool IsOverlapping(Coordinate other) => X == other.X && Y == other.Y;

    public bool IsDiagonal(Coordinate other) => Math.Abs(X - other.X) == 1 && Math.Abs(Y - other.Y) == 1;

    public bool IsAdjacent(Coordinate other) => IsOverlapping(other)
                                        || IsDiagonal(other)
                                        || (X == other.X && Math.Abs(Y - other.Y) == 1)
                                        || (Y == other.Y && Math.Abs(X - other.X) == 1);

    public void Move(string direction)
    {
        switch (direction)
        {
            case "R":
                X++;
                break;
            case "U":
                Y--;
                break;
            case "D":
                Y++;
                break;
            case "L":
                X--;
                break;
        }
    }

    public Coordinate MoveTowards(Coordinate other)
    {
        // Assume not adjacent
        if (other.X > X)
        {
            X++;
        }
        if (other.Y < Y)
        {
            Y--;
        }
        if (other.X < X)
        {
            X--;
        }
        if (other.Y > Y)
        {
            Y++;
        }

        return new Coordinate { X = X, Y = Y };
    }

    public static CoordinateEqualityComparer Comparer = new CoordinateEqualityComparer();
}

public class CoordinateEqualityComparer : IEqualityComparer<Coordinate>
{
    public bool Equals(Coordinate? x, Coordinate? y) => x != null && y != null && x.IsOverlapping(y);
    public int GetHashCode(Coordinate obj) => obj.X.GetHashCode() ^ obj.Y.GetHashCode();
}