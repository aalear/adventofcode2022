var input = File.ReadAllLines("input.txt");

var X = 1;
var cycle = 0;

var pos = 0;
var CRT_row = new char[40];

var signalStrength = 0;
for (var i = 0; i < input.Length; i++)
{
    var command = input[i];

    var op = command[0..4].Trim();

    if (op == "noop")
    {
        Draw();
        UpdateCycleAndSignalStrengthIfNeeded();
        continue;
    }

    Draw();
    UpdateCycleAndSignalStrengthIfNeeded();

    Draw();
    UpdateCycleAndSignalStrengthIfNeeded();

    X += int.Parse(command[5..]);
}

// Part 1
Console.WriteLine(signalStrength);

void UpdateCycleAndSignalStrengthIfNeeded()
{
    cycle++;

    if (cycle % 40 != 20)
        return;

    signalStrength += cycle * X;
}

// Part 2
void Draw()
{
    if (pos == X - 1 || pos == X || pos == X + 1)
    {
        CRT_row[pos] = '#';
    }
    else
    {
        CRT_row[pos] = '.';
    }
    pos++;

    if((cycle + 1) % 40 == 0)
    {
        Console.WriteLine(string.Join("", CRT_row));
        CRT_row = new char[40];
        pos = 0;
    }
}