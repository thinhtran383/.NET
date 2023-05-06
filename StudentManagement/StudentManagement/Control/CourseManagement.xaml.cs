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
using DocumentFormat.OpenXml.Office2010.Drawing;
using StudentManagement.Helper;
using StudentManagement.Models;
using StudentManagement.Utils;
using System.Text.RegularExpressions;

namespace StudentManagement.Control {
    public partial class CourseManagement:UserControl {
        private ObservableCollection<Course> coursesList = DataManager.GetCourseList();
        public CourseManagement() {
            InitializeComponent();
            initNganh();
        }

        private void dgCourses_Loaded(object sender,RoutedEventArgs e) {
            dgCourses.ItemsSource = coursesList;
        }

        private void initNganh() {
            SqlDataReader dataReader = ExecuteQuery.executeReader("select MaNganh from Nganh");
            while(dataReader.Read()) {
                cbMaNganh.Items.Add(dataReader["MaNganh"].ToString());
            }
        }
        private void TextChanged(object sender,TextChangedEventArgs e) {


            if(!string.IsNullOrEmpty(txtMaHocPhan.Text)) {
                lbErrMa.Content = "";
            }

            if(!string.IsNullOrEmpty(txtTenHocPhan.Text)) {
                lbErrTen.Content = "";
            }

        }

        private void clear() {
            txtMaHocPhan.Text = "";
            txtTenHocPhan.Text = "";
            txtTinChi.Text = "";
            cbMaNganh.SelectedIndex = -1;
            dgCourses.SelectedIndex = -1;
        }


        private void btnExport_Click(object sender,RoutedEventArgs e) {
            //  ExcelExporter.Export(dgCourses,"CourseList.xlsx");
        }

        private bool IsFieldEmpty<T>(T field,Label errorLabel,string errorMessage) { // phuong thuc generic
            if(field is TextBox && string.IsNullOrEmpty((field as TextBox).Text)) {
                errorLabel.Content = errorMessage;
                return true;
            } else if(field is ComboBox && (field as ComboBox).SelectedItem == null) {
                errorLabel.Content = errorMessage;
                return true;
            } else {
                errorLabel.Content = "";
                return false;
            }
        }




        private void btnAdd_Click(object sender,RoutedEventArgs e) {
            bool isError = false;


            isError |= IsFieldEmpty(txtMaHocPhan,lbErrMa,"Không được để trống phần này");
            isError |= IsFieldEmpty(txtTenHocPhan,lbErrTen,"Không được để trống phần này");
            isError |= IsFieldEmpty(txtTinChi,lbErrTin,"Không được để trống phần này");
            isError |= IsFieldEmpty(cbMaNganh,lbErrNganh,"Không được để trống phần này");

            if(!isError) {
                string maHocPhan = txtMaHocPhan.Text;
                string tenHocPhan = txtTenHocPhan.Text;
                int tinChi;
                string maNganh = cbMaNganh.SelectedValue.ToString();

                foreach(Course course in coursesList) {
                    if(course.MaMonHoc == maHocPhan) {
                        lbErrMa.Content = "Mã học phần đã tồn tại";
                        return;
                    }
                }

                Regex regex = new Regex(Constant.Regex.CREDITS); // Đoạn regex kiểm tra số nguyên dương
                if(!regex.IsMatch(txtTinChi.Text)) {
                    lbErrTin.Content = "Vui lòng nhập số nguyên dương hợp lệ";
                    return;
                } else {
                    tinChi = int.Parse(txtTinChi.Text);
                }

                string sqlAdd = $"INSERT INTO MonHoc VALUES ('{maHocPhan}', '{tenHocPhan}', {tinChi}, '{maNganh}')";
                ExecuteQuery.executeNonQuery(sqlAdd);
                coursesList.Add(new Course(maHocPhan,tenHocPhan,tinChi,maNganh));
                dgCourses.Items.Refresh();
                MessageBox.Show("Thêm thành công");
                clear();
            }

        }

     


        private void btnClear_Click(object sender,RoutedEventArgs e) {
            clear();
        }

        private void btnDelete_Click(object sender,RoutedEventArgs e) {
            if(dgCourses.SelectedItem == null) {
                return;
            }

            string maHocPhan = txtMaHocPhan.Text;
            string sqlDelete = $"DELETE FROM MonHoc WHERE MaMonHoc = '{maHocPhan}'";

            //MessageBox Confirm
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa không?","Xác nhận",MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes) {
                ExecuteQuery.executeNonQuery(sqlDelete);
                coursesList.Remove(coursesList.Where(course => course.MaMonHoc == maHocPhan).FirstOrDefault());
                dgCourses.Items.Refresh();
                MessageBox.Show("Xóa thành công");
                clear();
            }

        }

        private void btnUpdate_Click(object sender,RoutedEventArgs e) {
            if(dgCourses.SelectedItem == null) {
                return;
            }

            string maHocPhan = txtMaHocPhan.Text;
            string tenHocPhan = txtTenHocPhan.Text;
            string cbMaNganh = this.cbMaNganh.SelectedValue.ToString();
            int soTinChi = int.Parse(txtTinChi.Text);

            string sqlUpdate = $"UPDATE MonHoc SET MaNganh = '{cbMaNganh}', TenMonHoc = N'{tenHocPhan}', SoTinChi = {soTinChi} WHERE MaMonHoc = '{maHocPhan}'";
            ExecuteQuery.executeNonQuery(sqlUpdate);


            // cap nhat lai dg
            Course course = (Course) dgCourses.SelectedItem;
            course.SoTinChi = soTinChi;
            course.MaMonHoc = maHocPhan;
            course.TenMonHoc = tenHocPhan;
            course.MaNganh = cbMaNganh;


            MessageBox.Show("Cập nhật thành công");
            dgCourses.Items.Refresh();
            clear();
        }

        private void dgCourses_SelectionChanged(object sender,SelectionChangedEventArgs e) {
            if(dgCourses.SelectedItem != null) {
                // Lấy đối tượng Student được chọn
                dynamic selectedStudent = dgCourses.SelectedItem;
                string maMonHoc = selectedStudent.MaMonHoc;
                string tenMonHoc = selectedStudent.TenMonHoc;
                string soTinChi = (selectedStudent.SoTinChi).ToString();
                string maNganh = selectedStudent.MaNganh;

                txtMaHocPhan.Text = maMonHoc;
                txtTenHocPhan.Text = tenMonHoc;
                txtTinChi.Text = soTinChi;
                cbMaNganh.Text = maNganh;
            }
        }
    }
}
