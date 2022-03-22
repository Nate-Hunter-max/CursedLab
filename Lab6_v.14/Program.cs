namespace Lab6_v._14;

internal static class Program
{
    private static void Main()
    {
        PrintArr(Calc(3,4));
    }

    private static List<List<byte>> Calc(byte N, byte M)
    {
        var part = new Queue<byte>();
        var parts = new List<List<byte>>();

        for (var i = 1; i <= N; i++) part.Enqueue((byte) i);

        for (var i = 0; i < N; i++)
        {
            parts.Add(part.ToList());
            part.Enqueue(part.Dequeue());
        }

        return parts;
    }

    private static void PrintArr<T>(List<List<T>> arr)
    {
        foreach (var b in arr)
        {
            foreach (var c in b) Console.Write(c + " ");
            Console.WriteLine();
        }
    }
}