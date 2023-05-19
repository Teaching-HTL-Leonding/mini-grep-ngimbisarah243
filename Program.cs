bool caseInsensitiveSearch = false;
bool isRekcursive = false;
string path = "";
string fileType = "";
string searchText = "";

foreach (string element in args)
{
    if (element == "-i") caseInsensitiveSearch = true;
    else if (element == "-R") isRekcursive = true;
    else if (element == "-iR" || element == "-Ri") caseInsensitiveSearch = isRekcursive = true;
    else if (element.StartsWith('*')) fileType = element;
    else if (element == args[^1]) searchText = element;
    else path = element;
}

var datas = Directory.GetFiles(path, fileType, isRekcursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
int countLines = 0;
int countFiles = 0;
int countOccurences = 0;

foreach (string data in datas)
{
    bool alreadyOutputtedFileName = false;
    var rows = File.ReadAllLines(data);
    int sequentialNumbering = 1;

    foreach (string row in rows)
    {
        if (rows.Contains(searchText) || caseInsensitiveSearch && row.ToUpper().Contains(searchText.ToUpper()))
        {
            if (alreadyOutputtedFileName == false) { countFiles++; alreadyOutputtedFileName = true; SetOutputInColor(data); }
            countOccurences += CountOccurences(row, searchText, caseInsensitiveSearch);
            Console.Write("{0}: ", sequentialNumbering);
            Console.WriteLine(caseInsensitiveSearch ? row.Replace(searchText, $">>>{searchText.ToUpper()}<<<", StringComparison.OrdinalIgnoreCase) : row.Replace(searchText, $">>>{searchText.ToUpper()}<<<"));
            countLines++;
        }
        sequentialNumbering++;
    }
}
Console.WriteLine();
SetOutputInColor("SUMMARY:");
Console.WriteLine($"Number of found files: {countFiles}");
Console.WriteLine($"Number of found lines: {countLines}");
Console.WriteLine($"Number of occurences:  {countOccurences}");

int CountOccurences(string row, string searchtext, bool casingInsesitiveSearch)
{
    string tempString = string.Empty;
    int counter = 0;

    foreach (char letter in row)
    {
        tempString += letter;

        if (tempString.Contains(searchtext)) { counter++; tempString = string.Empty; }
        else if (casingInsesitiveSearch && tempString.Contains(searchtext, StringComparison.OrdinalIgnoreCase)) { counter++; tempString = string.Empty; }
    }

    return counter;
}
void SetOutputInColor(string input)
{
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine(input);
    Console.ResetColor();
}