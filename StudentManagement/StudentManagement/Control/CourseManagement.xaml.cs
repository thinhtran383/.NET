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

namespace StudentManagement.Control
{
    public partial class CourseManagement: UserControl {
        private ObservableCollection<Course> coursesList = DataManager.GetCourseList();
        public CourseManagement()
        {
            InitializeComponent();
            initNganh();
        }

        private void dgCourses_Loaded(object sender,RoutedEventArgs e) {
            dgCourses.ItemsSource = coursesList;
        }

        private void initNganh() {
            SqlDataReader dataReader = ExecuteQuery.executeReader("select MaNganh from Nganh");
            while (dataReader.Read()) {
                cbMaNganh.Items.Add(dataReader["MaNganh"].ToString());
            }
        }

        private void clear() {
            txtMaHocPhan.Text = "";
            txtTenHocPhan.Text = "";
            txtTinChi.Text = "";
            cbMaNganh.SelectedIndex = -1;
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
                int tinChi = int.Parse(txtTinChi.Text);
                string maNganh = cbMaNganh.SelectedValue.ToString();

                string sqlAdd = $"INSERT INTO MonHoc VALUES ('{maHocPhan}', '{tenHocPhan}', {tinChi}, '{maNganh}')";
                ExecuteQuery.executeNonQuery(sqlAdd);
                coursesList.Add(new Course(maHocPhan,tenHocPhan,tinChi,maNganh));
                dgCourses.Items.Refresh();
                MessageBox.Show("Thêm thành công");
                clear();
            }
        }

        private void TextChanged(object sender,TextChangedEventArgs e) {
            if(!string.IsNullOrEmpty(txtMaHocPhan.Text)) {
                lbErrMa.Content = "";
            }

            if(!string.IsNullOrEmpty(txtTenHocPhan.Text)) {
                lbErrTen.Content = "";
            }

            if(!string.IsNullOrEmpty(txtTinChi.Text)) {
                lbErrTin.Content = "";
            }
        }

        

        private void cbMaNganh_SelectionChanged(object sender,SelectionChangedEventArgs e) {
            if(cbMaNganh.SelectedItem != null) {
                lbErrNganh.Content = "";
            }
        }

        private void btnClear_Click(object sender,RoutedEventArgs e) {
            clear();
        }

        private void btnDelete_Click(object sender,RoutedEventArgs e) {
            string maHocPhan = txtMaHocPhan.Text;
            string sqlDelete = $"DELETE FROM MonHoc WHERE MaMonHoc = '{maHocPhan}'";
            //MessageBox Confirm
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes) {
                ExecuteQuery.executeNonQuery(sqlDelete);
                coursesList.Remove(coursesList.Where(course => course.MaMonHoc == maHocPhan).FirstOrDefault());
                dgCourses.Items.Refresh();
                MessageBox.Show("Xóa thành công");
                clear();
            }
            
        }

        private void btnUpdate_Click(object sender,RoutedEventArgs e) {
            string maHocPhan = txtMaHocPhan.Text;
            string sqlUpdate = $"UPDATE MonHoc SET TenMonHoc = N'{txtTenHocPhan.Text}', SoTinChi = {txtTinChi.Text}, MaNganh = '{cbMaNganh.SelectedValue.ToString()}' WHERE MaMonHoc = '{maHocPhan}'";
            ExecuteQuery.executeNonQuery(sqlUpdate);
            Course course = coursesList.Where(c => c.MaMonHoc == maHocPhan).FirstOrDefault();
            course.TenMonHoc = txtTenHocPhan.Text;
            course.SoTinChi = int.Parse(txtTinChi.Text);
            course.MaNganh = cbMaNganh.SelectedValue.ToString();
            dgCourses.Items.Refresh();
            MessageBox.Show("Cập nhật thành công");
        }
    }
}
