using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using StudentManagement.Helper;
using StudentManagement.Models;
using StudentManagement.Utils;

namespace StudentManagement.Control.Admin {
    /// <summary>
    /// Interaction logic for EnrollManagement.xaml
    /// </summary>
    public partial class EnrollManagement:UserControl {
        private ObservableCollection<Student> studensList = DataManager.GetStudentList();
        private ObservableCollection<Course> coursesList = new ObservableCollection<Course>();

        public EnrollManagement() {
            InitializeComponent();
        }

        private void dgStudent_Loaded(object sender,RoutedEventArgs e) {
            dgStudent.ItemsSource = studensList;
        }



        private void dgStudent_SelectionChanged(object sender,SelectionChangedEventArgs e) {
            Student student = (Student) dgStudent.SelectedItem;
            string maSinhVien = student.MaSV;

            string sql = $"select DangKi.MaMonHoc, MonHoc.TenMonHoc, MonHoc.SoTinChi, MonHoc.MaNganh\r\nFrom MonHoc\r\nJoin DangKi On DangKi.MaMonHoc = MonHoc.MaMonHoc\r\nwhere DangKi.MaSinhVien = '{maSinhVien}'";
            SqlDataReader reader = ExecuteQuery.executeReader(sql);
            coursesList.Clear();
            while(reader.Read()) {
                string maMonHoc = reader.GetString(0);
                string tenMonHoc = reader.GetString(1);
                int soTinChi = reader.GetInt32(2);
                string maNganh = reader.GetString(3);
                Course course = new Course(maMonHoc,tenMonHoc,soTinChi,maNganh);
                coursesList.Add(course);
            }

            dgEnroll.ItemsSource = coursesList;

        }

        private void ClickImg(object sender,MouseButtonEventArgs e) {
            if(dgEnroll.SelectedItem == null) {
                MessageBox.Show("Vui lòng môn học cần huỷ của sinh viên","Thông báo",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            
            Course selectedCourse = (Course) dgEnroll.SelectedItem;
            string maMonHoc = selectedCourse.MaMonHoc;

            // Hiển thị thông báo xác nhận
            MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn huỷ học phần {maMonHoc} không?","Xác nhận",MessageBoxButton.OKCancel);

            if(result == MessageBoxResult.OK) {
                // Nếu người dùng xác nhận, xóa bản ghi tương ứng
                string sql = $"delete from DangKi where MaMonHoc = '{maMonHoc}'";
                ExecuteQuery.executeNonQuery(sql);
                MessageBox.Show("Huỷ học phần thành công","Thông báo",MessageBoxButton.OK,MessageBoxImage.Information);
            }

           
        }

        private DataGrid virtualDataGrid() {
            DataGrid dataGrid = new DataGrid();

            // Tạo cột
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Mã sinh viên",Binding = new Binding("MaSinhVien") });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Tên sinh viên",Binding = new Binding("TenSinhVien") });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Khoa",Binding = new Binding("Khoa") });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Mã môn học",Binding = new Binding("MaMonHoc") });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Tên môn học",Binding = new Binding("TenMonHoc") });
            dataGrid.Columns.Add(new DataGridTextColumn(){Header = "Số tín chỉ", Binding = new Binding("MaMonHoc")});
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Mã ngành", Binding = new Binding("MaNganh")});

            List<Course> exportList = new List<Course>();
            string sql = $"select SinhVien.MaSinhVien, SinhVien.TenSinhVien, SinhVien.Khoa, DangKi.MaMonHoc, MonHoc.TenMonHoc, MonHoc.SoTinChi, MonHoc.MaNganh\r\nFrom MonHoc\r\nJoin DangKi On DangKi.MaMonHoc = MonHoc.MaMonHoc\r\nJoin SinhVien on SinhVien.MaSinhVien = DangKi.MaSinhVien";
            SqlDataReader dataReader =  ExecuteQuery.executeReader(sql);
            while (dataReader.Read()) { 
                string maSinhVien = dataReader.IsDBNull(dataReader.GetOrdinal("MaSinhVien")) ? "" : dataReader.GetString(dataReader.GetOrdinal("MaSinhVien"));
                string hoTen = dataReader.IsDBNull(dataReader.GetOrdinal("TenSinhVien")) ? "" : dataReader.GetString(dataReader.GetOrdinal("TenSinhVien"));
                string khoa = dataReader.IsDBNull(dataReader.GetOrdinal("Khoa")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Khoa"));
                string maMonHoc = dataReader.IsDBNull(dataReader.GetOrdinal("MaMonHoc")) ? "" : dataReader.GetString(dataReader.GetOrdinal("MaMonHoc"));
                string tenMonHoc = dataReader.IsDBNull(dataReader.GetOrdinal("TenMonHoc")) ? "" : dataReader.GetString(dataReader.GetOrdinal("TenMonHoc"));
                int soTinChi = dataReader.IsDBNull(dataReader.GetOrdinal("SoTinChi")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("SoTinChi"));
                string maNganh = dataReader.IsDBNull(dataReader.GetOrdinal("MaNganh")) ? "" : dataReader.GetString(dataReader.GetOrdinal("MaNganh"));
                exportList.Add(new Course(maSinhVien, hoTen, khoa, maMonHoc, tenMonHoc, soTinChi, maNganh));
            }

            dataGrid.ItemsSource = exportList;
            return dataGrid;
        }

        private void btnExport_Click(object sender,RoutedEventArgs e) {
            string defaultFileName = "exported_data";
            string fileName = FileSaveDialog.ShowSaveDialog(defaultFileName);

            

            if(!string.IsNullOrEmpty(fileName)) {
                ExcelExporter.ExportExcel(virtualDataGrid(),fileName);
                MessageBox.Show("Dữ liệu đã được xuất thành công!","Xuất dữ liệu",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }
    }
}
