using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Cursed_Lab
{
    public class LatinRectangle
    {
        public int OrderX { get; }
        public int OrderY { get; }
        public int OrderMax { get; }
        public int[,] LatinRect { get; }

        public LatinRectangle(int[,] arr)
        {
            OrderX = arr.GetLength(1);
            OrderY = arr.GetLength(0);
            OrderMax = OrderX >= OrderY ? OrderX : OrderY;
            LatinRect = new int[OrderY, OrderX];
            for (int i = 0; i < OrderY; i++) for (int k = 0; k < OrderX; k++) LatinRect[i, k] = arr[i, k];
        }


        public bool IsDiagonal()
        {
            var temp = new List<int>();
            var ordMin = OrderX > OrderY ? OrderY : OrderX;
            for (int i = 0; i < ordMin; i++) temp.Add(LatinRect[i, i]);
            if (!temp.SequenceEqual(temp.Distinct())) return false;
            temp.Clear();

            for (int i = 0; i < ordMin; i++) temp.Add(LatinRect[i, ordMin - i - 1]);
            if (!temp.SequenceEqual(temp.Distinct())) return false;
            temp.Clear();

            return true;
        }
        public string AsPlainText()
        {
            var mess = string.Empty;
            for (int i = 0; i < OrderY; i++)
            {
                for (int k = 0; k < OrderX; k++)
                {
                    mess += $"{LatinRect[i, k]} ";
                }
                mess += '\n';
            }
            return mess;
        }
        public bool IsNormal()
        {
            for (int i = 0; i < OrderX - 1; i++) if (LatinRect[0, i] > LatinRect[0, i + 1]) return false;
            for (int i = 0; i < OrderY - 1; i++) if (LatinRect[i, 0] > LatinRect[i + 1, 0]) return false;
            return true;
        }

        public void PlaceToCanvas(Canvas CnvRect)
        {
            var ordMax = Math.Max(OrderX, OrderY);
            var scale = OrderX > OrderY ? 400 / OrderX : 400 / OrderY;
            var width = OrderX * scale;
            var height = OrderY * scale;
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

            AddLines(OrderX, true);
            AddLines(OrderY, false);
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
                for (int i = 0; i < OrderX; i++)
                {
                    for (int k = 0; k < OrderY; k++)
                    {
                        CnvRect.Children.Add(new TextBlock()
                        {
                            RenderTransform = new TranslateTransform(
                                -width / 2 + (width / (OrderX * 2) - 40 / ordMax) + i * (width / OrderX),
                                -height / 2 + (height / (OrderY * 2) - 80 / ordMax) + k * (height / OrderY)),
                            Text = LatinRect[k, i].ToString(),
                            Foreground = new SolidColorBrush(colors[LatinRect[k, i]]),
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
