using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Curse_Lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var clickTimes = 0;

            Cross.MouseDown += (s, a) => { Close(); };
            Header.MouseDown += (s, a) => DragMove(s, a);
            StartButton.MouseDown += (s, a) => StartSearch(s, a);


            void StartSearch(object sender, MouseButtonEventArgs e)
            {
                clickTimes++;
                var orderX = int.Parse(OrdX.Text);
                var orderY = int.Parse(OrdY.Text);
                var permList = new List<int>();
                for (int i = 0; i < orderX * orderY; i++) permList.Add(i);
                var rectList = new List<string>();
                var rectangList = new List<LatinRectangle>();
                while (NarPermut.NextOrFalse(ref permList))
                {
                    if (LatinRectangle.IsListValid(permList, orderX, orderY))
                    {
                        var rectangle = new LatinRectangle(orderX, orderY);
                        rectangle.RectFromList(permList);
                        if (!rectangle.IsNormalRow()) break;
                        if (rectangle.IsNormalColumn() && !rectList.Contains(rectangle.GetString()))
                        {
                            rectList.Add(rectangle.GetString());
                            rectangle.PlaceToCanvas(CnvRect);
                            break;
                        }
                    }
                }
            }


            var foo = FindResource("Accent") as SolidColorBrush;
            void DragMove(object sender, MouseButtonEventArgs e)
            {
                if (e.ChangedButton == MouseButton.Left) this.DragMove();
            }
        }

    }

}