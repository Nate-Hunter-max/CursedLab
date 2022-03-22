namespace Lab7;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("Расчет цетра тяжести для случайного ряда\n");
        do
        {
            Calculate();
            Console.WriteLine(">esc - выход");
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }

    private static string PrintArr(IEnumerable<double> array)
    {
        return array.Aggregate("{", (current, d) => current + d + ", ").TrimEnd(',', ' ') + '}';
    }

    private static List<double> GenerateSet(int l = 30)
    {
        var rand = new Random();
        var arr = new double[rand.Next(5, l)];
        for (var i = 0; i < arr.Length; i++)
        {
            //arr[i] = Math.Round(-42.132 + rand.NextDouble() * (7.003 + 42.132), 3);
            arr[i] = rand.Next(-10, 10);
        }

        return arr.Distinct().ToList();
    }

    private static void Calculate()
    {
        var set = GenerateSet();
        Console.WriteLine("Сгенерирован ряд:");
        Console.WriteLine(PrintArr(set) + '\n');
        set = set.OrderBy(x => x).ToList();
        var halfLen = set.Count / 2;
        var center = (set.Count % 2 == 0) ? (set[halfLen]+set[halfLen-1])/2 : set[halfLen];
        Console.WriteLine($"Центр тяжести равен: {center}");
    }
}