using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using StudentManagement.Control.Student;
using StudentManagement.Helper;
using StudentManagement.Models;
using StudentManagement.Utils;

namespace StudentManagement {
    public partial class StudentRole:Window {
        private string username;
        string maSinhVien = "";

        private ObservableCollection<Grade> grades = DataManager.GetGradeList();
        private ObservableCollection<Student> students = DataManager.GetStudentList();



        public StudentRole(string username) {
            InitializeComponent();
            this.username = username;
            loadInfo();
        }

        private void loadInfo() {
          

            string sql = $"select MaSinhVien from StudentAccount where username = '{username}'";
            SqlDataReader reader = ExecuteQuery.executeReader(sql);
            if (reader.Read()) {
                maSinhVien = reader.GetString(0);
            }

            Student student = students.FirstOrDefault(s => s.MaSV.ToLower().Equals(maSinhVien.ToLower()));

            if (student != null) {

                tbHoTen.Text = student.HoTen;
                tbMaSV.Text = student.MaSV;
                tbEmail.Text = student.Email;
                tbSdt.Text = student.SoDT;
                tbGioiTinh.Text = student.GioiTinh;
                tbKhoa.Text = student.Khoa;
                tbMaNganh.Text = student.MaNganh;
                tbNgaySinh.Text = student.NgaySinh.ToString("yyyy-MM-dd");
            }
            else {
                MessageBox.Show("Không tìm thấy thông tin sinh viên");
            }

            //string sql2 = $"select * from SinhVien where MaSinhVien = '{maSinhVien}'";
            //SqlDataReader reader2 = ExecuteQuery.executeReader(sql2);

            //while (reader2.Read()) {
            //    tbHoTen.Text = reader2.GetString(reader2.GetOrdinal("TenSinhVien"));
            //    tbMaSV.Text = reader2.GetString(reader2.GetOrdinal("MaSinhVien"));
            //    tbEmail.Text = reader2.GetString(reader2.GetOrdinal("Email"));
            //    tbSdt.Text = reader2.GetString(reader2.GetOrdinal("SoDienThoai"));
            //    tbGioiTinh.Text = reader2.GetString(reader2.GetOrdinal("GioiTinh"));
            //    tbKhoa.Text = reader2.GetString(reader2.GetOrdinal("Khoa"));
            //    tbMaNganh.Text = reader2.GetString(reader2.GetOrdinal("MaNganh"));
            //    tbNgaySinh.Text = reader2.GetDateTime(reader2.GetOrdinal("NgaySinh")).ToString("yy-MM-dd");
            //}

        }

        private void dgGrade_Loaded(object sender,RoutedEventArgs e) {
            ObservableCollection<Grade> personalGrades = new ObservableCollection<Grade>(grades.Where(grade => grade.MaSinhVien.Equals(tbMaSV.Text)).ToList());
            dgGrade.ItemsSource = personalGrades;
        }

        
        private void btnUpdateInfo_Click(object sender,RoutedEventArgs e) {
            InputInfo inputInfo = new InputInfo(maSinhVien);
            inputInfo.ShowDialog();
        }

        private void btnExport_Click(object sender,RoutedEventArgs e) {
            string defaultFileName = "exported_data";

            string fileName = FileSaveDialog.ShowSaveDialog(defaultFileName);

            if(!string.IsNullOrEmpty(fileName)) {
                ExcelExporter.ExportExcel(dgGrade,fileName);
                MessageBox.Show("Dữ liệu đã được xuất thành công!","Xuất dữ liệu",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }
    }
}
