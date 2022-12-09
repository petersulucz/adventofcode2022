
class Pose
{
    public int Row { get; set; }
    public int Column { get; set; }

    public Pose(int row, int column)
    {
        this.Row = row;
        this.Column = column;
    }

    public void MoveTowards(Pose other)
    {
        var distRow = Math.Abs(other.Row - this.Row);
        var distCol = Math.Abs(other.Column - this.Column);
        if (distCol + distRow <= 1 || (distCol == 1 && distRow == 1))
        {
            return;
        }

        var moveRow = Math.Clamp(other.Row - this.Row, -1, 1);
        var moveCol = Math.Clamp(other.Column - this.Column, -1, 1);
        this.Row += moveRow;
        this.Column += moveCol;
    }
}

public static class Program
{
    public static int Main(string[] args)
    {
        using var file = File.OpenRead(args[0]);
        using var fileReader = new StreamReader(file);

        var hits = new HashSet<(int, int)>();
        var knots = Enumerable.Range(0, 10).Select(i => new Pose(int.MaxValue / 2, int.MaxValue / 2)).ToArray();
        hits.Add((knots[knots.Length - 1].Row, knots[knots.Length - 1].Column));
        string? line;
        while ((line = fileReader.ReadLine()) != null)
        {
            var moveRow = 0;
            var moveCol = 0;
            var split = line.Split(" ");
            var dist = int.Parse(split[1]);
            var add = 1;
            switch (split[0])
            {
                case "R": moveCol = dist; break;
                case "L": moveCol = dist*-1; add = -1; break;
                case "U": moveRow = dist*-1; add = -1; break;
                case "D": moveRow = dist; break;
            }

            while (moveCol != 0 || moveRow != 0)
            {
                if (moveCol != 0)
                {
                    knots[0].Column += add;
                    moveCol -= add;
                }
                if (moveRow != 0)
                {
                    knots[0].Row += add;
                    moveRow -= add;
                }

                for (var i = 1; i < knots.Length; i++)
                {
                    knots[i].MoveTowards(knots[i-1]);
                }
                hits.Add((knots[knots.Length-1].Row, knots[knots.Length - 1].Column));
            }
        }

        // Part 1
        Console.WriteLine("Part 1: " + hits.Count);
        //Console.WriteLine("Part 2: {0}", maxScore);

        return 0;
    }
}