var input = File.ReadAllLines("input.txt")[0];

for (var i = 0; i < input.Length - 4; i++)
{
    var candidateMarker = input.Substring(i, 4);

    if (candidateMarker.Distinct().Count() == 4)
    {
        Console.WriteLine(i + 4);
        break;
    }
}

for (var i = 0; i < input.Length - 14; i++)
{
    var candidateMessage = input.Substring(i, 14);

    if (candidateMessage.Distinct().Count() == 14)
    {
        Console.WriteLine(i + 14);
        break;
    }
}