# Cấu trúc cơ sở dữ liệu - Phần mềm kế toán bán hàng

Dưới đây là thiết kế cấu trúc cơ sở dữ liệu cho phần mềm kế toán bán hàng cho Công ty Cổ phần DEHA Việt Nam, tương ứng với các module chức năng đã được xác định.

## 1. Bảng thông tin người dùng

### Users

| Trường       | Kiểu dữ liệu | Mô tả                |
| ------------ | ------------ | -------------------- |
| Id           | INT          | Khóa chính, tự tăng  |
| UserName     | VARCHAR(100) | Tên đăng nhập        |
| Email        | VARCHAR(255) | Email người dùng     |
| PasswordHash | VARCHAR(255) | Mật khẩu được mã hóa |
| FullName     | VARCHAR(255) | Họ tên đầy đủ        |
| PhoneNumber  | VARCHAR(20)  | Số điện thoại        |
| IsActive     | BOOLEAN      | Trạng thái hoạt động |
| CreatedDate  | DATETIME     | Ngày tạo             |
| LastLogin    | DATETIME     | Lần đăng nhập cuối   |

### Roles

| Trường      | Kiểu dữ liệu | Mô tả               |
| ----------- | ------------ | ------------------- |
| Id          | INT          | Khóa chính, tự tăng |
| Name        | VARCHAR(100) | Tên vai trò         |
| Description | VARCHAR(255) | Mô tả vai trò       |

### UserRoles

| Trường | Kiểu dữ liệu | Mô tả                          |
| ------ | ------------ | ------------------------------ |
| UserId | INT          | Khóa ngoại tham chiếu Users.Id |
| RoleId | INT          | Khóa ngoại tham chiếu Roles.Id |

### Permissions

| Trường      | Kiểu dữ liệu | Mô tả               |
| ----------- | ------------ | ------------------- |
| Id          | INT          | Khóa chính, tự tăng |
| Name        | VARCHAR(100) | Tên quyền           |
| Description | VARCHAR(255) | Mô tả quyền         |

### RolePermissions

| Trường       | Kiểu dữ liệu | Mô tả                                |
| ------------ | ------------ | ------------------------------------ |
| RoleId       | INT          | Khóa ngoại tham chiếu Roles.Id       |
| PermissionId | INT          | Khóa ngoại tham chiếu Permissions.Id |

### UserLogs

| Trường     | Kiểu dữ liệu | Mô tả                          |
| ---------- | ------------ | ------------------------------ |
| Id         | INT          | Khóa chính, tự tăng            |
| UserId     | INT          | Khóa ngoại tham chiếu Users.Id |
| Action     | VARCHAR(100) | Hành động thực hiện            |
| Module     | VARCHAR(100) | Module được tác động           |
| Details    | TEXT         | Chi tiết hành động             |
| IpAddress  | VARCHAR(50)  | Địa chỉ IP                     |
| ActionTime | DATETIME     | Thời gian hành động            |

## 2. Bảng thông tin khách hàng

### Customers

| Trường          | Kiểu dữ liệu | Mô tả                                   |
| --------------- | ------------ | --------------------------------------- |
| Id              | INT          | Khóa chính, tự tăng                     |
| Code            | VARCHAR(50)  | Mã khách hàng                           |
| Name            | VARCHAR(255) | Tên khách hàng                          |
| TaxCode         | VARCHAR(50)  | Mã số thuế                              |
| Address         | VARCHAR(500) | Địa chỉ                                 |
| ContactPerson   | VARCHAR(255) | Người liên hệ                           |
| PhoneNumber     | VARCHAR(20)  | Số điện thoại                           |
| Email           | VARCHAR(255) | Email                                   |
| Website         | VARCHAR(255) | Website                                 |
| CustomerGroupId | INT          | Khóa ngoại tham chiếu CustomerGroups.Id |
| CreatedBy       | INT          | Người tạo (tham chiếu Users.Id)         |
| CreatedDate     | DATETIME     | Ngày tạo                                |
| IsActive        | BOOLEAN      | Trạng thái hoạt động                    |
| Notes           | TEXT         | Ghi chú                                 |

### CustomerGroups

| Trường          | Kiểu dữ liệu | Mô tả                     |
| --------------- | ------------ | ------------------------- |
| Id              | INT          | Khóa chính, tự tăng       |
| Name            | VARCHAR(255) | Tên nhóm khách hàng       |
| Description     | VARCHAR(500) | Mô tả                     |
| DiscountPercent | DECIMAL(5,2) | Tỷ lệ chiết khấu mặc định |

### CustomerContacts

| Trường      | Kiểu dữ liệu | Mô tả                              |
| ----------- | ------------ | ---------------------------------- |
| Id          | INT          | Khóa chính, tự tăng                |
| CustomerId  | INT          | Khóa ngoại tham chiếu Customers.Id |
| FullName    | VARCHAR(255) | Họ tên người liên hệ               |
| Position    | VARCHAR(255) | Chức vụ                            |
| PhoneNumber | VARCHAR(20)  | Số điện thoại                      |
| Email       | VARCHAR(255) | Email                              |
| IsPrimary   | BOOLEAN      | Là liên hệ chính                   |

## 3. Bảng thông tin hàng hóa/dịch vụ

### Products

| Trường           | Kiểu dữ liệu  | Mô tả                                      |
| ---------------- | ------------- | ------------------------------------------ |
| Id               | INT           | Khóa chính, tự tăng                        |
| Code             | VARCHAR(50)   | Mã sản phẩm                                |
| Name             | VARCHAR(255)  | Tên sản phẩm                               |
| Description      | TEXT          | Mô tả                                      |
| CategoryId       | INT           | Khóa ngoại tham chiếu ProductCategories.Id |
| UnitId           | INT           | Khóa ngoại tham chiếu Units.Id             |
| Price            | DECIMAL(18,2) | Giá bán                                    |
| Cost             | DECIMAL(18,2) | Giá vốn                                    |
| VatRate          | DECIMAL(5,2)  | Tỷ lệ thuế VAT                             |
| IsActive         | BOOLEAN       | Trạng thái hoạt động                       |
| IsService        | BOOLEAN       | Là dịch vụ (không phải hàng hóa)           |
| HasInventory     | BOOLEAN       | Quản lý tồn kho                            |
| MinimumInventory | DECIMAL(18,2) | Tồn kho tối thiểu                          |
| CreatedBy        | INT           | Người tạo (tham chiếu Users.Id)            |
| CreatedDate      | DATETIME      | Ngày tạo                                   |
| Image            | VARCHAR(255)  | Đường dẫn hình ảnh                         |

### ProductCategories

| Trường      | Kiểu dữ liệu | Mô tả                                      |
| ----------- | ------------ | ------------------------------------------ |
| Id          | INT          | Khóa chính, tự tăng                        |
| Name        | VARCHAR(255) | Tên danh mục                               |
| ParentId    | INT          | ID danh mục cha (NULL nếu là danh mục gốc) |
| Description | VARCHAR(500) | Mô tả                                      |

### Units

| Trường      | Kiểu dữ liệu | Mô tả               |
| ----------- | ------------ | ------------------- |
| Id          | INT          | Khóa chính, tự tăng |
| Name        | VARCHAR(50)  | Tên đơn vị tính     |
| Description | VARCHAR(255) | Mô tả               |

### Inventory

| Trường      | Kiểu dữ liệu  | Mô tả                             |
| ----------- | ------------- | --------------------------------- |
| ProductId   | INT           | Khóa ngoại tham chiếu Products.Id |
| Quantity    | DECIMAL(18,2) | Số lượng tồn kho                  |
| LastUpdated | DATETIME      | Lần cập nhật cuối                 |

## 4. Bảng quản lý hóa đơn bán hàng

### Invoices

| Trường         | Kiểu dữ liệu  | Mô tả                                          |
| -------------- | ------------- | ---------------------------------------------- |
| Id             | INT           | Khóa chính, tự tăng                            |
| InvoiceNumber  | VARCHAR(50)   | Số hóa đơn                                     |
| CustomerId     | INT           | Khóa ngoại tham chiếu Customers.Id             |
| InvoiceDate    | DATE          | Ngày hóa đơn                                   |
| DueDate        | DATE          | Ngày đến hạn thanh toán                        |
| TotalAmount    | DECIMAL(18,2) | Tổng tiền                                      |
| DiscountAmount | DECIMAL(18,2) | Tiền chiết khấu                                |
| VatAmount      | DECIMAL(18,2) | Tiền thuế VAT                                  |
| GrandTotal     | DECIMAL(18,2) | Tổng tiền thanh toán                           |
| PaidAmount     | DECIMAL(18,2) | Số tiền đã thanh toán                          |
| Status         | VARCHAR(50)   | Trạng thái (Draft, Confirmed, Paid, Cancelled) |
| Notes          | TEXT          | Ghi chú                                        |
| CreatedBy      | INT           | Người tạo (tham chiếu Users.Id)                |
| CreatedDate    | DATETIME      | Ngày tạo                                       |
| UpdatedBy      | INT           | Người cập nhật (tham chiếu Users.Id)           |
| UpdatedDate    | DATETIME      | Ngày cập nhật                                  |
| SalesPersonId  | INT           | Nhân viên bán hàng (tham chiếu Users.Id)       |
| PaymentMethod  | VARCHAR(50)   | Phương thức thanh toán                         |

### InvoiceDetails

| Trường          | Kiểu dữ liệu  | Mô tả                             |
| --------------- | ------------- | --------------------------------- |
| Id              | INT           | Khóa chính, tự tăng               |
| InvoiceId       | INT           | Khóa ngoại tham chiếu Invoices.Id |
| ProductId       | INT           | Khóa ngoại tham chiếu Products.Id |
| Quantity        | DECIMAL(18,2) | Số lượng                          |
| UnitPrice       | DECIMAL(18,2) | Đơn giá                           |
| DiscountPercent | DECIMAL(5,2)  | Tỷ lệ chiết khấu                  |
| DiscountAmount  | DECIMAL(18,2) | Tiền chiết khấu                   |
| VatRate         | DECIMAL(5,2)  | Tỷ lệ thuế VAT                    |
| VatAmount       | DECIMAL(18,2) | Tiền thuế VAT                     |
| LineTotal       | DECIMAL(18,2) | Thành tiền                        |
| Notes           | VARCHAR(500)  | Ghi chú cho dòng                  |

## 5. Bảng quản lý thanh toán

### Payments

| Trường          | Kiểu dữ liệu  | Mô tả                                        |
| --------------- | ------------- | -------------------------------------------- |
| Id              | INT           | Khóa chính, tự tăng                          |
| PaymentNumber   | VARCHAR(50)   | Số phiếu thu                                 |
| CustomerId      | INT           | Khóa ngoại tham chiếu Customers.Id           |
| PaymentDate     | DATE          | Ngày thanh toán                              |
| Amount          | DECIMAL(18,2) | Số tiền thanh toán                           |
| PaymentMethod   | VARCHAR(50)   | Phương thức thanh toán                       |
| ReferenceNumber | VARCHAR(100)  | Số tham chiếu (số chuyển khoản, số check...) |
| Notes           | TEXT          | Ghi chú                                      |
| CreatedBy       | INT           | Người tạo (tham chiếu Users.Id)              |
| CreatedDate     | DATETIME      | Ngày tạo                                     |
| Status          | VARCHAR(50)   | Trạng thái (Pending, Completed, Cancelled)   |

### PaymentDetails

| Trường    | Kiểu dữ liệu  | Mô tả                              |
| --------- | ------------- | ---------------------------------- |
| Id        | INT           | Khóa chính, tự tăng                |
| PaymentId | INT           | Khóa ngoại tham chiếu Payments.Id  |
| InvoiceId | INT           | Khóa ngoại tham chiếu Invoices.Id  |
| Amount    | DECIMAL(18,2) | Số tiền thanh toán cho hóa đơn này |

## 6. Bảng cấu hình hệ thống

### CompanyInfo

| Trường      | Kiểu dữ liệu | Mô tả               |
| ----------- | ------------ | ------------------- |
| Id          | INT          | Khóa chính, tự tăng |
| Name        | VARCHAR(255) | Tên công ty         |
| TaxCode     | VARCHAR(50)  | Mã số thuế          |
| Address     | VARCHAR(500) | Địa chỉ             |
| PhoneNumber | VARCHAR(20)  | Số điện thoại       |
| Email       | VARCHAR(255) | Email               |
| Website     | VARCHAR(255) | Website             |
| Logo        | VARCHAR(255) | Đường dẫn logo      |
| BankName    | VARCHAR(255) | Tên ngân hàng       |
| BankAccount | VARCHAR(50)  | Số tài khoản        |
| BankBranch  | VARCHAR(255) | Chi nhánh ngân hàng |

### SystemSettings

| Trường       | Kiểu dữ liệu | Mô tả               |
| ------------ | ------------ | ------------------- |
| Id           | INT          | Khóa chính, tự tăng |
| SettingKey   | VARCHAR(100) | Khóa cấu hình       |
| SettingValue | TEXT         | Giá trị cấu hình    |
| Description  | VARCHAR(500) | Mô tả               |
| Group        | VARCHAR(100) | Nhóm cấu hình       |

### PrintTemplates

| Trường    | Kiểu dữ liệu | Mô tả                                  |
| --------- | ------------ | -------------------------------------- |
| Id        | INT          | Khóa chính, tự tăng                    |
| Name      | VARCHAR(255) | Tên mẫu in                             |
| Type      | VARCHAR(50)  | Loại mẫu (Invoice, Payment, Report...) |
| Content   | TEXT         | Nội dung HTML của mẫu                  |
| IsDefault | BOOLEAN      | Là mẫu mặc định                        |

## Mối quan hệ giữa các bảng

Dưới đây là mô tả các mối quan hệ chính giữa các bảng trong cơ sở dữ liệu:

1. **Users - Roles**: Quan hệ nhiều-nhiều thông qua bảng UserRoles
2. **Roles - Permissions**: Quan hệ nhiều-nhiều thông qua bảng RolePermissions
3. **Users - UserLogs**: Quan hệ một-nhiều (một user có nhiều log)
4. **Customers - CustomerGroup**: Quan hệ nhiều-một (nhiều khách hàng thuộc một nhóm)
5. **Customers - CustomerContacts**: Quan hệ một-nhiều (một khách hàng có nhiều liên hệ)
6. **Products - ProductCategories**: Quan hệ nhiều-một (nhiều sản phẩm thuộc một danh mục)
7. **Products - Units**: Quan hệ nhiều-một (nhiều sản phẩm có cùng đơn vị tính)
8. **Products - Inventory**: Quan hệ một-một (một sản phẩm có một bản ghi tồn kho)
9. **Invoices - Customers**: Quan hệ nhiều-một (nhiều hóa đơn của một khách hàng)
10. **Invoices - InvoiceDetails**: Quan hệ một-nhiều (một hóa đơn có nhiều dòng chi tiết)
11. **InvoiceDetails - Products**: Quan hệ nhiều-một (nhiều dòng chi tiết có thể tham chiếu đến cùng một sản phẩm)
12. **Payments - Customers**: Quan hệ nhiều-một (nhiều phiếu thu của một khách hàng)
13. **Payments - PaymentDetails**: Quan hệ một-nhiều (một phiếu thu có thể thanh toán cho nhiều hóa đơn)
14. **PaymentDetails - Invoices**: Quan hệ nhiều-một (nhiều chi tiết thanh toán cho một hóa đơn)

## Chỉ mục và hiệu suất

Để đảm bảo hiệu suất cao cho cơ sở dữ liệu, chúng ta sẽ tạo các chỉ mục sau:

1. Chỉ mục cho tất cả các khóa ngoại (ForeignKey)
2. Chỉ mục cho các trường thường xuyên tìm kiếm:
   - Customers.Code, Customers.Name, Customers.TaxCode
   - Products.Code, Products.Name
   - Invoices.InvoiceNumber, Invoices.InvoiceDate, Invoices.Status
   - Payments.PaymentNumber, Payments.PaymentDate

## Ràng buộc tính toàn vẹn

1. Các khóa ngoại có ràng buộc tham chiếu (ON DELETE và ON UPDATE)
2. Ràng buộc CHECK cho các giá trị thuộc tính (ví dụ: số tiền, số lượng không âm)
3. Ràng buộc UNIQUE cho các mã code, số hóa đơn, số phiếu thu

## Lưu ý về triển khai

1. Sử dụng stored procedures hoặc transactions cho các thao tác phức tạp
2. Thiết kế triggers để cập nhật tự động các trường liên quan (ví dụ: cập nhật PaidAmount trong Invoices khi có thanh toán mới)
3. Nên cân nhắc sử dụng views để tạo các báo cáo phức tạp
