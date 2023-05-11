using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel;
using System.Windows.Controls;

public class ExcelExporter {
    public static void ExportExcel(DataGrid dataGrid,string fileName) {
        using(SpreadsheetDocument document = SpreadsheetDocument.Create(fileName,SpreadsheetDocumentType.Workbook)) {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
            Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart),SheetId = 1,Name = "Sheet1" };
            sheets.Append(sheet);

            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Tạo tiêu đề
            Row headerRow = new Row();
            foreach(DataGridColumn column in dataGrid.Columns) {
                headerRow.AppendChild(new Cell() { CellValue = new CellValue(column.Header.ToString()),DataType = CellValues.String });
            }
            sheetData.AppendChild(headerRow);

            // Thêm dữ liệu vào Sheet
            foreach(var item in dataGrid.ItemsSource) {
                Row newRow = new Row();
                foreach(DataGridColumn column in dataGrid.Columns) {
                    var propertyInfo = TypeDescriptor.GetProperties(item)[column.SortMemberPath];
                    var cellValue = propertyInfo.GetValue(item);
                    newRow.AppendChild(new Cell() { CellValue = new CellValue(cellValue?.ToString() ?? ""),DataType = CellValues.String });
                }
                sheetData.AppendChild(newRow);
            }

            workbookPart.Workbook.Save();
        }
    }
}
