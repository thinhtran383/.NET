    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Models {
    public class Course {
        private string maMonHoc;
        private string tenMonHoc;
        private int soTinChi;

        public Course(string maMonHoc, string tenMonHoc, int soTinChi) {
            this.maMonHoc = maMonHoc;
            this.tenMonHoc = tenMonHoc;
            this.soTinChi = soTinChi;
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
    }
}
