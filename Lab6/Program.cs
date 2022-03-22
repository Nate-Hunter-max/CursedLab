namespace Lab6;

internal static class Program
{
    private static void Main()
    {
        var n = 10;
        double pitStopTime, softTime, mediumTime, hardTime, softWear, mediumWear, hardWear;
        pitStopTime = 3.1;
        softTime = 2.1;
        mediumTime = 3.5;
        hardTime = 5.5;
        softWear = 3.1;
        mediumWear = 2.5;
        hardWear = 1.1;
        
        double fullTime;

        for (var i = 0; i < n; i++)
        {
            fullTime += Loop(0, i)

        }
        
        double Loop(byte tiresType, byte wearRate)
        {
            var wear = 0.0;
            var normal = 0.0;
            switch (tiresType)
            {
                case 0:
                    wear = softWear;
                    normal = softTime;
                    break;
                case 1:
                    wear = mediumWear;
                    normal = mediumTime;
                    break;
                case 2:
                    wear = hardWear;
                    normal = hardTime;
                    break;
            }

            var time = wearRate * wear + normal;
            return time;
        }
    }
}