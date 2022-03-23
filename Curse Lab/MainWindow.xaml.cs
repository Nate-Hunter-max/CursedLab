using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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
            Header.MouseDown += (s, a) => DragMove(s, a);

            void DragMove(object sender, MouseButtonEventArgs e) 
            { 
                if (e.ChangedButton == MouseButton.Left) this.DragMove(); 
            }

            Cross.MouseDown += (s, a) => { Close(); };


            var orderX = 4;
            var orderY = 3;
            var permList = new List<int>();
            for (int i = 0; i < orderX * orderY; i++) permList.Add(i);
            var rectList = new List<string>();
            while (NarPermut.NextOrFalse(ref permList))
            {
                if (LatinRectangle.IsListValid(permList, orderX, orderY))
                {
                    var rectange = new LatinRectangle(orderX, orderY);
                    rectange.RectFromList(permList);
                    if (!rectange.IsNormalRow()) break;
                    if (rectange.IsNormalColumn() && !rectList.Contains(rectange.GetString()))
                    {
                        //TextBlock.Text += '\n' + rectange.AsPlainText(formated: false);
                        rectList.Add(rectange.GetString());
                    }
                }
            }
            //TextBlockAll.Text = $"Всего {rectList.Count}";
        }

    }
}