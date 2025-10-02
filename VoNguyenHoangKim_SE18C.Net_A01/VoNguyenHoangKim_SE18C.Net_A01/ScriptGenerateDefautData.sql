-- ===========================
-- Chọn Database
-- ===========================
USE FUNewsManagement;
GO

-- ===========================
-- Script tạo dữ liệu mẫu
-- ===========================

-- ===========================
-- Bảng Categories
-- ===========================
INSERT INTO [dbo].[Categories] ([Id], [Name], [Description], [Status])
VALUES
('CATEGORY_00000001', N'Chính trị', N'Tin tức chính trị trong nước và quốc tế', 1),
('CATEGORY_00000002', N'Kinh tế', N'Tin tức kinh tế, tài chính và thị trường', 1),
('CATEGORY_00000003', N'Công nghệ', N'Tin tức công nghệ và khởi nghiệp', 1),
('CATEGORY_00000004', N'Sức khỏe', N'Tin tức y tế và chăm sóc sức khỏe', 1),
('CATEGORY_00000005', N'Giáo dục', N'Tin tức giáo dục, đào tạo và nghiên cứu', 1),
('CATEGORY_00000006', N'Thể thao', N'Tin tức thể thao trong và ngoài nước', 1),
('CATEGORY_00000007', N'Giải trí', N'Giải trí, phim ảnh, âm nhạc', 1),
('CATEGORY_00000008', N'Khoa học', N'Khám phá khoa học và nghiên cứu mới', 1),
('CATEGORY_00000009', N'Du lịch', N'Du lịch, khám phá địa điểm và văn hóa', 1),
('CATEGORY_00000010', N'Môi trường', N'Tin tức môi trường và biến đổi khí hậu', 1),
('CATEGORY_00000011', N'Thời trang', N'Thời trang, làm đẹp và phong cách', 1),
('CATEGORY_00000012', N'Ẩm thực', N'Ẩm thực, món ngon và công thức nấu ăn', 1),
('CATEGORY_00000013', N'Ô tô - Xe máy', N'Xe cộ, ô tô, xe máy và công nghệ vận tải', 1),
('CATEGORY_00000014', N'Bất động sản', N'Bất động sản, dự án và thị trường nhà đất', 1),
('CATEGORY_00000015', N'Văn hóa', N'Văn hóa, lễ hội và truyền thống địa phương', 1),
('CATEGORY_00000016', N'Ý kiến', N'Ý kiến, bình luận và phân tích', 1),
('CATEGORY_00000017', N'Phong cách sống', N'Phong cách sống, mẹo vặt, sức khỏe tinh thần', 1),
('CATEGORY_00000018', N'Doanh nghiệp', N'Doanh nghiệp, start-up và kinh doanh', 1),
('CATEGORY_00000019', N'Tin quốc tế', N'Tin tức quốc tế và sự kiện toàn cầu', 1),
('CATEGORY_00000020', N'Tin địa phương', N'Tin tức địa phương và sự kiện quanh bạn', 1);
GO

-- ===========================
-- Bảng NewsArticles
-- ===========================
INSERT INTO [dbo].[NewsArticles] 
([Id], [Title], [Headline], [Content], [NewsSource], [CreatedDate], [ModifiedDate], [Status], [CategoryId], [AccountId], [UpdatedById])
VALUES
('ARTICLE_00000001', N'Tin Công Nghệ 1', N'Khởi nghiệp và công nghệ', N'Nội dung bài viết 1...', N'FU News', GETDATE(), NULL, 1, 'CATEGORY_00000003', 'ACCOUNT_00000001', NULL),
('ARTICLE_00000002', N'Tin Kinh Tế 2', N'Thị trường và doanh nghiệp', N'Nội dung bài viết 2...', N'FU News', GETDATE(), NULL, 1, 'CATEGORY_00000002', 'ACCOUNT_00000002', NULL),
('ARTICLE_00000003', N'Tin Thể Thao 3', N'Bóng đá và các môn thể thao', N'Nội dung bài viết 3...', N'FU News', GETDATE(), NULL, 1, 'CATEGORY_00000006', 'ACCOUNT_00000003', NULL),
('ARTICLE_00000004', N'Tin Sức Khỏe 4', N'Tin tức y tế', N'Nội dung bài viết 4...', N'FU News', GETDATE(), NULL, 1, 'CATEGORY_00000004', 'ACCOUNT_00000004', NULL),
('ARTICLE_00000005', N'Tin Giáo Dục 5', N'Đào tạo và nghiên cứu', N'Nội dung bài viết 5...', N'FU News', GETDATE(), NULL, 1, 'CATEGORY_00000005', 'ACCOUNT_00000001', NULL),
('ARTICLE_00000006', N'Tin Giải Trí 6', N'Phim ảnh và âm nhạc', N'Nội dung bài viết 6...', N'FU News', GETDATE(), NULL, 1, 'CATEGORY_00000007', 'ACCOUNT_00000002', NULL),
('ARTICLE_00000007', N'Tin Du Lịch 7', N'Khám phá địa điểm', N'Nội dung bài viết 7...', N'FU News', GETDATE(), NULL, 1, 'CATEGORY_00000009', 'ACCOUNT_00000003', NULL),
('ARTICLE_00000008', N'Tin Văn Hóa 8', N'Lễ hội và truyền thống', N'Nội dung bài viết 8...', N'FU News', GETDATE(), NULL, 1, 'CATEGORY_00000015', 'ACCOUNT_00000004', NULL),
('ARTICLE_00000009', N'Tin Thời Trang 9', N'Mốt và phong cách', N'Nội dung bài viết 9...', N'FU News', GETDATE(), NULL, 1, 'CATEGORY_00000011', 'ACCOUNT_00000001', NULL),
('ARTICLE_00000010', N'Tin Ẩm Thực 10', N'Món ngon và nấu ăn', N'Nội dung bài viết 10...', N'FU News', GETDATE(), NULL, 1, 'CATEGORY_00000012', 'ACCOUNT_00000002', NULL);
GO

-- ===========================
-- Bảng Tags
-- ===========================
INSERT INTO [dbo].[Tags] ([Id], [Name])
VALUES
('TAG_00000001', N'Tin tức'),
('TAG_00000002', N'Chính trị'),
('TAG_00000003', N'Kinh tế'),
('TAG_00000004', N'Công nghệ'),
('TAG_00000005', N'Sức khỏe'),
('TAG_00000006', N'Giáo dục'),
('TAG_00000007', N'Thể thao'),
('TAG_00000008', N'Giải trí'),
('TAG_00000009', N'Khoa học'),
('TAG_00000010', N'Du lịch'),
('TAG_00000011', N'Môi trường'),
('TAG_00000012', N'Thời trang'),
('TAG_00000013', N'Ẩm thực'),
('TAG_00000014', N'Ô tô - Xe máy'),
('TAG_00000015', N'Bất động sản'),
('TAG_00000016', N'Văn hóa'),
('TAG_00000017', N'Ý kiến'),
('TAG_00000018', N'Phong cách sống'),
('TAG_00000019', N'Doanh nghiệp'),
('TAG_00000020', N'Tin quốc tế');
GO
