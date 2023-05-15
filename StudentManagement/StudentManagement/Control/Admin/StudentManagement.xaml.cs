using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudentManagement.Helper;
using StudentManagement.Utils;

namespace StudentManagement.Control {

    public partial class StudentManagement:UserControl {
        private ObservableCollection<Models.Student> studentList = DataManager.GetStudentList();

        public StudentManagement() {
            InitializeComponent();
            initNganh();
        }

        private void initNganh() {
            SqlDataReader dataReader = ExecuteQuery.executeReader("select MaNganh from Nganh");
            while(dataReader.Read()) {
                cbNganh.Items.Add(dataReader["MaNganh"].ToString());
            }
        }

        private void pickNgaySinh_SelectedDateChanged(object sender,SelectionChangedEventArgs e) {
            if(pickNgaySinh.SelectedDate != null) {
                DateTime ngaySinh = pickNgaySinh.SelectedDate.Value;
                lbErrNgay.Content = "";
            }
        }

        private void SelectionChanged(object sender,SelectionChangedEventArgs e) {
            CheckValid.isSelected(cbNganh,lbErrMaNganh);
            CheckValid.isSelected(cbGioiTinh, lbErrGioitinh);
        }


        private bool isExists(string maSinhVien) {
            foreach(Models.Student student in studentList) {
                if(student.MaSV.Equals(maSinhVien)) {
                    lbErrMa.Content = "Mã sinh viên đã tồn tại";
                    return true;
                }
            }

            return false;
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

      

        private void btnAdd_Click(object sender,RoutedEventArgs e) {
            bool isError = false;
            isError |= CheckValid.isFieldEmpty(txtMaSinhVien,lbErrMa,"Không được để trống phần này");
            isError |= CheckValid.isFieldEmpty(txtTenSinhVien,lbErrTen,"Không được để trống phần này");
            isError |= CheckValid.isFieldEmpty(txtDienThoai,lbErrSo,"Không được để trống phần này");
            isError |= CheckValid.isFieldEmpty(cbNganh,lbErrMaNganh,"Không được để trống phần này");
            isError |= CheckValid.isFieldEmpty(txtEmail,lbErrEmail,"Không được để trống phần này");
            isError |= CheckValid.isFieldEmpty(txtKhoa,lbErrKhoa,"Không được để trống phần này");
            isError |= CheckValid.isFieldEmpty(pickNgaySinh,lbErrNgay,"Không được để trống phần này");
            isError |= CheckValid.isFieldEmpty(cbGioiTinh,lbErrGioitinh,"Không được để trống phần này");
      

            if(!isError) {
                string maSinhVien = txtMaSinhVien.Text;
                string tenSinhVien = txtTenSinhVien.Text;
                DateTime ngaySinh = DateTime.Parse(pickNgaySinh.Text);
                string gioiTinh = cbGioiTinh.Text;
                string dienThoai = txtDienThoai.Text;
                string email = txtEmail.Text;
                string khoa = txtKhoa.Text;
                string maNganh = cbNganh.Text;

                if (isExists(maSinhVien)) return;

                if (!CheckValid.isValidate(email, dienThoai, lbErrSo, lbErrEmail)) return;
                if (CheckValid.isExitsEmail(email, studentList, lbErrEmail)) return;

                string sqlAdd = $"insert into SinhVien values ('{maSinhVien}', '{maNganh}', '{tenSinhVien}', '{ngaySinh}', '{gioiTinh}', '{dienThoai}', '{email}', '{khoa}' )";
                ExecuteQuery.executeNonQuery(sqlAdd);
                studentList.Add(new Models.Student(tenSinhVien,maSinhVien,maNganh,ngaySinh,gioiTinh,dienThoai,email,khoa));
                dgStudent.Items.Refresh();
                MessageBox.Show("Thêm thành công");
                clear();
            }
        }

     


        private void btnUpdate_Click(object sender,RoutedEventArgs e) {
            Models.Student student = (Models.Student) dgStudent.SelectedItem;

            string maSinhVienOld = student.MaSV;
            string maSinhVien = txtMaSinhVien.Text;
            string tenSinhVien = txtTenSinhVien.Text;
            DateTime ngaySinh = DateTime.Parse(pickNgaySinh.Text);
            string gioiTinh = cbGioiTinh.Text;
            string dienThoai = txtDienThoai.Text;
            string email = txtEmail.Text;
            string khoa = txtKhoa.Text;
            string maNganh = cbNganh.Text;

            
            if(!CheckValid.isValidate(email,dienThoai,lbErrSo,lbErrEmail)) return;

            string sqlUpdate = $"Update SinhVien Set MaSinhVien = '{maSinhVien}', MaNganh = '{maNganh}', TenSinhVien = '{tenSinhVien}', NgaySinh = '{ngaySinh}', GioiTinh = '{gioiTinh}', SoDienThoai = '{dienThoai}', Email = '{email}', Khoa = '{khoa}' where MaSinhVien = '{maSinhVienOld}'";
            ExecuteQuery.executeNonQuery(sqlUpdate);

           
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
            DataManager.ReLoadGradeList();

        }

        private void btnDelete_Click(object sender,RoutedEventArgs e) {
            if(MessageBox.Show("Bạn có chắc muốn xoá?","Xác nhận xoá",MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                string selectedId = ((Models.Student) dgStudent.SelectedItem).MaSV;
                string sqlDelete = "delete from SinhVien where MaSinhVien = '" + selectedId + "'";
                ExecuteQuery.executeNonQuery(sqlDelete);
                studentList.Remove((Models.Student) dgStudent.SelectedItem);
                dgStudent.Items.Refresh();
                DataManager.ReLoadGradeList();
            }
        }

        private void btnClear_Click(object sender,RoutedEventArgs e) {
            clear();
        }

        private void btnExport_Click(object sender,RoutedEventArgs e) {
            string defaultFileName = "exported_data";

            string fileName = FileSaveDialog.ShowSaveDialog(defaultFileName);

            if(!string.IsNullOrEmpty(fileName)) {
                ExcelExporter.ExportExcel(dgStudent,fileName);
                MessageBox.Show("Dữ liệu đã được xuất thành công!","Xuất dữ liệu",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }

        private void dgStudent_SelectionChanged(object sender,SelectionChangedEventArgs e) {
            if(dgStudent.SelectedIndex != -1) {
                Models.Student student = (Models.Student) dgStudent.SelectedItem;
                txtMaSinhVien.Text = student.MaSV;
                cbNganh.Text = student.MaNganh;
                txtTenSinhVien.Text = student.HoTen;
                txtDienThoai.Text = student.SoDT;
                txtEmail.Text = student.Email;
                txtKhoa.Text = student.Khoa;
                cbGioiTinh.Text = student.GioiTinh;
                pickNgaySinh.Text = student.NgaySinh.ToString("dd/MM/yyyy");
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