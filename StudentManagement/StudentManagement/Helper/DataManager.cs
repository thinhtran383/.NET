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

        private static List<Account> adminAccounts = new List<Account>();

        private static DataManager instance = null;

        private DataManager() {
            initAccount();
            initStudentList();
            initCourses();
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

    }

}