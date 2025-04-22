# Mẫu Thiết kế & Kiến trúc Hệ thống

## Kiến trúc tổng thể

Phần mềm kế toán bán hàng được xây dựng dựa trên kiến trúc web sử dụng mô hình MVC (Model-View-Controller) với ASP.NET Core 10.0. Kiến trúc này đảm bảo tính tách biệt rõ ràng giữa dữ liệu, giao diện người dùng và logic xử lý.

```
+----------------+     +----------------+     +----------------+
|                |     |                |     |                |
|     Model      |<--->|   Controller   |<--->|      View      |
|                |     |                |     |                |
+----------------+     +----------------+     +----------------+
        ^                      ^                     ^
        |                      |                     |
        v                      v                     v
+----------------+     +----------------+     +----------------+
|                |     |                |     |                |
|  DbContext     |     |   ViewModels   |     |   Partial     |
|                |     |                |     |    Views      |
+----------------+     +----------------+     +----------------+
        ^
        |
        v
+----------------+
|                |
|    Database    |
|   (SQLite/SQL) |
+----------------+
```

## Các thành phần chính

### 1. Model

- Đại diện cho dữ liệu và cấu trúc dữ liệu
- Các entity chính: Customer, Supplier, Product, ProductCategory, SalesOrder, PurchaseOrder, Invoice, Payment, InventoryMovement
- Entity Framework Core 10.0 được sử dụng cho ORM
- Code First Approach với Data Annotations và Fluent API

### 2. View

- Giao diện người dùng (UI) sử dụng Razor Views
- Responsive design với Bootstrap 5
- Partial views cho các thành phần tái sử dụng
- Client-side validation với jQuery và unobtrusive validation
- Tag Helpers để tạo UI và forms

### 3. Controller

- Xử lý các request từ client
- Thực hiện logic nghiệp vụ
- Tương tác với DbContext để thao tác dữ liệu
- Chuẩn bị và chuyển ViewModels cho Views
- Quản lý xác thực và phân quyền

### 4. ViewModels

- Chứa các thuộc tính cần thiết cho views
- Validation thông qua Data Annotations
- Tách biệt dữ liệu hiển thị với model dữ liệu cơ sở

### 5. DbContext

- ApplicationDbContext kế thừa từ DbContext của Entity Framework Core
- Định nghĩa các DbSet cho các entity
- Cấu hình relationship và constraints
- Migration để quản lý thay đổi schema

### 6. Database

- SQLite cho môi trường phát triển
- SQL Server cho môi trường sản xuất
- Cấu trúc bảng theo thiết kế từ ApplicationDbContext và migrations

## Mẫu thiết kế

1. **MVC Pattern**

   - Tách biệt rõ ràng giữa Model, View và Controller
   - Controller điều hướng luồng dữ liệu và logic
   - View chỉ hiển thị dữ liệu, không chứa logic phức tạp

2. **Repository Pattern (đơn giản hóa)**

   - Sử dụng DbContext trực tiếp trong Controller
   - Tận dụng các tính năng của Entity Framework Core
   - Không sử dụng UnitOfWork để đơn giản hóa

3. **Dependency Injection**

   - ASP.NET Core DI container
   - Đăng ký và tiêm DbContext và các service khác
   - Dễ dàng thay thế các thành phần cho testing

4. **Data Annotations & Fluent API**

   - Sử dụng Data Annotations cho validation và metadata
   - Fluent API cho cấu hình phức tạp
   - Định nghĩa ràng buộc và quan hệ giữa các entity

## Luồng dữ liệu

1. Client gửi request đến server
2. Routing định tuyến request đến Controller và Action phù hợp
3. Controller xử lý request, tương tác với Model để lấy/cập nhật dữ liệu
4. Controller chuẩn bị ViewModel phù hợp
5. Controller trả về View tương ứng với ViewModel
6. View render HTML và gửi response về client

## Cơ chế xác thực và phân quyền

- ASP.NET Core Identity cho xác thực người dùng
- Role-based authorization với các vai trò: Admin, Accountant, Sales, Manager
- Policy-based authorization cho các quyền chi tiết
- Sử dụng Authorize attribute trên Controller/Action
- Quản lý JWT token cho API authentication (tương lai)

## Xử lý dữ liệu

1. **Eager Loading**

   - Sử dụng Include() cho các related entities cần thiết
   - Tránh N+1 query problem

2. **Lazy Loading**

   - Tránh sử dụng khi không cần thiết để giảm complexity

3. **Explicit Loading**

   - Sử dụng Entry().Collection().Load() khi cần

4. **Transactions**
   - Sử dụng DbContext.Database.BeginTransaction() cho các thao tác phức tạp
   - Đảm bảo tính toàn vẹn dữ liệu

## Ưu điểm của kiến trúc

- **Rõ ràng**: Phân tách trách nhiệm giữa các thành phần
- **Dễ bảo trì**: Thay đổi một thành phần không ảnh hưởng đến các thành phần khác
- **Dễ mở rộng**: Có thể thêm chức năng mới một cách dễ dàng
- **Dễ test**: Các thành phần tách biệt dễ viết unit test
- **Quen thuộc**: Mô hình MVC phổ biến, dễ dàng onboard developer mới

## Hướng dẫn triển khai

1. Tất cả controllers nên kế thừa từ Controller base class
2. Sử dụng async/await cho các thao tác I/O
3. ViewModels nên chỉ chứa dữ liệu cần thiết cho view
4. Sử dụng partial views cho các thành phần UI tái sử dụng
5. Sử dụng Data Annotations cho validation
6. Sử dụng Include() để tránh N+1 query problem
7. Sử dụng transactions cho các thao tác phức tạp
