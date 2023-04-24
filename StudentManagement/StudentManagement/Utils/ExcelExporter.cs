using System.Data;
using System.IO;
using System.Windows.Controls;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

public class ExcelExporter {
    public static void Export(DataGrid dataGrid,string fileName) {
        // Tạo tệp Excel mới
        SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(fileName,SpreadsheetDocumentType.Workbook);

        // Thêm một WorkbookPart vào tài liệu
        WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        // Thêm một WorksheetPart vào WorkbookPart
        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        worksheetPart.Worksheet = new Worksheet(new SheetData());

        // Thêm một Sheet vào Workbook
        Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
        Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),SheetId = 1,Name = "Sheet1" };
        sheets.Append(sheet);

        // Lấy DataTable chứa dữ liệu từ DataGrid
        var dataTable = ((DataView) dataGrid.ItemsSource).Table;

        // Thêm các hàng và cột vào SheetData
        SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
        for(int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++) {
            Row row = new Row();
            for(int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++) {
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(dataTable.Rows[rowIndex][columnIndex].ToString());
                row.Append(cell);
            }
            sheetData.Append(row);
        }

        // Lưu tệp Excel
        workbookPart.Workbook.Save();
        spreadsheetDocument.Close();
    }
}