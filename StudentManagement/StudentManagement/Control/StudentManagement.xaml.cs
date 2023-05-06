using System;

using System.Collections.ObjectModel;

using System.Data.SqlClient;

using System.Windows;
using System.Windows.Controls;

using StudentManagement.Helper;
using StudentManagement.Models;
using StudentManagement.Utils;

namespace StudentManagement.Control {

    public partial class StudentManagement:UserControl {
        private ObservableCollection<Student> studentList = DataManager.GetStudentList();

        public StudentManagement() {
            InitializeComponent();
            initNganh();
        }

        private void dgStudent_Loaded(object sender,RoutedEventArgs e) {
            dgStudent.ItemsSource = studentList;
        }

      

        private void clear() {
            txtMaSinhVien.Text = "";
            cbNganh.Text = "";
            txtTenSinhVien.Text = "";
            txtDienThoai.Text = "";
            txtEmail.Text = "";
            txtKhoa.Text = "";
            cbGioiTinh.Text = "";
            pickNgaySinh.Text = "";
            dgStudent.SelectedIndex = -1;

        }

        private void initNganh() {
            SqlDataReader dataReader = ExecuteQuery.executeReader("select MaNganh from Nganh");
            while(dataReader.Read()) {
                cbNganh.Items.Add(dataReader["MaNganh"].ToString());
            }
        }

        private void btnAdd_Click(object sender,RoutedEventArgs e) {
            string maSinhVien = txtMaSinhVien.Text;
            string tenSinhVien = txtTenSinhVien.Text;
            DateTime ngaySinh = DateTime.Parse(pickNgaySinh.Text);
            string gioiTinh = cbGioiTinh.Text;
            string dienThoai = txtDienThoai.Text;
            string email = txtEmail.Text;
            string khoa = txtKhoa.Text;
            string maNganh = cbNganh.Text;

            string sqlAdd = $"insert into SinhVien values ('{maSinhVien}', '{maNganh}', '{tenSinhVien}', '{ngaySinh}', '{gioiTinh}', '{dienThoai}', '{email}', '{khoa}' )";

            ExecuteQuery.executeNonQuery(sqlAdd);
            studentList.Add(new Student(maSinhVien,maNganh,tenSinhVien,ngaySinh,gioiTinh,dienThoai,email,khoa));
            dgStudent.Items.Refresh();

        }


        private void btnUpdate_Click(object sender,RoutedEventArgs e) {
            string maSinhVien = txtMaSinhVien.Text;
            string tenSinhVien = txtTenSinhVien.Text;
            DateTime ngaySinh = DateTime.Parse(pickNgaySinh.Text);
            string gioiTinh = cbGioiTinh.Text;
            string dienThoai = txtDienThoai.Text;
            string email = txtEmail.Text;
            string khoa = txtKhoa.Text;
            string maNganh = cbNganh.Text;

            string sqlUpdate = $"Update SinhVien Set MaSinhVien = '{maSinhVien}', MaNganh = '{maNganh}', TenSinhVien = '{tenSinhVien}', NgaySinh = '{ngaySinh}', GioiTinh = '{gioiTinh}', SoDienThoai = '{dienThoai}', Email = '{email}', Khoa = '{khoa}' where MaSinhVien = '{maSinhVien}'";
            ExecuteQuery.executeNonQuery(sqlUpdate);

            Student student = (Student) dgStudent.SelectedItem;
            student.MaSV = maSinhVien;
            student.MaNganh = maNganh;
            student.HoTen = tenSinhVien;
            student.NgaySinh = ngaySinh;
            student.GioiTinh = gioiTinh;
            student.SoDT = dienThoai;
            student.Email = email;
            student.Khoa = khoa;

            dgStudent.Items.Refresh();

        }

        private void btnDelete_Click(object sender,RoutedEventArgs e) {
            if(MessageBox.Show("Bạn có chắc muốn xoá?","Xác nhận xoá",MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                string selectedId = ((Student) dgStudent.SelectedItem).MaSV;
                string sqlDelete = "delete from SinhVien where MaSinhVien = '" + selectedId + "'";
                ExecuteQuery.executeNonQuery(sqlDelete);
                studentList.Remove((Student) dgStudent.SelectedItem);
                dgStudent.Items.Refresh();
            }
        }

        private void btnClear_Click(object sender,RoutedEventArgs e) {
            clear();
        }

        private void btnExport_Click(object sender,RoutedEventArgs e) {
            ExcelExporter.Export(dgStudent,"studentList.xlsx");
        }

        private void dgStudent_SelectionChanged(object sender,SelectionChangedEventArgs e) {
            if(dgStudent.SelectedIndex != -1) {
                Student student = (Student) dgStudent.SelectedItem;
                txtMaSinhVien.Text = student.MaSV;
                cbNganh.Text = student.MaNganh;
                txtTenSinhVien.Text = student.HoTen;
                txtDienThoai.Text = student.SoDT;
                txtEmail.Text = student.Email;
                txtKhoa.Text = student.Khoa;
                cbGioiTinh.Text = student.GioiTinh;
                pickNgaySinh.Text = student.NgaySinh.ToString();
            }
        }
    }
}