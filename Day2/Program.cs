var input = File.ReadAllLines("input.txt").Select(l => l.Trim().Split(" "));

var moveMap = new char[3,3]
{
   // Win, Draw, Lose
   { 'C', 'A', 'B' }, // Rock
   { 'A', 'B', 'C' }, // Paper
   { 'B', 'C', 'A' }  // Scissors
};

var partOneScore = 0;
var partTwoScore = 0;
foreach (var round in input)
{
    var opponent = Convert.ToChar(round[0]);
    var you = Convert.ToChar(round[1]);

    // Part 1
    partOneScore += GetRoundScore(GetWinner(opponent, you), you, opponent);

    // Part 2
    you = PickResponse(opponent, you);
    partTwoScore += GetRoundScore(GetWinner(opponent, you), you, opponent);
}

Console.WriteLine(partOneScore);
Console.WriteLine(partTwoScore);

char PickResponse(char move, char direction) => moveMap[move - 'A', direction - 'X'];

int GetRoundScore(char? winner, char you, char opponent)
{
    const int DRAW_POINTS = 3;
    const int WIN_POINTS = 6;

    // Subtract the preceding character to 'A' and 'X' because points are 1-based.
    var points = you - 'X' < 0 ? you - '@' : you - 'W';

    if (winner == you)
    {
        return WIN_POINTS + points;
    }
    else if (winner == opponent)
    {
        return points;
    }
    else
    {
        return DRAW_POINTS + points;
    }
}

char? GetWinner(char one, char two)
{
    if (IsRock(one))
    {
        if (IsPaper(two))
            return two;
        if (IsScissors(two))
            return one;
    }
    if (IsPaper(one))
    {
        if (IsRock(two))
            return one;
        if (IsScissors(two))
            return two;
    }
    if (IsScissors(one))
    {
        if (IsRock(two))
            return two;
        if (IsPaper(two))
            return one;
    }

    return null;
}

bool IsRock(char move) => move == 'A' || move == 'X';
bool IsPaper(char move) => move == 'B' || move == 'Y';
bool IsScissors(char move) => move == 'C' || move == 'Z';