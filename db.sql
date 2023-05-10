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
    DiemChuyenCan FLOAT NOT NULL,
	DiemGiuaKy FLOAT NOT NULL,
	DiemCuoiKy FLOAT NOT NULL,
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

INSERT INTO DangKi(MaSinhVien, MaMonHoc, SoTinChi) Values
('SV001', 'INT1001', 3),
('SV001', 'INT1002', 4),
('SV002', 'ECO1001', 3),
('SV002', 'ECO1002', 4),
('SV003', 'ENG1001', 2),
('SV003', 'ENG1002', 3)



INSERT INTO SinhVien(MaSinhVien, TenSinhVien, NgaySinh, GioiTinh, MaNganh) VALUES
('SV001', N'Nguyễn Văn A', '2000-01-01', 'Nam', 'CNTT'),
('SV002', N'Phạm Thị B', '2000-02-02', 'Nu', 'KinhTe'),
('SV003', N'Trần Văn C', '2000-03-03', 'Nu', 'NgoaiNgu');

INSERT INTO Diem(MaSinhVien, MaMonHoc, DiemChuyenCan, DiemGiuaKy, DiemCuoiKy) VALUES
('SV001', 'INT1001', 9.0, 8.5, 7.0),
('SV001', 'INT1002', 8.0, 7.5, 8.0),
('SV002', 'ECO1001', 8.5, 7.0, 9.0),
('SV002', 'ECO1002', 9.0, 8.0, 8.5),
('SV003', 'ENG1001', 8.0, 7.5, 7.0),
('SV003', 'ENG1002', 9.0, 9.0, 8.5);

select * from SinhVien;

update SinhVien set TenSinhVien = '12' where MaSinhVien = '1';



--Trigger
CREATE TRIGGER trgDeleteSinhVien
ON SinhVien
INSTEAD OF DELETE
AS
BEGIN
    -- Xoá các bản ghi trong bảng DangKi liên quan đến sinh viên đã bị xoá
    DELETE FROM DangKi WHERE MaSinhVien IN (SELECT MaSinhVien FROM deleted)

    -- Xoá các bản ghi trong bảng Diem liên quan đến sinh viên đã bị xoá
    DELETE FROM Diem WHERE MaSinhVien IN (SELECT MaSinhVien FROM deleted)
	
	-- Xoá các bản khi trong bảng SinhVien
	Delete from SinhVien where MaSinhVien in (select MaSinhVien from deleted)

END


CREATE TRIGGER trg_DeleteMonHoc
ON MonHoc
FOR DELETE
AS
BEGIN
    DELETE FROM Diem
    WHERE MaMonHoc IN (SELECT MaMonHoc FROM deleted)
END;







DELETE FROM Diem WHERE MaMonHoc = 'ECO1001';
Delete from DangKi where MaMonHoc = 'ECO1001';

DELETE FROM MonHoc WHERE MaMonHoc = 'ECO1002';




CREATE TRIGGER tr_DeleteMonHoc
ON MonHoc
INSTEAD OF DELETE
AS
BEGIN
    DELETE FROM Diem
    WHERE MaMonHoc IN (SELECT MaMonHoc FROM deleted);
    
    DELETE FROM DangKi
    WHERE MaMonHoc IN (SELECT MaMonHoc FROM deleted);

	delete from MonHoc
	where MaMonHoc in (select MaMonHoc from deleted);
END;





SELECT SinhVien.MaSinhVien, MonHoc.MaMonHoc, MonHoc.TenMonHoc, Diem.DiemChuyenCan, Diem.DiemGiuaKy, Diem.DiemCuoiKy, (Diem.DiemChuyenCan * 0.1 + Diem.DiemGiuaKy * 0.4 + Diem.DiemCuoiKy * 0.5) as TongKet
FROM SinhVien
INNER JOIN Diem ON SinhVien.MaSinhVien = Diem.MaSinhVien
INNER JOIN MonHoc ON Diem.MaMonHoc = MonHoc.MaMonHoc;


delete from SinhVien

delete from Diem

delete from MonHoc

select * from DangKi
select * from SinhVien
select * from Diem
select * from MonHoc

delete from SinhVien where MaSinhVien = 'SV001'