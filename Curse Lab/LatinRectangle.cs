using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Curse_Lab
{
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
            if (map.Length != orderMax) throw new ArgumentException("Invalid map");
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
        public bool IsNormalRow()
        {
            for (int i = 0; i < orderX - 1; i++) if (latinRect[0, i] > latinRect[0, i + 1]) return false;
            return true;
        }
        public bool IsNormalColumn()
        {
            for (int i = 0; i < orderY - 1; i++) if (latinRect[i, 0] > latinRect[i + 1, 0]) return false;
            return true;
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
            for (int k = 0; k < orderX; k++)
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
                for (int k = i + 1; k < lst.Count; k++)
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

        public void PlaceToCanvas(Canvas CnvRect)
        {
            var ordMax = Math.Max(orderX, orderY);
            var scale = orderX > orderY ? 400 / orderX : 400 / orderY;
            var width = orderX * scale;
            var height = orderY * scale;
            var rectStyle = Application.Current.FindResource("RectStyle") as Style;
            var rectLineStyle = Application.Current.FindResource("LineRectStyle") as Style;
            var txtStyle = Application.Current.FindResource("TxtParams") as Style;
            var lineThickness = scale / 50 > 0 ? scale / 50 : 1;

            CnvRect.Children.Clear();
            CnvRect.Children.Add(new Rectangle()
            {
                Width = width,
                StrokeThickness = lineThickness,
                Height = height,
                RenderTransform = new TranslateTransform(-width / 2, -height / 2),
                Style = rectStyle
            });

            AddLines(orderX, true);
            AddLines(orderY, false);
            AddText();

            void AddLines(int ord, bool isVert)
            {
                for (int i = 1; i < ord; i++)
                    CnvRect.Children.Add(new Line()
                    {
                        RenderTransform = new TranslateTransform(-width / 2, -height / 2),
                        Style = rectLineStyle,
                        StrokeThickness = lineThickness,
                        X1 = isVert ? i * scale : 10,
                        X2 = isVert ? i * scale : width - 10,
                        Y1 = !isVert ? i * scale : 10,
                        Y2 = !isVert ? i * scale : height - 10,
                    });
            }
            void AddText()
            {
                var colors = new List<Color>();
                var brush = Application.Current.FindResource("Accent") as SolidColorBrush ?? new SolidColorBrush();
                var range = 60;
                for (int i = -range; i < range; i += range * 2 / ordMax)
                {
                    colors.Add(new Color()
                    {
                        A = 255,
                        R = brush.Color.R,
                        G = (byte)(brush.Color.G + i),
                        B = brush.Color.B
                    }); ;
                }
                for (int i = 0; i < orderX; i++)
                {
                    for (int k = 0; k < orderY; k++)
                    {
                        CnvRect.Children.Add(new TextBlock()
                        {
                            RenderTransform = new TranslateTransform(
                                -width / 2 + (width / (orderX * 2) - 40 / ordMax) + i * (width / orderX),
                                -height / 2 + (height / (orderY * 2) - 80 / ordMax) + k * (height / orderY)),

                            Text = latinRect[k, i].ToString(),
                            Foreground = new SolidColorBrush(colors[latinRect[k, i]]),
                            FontFamily = new FontFamily("Calibri"),
                            FontSize = 160 / ordMax,
                            Style = txtStyle,
                        });
                    }
                }
            }
        }
    }
}
