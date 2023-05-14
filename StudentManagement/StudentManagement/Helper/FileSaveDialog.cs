using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace StudentManagement.Helper {
    public static class FileSaveDialog {
        public static string ShowSaveDialog(string defaultFileName) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveFileDialog.DefaultExt = ".xlsx";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = defaultFileName;

            bool? result = saveFileDialog.ShowDialog();

            if(result.HasValue && result.Value) {
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}
