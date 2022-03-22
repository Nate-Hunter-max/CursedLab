using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;


namespace Lab13
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Choose_folder.Click += ChooseClick;
        }

        void ChooseClick(object sender, RoutedEventArgs e)
        {
            var path = ChoosePath();
            if (path != string.Empty) DeleteFiles(path);
        }

        void DeleteFiles(string startPath)
        {
            OutputView.Clear();
            var options = new EnumerationOptions { RecurseSubdirectories = true };
            var files = new DirectoryInfo(startPath).EnumerateFiles("*.exe", options);
            foreach (var file in files)
            {
                if (Regex.Replace(file.Name, @".exe", "") != file.Directory?.Name) continue;
                //file.Delete();
                OutputView.Text += $"файл {file.Name} в папке {file.Directory.Name} удален\n\n";
            }
            if (OutputView.Text == string.Empty) OutputView.Text = "Удаление не требуется";
        }

        private static string ChoosePath()
        {
            var fd = new CommonOpenFileDialog { IsFolderPicker = true, InitialDirectory = "c:\\programfiles" };
            if (fd.ShowDialog() == CommonFileDialogResult.Ok) return fd.FileName; else return string.Empty;
        }
    }
}