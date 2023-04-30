using System.Collections.Generic;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using StudentManagement.Models;
using System.Linq;

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
        var items = dataGrid.ItemsSource.Cast<Student>().ToList();
        var dataTable = ToDataTable(items);


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

    public static DataTable ToDataTable<T>(List<T> items) { // xu ly du lieu
        DataTable dataTable = new DataTable(typeof(T).Name);

        // Lấy tất cả các thuộc tính public của kiểu T
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Thêm các cột vào DataTable
        foreach(PropertyInfo property in properties) {
            dataTable.Columns.Add(property.Name,Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
        }

        // Thêm các hàng vào DataTable
        foreach(T item in items) {
            DataRow row = dataTable.NewRow();
            foreach(PropertyInfo property in properties) {
                row[property.Name] = property.GetValue(item) ?? DBNull.Value;
            }
            dataTable.Rows.Add(row);
        }

        return dataTable;
    }
}