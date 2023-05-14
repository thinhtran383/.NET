using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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

    }
}
