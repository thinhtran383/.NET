using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Models {
    public class Grade {
        private string maSinhVien;
        private string maMonHoc;
        private string tenMonHoc;
        private float diemChuyenCan;
        private float diemGiuaKi;
        private float diemCuoiKi;
        private float tongKet;

        public Grade(string maSinhVien, string maMonHoc, string tenMonHoc) {
            this.maSinhVien = maSinhVien;
            this.maMonHoc = maMonHoc;
            this.tenMonHoc = tenMonHoc;
        }

        public Grade(string maSinhVien, string maMonHoc, string tenMonHoc, float diemChuyenCan, float diemGiuaKi, float diemCuoiKi, float tongKet) {
            this.maSinhVien = maSinhVien;
            this.maMonHoc = maMonHoc;
            this.tenMonHoc = tenMonHoc;
            this.diemChuyenCan = diemChuyenCan;
            this.diemGiuaKi = diemGiuaKi;
            this.diemCuoiKi = diemCuoiKi;
            this.TongKet = tongKet;

        }

        public string MaSinhVien {
            get => maSinhVien;
            set => maSinhVien = value;
        }

        public string MaMonHoc {
            get => maMonHoc;
            set => maMonHoc = value;
        }

        public string TenMonHoc {
            get => tenMonHoc;
            set => tenMonHoc = value;
        }

        public float DiemChuyenCan {
            get => diemChuyenCan;
            set => diemChuyenCan = value;
        }

        public float DiemGiuaKi {
            get => diemGiuaKi;
            set => diemGiuaKi = value;
        }

        public float DiemCuoiKi {
            get => diemCuoiKi;
            set => diemCuoiKi = value;
        }

        public float TongKet {
            get => tongKet;
            set => tongKet = value;
        }
    }
}
