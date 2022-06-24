using System;
using System.Collections.Generic;

namespace Cursed_Lab
{
    public class SquaresGen
    {
        public List<int[,]> Results { get; private set; } = new List<int[,]>();
        public void FindAllDlr(int ord)
        {
            int[,] temp = new int[ord, ord];
            for (int i = 0; i < ord; i++) for (int k = 0; k < ord; k++) temp[i, k] = int.MaxValue;
            Brutforce(ord, temp);
        }
        private void Brutforce(int ord, int[,] arr, int currX = 0, int currY = 0)
        {
            var lim = ord - 1;
            bool Validate(int cell)
            {
                for (byte k = 0; k < lim; k++) if (arr[k, currX] == cell) return false;
                for (byte m = 0; m < lim; m++) if (arr[currY, m] == cell) return false;
                return true;
            }
            bool DiaValidate(int i)
            {
                if (currX == currY) { for (byte k = 0; k < lim; k++) if (arr[k, k] == i) return false; };
                if (currY == lim - currX) { for (byte k = 0; k < lim; k++) if (arr[k, lim - k] == i) return false; }
                return true;
            }
            for (byte i = 0; i < Math.Max(ord, ord); i++)
            {
                if (Validate(i) && DiaValidate(i))
                {

                    arr[currY, currX] = i;
                    currX++;
                    if (currX == ord) { currX = 0; currY++; }
                    if (currY == ord)
                    {

                        Results.Add(Clone(arr));
                        return;
                    }
                    Brutforce(ord, arr, currX, currY);
                    arr[currY, currX] = int.MaxValue;
                    currX--;
                    if (currX == -1) { currX = ord - 1; currY--; }
                }

            }
        }
        private static int[,] Clone(int[,] arr)
        {
            var ord = arr.GetLength(0);
            var newArr = new int[ord, ord];
            for (int i = 0; i < ord; i++) { for (int k = 0; k < ord; k++) newArr[i, k] = arr[i, k]; };
            return newArr;
        }
    }
}
