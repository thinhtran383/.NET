using StudentManagement.Utils;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudentManagement.Helper;
using StudentManagement.Models;
using System.Text.RegularExpressions;

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
            cbFilter.Text = "<None>";
        }

        private bool isValidate(float number) {
            Regex regex = new Regex(Constant.Regex.GRADE);
            if (!regex.IsMatch(number.ToString())) {
                return false;
            }
            return true;
        }



        private void dgGrades_Loaded(object sender,RoutedEventArgs e) {
            dgGrades.ItemsSource = grades;
        }

        private void dgGrades_SelectionChanged(object sender,SelectionChangedEventArgs e) {
            if (dgGrades.SelectedIndex != -1) {
                btnUpdate.IsEnabled = true;
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
            float diemChuyenCan = float.Parse(txtDiemChuyenCan.Text);
            float diemGiuaKi = float.Parse(txtDiemGiuaKi.Text);
            float diemCuoiKi = float.Parse(txtDiemCuoiKi.Text);
            float diemTongKet = diemChuyenCan * 0.1f + diemGiuaKi * 0.4f + diemCuoiKi * 0.5f;

            if(isValidate(diemChuyenCan)) {
                lbErrCC.Content = "Điểm chuyên cần không hợp lệ";
                
            }

            if (isValidate(diemGiuaKi)) {
                lbErrGK.Content = "Điểm giữa kì không hợp lệ";
                
            }

            if (isValidate(diemCuoiKi)) {
                lbErrCK.Content = "Điểm cuối kì không hợp lệ";
                return;
            }

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


        private void TextChanged(object sender,TextChangedEventArgs e) {
            if (!string.IsNullOrEmpty(txtDiemChuyenCan.Text)) {
                lbErrCC.Content = "";
            }
             
            if(!string.IsNullOrEmpty(txtDiemGiuaKi.Text)){
                 lbErrGK.Content = "";
            }

            if (!string.IsNullOrEmpty(txtDiemCuoiKi.Text)) {
                lbErrCK.Content = "";
            }
        }

        private void btnExport_Click(object sender,RoutedEventArgs e) {
            string defaultFileName = "exported_data";

            string fileName = FileSaveDialog.ShowSaveDialog(defaultFileName);

            if(!string.IsNullOrEmpty(fileName)) {
                ExcelExporter.ExportExcel(dgGrades,fileName);
                MessageBox.Show("Dữ liệu đã được xuất thành công!","Xuất dữ liệu",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }

        private void txtSearch_TextChanged(object sender,TextChangedEventArgs e) {
            string search = txtSearch.Text.ToLower();

            if (search.Equals("")) dgGrades.ItemsSource = grades;
            else {
                dgGrades.ItemsSource = grades.Where(grade =>
                    grade.MaSinhVien.ToLower().Contains(search) || grade.MaMonHoc.ToLower().Contains(search) ||
                    grade.TenMonHoc.ToLower().Contains(search));
            }
        }
    }
}
