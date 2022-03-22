namespace Lab7_v._14;

internal static class Program
{
    private static void Main()
    {
        var randArray = Shuffle(new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9});
        Console.WriteLine("Создана перестановка: ");
        Console.Write("|");
        for (var i = 0; i < 9; i++) Console.Write(i+" ");
        Console.WriteLine("9|");
        Console.Write("|");
        for (var i  = 0; i < randArray.Count-1; i++) Console.Write(randArray[i]+" ");
        Console.WriteLine(randArray[9]+"|");

        Console.WriteLine("Введете ограничения");

        var limits = new[,]
        {
            {int.Parse(Console.ReadLine()!), int.Parse(Console.ReadLine()!)},
            {int.Parse(Console.ReadLine()!), int.Parse(Console.ReadLine()!)}
        }; PrintLimits(limits);


        for (var i = 0; i < limits.GetLength(0); i++)
        {
            var limit = new List<int>
            {
                limits[i, 0],
                limits[i, 1]
            };
            if (randArray.FindIndex(x => x == limit[0]) > randArray.FindIndex(x => x == limit[1]))
                Console.WriteLine("Перестановка некорректна для ограничения " + (i + 1));
            else Console.WriteLine("Перестановка корректна для ограничения " + (i + 1));
        }

        Console.ReadKey(true);
    }

    private static List<int> Shuffle(List<int> array)
    {
        var rand = new Random();

        for (var i = array.Count - 1; i >= 1; i--)
        {
            var j = rand.Next(i + 1);
            (array[j], array[i]) = (array[i], array[j]);
        }

        return array;
    }

    private static void PrintLimits(int[,] data)
    {
        Console.Write("Вы ввели: ");
        Console.Write("{");
        for (var i = 0; i < data.GetLength(0) - 1; i++) Console.Write($"({data[i, 0]},{data[i, 1]}),");
        Console.Write($"({data[data.GetLength(0) - 1, 0]},{data[data.GetLength(0) - 1, 1]})");
        Console.WriteLine("}");
    }
}