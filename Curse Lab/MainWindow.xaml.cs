using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Curse_Lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        public class LatinRectangle
        {
            readonly int orderX;
            readonly int orderY;
            readonly int orderMax;


            int[,] latinRect;
            char[,] formatedRect;
            char[] customMap;

            public LatinRectangle(int orderX, int orderY)
            {
                this.orderX = orderX;
                this.orderY = orderY;
                latinRect = new int[orderY, orderX];
                formatedRect = new char[orderY, orderX];
                orderMax = orderX >= orderY ? orderX : orderY;
                customMap = new char[orderMax];
                for (int i = 0; i < orderMax; i++) customMap[i] = (char)(i + 1);
            }
            public LatinRectangle(int orderX, int orderY, char[] customMap) : this(orderX, orderY)
            {
                this.customMap = customMap;
            }
            public void RectFromString(string str)
            {
                str = Regex.Replace(str, @"\D+", "");
                var list = new List<int>();
                foreach (char c in str) list.Add(int.Parse(c.ToString()));
                RectFromList(list);
            }

            public void RectFromList(in List<int> lst)
            {
                int index = 0;
                for (int i = 0; i < orderY; i++)
                {
                    for (int k = 0; k < orderX; k++)
                    {
                        latinRect[i, k] = lst[index] % orderMax;
                        index++;
                    }
                }
                ChangeRect(this.customMap, isFormat: true);
            }

            public int[,] As2DArray() => latinRect;

            public bool IsValid()
            {
                var temp = new List<int>();
                for (int i = 0; i < orderY; i++)
                {
                    for (int k = 0; k < orderX; k++) temp.Add(latinRect[i, k]);
                    if (!temp.SequenceEqual(temp.Distinct())) return false;
                    temp.Clear();
                }

                for (int i = 0; i < orderX; i++)
                {
                    for (int k = 0; k < orderY; k++) temp.Add(latinRect[k, i]);
                    if (!temp.SequenceEqual(temp.Distinct())) return false;
                    temp.Clear();
                }

                return true;
            }

            public bool IsDiaValid()
            {
                var temp = new List<int>();
                var ordMin = orderX > orderY ? orderY : orderX;
                for (int i = 0; i < ordMin; i++) temp.Add(latinRect[i, i]);
                if (!temp.SequenceEqual(temp.Distinct())) return false;
                temp.Clear();

                for (int i = 0; i < ordMin; i++) temp.Add(latinRect[i, ordMin - i - 1]);
                if (!temp.SequenceEqual(temp.Distinct())) return false;
                temp.Clear();

                return true;
            }
            public string AsPlainText(bool formated = false)
            {
                var mess = string.Empty;
                for (int i = 0; i < orderY; i++)
                {
                    for (int k = 0; k < orderX; k++)
                    {
                        mess += formated ? $"{formatedRect[i, k]} " : $"{latinRect[i, k]} ";
                    }
                    mess += '\n';
                }
                return mess;
            }

            public void Format(char[] map)
            {
                customMap = map;
                ChangeRect(map, isFormat: true);
            }

            private void ChangeRect(char[] map, bool isFormat = false)
            {
                for (int i = 0; i < orderY; i++)
                {
                    for (int k = 0; k < orderX; k++)
                    {
                        if (isFormat) formatedRect[i, k] = map[latinRect[i, k]];
                        else latinRect[i, k] = map[latinRect[i, k]];
                    }
                }
            }
            public void Normalize()
            {
                var normMap = new char[orderMax];
                for (int i = 0; i < orderX; i++) normMap[latinRect[0, i]] = (char)i;
                ChangeRect(normMap);
                ChangeRect(customMap, isFormat: true);
            }
            public void Swap(bool isColumn, int first, int second)
            {
                if (isColumn)
                {
                    for (int i = 0; i < orderY; i++)
                    {
                        (latinRect[i, first], latinRect[i, second]) = (latinRect[i, second], latinRect[i, first]);
                    }
                }
                else
                {
                    for (int i = 0; i < orderX; i++)
                    {
                        (latinRect[first, i], latinRect[second, i]) = (latinRect[second, i], latinRect[first, i]);
                    }
                }
                ChangeRect(customMap, isFormat: true);
            }
            public static bool IsListValid(in List<int> lst, in int orderX, in int orderY)
            {
                var temp = new List<int>();
                var orderMax = orderY > orderX ? orderY : orderX;
                for (int k = 0; k < orderY; k++)
                {
                    for (int i = k; i < lst.Count; i += orderX) temp.Add(lst[i] % orderMax);
                    if (IsContainsSame(temp)) return false;
                    temp.Clear();
                }
                for (int k = 0; k < lst.Count; k += orderX)
                {
                    for (int i = k; i < k + orderX; i++) temp.Add(lst[i] % orderMax);
                    if (IsContainsSame(temp)) return false;
                    temp.Clear();
                }
                return true;
            }

            private static bool IsContainsSame(List<int> lst)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    for (int k = i+1; k < lst.Count; k++) 
                        if (lst[k] == lst[i]) return true;
                }
                return false;
            }
            public string GetString()
            {
                var str = string.Empty;
                foreach (var item in latinRect) str += item;
                return str;
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            var rect = new LatinRectangle(3, 2,new char[] {'a','b','c'});
            rect.RectFromString("012,120");
            rect.Swap(isColumn: false, 0, 1);
            rect.Normalize();
            var mess = $"{LatinRectangle.IsListValid(new List<int> { 0, 1, 2, 1, 2, 1 }, 3, 2)}";
            MessageBox.Show(rect.AsPlainText(formated:true)+$"\n{rect.IsValid()}\n{rect.IsDiaValid()}\n{mess}");
            //var order = 3;
            //var perm = new List<int>();
            //for (int i = 0; i < order * order; i++) perm.Add(i);
            //var squares = new List<string>();
            //while (NarPermut.NextOrFalse(ref perm))
            //{
            //    if (LatinRectangle.IsFuckableList(perm, order))
            //    {
            //        var fuckNew = new LatinRectangle(order,order);
            //        fuckNew.RectFromList(perm);
            //        if (!squares.Contains(fuckNew.UnfuckString()))
            //        {
            //            Fucker.Text += '\n' + fuckNew.UnfuckAsPlainText(formated: false);
            //            squares.Add(fuckNew.UnfuckString());
            //            MessageBox.Show(fuckNew.UnfuckAsPlainText());
            //        }
            //    }
            //}
            //Fucker.Text += $"всего:{squares.Count}";
        }

    }
}