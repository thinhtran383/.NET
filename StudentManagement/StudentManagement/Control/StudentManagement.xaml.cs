using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StudentManagement.Helper;
using StudentManagement.Models;

namespace StudentManagement.Control {

    public partial class StudentManagement:UserControl {
        private ObservableCollection<Student> studentList = DataManager.getStudentList();
        public StudentManagement() {
            InitializeComponent();
        }

        private void dgStudent_Loaded(object sender,RoutedEventArgs e) {

            DataTable dt = new DataTable();

            dt.Columns.Add("Mã sinh viên");
            dt.Columns.Add("Mã ngành");
            dt.Columns.Add("Họ tên");
            dt.Columns.Add("Ngày sinh");
            dt.Columns.Add("Giới tính");
            dt.Columns.Add("Số điện thoại");
            dt.Columns.Add("Email");
            dt.Columns.Add("Khoa");

            
          

            foreach(var student in studentList) {
                DataRow row = dt.NewRow();
                row["Mã sinh viên"] = student.getMaSV();
                row["Mã ngành"] = student.getMaNganh();
                row["Họ tên"] = student.getHoTen();
                row["Ngày sinh"] = student.getNgaySinh();
                row["Giới tính"] = student.getGioiTinh();
                row["Số điện thoại"] = student.getSoDT();
                row["Email"] = student.getEmail();
                row["Khoa"] = student.getKhoa();
                dt.Rows.Add(row);
            }

            dgStudent.ItemsSource = dt.DefaultView;

        }

        private void dgStudent_SelectionChanged(object sender,SelectionChangedEventArgs e) {
            
        }

        private void Button_Click(object sender,RoutedEventArgs e) {
            ExcelExporter.Export(dgStudent,"test.xlsx");
        }
    }
}
