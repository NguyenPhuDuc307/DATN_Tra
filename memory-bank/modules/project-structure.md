# Cấu trúc Dự án

Dưới đây là cấu trúc hiện tại của dự án phần mềm kế toán bán hàng DEHA, sử dụng ASP.NET Core MVC với .NET 10:

```
DehaAccounting/
│
├── DehaAccountingMvc/                # Dự án ASP.NET Core MVC (.NET 10)
│   ├── Areas/                        # Areas cho Identity và các phân hệ khác
│   │   └── Identity/                 # Chức năng xác thực và quản lý người dùng
│   │
│   ├── Controllers/                  # Controllers xử lý request
│   │   ├── HomeController.cs         # Controller mặc định cho dashboard
│   │   ├── CustomersController.cs    # Controller quản lý khách hàng
│   │   ├── SuppliersController.cs    # Controller quản lý nhà cung cấp
│   │   ├── ProductsController.cs     # Controller quản lý sản phẩm
│   │   ├── ProductCategoriesController.cs # Controller quản lý danh mục sản phẩm
│   │   ├── InventoryMovementsController.cs # Controller quản lý nhập xuất kho
│   │   └── ... (sẽ bổ sung thêm)
│   │
│   ├── Data/                         # Lớp tương tác với database
│   │   ├── ApplicationDbContext.cs   # DbContext chính của ứng dụng
│   │   └── Migrations/               # EF Core migrations đã tạo
│   │
│   ├── Models/                       # Entity và View Models
│   │   ├── ErrorViewModel.cs         # Model cho trang lỗi
│   │   └── Accounting/               # Thư mục chứa các model kế toán
│   │       ├── Customer.cs           # Model khách hàng
│   │       ├── Supplier.cs           # Model nhà cung cấp
│   │       ├── Product.cs            # Model sản phẩm
│   │       ├── ProductCategory.cs    # Model danh mục sản phẩm
│   │       ├── SalesOrder.cs         # Model đơn bán hàng
│   │       ├── SalesOrderDetail.cs   # Model chi tiết đơn bán hàng
│   │       ├── PurchaseOrder.cs      # Model đơn mua hàng
│   │       ├── PurchaseOrderDetail.cs # Model chi tiết đơn mua hàng
│   │       ├── Invoice.cs            # Model hóa đơn
│   │       ├── InvoiceDetail.cs      # Model chi tiết hóa đơn
│   │       ├── Payment.cs            # Model thanh toán
│   │       └── InventoryMovement.cs  # Model quản lý nhập xuất kho
│   │
│   ├── Views/                        # Razor Views
│   │   ├── Home/                     # Views trang chủ và dashboard
│   │   │   ├── Index.cshtml          # Trang chủ/dashboard
│   │   │   └── Privacy.cshtml        # Trang privacy
│   │   ├── Products/                 # Views quản lý sản phẩm
│   │   ├── ProductCategories/        # Views quản lý danh mục sản phẩm
│   │   ├── InventoryMovements/       # Views quản lý nhập xuất kho
│   │   └── Shared/                   # Views dùng chung
│   │       ├── _Layout.cshtml        # Layout chính
│   │       └── ...
│   │
│   ├── wwwroot/                      # Static files
│   │   ├── css/                      # CSS files
│   │   ├── js/                       # JavaScript files
│   │   ├── lib/                      # Thư viện (Bootstrap, jQuery, etc.)
│   │   └── ...
│   │
│   ├── Properties/                   # Project properties
│   │   └── launchSettings.json       # Cấu hình khởi chạy
│   │
│   ├── appsettings.json              # Cấu hình ứng dụng
│   ├── appsettings.Development.json  # Cấu hình môi trường phát triển
│   ├── Program.cs                    # Entry point và cấu hình dịch vụ
│   ├── DehaAccounting.db             # File cơ sở dữ liệu SQLite
│   └── DehaAccountingMvc.csproj      # File project với target framework .NET 10
│
└── DehaAccountingApp/                # Dự án cũ (sẽ không sử dụng)
```

## Cấu hình hiện tại

Dự án hiện đang sử dụng:

- Framework: .NET 10.0 (Preview)
- Kiến trúc: ASP.NET Core MVC
- Xác thực: ASP.NET Core Identity với Individual user accounts
- Database: Entity Framework Core với SQLite (phát triển) / SQL Server (sản xuất)

## Các gói NuGet chính hiện có

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore" Version="10.0.0-preview.1.25120.3" />
  <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.0-preview.1.25120.3" />
  <PackageReference Include="Microsoft.AspNetCore.Identity.UI" Version="10.0.0-preview.1.25120.3" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.0-preview.1.25081.1">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.0-preview.1.25081.1" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.0-preview.1.25081.1" />
  <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="9.0.0" />
</ItemGroup>
```

## Tiến độ phát triển hiện tại

1. **Đã hoàn thành**:

   - Cấu trúc dự án cơ bản
   - Các model cơ sở đã tạo đầy đủ
   - Các controller chính: Customers, Suppliers, Products, ProductCategories, InventoryMovements
   - Migration cơ sở dữ liệu và thiết lập SQLite
   - Views cho Products, ProductCategories, và InventoryMovements

2. **Đang phát triển**:

   - Views cho Customers và Suppliers
   - Dashboard trên HomeController
   - Logic nghiệp vụ cho InventoryMovements

3. **Cần phát triển tiếp**:
   - Controllers và Views cho SalesOrders, PurchaseOrders, Invoices, và Payments
   - Báo cáo và thống kê
   - Phân quyền chi tiết

## Kế hoạch phát triển cấu trúc dự án

Dự án sẽ được mở rộng theo hướng:

1. **Cập nhật Views thiếu**:

   - Triển khai Views cho CustomersController
   - Triển khai Views cho SuppliersController
   - Cải thiện View Index.cshtml của HomeController thành Dashboard

2. **Phát triển Controllers và Views mới**:

   - Phát triển SalesOrdersController và Views liên quan
   - Phát triển PurchaseOrdersController và Views liên quan
   - Phát triển InvoicesController và Views liên quan
   - Phát triển PaymentsController và Views liên quan

3. **Báo cáo và thống kê**:

   - Phát triển ReportsController
   - Phát triển các View báo cáo bán hàng
   - Phát triển các View báo cáo tài chính
   - Phát triển các View báo cáo tồn kho

4. **Phân quyền và bảo mật**:
   - Mở rộng Identity với các role cụ thể (Admin, Accountant, Sales, Manager)
   - Áp dụng Attribute Authorization cho các controller và action

## Lưu ý phát triển

- **Entity Framework**: Đang sử dụng Code-First approach với migrations và SQLite
- **SQLite vs SQL Server**: Sử dụng SQLite cho môi trường phát triển, dự kiến chuyển sang SQL Server cho môi trường sản xuất
- **Validation**: Kết hợp server-side validation và client-side validation
- **Dashboard**: Cần phát triển dashboard hiển thị thông tin tổng quan về doanh số, số dư công nợ, tồn kho, và các KPI chính
- **NuGet Packages**: Cần cập nhật lên phiên bản mới nhất khi có điều kiện để đảm bảo tính ổn định
- **.NET 10 Preview**: Chú ý các vấn đề tương thích do sử dụng phiên bản preview

## Ưu điểm của cấu trúc này

- **Đơn giản, dễ hiểu**: Cấu trúc folder chuẩn của ASP.NET Core MVC
- **Tích hợp Identity**: ASP.NET Core Identity đã được tích hợp sẵn
- **SQLite cho phát triển**: Không cần cài đặt SQL Server, dễ dàng cho việc phát triển trên môi trường khác nhau
- **Phân chia rõ ràng**: Tuân thủ mô hình MVC với sự phân tách rõ ràng giữa dữ liệu, logic, và giao diện
- **Độc lập database**: Cấu hình EF Core cho phép dễ dàng chuyển đổi giữa SQLite và SQL Server

## Hướng dẫn phát triển

### Dashboard

- Cập nhật `HomeController.Index()` để hiển thị thông tin tổng quan về dữ liệu kế toán
- Sử dụng ViewBag hoặc ViewModel để truyền dữ liệu tổng hợp
- Thêm biểu đồ bằng Chart.js hoặc Google Charts
- Hiển thị KPI quan trọng: doanh số, lợi nhuận, tồn kho, công nợ

### Cập nhật Views cho Customers và Suppliers

- Tạo các file Views cho Customer theo mẫu CRUD:

  - Index.cshtml: Danh sách khách hàng
  - Create.cshtml: Tạo mới khách hàng
  - Edit.cshtml: Chỉnh sửa khách hàng
  - Details.cshtml: Chi tiết khách hàng
  - Delete.cshtml: Xác nhận xóa khách hàng

- Tạo các file Views cho Supplier tương tự như Customer

### Cập nhật NuGet Packages

- Thường xuyên kiểm tra các bản cập nhật của NuGet packages
- Ưu tiên cập nhật các package liên quan đến bảo mật
- Cẩn thận khi cập nhật packages giữa các phiên bản preview
- Sử dụng lệnh: `dotnet restore` sau khi cập nhật
