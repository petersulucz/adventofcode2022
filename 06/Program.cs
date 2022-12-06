
public static class Program
{

    private static int FindStartSequence(ReadOnlyMemory<char> input, int len) => 
        Enumerable.Range(len, input.Length - len)
            .First(idx => input.Slice(idx - len, len).ToArray().Distinct().Count() == len);

    public static int Main(string[] args)
    {
        using var file = File.OpenRead(args[0]);
        using var fileReader = new StreamReader(file);

        string? line;
        while ((line = fileReader.ReadLine()) != null)
        {
            var mem = line.AsMemory();
            var packet = FindStartSequence(mem, 4);
            Console.WriteLine("Start packet: " + packet);
            Console.WriteLine("Start Message: " + (FindStartSequence(mem.Slice(packet), 14) + packet));
        }

        return 0;
    }
}