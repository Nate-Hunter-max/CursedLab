using System.Windows;

namespace Database
{
    public partial class MainWindow : Window
    {
        struct Rec
        {
            public string brand;
            public byte buttons;
            public bool scroll;
        }
        public MainWindow()
        {
            InitializeComponent();
            var m = new Rec() { brand = "Mouse", buttons = 3, scroll = false };
        }
        void AddRec(Rec r)
        {

        }
    }
}