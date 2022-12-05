using System.Text.RegularExpressions;

public static class Program
{
    private static List<Stack<char>> ReadStacks(StreamReader reader)
    {
        var results = new List<Stack<char>>();
        var lines = new List<string>();

        string? line;
        while (!string.IsNullOrWhiteSpace((line = reader.ReadLine())))
        {
           lines.Add(line);
        }

        lines.Reverse();

        var lastLine = lines.First();
        for (var i = 0; i < lastLine.Length; i++)
        {
            if (false == char.IsDigit(lastLine[i]))
            {
                continue;
            }

            var stack = new Stack<char>();
            for (var s = 1; s < lines.Count; s++)
            {
                var current = lines[s][i];
                if (!char.IsLetter(current))
                {
                    break;
                }

                stack.Push(current);
            }

            results.Add(stack);
        }
        return results;
    }

    private static void MoveCratesV1(Stack<char> moveFrom, Stack<char> moveTo, int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            moveTo.Push(moveFrom.Pop());
        }
    }

    private static void MoveCratesV2(Stack<char> moveFrom, Stack<char> moveTo, int amount)
    {
        var tmpStack = new Stack<char>();
        for (var i = 0; i < amount; i++)
        {
            tmpStack.Push(moveFrom.Pop());
        }
        while (tmpStack.Count> 0)
        {
            moveTo.Push(tmpStack.Pop());
        }
    }

    private static void PerformMoves(List<Stack<char>> stacks, StreamReader reader)
    {
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var split = line.Split(' ');
            var amount = int.Parse(split[1]);
            var moveFrom = int.Parse(split[3]) - 1;
            var moveTo = int.Parse(split[5]) - 1;

           // MoveCratesV1(stacks[moveFrom], stacks[moveTo], amount);
           MoveCratesV2(stacks[moveFrom], stacks[moveTo], amount);
        }
    }

    public static int Main(string[] args)
    {
        using var file = File.OpenRead(args[0]);
        using var fileReader = new StreamReader(file);

        var stacks = ReadStacks(fileReader);
        PerformMoves(stacks, fileReader);

        Console.Write(string.Join("", stacks.Select(s => s.Count == 0 ? '_' : s.Peek())));

        return 0;
    }
}