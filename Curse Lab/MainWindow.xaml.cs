using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Cursed_Lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var currentIndex = -1;
            WinBg.CornerRadius = new CornerRadius(WindowState == WindowState.Maximized ? 0 : 10);
            var RecObjList = new List<LatinRectangle>();
            Cross.MouseDown += (s, a) => { Close(); };
            Header.MouseDown += (s, a) => { if (a.ChangedButton == MouseButton.Left) this.DragMove(); };
            StartButton.MouseDown += (s, a) => StartPressed(int.Parse(OrdX.Text), int.Parse(OrdY.Text), s, a);
            Fwd.MouseDown += (s, a) => { ChangeRect(RecObjList, true); };
            Bwd.MouseDown += (s, a) => { ChangeRect(RecObjList, false); };
            OrdX.KeyDown += (s, a) => { if (a.Key == Key.Escape && currentIndex != -1) CnvRect.Focus(); };
            OrdY.KeyDown += (s, a) => { if (a.Key == Key.Escape && currentIndex != -1) CnvRect.Focus(); };
            KeyDown += (s, a) =>
            {
                if (a.Key == Key.Left) ChangeRect(RecObjList, false);
                if (a.Key == Key.Right) ChangeRect(RecObjList, true);
                if (a.Key == Key.Enter) StartPressed(int.Parse(OrdX.Text), int.Parse(OrdY.Text), s);
                if (a.Key == Key.F) WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
                if (a.Key == Key.T) SaveAll(s);
            };

            void SaveAll(object s)
            {
                var outFile = new CsvWriter();
                outFile.Append("[N, M]", "R(n)");
                for (int i = 1; i < int.Parse(OrdX.Text) + 1; i++)
                {
                    for (int k = 1; k < int.Parse(OrdX.Text) + 1; k++)
                    {
                        StartPressed(i, k, s);
                        outFile.Append($"[{i}, {k}]", RecObjList.Count.ToString());
                    }
                }
                outFile.Close();
            }

            void UpdateInfo(bool isNormal, bool isDiagonal)
            {
                var colorTrue = FindResource("Accent") as SolidColorBrush;
                var colorFalse = FindResource("FgMain") as SolidColorBrush;
                var style = FindResource("LineStyle") as Style;
                DiagonalCnv.Children.Clear();
                NormalCnv.Children.Clear();

                if (isNormal) { Draw(NormalCnv, true); NormalTxt.Foreground = colorTrue; }
                else { Draw(NormalCnv, false); NormalTxt.Foreground = colorFalse; }

                if (isDiagonal) { Draw(DiagonalCnv, true); DiagonalTxt.Foreground = colorTrue; }
                else { Draw(DiagonalCnv, false); DiagonalTxt.Foreground = colorFalse; }

                void Draw(Canvas cnv, bool state)
                {
                    Diagonal.Visibility = Visibility.Visible;
                    Normal.Visibility = Visibility.Visible;
                    if (state)
                    {
                        cnv.Children.Add(new Line() { X1 = 0, Y1 = 5, X2 = 7, Y2 = 15, Style = style });
                        cnv.Children.Add(new Line() { X1 = 7, Y1 = 15, X2 = 15, Y2 = -5, Style = style });
                    }
                    else
                    {
                        for (int i = 0; i <= 15; i += 15)
                        {
                            cnv.Children.Add(new Line()
                            {
                                X1 = 0,
                                Y1 = 0 + i,
                                X2 = 15,
                                Y2 = 15 - i,
                                Style = style,
                                Stroke = colorFalse
                            });
                        }

                    }
                }

            }

            void UpdateCount(in List<LatinRectangle> lst, int index)
            {
                Index.Text = $"{index}/{lst.Count}";
                IndexBorder.Visibility = Visibility.Visible;
            }

            void ChangeRect(List<LatinRectangle> lst, bool isNext)
            {
                currentIndex = isNext ? currentIndex + 1 : currentIndex - 1;
                if (currentIndex < 0) currentIndex = lst.Count - 1;
                if (currentIndex == lst.Count) currentIndex = 0;
                var rect = lst[currentIndex];
                rect.PlaceToCanvas(CnvRect);
                UpdateInfo(rect.IsNormal(), rect.IsDiagonal());
                UpdateCount(lst, currentIndex + 1);
            }


            void StartPressed(int orderX, int orderY, object sender, MouseButtonEventArgs? e = null)
            {
                RecObjList.Clear();
                currentIndex = -1;
                int[,] temp = new int[orderY, orderX];
                for (int i = 0; i < orderY; i++) for (int k = 0; k < orderX; k++) temp[i, k] = int.MaxValue;
                FindAll(orderX, orderY, temp);
                if (orderX == 1 || orderY == 1) foreach (var it in RecObjList.ToList()) if (!it.IsNormal()) RecObjList.Remove(it);
                ChangeRect(RecObjList, true);
            }
            void FindAll(int ordX, int ordY, int[,] arr, int currX = 0, int currY = 0)
            {
                bool Validate(int cell)
                {
                    for (int k = 0; k < ordY - 1; k++) if (arr[k, currX] == cell) return false;
                    for (int m = 0; m < ordX - 1; m++) if (arr[currY, m] == cell) return false;
                    if (NrmCheck.IsChecked ??= false)
                    {
                        for (int k = 0; k < ordX - 1; k++) if (arr[0, k] > arr[0, k + 1]) return false;
                        for (int k = 0; k < ordY - 1; k++) if (arr[k, 0] > arr[k + 1, 0]) return false;
                    }
                    return true;
                }

                for (int i = 0; i < Math.Max(ordY, ordX); i++)
                {
                    if (Validate(i))
                    {

                        arr[currY, currX] = i;
                        currX++;
                        if (currX == ordX) { currX = 0; currY++; }
                        if (currY == ordY)
                        {

                            RecObjList.Add(new LatinRectangle(arr));
                            currX = ordX - 1;
                            currY = ordY - 1;
                            return;
                        }
                        FindAll(ordX, ordY, arr, currX, currY);
                        arr[currY, currX] = int.MaxValue;
                        currX--;
                        if (currX == -1) { currX = ordX - 1; currY--; }
                    }

                }
            }
        }

    }

}