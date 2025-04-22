# Sơ đồ cơ sở dữ liệu phần mềm kế toán bán hàng

## Tổng quan

Cơ sở dữ liệu được thiết kế để hỗ trợ đầy đủ các chức năng kế toán bán hàng cho Công ty Cổ phần DEHA Việt Nam. Hệ thống được thiết kế theo mô hình quan hệ (RDBMS) với SQL Server làm nền tảng chính.

## Các thực thể chính

### 1. Quản lý người dùng

#### Users

- `UserId` (PK): Định danh người dùng
- `Username`: Tên đăng nhập
- `Email`: Email người dùng
- `PasswordHash`: Mật khẩu được mã hóa
- `FullName`: Họ tên đầy đủ
- `PhoneNumber`: Số điện thoại
- `IsActive`: Trạng thái hoạt động
- `CreatedDate`: Ngày tạo
- `LastLogin`: Lần đăng nhập cuối
- `DepartmentId` (FK): Phòng ban

#### Roles

- `RoleId` (PK): Định danh vai trò
- `RoleName`: Tên vai trò
- `Description`: Mô tả vai trò

#### UserRoles

- `UserRoleId` (PK): Định danh liên kết
- `UserId` (FK): Liên kết với Users
- `RoleId` (FK): Liên kết với Roles

#### Permissions

- `PermissionId` (PK): Định danh quyền
- `PermissionName`: Tên quyền
- `Description`: Mô tả quyền
- `ModuleId` (FK): Module chức năng

#### RolePermissions

- `RolePermissionId` (PK): Định danh liên kết
- `RoleId` (FK): Liên kết với Roles
- `PermissionId` (FK): Liên kết với Permissions

### 2. Quản lý khách hàng và nhà cung cấp

#### Customers

- `CustomerId` (PK): Định danh khách hàng
- `CustomerCode`: Mã khách hàng
- `CustomerName`: Tên khách hàng
- `ContactPerson`: Người liên hệ
- `PhoneNumber`: Số điện thoại
- `Email`: Email
- `Address`: Địa chỉ
- `TaxCode`: Mã số thuế
- `CustomerType`: Loại khách hàng
- `IsActive`: Trạng thái hoạt động
- `CreatedDate`: Ngày tạo
- `CreatedBy` (FK): Người tạo
- `CreditLimit`: Hạn mức tín dụng
- `PaymentTerms`: Điều khoản thanh toán

#### Suppliers

- `SupplierId` (PK): Định danh nhà cung cấp
- `SupplierCode`: Mã nhà cung cấp
- `SupplierName`: Tên nhà cung cấp
- `ContactPerson`: Người liên hệ
- `PhoneNumber`: Số điện thoại
- `Email`: Email
- `Address`: Địa chỉ
- `TaxCode`: Mã số thuế
- `IsActive`: Trạng thái hoạt động
- `CreatedDate`: Ngày tạo
- `CreatedBy` (FK): Người tạo
- `PaymentTerms`: Điều khoản thanh toán

### 3. Quản lý sản phẩm và dịch vụ

#### ProductCategories

- `CategoryId` (PK): Định danh loại sản phẩm
- `CategoryName`: Tên loại sản phẩm
- `Description`: Mô tả
- `ParentCategoryId` (FK): Loại cha

#### Products

- `ProductId` (PK): Định danh sản phẩm
- `ProductCode`: Mã sản phẩm
- `ProductName`: Tên sản phẩm
- `CategoryId` (FK): Loại sản phẩm
- `UnitId` (FK): Đơn vị tính
- `PurchasePrice`: Giá mua vào
- `SellingPrice`: Giá bán ra
- `VATRate`: Thuế suất VAT
- `Description`: Mô tả
- `IsActive`: Trạng thái hoạt động
- `CreatedDate`: Ngày tạo
- `CreatedBy` (FK): Người tạo
- `ImageUrl`: Đường dẫn hình ảnh
- `MinimumQuantity`: Số lượng tối thiểu

#### Services

- `ServiceId` (PK): Định danh dịch vụ
- `ServiceCode`: Mã dịch vụ
- `ServiceName`: Tên dịch vụ
- `CategoryId` (FK): Loại dịch vụ
- `UnitPrice`: Đơn giá
- `VATRate`: Thuế suất VAT
- `Description`: Mô tả
- `IsActive`: Trạng thái hoạt động
- `CreatedDate`: Ngày tạo
- `CreatedBy` (FK): Người tạo

#### Units

- `UnitId` (PK): Định danh đơn vị tính
- `UnitName`: Tên đơn vị tính
- `Description`: Mô tả

### 4. Quản lý bán hàng

#### SalesOrders

- `SalesOrderId` (PK): Định danh đơn hàng
- `OrderNumber`: Số đơn hàng
- `CustomerId` (FK): Khách hàng
- `OrderDate`: Ngày đặt hàng
- `DeliveryDate`: Ngày giao hàng
- `SalesPersonId` (FK): Nhân viên bán hàng
- `TotalAmount`: Tổng tiền
- `VATAmount`: Tiền thuế
- `DiscountAmount`: Tiền chiết khấu
- `FinalAmount`: Thành tiền
- `Status`: Trạng thái đơn hàng
- `Notes`: Ghi chú
- `CreatedDate`: Ngày tạo
- `CreatedBy` (FK): Người tạo
- `PaymentTerms`: Điều khoản thanh toán
- `ApprovedBy` (FK): Người duyệt
- `ApprovedDate`: Ngày duyệt

#### SalesOrderDetails

- `OrderDetailId` (PK): Định danh chi tiết đơn hàng
- `SalesOrderId` (FK): Đơn hàng
- `ProductId` (FK): Sản phẩm
- `ServiceId` (FK): Dịch vụ
- `Quantity`: Số lượng
- `UnitPrice`: Đơn giá
- `VATRate`: Thuế suất
- `DiscountRate`: Tỷ lệ chiết khấu
- `Amount`: Thành tiền
- `Notes`: Ghi chú

#### Invoices

- `InvoiceId` (PK): Định danh hóa đơn
- `InvoiceNumber`: Số hóa đơn
- `SalesOrderId` (FK): Đơn hàng
- `CustomerId` (FK): Khách hàng
- `InvoiceDate`: Ngày hóa đơn
- `DueDate`: Ngày đáo hạn
- `TotalAmount`: Tổng tiền
- `VATAmount`: Tiền thuế
- `DiscountAmount`: Tiền chiết khấu
- `FinalAmount`: Thành tiền
- `Status`: Trạng thái hóa đơn
- `Notes`: Ghi chú
- `CreatedDate`: Ngày tạo
- `CreatedBy` (FK): Người tạo
- `IsPaid`: Đã thanh toán
- `PaymentDate`: Ngày thanh toán
- `EInvoiceNo`: Số hóa đơn điện tử
- `EInvoiceDate`: Ngày hóa đơn điện tử

#### InvoiceDetails

- `InvoiceDetailId` (PK): Định danh chi tiết hóa đơn
- `InvoiceId` (FK): Hóa đơn
- `ProductId` (FK): Sản phẩm
- `ServiceId` (FK): Dịch vụ
- `Quantity`: Số lượng
- `UnitPrice`: Đơn giá
- `VATRate`: Thuế suất
- `DiscountRate`: Tỷ lệ chiết khấu
- `Amount`: Thành tiền
- `Notes`: Ghi chú

### 5. Quản lý thanh toán

#### Payments

- `PaymentId` (PK): Định danh thanh toán
- `PaymentNumber`: Số phiếu thanh toán
- `CustomerId` (FK): Khách hàng
- `InvoiceId` (FK): Hóa đơn
- `PaymentDate`: Ngày thanh toán
- `Amount`: Số tiền
- `PaymentMethod`: Phương thức thanh toán
- `AccountId` (FK): Tài khoản kế toán
- `BankAccountId` (FK): Tài khoản ngân hàng
- `Reference`: Tham chiếu
- `Notes`: Ghi chú
- `CreatedDate`: Ngày tạo
- `CreatedBy` (FK): Người tạo
- `Status`: Trạng thái

#### BankAccounts

- `BankAccountId` (PK): Định danh tài khoản ngân hàng
- `AccountNumber`: Số tài khoản
- `BankName`: Tên ngân hàng
- `BankBranch`: Chi nhánh ngân hàng
- `AccountName`: Tên tài khoản
- `Currency`: Loại tiền tệ
- `IsActive`: Trạng thái hoạt động
- `OpeningBalance`: Số dư ban đầu
- `CurrentBalance`: Số dư hiện tại
- `Notes`: Ghi chú

### 6. Quản lý kế toán

#### ChartOfAccounts

- `AccountId` (PK): Định danh tài khoản kế toán
- `AccountNumber`: Số hiệu tài khoản
- `AccountName`: Tên tài khoản
- `AccountType`: Loại tài khoản
- `ParentAccountId` (FK): Tài khoản cha
- `IsActive`: Trạng thái hoạt động
- `Description`: Mô tả

#### JournalEntries

- `EntryId` (PK): Định danh bút toán
- `EntryNumber`: Số bút toán
- `EntryDate`: Ngày ghi sổ
- `Description`: Mô tả
- `Reference`: Tham chiếu
- `Status`: Trạng thái
- `CreatedDate`: Ngày tạo
- `CreatedBy` (FK): Người tạo
- `ApprovedBy` (FK): Người duyệt
- `ApprovedDate`: Ngày duyệt

#### JournalEntryDetails

- `EntryDetailId` (PK): Định danh chi tiết bút toán
- `EntryId` (FK): Bút toán
- `AccountId` (FK): Tài khoản kế toán
- `DebitAmount`: Số tiền nợ
- `CreditAmount`: Số tiền có
- `Description`: Mô tả
- `DepartmentId` (FK): Phòng ban

#### FiscalPeriods

- `PeriodId` (PK): Định danh kỳ kế toán
- `PeriodName`: Tên kỳ kế toán
- `StartDate`: Ngày bắt đầu
- `EndDate`: Ngày kết thúc
- `IsClosed`: Đã đóng
- `ClosedBy` (FK): Người đóng
- `ClosedDate`: Ngày đóng

### 7. Quản lý báo cáo

#### Reports

- `ReportId` (PK): Định danh báo cáo
- `ReportName`: Tên báo cáo
- `ReportType`: Loại báo cáo
- `Description`: Mô tả
- `TemplateFile`: File mẫu
- `IsSystem`: Là báo cáo hệ thống

#### ReportParameters

- `ParameterId` (PK): Định danh tham số
- `ReportId` (FK): Báo cáo
- `ParameterName`: Tên tham số
- `ParameterType`: Loại tham số
- `DefaultValue`: Giá trị mặc định
- `IsRequired`: Bắt buộc

### 8. Quản lý lưu trữ và tài liệu

#### Documents

- `DocumentId` (PK): Định danh tài liệu
- `DocumentNumber`: Số tài liệu
- `DocumentType`: Loại tài liệu
- `ReferenceId`: ID tham chiếu
- `ReferenceType`: Loại tham chiếu
- `DocumentDate`: Ngày tài liệu
- `FileName`: Tên file
- `FilePath`: Đường dẫn file
- `FileSize`: Kích thước file
- `UploadedBy` (FK): Người tải lên
- `UploadedDate`: Ngày tải lên
- `Description`: Mô tả

## Sơ đồ quan hệ

```
Users 1 --- * UserRoles * --- 1 Roles
Roles 1 --- * RolePermissions * --- 1 Permissions

Customers * --- * SalesOrders
SalesOrders 1 --- * SalesOrderDetails
SalesOrderDetails * --- 1 Products
SalesOrderDetails * --- 1 Services
Products * --- 1 ProductCategories
Products * --- 1 Units
Services * --- 1 ProductCategories

SalesOrders 1 --- * Invoices
Invoices 1 --- * InvoiceDetails
InvoiceDetails * --- 1 Products
InvoiceDetails * --- 1 Services

Invoices 1 --- * Payments
Payments * --- 1 BankAccounts
Payments * --- 1 ChartOfAccounts

JournalEntries 1 --- * JournalEntryDetails
JournalEntryDetails * --- 1 ChartOfAccounts

Reports 1 --- * ReportParameters

Documents * --- 1 Users
```

## Chỉ mục và tối ưu hóa

1. **Chỉ mục Primary Key**: Tất cả các bảng đều có chỉ mục Primary Key tự động

2. **Chỉ mục Foreign Key**: Tất cả các khóa ngoại được chỉ mục để tối ưu join

3. **Chỉ mục bổ sung**:

   - `Customers`: CustomerCode, TaxCode
   - `Products`: ProductCode
   - `SalesOrders`: OrderNumber, CustomerId, OrderDate
   - `Invoices`: InvoiceNumber, CustomerId, InvoiceDate
   - `Payments`: PaymentNumber, CustomerId, PaymentDate
   - `ChartOfAccounts`: AccountNumber
   - `JournalEntries`: EntryNumber, EntryDate

4. **Tối ưu truy vấn**:
   - Sử dụng stored procedures cho các truy vấn phức tạp
   - Caching cho các báo cáo thường xuyên sử dụng
   - Phân trang cho các danh sách lớn

## Chiến lược sao lưu và phục hồi

1. **Sao lưu đầy đủ**: Hàng ngày vào 00:00
2. **Sao lưu transaction log**: Mỗi giờ
3. **Kiểm tra toàn vẹn dữ liệu**: Hàng tuần
4. **Thời gian lưu trữ**: 30 ngày cho sao lưu đầy đủ, 7 ngày cho transaction log

## Kiến trúc nâng cao cho tương lai

1. **Phân vùng dữ liệu**: Các bảng có dữ liệu lớn như Invoices, JournalEntries được phân vùng theo năm/tháng
2. **Lưu trữ dữ liệu lịch sử**: Dữ liệu cũ được chuyển sang các bảng lịch sử theo định kỳ
3. **Replikation và High Availability**: Thiết lập Always On Availability Groups cho môi trường sản xuất
4. **Nâng cấp hiệu suất**: Chỉ số đánh giá và thiết lập alert cho các vấn đề hiệu suất tiềm ẩn
