using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curse_Lab
{
    internal class CsvWriter
    {
        StreamWriter csvWriter;
        public CsvWriter() {
            var fd = new SaveFileDialog { Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*" };
            fd.ShowDialog();
            csvWriter = new StreamWriter(new FileStream(fd.FileName, FileMode.OpenOrCreate));
        }
        public void Append(string key, string value) { csvWriter.WriteLine($"{key};{value}"); }
        public void Close() { csvWriter.Close(); }
    }
}
