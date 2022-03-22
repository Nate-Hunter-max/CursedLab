using System.Collections.Generic;
using System.Windows;

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
                        TextBlock.Text += '\n' + rectange.AsPlainText(formated: false);
                        rectList.Add(rectange.GetString());
                        MessageBox.Show(rectange.AsPlainText());
                    }
                }
            }
            MessageBox.Show($"Всего {rectList.Count}");
        }

    }
}