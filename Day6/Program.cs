var input = File.ReadAllLines("input.txt")[0];

var markerIndex = 0;
var marker = "";
var markerFound = false;

var messageIndex = 0;
var message = "";
var messageFound = false;

foreach (var c in input)
{
    if (!markerFound) {
        markerIndex++;
        marker += c;
        if (marker.Length == 4)
        {
            if (marker.Distinct().Count() == 4)
            {
                markerFound = true;
                Console.WriteLine(markerIndex);
            }
            else
            {
                marker = marker[1..];
            }
        }
    }

    if (!messageFound)
    {
        messageIndex++;
        message += c;
        if (message.Length == 14)
        {
            if (message.Distinct().Count() == 14)
            {
                messageFound = true;
                Console.WriteLine(messageIndex);
            }
            else
            {
                message = message[1..];
            }
        }
    }
}