using StudentManagement.Utils;
using System.Data.SqlClient;
using System.Windows;
using StudentManagement.Helper;
using StudentManagement.Models;
using System;

namespace StudentManagement.Control.Student {
   
    public partial class InputInfo:Window {
        private string maSinhVien;
        public InputInfo(string maSinhVien) {
            InitializeComponent();
            initNganh();
            this.maSinhVien = maSinhVien;

        }

        private void initNganh() {
            SqlDataReader dataReader = ExecuteQuery.executeReader("select MaNganh from Nganh");
            while(dataReader.Read()) {
                cbNganh.Items.Add(dataReader["MaNganh"].ToString());
            }
        }

        private void btnOk_Click(object sender,RoutedEventArgs e) {
            bool isError = false;

            isError |= CheckValid.isFieldEmpty(txtDienThoai, lbErrSo, "Không được để trống");
            isError |= CheckValid.isFieldEmpty(txtTenSinhVien, lbErrTen, "Không được để trống");
            isError |= CheckValid.isFieldEmpty(cbNganh, lbErrMaNganh, "Không được để trống");
            isError |= CheckValid.isFieldEmpty(cbGioiTinh, lbErrGioitinh, "Không được để trống"); 
            isError |= CheckValid.isFieldEmpty(pickNgaySinh, lbErrNgay, "Không được để trống");
            isError |= CheckValid.isFieldEmpty(txtKhoa, lbErrKhoa, "Không được để trống");

            if(!isError) {
                string tenSinhVien = txtTenSinhVien.Text;
                DateTime ngaySinh = DateTime.Parse(pickNgaySinh.Text);
                string gioiTinh = cbGioiTinh.Text;
                string dienThoai = txtDienThoai.Text;
           
                string khoa = txtKhoa.Text;
                string maNganh = cbNganh.Text;

                string sql = $"insert into Pending(MaSinhVien, TenSinhVien, GioiTinh, Khoa, MaNganh) values('{maSinhVien}', '{tenSinhVien}', '{gioiTinh}', '{khoa}', '{maNganh}')";
                ExecuteQuery.executeNonQuery(sql);
                MessageBox.Show("Gửi yêu cầu thành công");

            }

        }
    }
}
