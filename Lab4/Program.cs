using System.Diagnostics;

namespace Lab4
{
    internal static class Program
    {
        private const double Y = 0.577215664901533;
        private static readonly double[] ReferenceArray = {0.454219905, 3.301285449}; //accurate for x = 0.5 and x = 1.5

        private static void Main()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Convergent series for Exponential integral");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Choose method: \n>i - iterative\n>r - recursive\n");
                StartOnKeyPressedListener();
                Console.WriteLine(">r - restart program\n>another key - exit\n \n");
                if (Console.ReadKey(true).Key == ConsoleKey.R) continue;
                break;
            }
        }

        private static void StartOnKeyPressedListener()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.I:
                    Console.WriteLine("calculate Ei x series using iterative method: \n>1. for x=0.5, \n>2. for x=1,5");
                    StartSecondListener();
                    break;
                case ConsoleKey.R:
                    Console.WriteLine("calculate Ei x series using recursive method: \n>1. for x=0.5, \n>2. for x=1,5");
                    StartSecondListener();
                    break;
                default:
                    StartOnKeyPressedListener();
                    break;
            }
        }

        private static void StartSecondListener()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    Calculate(true, 0);
                    break;
                case ConsoleKey.D2:
                    Calculate(false, 1);
                    break;
                default:
                    StartSecondListener();
                    break;
            }

            static void Calculate(bool isIterative, int refIndex)
            {
                double e;
                var timer = new Stopwatch();
                while (true)
                {
                    Console.Write("\nEnter error rate e: e=");
                    var parse = double.TryParse(Console.ReadLine(), out e);
                    if (!parse)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Input was not in a correct format");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else break;
                }

                timer.Start();
                if (isIterative) CalcIterative(e, refIndex);
                else CalcRecursive(e, refIndex);
                timer.Stop();
                var elapsed = timer.ElapsedMilliseconds;
                Console.WriteLine($"\nCalculated in {elapsed} milliseconds");
            }
        }

        private static void CalcIterative(double e, int reference)
        {
            var x = (reference == 0) ? 0.5 : 1.5;
            var result = Y + Math.Log(x);
            var i = 1;
            double currentError;
            do
            {
                result += Math.Pow(x, i) / (i * Factorial(i));
                currentError = CalcError(result, reference);

                Console.WriteLine("\nIteration {0} :", i);
                Console.WriteLine("Result = " + $"{result:F20}");
                Console.WriteLine("Current error rate = " + $"{currentError:F10}");
                Console.WriteLine("---------------------------------");

                i++;
            } while (currentError > e);
        }

        private static void CalcRecursive(double e, int reference)
        {
            var x = (reference == 0) ? 0.5 : 1.5;
            var n = x;
            var result = Y + Math.Log(x);
            double currentError;
            var i = 2;
            do
            {
                result += n;
                n = n * x * (i - 1) / (i * i);
                currentError = CalcError(result, reference);
                Console.WriteLine("\nItems calculated : " + (i - 1));
                Console.WriteLine("Result = " + $"{result:F20}");
                Console.WriteLine("Current error rate = " + $"{currentError:F10}");
                Console.WriteLine("---------------------------------");
                i++;
            } while (currentError > e);
        }

        private static double CalcError(double res, int refIndex)
        {
            return Math.Abs(ReferenceArray[refIndex] - res);
        }

        private static int Factorial(int n)
        {
            return (n == 1) ? 1 : n * Factorial(n - 1);
        }
    }
}