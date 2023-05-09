using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using StudentManagement.Helper;
using StudentManagement.Models;
using StudentManagement.Utils;
using System.Text.RegularExpressions;

namespace StudentManagement.Control {

    public partial class StudentManagement:UserControl {
        private ObservableCollection<Student> studentList = DataManager.GetStudentList();


        public StudentManagement() {
            InitializeComponent();
            initNganh();
        }

      

        private bool isFieldEmpty<T>(T field,Label errorLabel,string errorMessage) { // phuong thuc generic
            if(field is TextBox && string.IsNullOrEmpty((field as TextBox).Text)) {
                errorLabel.Content = errorMessage;
                return true;
            } else if(field is ComboBox && (field as ComboBox).SelectedItem == null) {
                errorLabel.Content = errorMessage;
                return true;
            } else if (field is DatePicker && (field as DatePicker).SelectedDate == null) {
                errorLabel.Content = errorMessage;
                return true;
            } else {
                errorLabel.Content = "";
                return false;
            }
        }

        private bool isValidate(string email,string dienThoai) {
            Regex regex = new Regex(Constant.Regex.EMAIL);
            Regex regexPhone = new Regex(Constant.Regex.PHONE);
            if(!regex.IsMatch(email)) {
                lbErrEmail.Content = "Email không hợp lệ";
                return true;
            }

            if(!regexPhone.IsMatch(dienThoai)) {
                lbErrSo.Content = "Số điện thoại không hợp lệ";
                return true;
            }

            return false;
        }

        private bool isExits(string maSinhVien) {
            foreach(Student student in studentList) {
                if(student.MaSV.Equals(maSinhVien)) {
                    lbErrMa.Content = "Mã sinh viên đã tồn tại";
                    return true;
                }
            }

            return false;
        }

        private void pickNgaySinh_SelectedDateChanged(object sender,SelectionChangedEventArgs e) {
            if(pickNgaySinh.SelectedDate != null) {
                DateTime ngaySinh = pickNgaySinh.SelectedDate.Value;
                lbErrNgay.Content = "";
            }
        }

        private void SelectionChanged(object sender,SelectionChangedEventArgs e) {
            if(cbNganh.SelectedItem != null) {
                lbErrMaNganh.Content = "";
               
            }

            if(cbGioiTinh.SelectedItem != null) {
                lbErrGioitinh.Content = "";
              
            }
        }

        private void TextChanged(object sender,TextChangedEventArgs e) {
            if(!string.IsNullOrEmpty(txtMaSinhVien.Text)) {
                lbErrMa.Content = "";
            }

            if(!string.IsNullOrEmpty(txtTenSinhVien.Text)) {
                lbErrTen.Content = "";
            }

            if(!string.IsNullOrEmpty(txtDienThoai.Text)) {
                lbErrSo.Content = "";
            }
            if(!string.IsNullOrEmpty(txtEmail.Text)) {
                lbErrEmail.Content = "";
            }

            if(!string.IsNullOrEmpty(txtKhoa.Text)) {
                lbErrKhoa.Content = "";
            }


            if(!string.IsNullOrEmpty(cbGioiTinh.Text)) {
                lbErrGioitinh.Content = "";
            }
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
            bool isError = false;
            isError |= isFieldEmpty(txtMaSinhVien,lbErrMa,"Không được để trống phần này");
            isError |= isFieldEmpty(txtTenSinhVien,lbErrTen,"Không được để trống phần này");
            isError |= isFieldEmpty(txtDienThoai,lbErrSo,"Không được để trống phần này");
            isError |= isFieldEmpty(cbNganh,lbErrMaNganh,"Không được để trống phần này");
            isError |= isFieldEmpty(txtEmail,lbErrEmail,"Không được để trống phần này");
            isError |= isFieldEmpty(txtKhoa,lbErrKhoa,"Không được để trống phần này");
            isError |= isFieldEmpty(pickNgaySinh,lbErrNgay,"Không được để trống phần này");
            isError |= isFieldEmpty(cbGioiTinh,lbErrGioitinh,"Không được để trống phần này");
      

            if(!isError) {
                string maSinhVien = txtMaSinhVien.Text;
                string tenSinhVien = txtTenSinhVien.Text;
                DateTime ngaySinh = DateTime.Parse(pickNgaySinh.Text);
                string gioiTinh = cbGioiTinh.Text;
                string dienThoai = txtDienThoai.Text;
                string email = txtEmail.Text;
                string khoa = txtKhoa.Text;
                string maNganh = cbNganh.Text;

                if (isExits(maSinhVien)) return;

                if (isValidate(email, dienThoai)) return;

                string sqlAdd = $"insert into SinhVien values ('{maSinhVien}', '{maNganh}', '{tenSinhVien}', '{ngaySinh}', '{gioiTinh}', '{dienThoai}', '{email}', '{khoa}' )";
                ExecuteQuery.executeNonQuery(sqlAdd);
                studentList.Add(new Student(tenSinhVien,maSinhVien,maNganh,ngaySinh,gioiTinh,dienThoai,email,khoa));
                dgStudent.Items.Refresh();
                clear();
            }
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

            

            if(isValidate(email,dienThoai))
                return;

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
            MessageBox.Show("Cập nhật thành công");

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

        private void txtSearch_TextChanged(object sender,TextChangedEventArgs e) {
            string search = txtSearch.Text.ToLower();
            if(search == "") {
                dgStudent.ItemsSource = studentList;
            } else {
                dgStudent.ItemsSource = studentList.Where(student => student.MaSV.ToLower().Contains(search) || student.HoTen.ToLower().Contains(search) || student.MaNganh.ToLower().Contains(search) || student.SoDT.ToLower().Contains(search) || student.Email.ToLower().Contains(search) || student.Khoa.ToLower().Contains(search) || student.GioiTinh.ToLower().Contains(search) || student.NgaySinh.ToString().ToLower().Contains(search));
            }
        }
    }



}