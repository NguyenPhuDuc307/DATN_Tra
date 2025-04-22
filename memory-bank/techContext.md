# Bối cảnh Kỹ thuật

## Stack công nghệ

- **Backend Framework**: ASP.NET Core 10.0 (Preview)
- **Frontend**:
  - MVC với Razor Views
  - JavaScript/jQuery
  - Bootstrap 5
- **Database**:
  - SQLite (cho môi trường phát triển)
  - SQL Server (cho môi trường sản xuất)
  - Entity Framework Core 10.0
- **Authentication & Authorization**:
  - ASP.NET Core Identity
  - Role-based authorization
- **Deployment**:
  - Docker (tương lai)
  - Azure DevOps (CI/CD) (tương lai)

## Công cụ phát triển

- **IDE**: Visual Studio 2022 hoặc Visual Studio Code
- **Source Control**: Git/GitHub
- **Package Management**: NuGet
- **Project Management**: Azure DevOps/Jira
- **Documentation**: Swagger/OpenAPI

## Yêu cầu kỹ thuật

### Hiệu suất cơ bản

- Đáp ứng được nhu cầu sử dụng hàng ngày của người dùng
- Thời gian phản hồi chấp nhận được cho các thao tác thông thường
- Khả năng xử lý đủ lượng người dùng của công ty

### Bảo mật cơ bản

- Xác thực người dùng bằng Identity
- Role-based access control (Admin, Accountant, Sales, Manager)
- Bảo vệ chống SQL injection thông qua Entity Framework
- Bảo vệ chống CSRF attacks
- Sử dụng HTTPS

### Khả năng mở rộng

- Thiết kế linh hoạt để có thể mở rộng trong tương lai
- Database có khả năng mở rộng khi cần
- Có khả năng thêm module mới khi cần thiết

### Tính khả dụng

- Hoạt động ổn định trong giờ làm việc
- Backup dữ liệu định kỳ
- Quy trình khôi phục dữ liệu

## Ràng buộc kỹ thuật

- Hỗ trợ các trình duyệt hiện đại (Chrome, Firefox, Safari, Edge)
- Responsive design cho desktop và tablet
- Tương thích với các chuẩn kế toán Việt Nam
- Ưu tiên tốc độ phát triển và dễ bảo trì hơn là hiệu suất
- Sử dụng Entity Framework Core với Code First approach
- Tất cả các model class phải có constructor với giá trị mặc định hợp lý
- Tất cả các trường NOT NULL phải được khởi tạo trong constructor hoặc khi tạo đối tượng

## Thay đổi kỹ thuật

- **Thay đổi từ .NET 9.0 sang .NET 10.0** (Tháng 5/2025):
  - Lý do: Gặp lỗi với StaticWebAssets và Identity UI trong .NET 9.0
  - Ưu điểm: Tiếp cận phiên bản mới nhất, có nhiều cải tiến về hiệu suất
  - Rủi ro: Vẫn đang trong giai đoạn preview, cần thận trọng
- **Chuyển từ Razor Pages sang MVC** (Tháng 5/2025):
  - Lý do: Cấu trúc code rõ ràng hơn, dễ quản lý với dự án phức tạp
  - Ưu điểm: Phân tách rõ ràng giữa Model, View và Controller
  - Thách thức: Cần viết nhiều code hơn so với Razor Pages
- **Chuyển từ SQL Server sang SQLite cho môi trường phát triển** (Tháng 5/2025):
  - Lý do: Đơn giản hóa môi trường phát triển, không cần cài đặt SQL Server
  - Ưu điểm: Nhẹ, dễ thiết lập, dễ backup và khôi phục
  - Thách thức: Khác biệt nhỏ giữa SQLite và SQL Server có thể dẫn đến vấn đề khi triển khai
- **Cải thiện mô hình dữ liệu với Non-Nullable Reference Types** (Tháng 5/2025):
  - Lý do: Tăng cường tính chính xác và an toàn dữ liệu
  - Ưu điểm: Giảm thiểu lỗi null reference, cung cấp các cảnh báo khi biên dịch
  - Thách thức: Cần cập nhật nhiều model class và xử lý đúng các navigation properties

## Công nghệ được sử dụng

### Entity Framework Core

- Sử dụng Code First approach
- Migrations để quản lý thay đổi cơ sở dữ liệu
- Eager loading để tối ưu truy vấn (Include)
- Sử dụng Fluent API để cấu hình model
- Sử dụng seeding để tạo dữ liệu ban đầu

### ASP.NET Core MVC

- Controllers với CRUD operations
- ViewModels để truyền dữ liệu từ Controller sang View
- Razor Views với các partial views cho các thành phần tái sử dụng
- Tag Helpers để tạo UI và forms
- Data Annotations cho validation

### Bootstrap 5

- Grid system cho responsive design
- Components (cards, tables, forms, etc.)
- Utilities cho styling
- Modal dialogs cho các thao tác nhanh
- Tooltips và popovers

### jQuery

- Ajax calls để tải dữ liệu không đồng bộ
- DOM manipulation
- Event handling
- Form validation

## Tích hợp hệ thống

- API RESTful cho tích hợp với hệ thống khác
- Khả năng xuất/nhập dữ liệu Excel, CSV, PDF
- Tích hợp với hệ thống ngân hàng (tương lai)
- Tích hợp với hệ thống hóa đơn điện tử (VNPT/Viettel)

## Môi trường triển khai

- **Development**: Local workstations với SQLite
- **Testing**: Azure Web App với SQL Server (tương lai)
- **Production**: Azure Web App/VM hoặc on-premise server với SQL Server (tương lai)

## Kế hoạch kỹ thuật

1. **Giai đoạn 1**: Xây dựng core infrastructure (Đã hoàn thành)

   - Database schema
   - Identity authentication system
   - Base UI components

2. **Giai đoạn 2**: Phát triển các module chính (Đang tiến hành)

   - Quản lý sản phẩm và danh mục (Đã hoàn thành)
   - Quản lý khách hàng và nhà cung cấp (Đã hoàn thành)
   - Quản lý tồn kho (Đang tiến hành)
   - Quản lý bán hàng (Lên kế hoạch)
   - Quản lý mua hàng (Lên kế hoạch)
   - Quản lý công nợ (Lên kế hoạch)

3. **Giai đoạn 3**: Báo cáo và phân tích (Chưa bắt đầu)

   - Báo cáo tài chính
   - Dashboard analytics
   - Export/Import functionality

4. **Giai đoạn 4**: Hoàn thiện và triển khai (Chưa bắt đầu)
   - Tinh chỉnh UI/UX
   - Triển khai lên môi trường sản xuất
   - Training người dùng

## Các bài học kỹ thuật đã rút ra

1. **Non-Nullable Reference Types**:

   - Cần khởi tạo tất cả các thuộc tính non-nullable trong constructor
   - Với navigation properties non-nullable, cần sử dụng `= null!` để đánh dấu chúng sẽ được khởi tạo bởi EF Core
   - Sử dụng navigation properties nullable (`?`) cho các mối quan hệ không bắt buộc

2. **Seeding Data**:

   - Tất cả các trường NOT NULL trong database phải được gán giá trị rõ ràng khi seed
   - Đặc biệt cẩn thận với các trường string không được phép null như `UpdatedBy`
   - Kiểm tra kỹ model và migration trước khi áp dụng

3. **Entity Framework Relationships**:
   - Cần cấu hình rõ ràng các cascade delete behavior
   - Sử dụng Fluent API để định nghĩa các mối quan hệ phức tạp
   - Thiết lập các foreign key constraints phù hợp với logic nghiệp vụ
