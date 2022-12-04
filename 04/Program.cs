
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

public record SearchRange(int StartRange, int EndRange)
{
    public static SearchRange Parse(string input)
    {
        var ends = input.Split("-");
        return new SearchRange(int.Parse(ends[0]), int.Parse(ends[1]));
    }

    public int Length => (this.EndRange - this.StartRange) + 1;

    public bool Contains(SearchRange range)
    {
        return this.StartRange <= range.StartRange
            && this.EndRange >= range.EndRange;
    }

    public bool Overlaps(SearchRange range)
    {
        if (this.Contains(range)) return true;
        if (range.Contains(this)) return true;

        if (this.StartRange <= range.StartRange
            && this.EndRange >= range.StartRange)
        {
            return true;
        }

        if (this.StartRange <= range.EndRange
            && this.EndRange >= range.EndRange)
        {
            return true;
        }

        return false;
    }
}

public static class Program
{
    private static int Part1(string line)
    {
        var ranges = line.Split(',')
                .Select(SearchRange.Parse)
                .OrderByDescending(s => s.Length)
                .ToList();

        return ranges[0].Contains(ranges[1]) ? 1 : 0;
    }

    private static int Part2(string line)
    {
        var ranges = line.Split(',')
                .Select(SearchRange.Parse)
                .ToList();

        return ranges[0].Overlaps(ranges[1]) ? 1 : 0;
    }

    public static int Main(string[] args)
    {
        using var file = File.OpenRead(args[0]);
        using var fileReader = new StreamReader(file);

        var count = 0;
        var count2 = 0;

        string? line;
        while ((line = fileReader.ReadLine()) != null)
        {
            count += Part1(line);
            count2 += Part2(line);
        }

        Console.WriteLine("Result 1: " + count);
        Console.WriteLine("Result 2: " + count2);
        return 0;
    }
}