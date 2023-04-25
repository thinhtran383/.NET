using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace StudentManagement.Control {

    public partial class StudentManagement : UserControl {
        private ObservableCollection<Student> studentList = DataManager.getStudentList();

        public StudentManagement() {
            InitializeComponent();
            initNganh();
        }

        private void dgStudent_Loaded(object sender, RoutedEventArgs e) {
            dgStudent.ItemsSource = studentList;
        }

        private void dgStudent_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (dgStudent.SelectedItem != null) {
                // Lấy đối tượng Student được chọn
                dynamic selectedStudent = dgStudent.SelectedItem;

                string maSv = selectedStudent["Mã sinh viên"].ToString();
                string maNganh = selectedStudent["Mã ngành"].ToString();
                string hoTen = selectedStudent["Họ tên"].ToString();
                string ngaySinh = selectedStudent["Ngày sinh"].ToString("dd/MM/yyyy");
                string gioiTinh = selectedStudent["Giới tính"].ToString();
                string soDT = selectedStudent["Số điện thoại"].ToString();
                string email = selectedStudent["Email"].ToString();
                string khoa = selectedStudent["Khoa"].ToString();

                txtMaSinhVien.Text = maSv; 
                cbNganh.Text = maNganh;
               
                txtTenSinhVien.Text = hoTen;
                txtDienThoai.Text = soDT;
                txtEmail.Text = email;
                txtKhoa.Text = khoa;
                cbGioiTinh.Text = gioiTinh;
                pickNgaySinh.Text = ngaySinh;
            }


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
            while (dataReader.Read()) {
                cbNganh.Items.Add(dataReader["MaNganh"].ToString());
            }
        }

        private void btnAdd_Click(object sender,RoutedEventArgs e) {
            DateTime date = DateTime.Parse(pickNgaySinh.Text);
            string sqlAdd = "INSERT INTO SinhVien VALUES ('" + txtMaSinhVien.Text + "', '" + cbNganh.Text + "', N'" + txtTenSinhVien.Text + "', '" + date + "', N'" + cbGioiTinh.Text + "', '" + txtDienThoai.Text + "', '" + txtEmail.Text + "', N'" + txtKhoa.Text + "')";
           // ExecuteQuery.executeNonQuery(sqlAdd);
          

            // Chuyển đổi giá trị của pickNgaySinh.Text sang kiểu DateTime
          

            // Tạo một đối tượng Student mới và thêm vào danh sách studentList
            studentList.Add(new Student(txtMaSinhVien.Text,cbNganh.Text,txtTenSinhVien.Text,date,cbGioiTinh.Text,txtDienThoai.Text,txtEmail.Text,txtKhoa.Text));

            // Cập nhật lại hiển thị danh sách sinh viên
            dgStudent.Items.Refresh();

            // Xóa dữ liệu đã nhập trong các trường
            clear();
        }


        private void btnUpdate_Click(object sender,RoutedEventArgs e) {

        }

        private void btnDelete_Click(object sender,RoutedEventArgs e) {

        }

        private void btnClear_Click(object sender,RoutedEventArgs e) {
            clear();
        }

        private void btnExport_Click(object sender,RoutedEventArgs e) {

        }

       
    }
}
