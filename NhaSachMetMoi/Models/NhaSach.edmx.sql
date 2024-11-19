
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/04/2024 21:51:35
-- Generated from EDMX file: D:\NhaSachMetMoi\NhaSachMetMoi\Models\NhaSach.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [NhaSach];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ChiTietDH_DonHang]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietDH] DROP CONSTRAINT [FK_ChiTietDH_DonHang];
GO
IF OBJECT_ID(N'[dbo].[FK_ChiTietDH_Sach1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietDH] DROP CONSTRAINT [FK_ChiTietDH_Sach1];
GO
IF OBJECT_ID(N'[dbo].[FK_DonHang_KH]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DonHang] DROP CONSTRAINT [FK_DonHang_KH];
GO
IF OBJECT_ID(N'[dbo].[FK_Sach_NXB1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sach] DROP CONSTRAINT [FK_Sach_NXB1];
GO
IF OBJECT_ID(N'[dbo].[FK_Sach_TG1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sach] DROP CONSTRAINT [FK_Sach_TG1];
GO
IF OBJECT_ID(N'[dbo].[FK_Sach_TL1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sach] DROP CONSTRAINT [FK_Sach_TL1];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Admin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Admin];
GO
IF OBJECT_ID(N'[dbo].[ChiTietDH]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChiTietDH];
GO
IF OBJECT_ID(N'[dbo].[DonHang]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DonHang];
GO
IF OBJECT_ID(N'[dbo].[KH]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KH];
GO
IF OBJECT_ID(N'[dbo].[MaGiamGia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaGiamGia];
GO
IF OBJECT_ID(N'[dbo].[NXB]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NXB];
GO
IF OBJECT_ID(N'[dbo].[Sach]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sach];
GO
IF OBJECT_ID(N'[dbo].[TG]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TG];
GO
IF OBJECT_ID(N'[dbo].[TL]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TL];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ChiTietDHs'
CREATE TABLE [dbo].[ChiTietDHs] (
    [MaDH] int  NOT NULL,
    [MaSach] int  NOT NULL,
    [SoLuong] int  NULL,
    [Gia] decimal(18,0)  NULL,
    [TenSach] nvarchar(255)  NULL,
    [AnhBia] nvarchar(255)  NULL,
    [GiaBan] decimal(18,2)  NULL,
    [Sale] int  NULL
);
GO

-- Creating table 'DonHangs'
CREATE TABLE [dbo].[DonHangs] (
    [MaDH] int IDENTITY(1,1) NOT NULL,
    [NgayDat] datetime  NULL,
    [NgayGiao] datetime  NULL,
    [MaKH] int  NULL,
    [TongTien] int  NOT NULL,
    [MaGiamGia] int  NULL
);
GO

-- Creating table 'KHs'
CREATE TABLE [dbo].[KHs] (
    [MaKH] int IDENTITY(1,1) NOT NULL,
    [HoTen] nvarchar(50)  NULL,
    [TaiKhoan] nvarchar(50)  NULL,
    [MatKhau] varchar(50)  NULL,
    [Email] varchar(100)  NULL,
    [DiaChi] nvarchar(500)  NULL,
    [DienThoai] varchar(11)  NULL,
    [NgaySinh] datetime  NULL
);
GO

-- Creating table 'NXBs'
CREATE TABLE [dbo].[NXBs] (
    [MaNXB] int IDENTITY(1,1) NOT NULL,
    [TenNXB] nvarchar(50)  NULL
);
GO

-- Creating table 'Saches'
CREATE TABLE [dbo].[Saches] (
    [MaSach] int IDENTITY(1,1) NOT NULL,
    [TenSach] nvarchar(100)  NULL,
    [Gia] decimal(18,0)  NULL,
    [KichThuoc] varchar(20)  NULL,
    [MoTa] nvarchar(max)  NULL,
    [AnhBia] nvarchar(50)  NULL,
    [NamXB] int  NULL,
    [SoLuongTon] int  NULL,
    [MaTL] int  NULL,
    [MaTG] int  NULL,
    [MaNXB] int  NULL,
    [Sale] int  NULL
);
GO

-- Creating table 'TGs'
CREATE TABLE [dbo].[TGs] (
    [MaTG] int IDENTITY(1,1) NOT NULL,
    [TenTG] nvarchar(50)  NULL
);
GO

-- Creating table 'TLs'
CREATE TABLE [dbo].[TLs] (
    [MaTL] int IDENTITY(1,1) NOT NULL,
    [TenTL] nvarchar(50)  NULL
);
GO

-- Creating table 'Admins'
CREATE TABLE [dbo].[Admins] (
    [TaiKhoan] char(50)  NULL,
    [MatKhau] char(50)  NULL,
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'MaGiamGias'
CREATE TABLE [dbo].[MaGiamGias] (
    [id] int IDENTITY(1,1) NOT NULL,
    [ma] nvarchar(50)  NULL,
    [giam] decimal(5,2)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MaDH], [MaSach] in table 'ChiTietDHs'
ALTER TABLE [dbo].[ChiTietDHs]
ADD CONSTRAINT [PK_ChiTietDHs]
    PRIMARY KEY CLUSTERED ([MaDH], [MaSach] ASC);
GO

-- Creating primary key on [MaDH] in table 'DonHangs'
ALTER TABLE [dbo].[DonHangs]
ADD CONSTRAINT [PK_DonHangs]
    PRIMARY KEY CLUSTERED ([MaDH] ASC);
GO

-- Creating primary key on [MaKH] in table 'KHs'
ALTER TABLE [dbo].[KHs]
ADD CONSTRAINT [PK_KHs]
    PRIMARY KEY CLUSTERED ([MaKH] ASC);
GO

-- Creating primary key on [MaNXB] in table 'NXBs'
ALTER TABLE [dbo].[NXBs]
ADD CONSTRAINT [PK_NXBs]
    PRIMARY KEY CLUSTERED ([MaNXB] ASC);
GO

-- Creating primary key on [MaSach] in table 'Saches'
ALTER TABLE [dbo].[Saches]
ADD CONSTRAINT [PK_Saches]
    PRIMARY KEY CLUSTERED ([MaSach] ASC);
GO

-- Creating primary key on [MaTG] in table 'TGs'
ALTER TABLE [dbo].[TGs]
ADD CONSTRAINT [PK_TGs]
    PRIMARY KEY CLUSTERED ([MaTG] ASC);
GO

-- Creating primary key on [MaTL] in table 'TLs'
ALTER TABLE [dbo].[TLs]
ADD CONSTRAINT [PK_TLs]
    PRIMARY KEY CLUSTERED ([MaTL] ASC);
GO

-- Creating primary key on [Id] in table 'Admins'
ALTER TABLE [dbo].[Admins]
ADD CONSTRAINT [PK_Admins]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'MaGiamGias'
ALTER TABLE [dbo].[MaGiamGias]
ADD CONSTRAINT [PK_MaGiamGias]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MaDH] in table 'ChiTietDHs'
ALTER TABLE [dbo].[ChiTietDHs]
ADD CONSTRAINT [FK_ChiTietDH_DonHang]
    FOREIGN KEY ([MaDH])
    REFERENCES [dbo].[DonHangs]
        ([MaDH])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [MaSach] in table 'ChiTietDHs'
ALTER TABLE [dbo].[ChiTietDHs]
ADD CONSTRAINT [FK_ChiTietDH_Sach1]
    FOREIGN KEY ([MaSach])
    REFERENCES [dbo].[Saches]
        ([MaSach])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChiTietDH_Sach1'
CREATE INDEX [IX_FK_ChiTietDH_Sach1]
ON [dbo].[ChiTietDHs]
    ([MaSach]);
GO

-- Creating foreign key on [MaKH] in table 'DonHangs'
ALTER TABLE [dbo].[DonHangs]
ADD CONSTRAINT [FK_DonHang_KH]
    FOREIGN KEY ([MaKH])
    REFERENCES [dbo].[KHs]
        ([MaKH])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DonHang_KH'
CREATE INDEX [IX_FK_DonHang_KH]
ON [dbo].[DonHangs]
    ([MaKH]);
GO

-- Creating foreign key on [MaNXB] in table 'Saches'
ALTER TABLE [dbo].[Saches]
ADD CONSTRAINT [FK_Sach_NXB1]
    FOREIGN KEY ([MaNXB])
    REFERENCES [dbo].[NXBs]
        ([MaNXB])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Sach_NXB1'
CREATE INDEX [IX_FK_Sach_NXB1]
ON [dbo].[Saches]
    ([MaNXB]);
GO

-- Creating foreign key on [MaTG] in table 'Saches'
ALTER TABLE [dbo].[Saches]
ADD CONSTRAINT [FK_Sach_TG1]
    FOREIGN KEY ([MaTG])
    REFERENCES [dbo].[TGs]
        ([MaTG])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Sach_TG1'
CREATE INDEX [IX_FK_Sach_TG1]
ON [dbo].[Saches]
    ([MaTG]);
GO

-- Creating foreign key on [MaTL] in table 'Saches'
ALTER TABLE [dbo].[Saches]
ADD CONSTRAINT [FK_Sach_TL1]
    FOREIGN KEY ([MaTL])
    REFERENCES [dbo].[TLs]
        ([MaTL])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Sach_TL1'
CREATE INDEX [IX_FK_Sach_TL1]
ON [dbo].[Saches]
    ([MaTL]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------