DROP DATABASE IF EXISTS SinhVienDb;

CREATE DATABASE SinhVienDb;

USE SinhVienDb;

-- Tạo bảng Nganh
CREATE TABLE Nganh (
    MaNganh VARCHAR(10) PRIMARY KEY,
    TenNganh NVARCHAR(100) NOT NULL
);

-- Tạo bảng MonHoc
CREATE TABLE MonHoc (
    MaMonHoc VARCHAR(10) PRIMARY KEY,
    TenMonHoc NVARCHAR(100) NOT NULL,
    SoTinChi INT NOT NULL,
    MaNganh VARCHAR(10) NOT NULL,
    CONSTRAINT FK_MonHoc_Nganh FOREIGN KEY (MaNganh) REFERENCES Nganh(MaNganh)
);

-- Tạo bảng SinhVien
CREATE TABLE SinhVien (
    MaSinhVien VARCHAR(10) PRIMARY KEY,
    MaNganh VARCHAR(10) NOT NULL,
    TenSinhVien NVARCHAR(100) NOT NULL,
    NgaySinh DATE NOT NULL,
    GioiTinh VARCHAR(4) NOT NULL,
    SoDienThoai VARCHAR(11),
    Email VARCHAR(100),
    Khoa VARCHAR(100),
    CONSTRAINT FK_SinhVien_Nganh FOREIGN KEY (MaNganh) REFERENCES Nganh(MaNganh)
);

-- Tạo bảng DangKi
CREATE TABLE DangKi (
    MaSinhVien VARCHAR(10) NOT NULL,
    MaMonHoc VARCHAR(10) NOT NULL,
    SoTinChi INT NOT NULL,
    CONSTRAINT PK_DangKi PRIMARY KEY (MaSinhVien, MaMonHoc),
    CONSTRAINT FK_DangKi_SinhVien FOREIGN KEY (MaSinhVien) REFERENCES SinhVien(MaSinhVien),
    CONSTRAINT FK_DangKi_MonHoc FOREIGN KEY (MaMonHoc) REFERENCES MonHoc(MaMonHoc)
);

-- Tạo bảng Diem
CREATE TABLE Diem (
    MaSinhVien VARCHAR(10) NOT NULL,
    MaMonHoc VARCHAR(10) NOT NULL,
    Diem FLOAT NOT NULL,
    CONSTRAINT PK_Diem PRIMARY KEY (MaSinhVien, MaMonHoc),
    CONSTRAINT FK_Diem_SinhVien FOREIGN KEY (MaSinhVien) REFERENCES SinhVien(MaSinhVien),
    CONSTRAINT FK_Diem_MonHoc FOREIGN KEY (MaMonHoc) REFERENCES MonHoc(MaMonHoc)
);

-- Tạo bảng AdminAccount
CREATE TABLE AdminAccount (
    id INT PRIMARY KEY,
    username VARCHAR(10),
    password VARCHAR(10)
);


select * from Nganh;

delete from Nganh

select * from SinhVien;

select * from Diem;

delete  from SinhVien where MaSinhVien = 'SV001';
delete from Diem where MaSinhVien ='SV001';
DELETE FROM DangKi WHERE MaSinhVien = 'SV001'


--Thêm dữ liệu

insert into AdminAccount values(1,'1','1');

INSERT INTO Nganh(MaNganh, TenNganh) VALUES
('CNTT', N'Công nghệ thông tin'),
('KinhTe', N'Kinh tế'),
('NgoaiNgu', N'Ngoại ngữ');

INSERT INTO MonHoc(MaMonHoc, TenMonHoc, SoTinChi, MaNganh) VALUES
('INT1001', N'Lập trình căn bản', 3, 'CNTT'),
('INT1002', N'Cấu trúc dữ liệu và giải thuật', 4, 'CNTT'),
('ECO1001', N'Kinh tế học đại cương', 3, 'KinhTe'),
('ECO1002', N'Quản trị kinh doanh', 4, 'KinhTe'),
('ENG1001', N'Tiếng Anh cơ bản', 2, 'NgoaiNgu'),
('ENG1002', N'Tiếng Anh giao tiếp', 3, 'NgoaiNgu');

insert into SinhVien values ('s1','CNTT','2','1-10-2022','3','4','312','12');

INSERT INTO SinhVien(MaSinhVien, TenSinhVien, NgaySinh, GioiTinh, MaNganh) VALUES
('SV001', N'Nguyễn Văn A', '2000-01-01', 'Nam', 'CNTT'),
('SV002', N'Phạm Thị B', '2000-02-02', 'Nu', 'KinhTe'),
('SV003', N'Trần Văn C', '2000-03-03', 'Nu', 'NgoaiNgu');

INSERT INTO LopHoc(MaLopHoc, TenLopHoc, MaMonHoc, MaNganh) VALUES
('LT001', N'Lập trình căn bản A', 'INT1001', 'CNTT'),
('LT002', N'Cấu trúc dữ liệu và giải thuật A', 'INT1002', 'CNTT'),
('KT001', N'Kinh tế học đại cương A', 'ECO1001', 'KinhTe'),
('KT002', N'Quản trị kinh doanh A', 'ECO1002', 'KinhTe'),
('NN001', N'Tiếng Anh cơ bản A', 'ENG1001', 'NgoaiNgu'),
('NN002', N'Tiếng Anh giao tiếp A', 'ENG1002', 'NgoaiNgu');

INSERT INTO DangKi(MaSinhVien, MaLopHoc, SoTinChi) VALUES
('SV001', 'LT001', 3),
('SV001', 'LT002', 4),
('SV002', 'KT001', 3),
('SV002', 'KT002', 4),
('SV003', 'NN001', 2),
('SV003', 'NN002', 3);

INSERT INTO Diem(MaSinhVien, MaMonHoc, Diem) VALUES
('SV001', 'INT1001', 8.5),
('SV001', 'INT1002', 9.0),
('SV002', 'ECO1001', 7.5),
('SV002', 'ECO1002', 8.0),
('SV003', 'ENG1001', 6.5),
('SV003', 'ENG1002', 7.0);


-- Test
SELECT SV.MaSinhVien, SUM(MH.SoTinChi) AS TongTinChi, SUM(MH.SoTinChi)*600 AS TienHocPhi
FROM SinhVien SV
JOIN DangKi DK ON SV.MaSinhVien = DK.MaSinhVien
JOIN LopHoc LH ON DK.MaLopHoc = LH.MaLopHoc
JOIN MonHoc MH ON LH.MaMonHoc = MH.MaMonHoc
WHERE SV.MaSinhVien = 'SV001' --thay MSV001 bằng mã sinh viên cần tìm
GROUP BY SV.MaSinhVien;

--Trigger
CREATE TRIGGER trgDeleteSinhVien
ON SinhVien
AFTER DELETE
AS
BEGIN
    -- Xoá các bản ghi trong bảng DangKi liên quan đến sinh viên đã bị xoá
    DELETE FROM DangKi WHERE MaSinhVien IN (SELECT MaSinhVien FROM deleted)

    -- Xoá các bản ghi trong bảng Diem liên quan đến sinh viên đã bị xoá
    DELETE FROM Diem WHERE MaSinhVien IN (SELECT MaSinhVien FROM deleted)
END
