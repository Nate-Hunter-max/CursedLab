using System.Collections.Generic;

namespace Curse_Lab
{
    static class NarPermut
    {
        public static bool NextOrFalse(ref List<int> arr)
        {
            int j, l;
            for (j = arr.Count - 1; j > 0; j--) if (arr[j - 1] < arr[j]) break;
            if (j != 0)
            {
                j--;
                for (l = arr.Count - 1; l > j; l--) if (arr[l] > arr[j]) break;
                (arr[l], arr[j]) = (arr[j], arr[l]);
                arr.Reverse(j + 1, arr.Count - j - 1);
                return true;
            }
            else return false;
        }

        public static string ToString(List<int> arr)
        {
            var output = string.Empty;
            foreach (var item in arr)
            {
                output += item;
            }
            return output;
        }

    }
}
