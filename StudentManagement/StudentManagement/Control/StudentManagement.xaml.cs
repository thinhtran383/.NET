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
            //DateTime date = DateTime.Parse(pickNgaySinh.Text);
            //string sqlAdd = "insert into SinhVien values('" + txtMaSinhVien.Text + "','" + cbNganh.Text + "',N'" + txtTenSinhVien.Text + "','" + date + "',N'" + cbGioiTinh.Text + "','" + txtDienThoai.Text + "','" + txtEmail.Text + "',N'" + txtKhoa.Text + "')";
            //ExecuteQuery.executeNonQuery(sqlAdd);
            //studentList.Add(new Student(txtMaSinhVien.Text,cbNganh.Text,txtTenSinhVien.Text,date,cbGioiTinh.Text,txtDienThoai.Text,txtEmail.Text,txtKhoa.Text));
            //dgStudent.Items.Refresh();
            //clear();
        }


        private void btnUpdate_Click(object sender,RoutedEventArgs e) {

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


    }
}