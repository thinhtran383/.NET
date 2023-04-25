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

        public string MaSV {
            get => maSV;
            set => maSV = value;
        }

        public string MaNganh {
            get => maNganh;
            set => maNganh = value;
        }

        public string HoTen {
            get => hoTen;
            set => hoTen = value;
        }

        public DateTime NgaySinh {
            get => ngaySinh;
            set => ngaySinh = value.Date;
        }

        public string GioiTinh {
            get => gioiTinh;
            set => gioiTinh = value;
        }

        public string SoDT {
            get => soDT;
            set => soDT = value;
        }

        public string Email {
            get => email;
            set => email = value;
        }

        public string Khoa {
            get => khoa;
            set => khoa = value;
        }

        public Student() {
        }

        public Student(string hoTen,string maSV,string maNganh,DateTime ngaySinh,string gioiTinh,string soDT,string email,string khoa) {
            this.hoTen = hoTen;
            this.maSV = maSV;
            this.maNganh = maNganh;
            this.ngaySinh = ngaySinh.Date;
            this.gioiTinh = gioiTinh;
            this.soDT = soDT;
            this.email = email;
            this.khoa = khoa;
        }
    }
}