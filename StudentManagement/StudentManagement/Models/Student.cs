using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Models {
    public class Student {
        private string maSV;
        private string maNganh;
        private string hoTen;
        private DateTime ngaySinh;
        private string gioiTinh;
        private string soDT;
        private string email;
        private string khoa;


        public Student() {
        }

        public Student(string hoTen,string maSV, string maNganh, DateTime ngaySinh, string gioiTinh, string soDT, string email, string khoa) {
            this.hoTen = hoTen;
            this.maSV = maSV;
            this.maNganh = maNganh;
            this.ngaySinh = ngaySinh;
            this.gioiTinh = gioiTinh;
            this.soDT = soDT;
            this.email = email;
            this.khoa = khoa;
        }

       

        public void setMaSV(string maSV) {
            this.maSV = maSV;
        }

        public string getMaSV() {
            return this.maSV;
        }

        public void setMaNganh(string maNganh) {
            this.maNganh = maNganh;
        }

        public string getMaNganh() {
            return this.maNganh;
        }

        public void setHoTen(string hoTen) {
            this.hoTen = hoTen;
        }

        public string getHoTen() {
            return this.hoTen;
        }

        public void setNgaySinh(DateTime ngaySinh) {
            this.ngaySinh = ngaySinh;
        }

        public DateTime getNgaySinh() {
            return this.ngaySinh;
        }

        public void setGioiTinh(string gioiTinh) {
            this.gioiTinh = gioiTinh;
        }

        public string getGioiTinh() {
            return this.gioiTinh;
        }

        public void setSoDT(string soDT) {
            this.soDT = soDT;
        }

        public string getSoDT() {
            return this.soDT;
        }

        public void setEmail(string email) {
            this.email = email;
        }

        public string getEmail() {
            return this.email;
        }

        public void setKhoa(string khoa) {
            this.khoa = khoa;
        }

        public string getKhoa() {
            return this.khoa;
        }

    }
}
