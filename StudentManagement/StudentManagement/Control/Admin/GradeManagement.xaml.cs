using StudentManagement.Utils;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudentManagement.Helper;
using StudentManagement.Models;

namespace StudentManagement.Control {
   
    public partial class GradeManagement:UserControl {
        private ObservableCollection<Grade> grades = DataManager.GetGradeList();

        public GradeManagement() {
            InitializeComponent();
            initFilter();
           
        }

        private void initFilter() {
            cbFilter.Items.Add("Đỗ");
            cbFilter.Items.Add("Trượt");
            cbFilter.Items.Add("<None>");
            cbFilter.SelectedIndex = 3;

        }


        private void dgGrades_Loaded(object sender,RoutedEventArgs e) {
            dgGrades.ItemsSource = grades;
        }

        private void dgGrades_SelectionChanged(object sender,SelectionChangedEventArgs e) {
            if (dgGrades.SelectedIndex != -1) {
                Grade selectedGrade = (Grade)dgGrades.SelectedItem;
                string maSinhVien = selectedGrade.MaSinhVien;
                string maMon = selectedGrade.MaMonHoc;
                string tenMon = selectedGrade.TenMonHoc;
                string diemChuyenCan = selectedGrade.DiemChuyenCan.ToString();
                string diemGiuaKi = selectedGrade.DiemGiuaKi.ToString();
                string diemCuoiKi = selectedGrade.DiemCuoiKi.ToString();
                string diemTongKet = selectedGrade.TongKet.ToString();

                txtMaSinhVien.Text = maSinhVien;
                txtMaMon.Text = maMon;
                txtTenMon.Text = tenMon;
                txtDiemChuyenCan.Text = diemChuyenCan;
                txtDiemGiuaKi.Text = diemGiuaKi;
                txtDiemCuoiKi.Text = diemCuoiKi;
                txtTongKet.Text = diemTongKet;
                    
            }
        }

        private void cbFilter_SelectionChanged(object sender,SelectionChangedEventArgs e) {
            if (cbFilter.SelectedValue.ToString().Equals("<None>")) {
                dgGrades.ItemsSource = grades;
            } else if (cbFilter.SelectedValue.ToString().Equals("Đỗ")) {
                dgGrades.ItemsSource = grades.Where(grade => grade.TongKet >= 5.0);
            }
            else {
                dgGrades.ItemsSource = grades.Where(grade => grade.TongKet < 5.0);
            }

        }

        private void btnUpdate_Click(object sender,RoutedEventArgs e) {
            string maSinhVien = txtMaSinhVien.Text;
            string maMon = txtMaMon.Text;
            string tenMon = txtTenMon.Text;
            float diemChuyenCan = float.Parse(txtDiemChuyenCan.Text);
            float diemGiuaKi = float.Parse(txtDiemGiuaKi.Text);
            float diemCuoiKi = float.Parse(txtDiemCuoiKi.Text);
            float diemTongKet = diemChuyenCan * 0.1f + diemGiuaKi * 0.4f + diemCuoiKi * 0.5f;
            
            

            string sqlUpdate = $"Update Diem SET DiemChuyenCan = {diemChuyenCan}, DiemGiuaKy = {diemGiuaKi}, DiemCuoiKy = {diemCuoiKi} where MaSinhVien = '{maSinhVien}' and MaMonHoc = '{maMon}'";
            ExecuteQuery.executeNonQuery(sqlUpdate);
                
            Grade grade = (Grade)dgGrades.SelectedItem;
            grade.DiemChuyenCan = diemChuyenCan;
            grade.DiemGiuaKi = diemGiuaKi;
            grade.DiemCuoiKi = diemCuoiKi;
            grade.TongKet = diemTongKet;


            dgGrades.Items.Refresh();
            MessageBox.Show("Cập nhật thành công!");
            
        }
    }
}
