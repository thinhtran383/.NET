using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Wordprocessing;
using StudentManagement.Models;
using StudentManagement.Utils;


namespace StudentManagement.Helper {
    public class DataManager {
        ObservableCollection<Student> studentList = new ObservableCollection<Student>();
        ObservableCollection<Course> courseList = new ObservableCollection<Course>();
        ObservableCollection<Grade> gradeList = new ObservableCollection<Grade>();

        private static List<Account> adminAccounts = new List<Account>();

        private static DataManager instance = null;

        private DataManager() {
            initAccount();
            initStudentList();
            initCourses();
            initGrades();
        }

        private void initGrades() {
            string sql = "\r\nSELECT SinhVien.MaSinhVien, MonHoc.MaMonHoc, MonHoc.TenMonHoc, Diem.DiemChuyenCan, Diem.DiemGiuaKy, Diem.DiemCuoiKy, (Diem.DiemChuyenCan * 0.1 + Diem.DiemGiuaKy * 0.4 + Diem.DiemCuoiKy * 0.5) as TongKet\r\nFROM SinhVien\r\nINNER JOIN Diem ON SinhVien.MaSinhVien = Diem.MaSinhVien\r\nINNER JOIN MonHoc ON Diem.MaMonHoc = MonHoc.MaMonHoc;\r\n";
            SqlDataReader dataReader = ExecuteQuery.executeReader(sql);
            while(dataReader.Read()) {
                string maSinhVien = dataReader.IsDBNull(dataReader.GetOrdinal("MaSinhVien")) ? "" : dataReader.GetString(dataReader.GetOrdinal("MaSinhVien"));
                string maMonHoc = dataReader.IsDBNull(dataReader.GetOrdinal("MaMonHoc"))
                    ? ""
                    : dataReader.GetString(dataReader.GetOrdinal("MaMonHoc"));
                string tenMonHoc = dataReader.IsDBNull(dataReader.GetOrdinal("TenMonHoc"))
                    ? ""
                    : dataReader.GetString(dataReader.GetOrdinal("TenMonHoc"));
                float diemChuyenCan = dataReader.IsDBNull(dataReader.GetOrdinal("DiemChuyenCan")) ? 0 : (float) dataReader.GetDouble(dataReader.GetOrdinal("DiemChuyenCan"));

                float diemGiuaKy = dataReader.IsDBNull(dataReader.GetOrdinal("DiemGiuaKy")) ? 0 : (float) dataReader.GetDouble(dataReader.GetOrdinal("DiemGiuaKy"));

                float diemCuoiKy = dataReader.IsDBNull(dataReader.GetOrdinal("DiemCuoiKy")) ? 0 : (float) dataReader.GetDouble(dataReader.GetOrdinal("DiemCuoiKy"));

                float diemTongKet = dataReader.IsDBNull(dataReader.GetOrdinal("TongKet"))
                    ? 0
                    : (float)dataReader.GetDouble(dataReader.GetOrdinal("TongKet"));

                Grade grade = new Grade(maSinhVien,maMonHoc,tenMonHoc,diemChuyenCan,diemGiuaKy, diemCuoiKy,diemTongKet);
                gradeList.Add(grade);
            }
        }

        private void initCourses() {
           SqlDataReader _dataReader = ExecuteQuery.executeReader("Select * from MonHoc");
           while (_dataReader.Read()) {
               string maMonHoc = _dataReader.IsDBNull(_dataReader.GetOrdinal("MaMonHoc")) ? "" : _dataReader.GetString(_dataReader.GetOrdinal("MaMonHoc"));
               string tenMonHoc = _dataReader.IsDBNull(_dataReader.GetOrdinal("TenMonHoc")) ? "" : _dataReader.GetString(_dataReader.GetOrdinal("TenMonHoc"));
               int soTinChi = _dataReader.IsDBNull(_dataReader.GetOrdinal("SoTinChi")) ? 0 : _dataReader.GetInt32(_dataReader.GetOrdinal("SoTinChi"));
               string maNganh = _dataReader.IsDBNull(_dataReader.GetOrdinal("MaNganh")) ? "" : _dataReader.GetString(_dataReader.GetOrdinal("MaNganh"));
               courseList.Add(new Course(maMonHoc,tenMonHoc,soTinChi,maNganh));
           }
        }

        private void initAccount() {
            SqlDataReader _dataReader = ExecuteQuery.executeReader("Select * from AdminAccount");
            while(_dataReader.Read()) {
                string username = _dataReader.GetString(_dataReader.GetOrdinal("username"));
                string password = _dataReader.GetString(_dataReader.GetOrdinal("password"));
                Account account = new Account(username,password);
                adminAccounts.Add(account);
            }
        }

        private void initStudentList() {
            SqlDataReader _dataReader = ExecuteQuery.executeReader("Select * from SinhVien");
            while(_dataReader.Read()) {
                string maSV = _dataReader.IsDBNull(_dataReader.GetOrdinal("MaSinhVien")) ? "" : _dataReader.GetString(_dataReader.GetOrdinal("MaSinhVien"));
                string maNganh = _dataReader.IsDBNull(_dataReader.GetOrdinal("MaNganh")) ? "" : _dataReader.GetString(_dataReader.GetOrdinal("MaNganh"));
                string hoTen = _dataReader.IsDBNull(_dataReader.GetOrdinal("TenSinhVien")) ? "" : _dataReader.GetString(_dataReader.GetOrdinal("TenSinhVien"));
                DateTime ngaySinh = _dataReader.IsDBNull(_dataReader.GetOrdinal("NgaySinh")) ? DateTime.MinValue : _dataReader.GetDateTime(_dataReader.GetOrdinal("NgaySinh"));
                string gioiTinh = _dataReader.IsDBNull(_dataReader.GetOrdinal("GioiTinh")) ? "" : _dataReader.GetString(_dataReader.GetOrdinal("GioiTinh"));
                string soDT = _dataReader.IsDBNull(_dataReader.GetOrdinal("SoDienThoai")) ? "" : _dataReader.GetString(_dataReader.GetOrdinal("SoDienThoai"));
                string email = _dataReader.IsDBNull(_dataReader.GetOrdinal("Email")) ? "" : _dataReader.GetString(_dataReader.GetOrdinal("Email"));
                string khoa = _dataReader.IsDBNull(_dataReader.GetOrdinal("Khoa")) ? "" : _dataReader.GetString(_dataReader.GetOrdinal("Khoa"));
                Student student = new Student(hoTen,maSV,maNganh,ngaySinh,gioiTinh,soDT,email,khoa);
                studentList.Add(student);
            }
        }


        public static List<Account> GetAdminAccounts() {
            if(instance == null) {
                instance = new DataManager();
            }
            return adminAccounts;
        }

        public static ObservableCollection<Student> GetStudentList() {
            if(instance == null) {
                instance = new DataManager();
            }
            return instance.studentList;
        }

        public static ObservableCollection<Course> GetCourseList() {
            if(instance == null) {
                instance = new DataManager();
            }
            return instance.courseList;
        }

        public static ObservableCollection<Grade> GetGradeList() {
            if(instance == null) {
                instance = new DataManager();
            }
            return instance.gradeList;
        }
    }

}