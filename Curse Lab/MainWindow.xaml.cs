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

        public class FuckingSquare
        {
            readonly int _order;

            int[,] square;
            char[,] formatedSquare;
            char[] customMap;

            public FuckingSquare(int n)
            {
                _order = n;
                square = new int[n, n];
                formatedSquare = new char[n, n];
                customMap = new char[n];
                for (int i = 0; i < n; i++) customMap[i] = (char)(i + 1);
            }
            public FuckingSquare(int n, char[] customMap) : this(n)
            {
                this.customMap = customMap;
            }

            public void FuckFromString(string str)
            {
                str = Regex.Replace(str, @"\D+", "");
                var list = new List<int>();
                foreach (char c in str) list.Add(int.Parse(c.ToString()));
                FuckFromList(list);
            }

            public void FuckFromList(in List<int> lst)
            {
                int index = 0;
                for (int i = 0; i < _order; i++)
                {
                    for (int k = 0; k < _order; k++)
                    {
                        square[i, k] = lst[index] % _order;
                        index++;
                    }
                }
                ChangeSquare(this.customMap, isFormat: true);
            }

            public int[,] FuckAs2DArray() => square;

            public bool IsFuckable()
            {
                var temp = new List<int>();
                for (int i = 0; i < _order; i++)
                {
                    for (int k = 0; k < _order; k++) temp.Add(square[i, k]);
                    if (!temp.SequenceEqual(temp.Distinct())) return false;
                    temp.Clear();
                }

                for (int i = 0; i < _order; i++)
                {
                    for (int k = 0; k < _order; k++) temp.Add(square[k, i]);
                    if (!temp.SequenceEqual(temp.Distinct())) return false;
                    temp.Clear();
                }

                return true;
            }

            public bool IsDiaFuckable()
            {
                var temp = new List<int>();
                for (int i = 0; i < _order; i++) temp.Add(square[i, i]);
                if (!temp.SequenceEqual(temp.Distinct())) return false;
                temp.Clear();

                for (int i = 0; i < _order; i++) temp.Add(square[i, _order - i - 1]);
                if (!temp.SequenceEqual(temp.Distinct())) return false;
                temp.Clear();

                return true;
            }
            public string UnfuckAsPlainText(bool formated = false)
            {
                var mess = string.Empty;
                for (int i = 0; i < _order; i++)
                {
                    for (int k = 0; k < _order; k++)
                    {
                        mess += formated ? $"{formatedSquare[i, k]} " : $"{square[i, k]} ";
                    }
                    mess += '\n';
                }
                return mess;
            }

            public void ReFuck(char[] map)
            {
                customMap = map;
                ChangeSquare(map, isFormat: true);
            }

            private void ChangeSquare(char[] map, bool isFormat = false)
            {
                for (int i = 0; i < _order; i++)
                {
                    for (int k = 0; k < _order; k++)
                    {
                        if (isFormat) formatedSquare[i, k] = map[square[i, k]];
                        else square[i, k] = map[square[i, k]];
                    }
                }
            }
            public void DoNormalFucking()
            {
                var normMap = new char[_order];
                for (int i = 0; i < _order; i++) normMap[square[0, i]] = (char)i;
                ChangeSquare(normMap);
                ChangeSquare(customMap, isFormat: true);
            }
            public void SwapFuckers(bool isColumn, int first, int second)
            {
                if (isColumn)
                {
                    for (int i = 0; i < _order; i++)
                    {
                        (square[i, first], square[i, second]) = (square[i, second], square[i, first]);
                    }
                }
                else
                {
                    for (int i = 0; i < _order; i++)
                    {
                        (square[first, i], square[second, i]) = (square[second, i], square[first, i]);
                    }
                }
                ChangeSquare(customMap, isFormat: true);
            }
            public static bool IsFuckableList(in List<int> lst, int order)
            {
                var temp = new List<int>();
                for (int k = 0; k < order; k++)
                {
                    for (int i = k; i < lst.Count; i += order) temp.Add(lst[i] % order);
                    if (IsContainsSame(temp)) return false;
                    temp.Clear();
                }
                for (int k = 0; k < lst.Count; k += order)
                {
                    for (int i = k; i < k + order; i++) temp.Add(lst[i] % order);
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
            public string UnfuckString()
            {
                var str = string.Empty;
                foreach (var item in square) str += item;
                return str;
            }
        }


        public MainWindow()
        {
            InitializeComponent();

            var fuck = new FuckingSquare(3, new char[] { '1', '2', '3' });
            fuck.FuckFromString("102.021.210");
            Fucker.Text = fuck.UnfuckAsPlainText(formated: false);
            fuck.ReFuck(new char[] { 'ḁ', 'ḅ', 'ḉ' });
            Fucker.Text += '\n' + fuck.UnfuckAsPlainText(formated: true);
            int[,] fknArray;
            if (fuck.IsFuckable()) fknArray = fuck.FuckAs2DArray();
            fuck.DoNormalFucking();
            Fucker.Text += '\n' + fuck.UnfuckAsPlainText(formated: true);
            Fucker.Text += fuck.IsFuckable() ? "\nFS true" : "\nFS false";
            Fucker.Text += fuck.IsDiaFuckable() ? "    DFS true\n" : "   DFS false\n";
            fuck.SwapFuckers(isColumn: true, 0, 1);
            Fucker.Text += '\n' + fuck.UnfuckAsPlainText(formated: true);

            var order = 3;
            var perm = new List<int>();
            for (int i = 0; i < order * order; i++) perm.Add(i);
            var squares = new List<string>();
            while (NarPermut.NextOrFalse(ref perm))
            {
                if (FuckingSquare.IsFuckableList(perm, order))
                {
                    var fuckNew = new FuckingSquare(order);
                    fuckNew.FuckFromList(perm);
                    if (!squares.Contains(fuckNew.UnfuckString()))
                    {
                        Fucker.Text += '\n' + fuckNew.UnfuckAsPlainText(formated: false);
                        squares.Add(fuckNew.UnfuckString());
                        MessageBox.Show(fuckNew.UnfuckAsPlainText());
                    }
                }
            }
            Fucker.Text += $"всего:{squares.Count}";
        }

    }
}