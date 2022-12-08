public class Tree
{
    public int Height { get; }

    public Tree(int height)
    {
        this.Height = height;
    }

    public bool IsVisible { get; set; }
}


public static class Program
{

    public static Tree[] ReadLine(string line)
    {
        return line.ToCharArray().Select(c => new Tree(int.Parse(c + ""))).ToArray();   
    }

    private static int CalculateViewDistance(IList<Tree[]> tree, int row, int column, int rd, int cd)
    {
        var height = tree[row][column].Height;

        row += rd;
        column += cd;
        var distance = 0;
        while (row >= 0 && column >= 0 && row < tree.Count && column < tree[0].Length)
        {
            if (tree[row][column].Height < height)
            {
                distance++;
            }
            else
            {
                distance++;
                break;
            }

            row += rd;
            column += cd;
        }
        return distance;
    }

    private static void MarkVisible(IList<Tree[]> tree, int row, int column, int rd, int cd)
    {
        var previousHeight = -1;
        while (row >= 0 && column >= 0 && row < tree.Count && column < tree[0].Length)
        {
            if (tree[row][column].Height > previousHeight)
            {
                tree[row][column].IsVisible = true;
                previousHeight = tree[row][column].Height;
            }

            row += rd;
            column += cd;
        }
    }

    public static int CalculateScenicScore(IList<Tree[]> tree, int row, int column)
    {
        var height = tree[row][column].Height;
        return CalculateViewDistance(tree, row, column, -1, 0)
            * CalculateViewDistance(tree, row, column, 1, 0)
            * CalculateViewDistance(tree, row, column, 0, -1)
            * CalculateViewDistance(tree, row, column, 0, 1);
    }

    public static void MarkVisible(IList<Tree[]> tree)
    {
        var cols = tree[0].Length;
        for (var r = 0; r < tree.Count; r++)
        {
            MarkVisible(tree, r, 0, 0, 1);
            MarkVisible(tree, r, cols - 1, 0, -1);
        }
        for (var c = 0; c < cols; c++)
        {
            MarkVisible(tree, 0, c, 1, 0);
            MarkVisible(tree, tree.Count - 1, c, -1, 0);
        }
    }


    public static int Main(string[] args)
    {
        using var file = File.OpenRead(args[0]);
        using var fileReader = new StreamReader(file);

        var lines = new List<Tree[]>();
        string? line;
        while ((line = fileReader.ReadLine()) != null)
        {
            lines.Add(ReadLine(line));
        }

        MarkVisible(lines);

        // Part 1
        Console.WriteLine("Part 1: " + lines.SelectMany(l => l).Where(t => t.IsVisible).Count());

        // Part 2
        int maxScore = 0;
        for (var r = 1; r < lines.Count - 1; r++)
        {
            for (var c = 1; c < lines[0].Length -1 ; c++)
            {
                maxScore = Math.Max(maxScore, CalculateScenicScore(lines, r, c));
            }
        }
        Console.WriteLine("Part 2: {0}", maxScore);

        return 0;
    }
}