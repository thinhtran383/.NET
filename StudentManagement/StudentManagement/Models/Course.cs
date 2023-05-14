using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Models {
    public class Course {
        private string maSinhVien;
        private string tenSinhVien;
        private string khoa;

        private string maMonHoc;
        private string tenMonHoc;
        private int soTinChi;
        private string maNganh;


        public Course(string maSinhVien, string tenSinhVien, string khoa, string maMonHoc, string tenMonHoc, int soTinChi, string maNganh) {
            this.maSinhVien = maSinhVien;
            this.tenSinhVien = tenSinhVien;
            this.khoa = khoa;
            this.maMonHoc = maMonHoc;
            this.tenMonHoc = tenMonHoc;
            this.soTinChi = soTinChi;
            this.maNganh = maNganh;
        }


        public Course(string maMonHoc, string tenMonHoc, int soTinChi, string maNganh) {
            this.maMonHoc = maMonHoc;
            this.tenMonHoc = tenMonHoc;
            this.soTinChi = soTinChi;
            this.maNganh = maNganh;
        }

       
        public int SoTinChi {
            get => soTinChi;
            set => soTinChi = value;
        }
        public string TenMonHoc {
            get => tenMonHoc;
            set => tenMonHoc = value;
        }
        public string MaMonHoc {
            get => maMonHoc;
            set => maMonHoc = value;
        }

        public string MaNganh {
            get => maNganh;
            set => maNganh = value;
        }

        public string TenSinhVien {
            get => tenSinhVien;
            set => tenSinhVien = value;
        }

        public string MaSinhVien {
            get => maSinhVien;
            set => maSinhVien = value;
        }

        public string Khoa {
            get => khoa;
            set => khoa = value;
        }
    }
}
