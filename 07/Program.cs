
public abstract class FSPrimitive
{
    public string Name { get; }
    public FSDirectory Parent { get; }

    public abstract long GetTotalSize();

    public FSPrimitive(FSDirectory parent, string name)
    {
        this.Parent = parent;
        this.Name = name;
    }
}

public class FSFile : FSPrimitive
{
    public int Size { get; }

    public FSFile(FSDirectory parent, string name, int size) : base(parent, name)
    {
        this.Size= size;
    }

    public override long GetTotalSize()
    {
        return this.Size;
    }
}

public class FSDirectory : FSPrimitive
{
    private readonly Dictionary<string, FSPrimitive> children;

    public FSDirectory(FSDirectory parent, string name) : base(parent, name)
    {
        this.children = new Dictionary<string, FSPrimitive>();
    }

    public FSPrimitive this[string name]
    {
        get => children[name];
        set => children[name] = value;
    }

    public IEnumerable<FSPrimitive> AllChildren => this.children.Values;

    public IEnumerable<FSDirectory> Directories => this.children.Values.OfType<FSDirectory>();

    public override long GetTotalSize()
    {
        return this.children.Values.Sum(v => v.GetTotalSize());
    }
}

public static class Program
{

    public static FSDirectory ParseLine(
        FSDirectory root,
        FSDirectory current,
        string input,
        List<string> outputs)
    {
        if (string.Equals(input, "cd /"))
        {
            return root;
        }

        if (string.Equals(input, "cd .."))
        {
            return current.Parent;
        }

        if (input.StartsWith("cd"))
        {
            return (FSDirectory)current[input.Split(" ")[1]];
        }

        // ls is all that remains
        foreach (var item in outputs)
        {
            var split = item.Split(" ");
            var name = split[1];
            if (split[0].Equals("dir", StringComparison.Ordinal))
            {
                current[name] = new FSDirectory(current, name);
            }
            else
            {
                current[name] = new FSFile(current, name, int.Parse(split[0]));
            }
        }
        return current;
    }

    public static IEnumerable<FSDirectory> EnumerateDirectories(FSDirectory root)
    {
        foreach (var child in root.Directories.SelectMany(EnumerateDirectories))
        {
            yield return child;
        }
        yield return root;
    }

    public static IEnumerable<FSDirectory> EnumerateDirectoriesWithSizeCap(FSDirectory root, long sizeCapInclusive)
    {
        foreach (var child in EnumerateDirectories(root).Where(c => c.GetTotalSize() < sizeCapInclusive))
        {
            yield return child;
        }
    }

    public static int Main(string[] args)
    {
        using var file = File.OpenRead(args[0]);
        using var fileReader = new StreamReader(file);

        string input = null;
        List<string> outputs = new List<string>();

        var root = new FSDirectory(null, "/");
        var current = root;

        string? line;
        while ((line = fileReader.ReadLine()) != null)
        {
            if (line.StartsWith("$"))
            {
                if (input != null)
                {
                    current = ParseLine(root, current, input, outputs);
                }

                input = line.Substring(2);
                outputs.Clear();
            }
            else
            {
                outputs.Add(line);
            }
        }
        if (input != null)
        {
            ParseLine(root, current, input, outputs);
        }

        // Part 1
        Console.WriteLine("Part 1: " + EnumerateDirectoriesWithSizeCap(root, 100000).Sum(d => d.GetTotalSize()));

        // Part 2
        var requiredSpace = 30000000 - (70000000 - root.GetTotalSize());
        var deleteMe = EnumerateDirectories(root).OrderBy(d => d.GetTotalSize()).SkipWhile(d => d.GetTotalSize() < requiredSpace).First();
        Console.WriteLine("Part 2: {0} {1}. Required space: {2}", deleteMe.Name, deleteMe.GetTotalSize(), requiredSpace);

        return 0;
    }
}