# Tài liệu các bảng dữ liệu Accounting

## Danh mục nhóm khách hàng - DMNhomKH

| Column Name | Data Type    | Allow Nulls | Explain       |
| ----------- | ------------ | ----------- | ------------- |
| MaLN        | varchar(10)  | Unchecked   | Mã loại nhóm  |
| TenLN       | nvarchar(50) | Checked     | Tên loại nhóm |
| MaN         | varchar(10)  | Unchecked   | Mã nhóm       |
| TenN        | nvarchar(50) | Checked     | Tên nhóm      |
| GhiChu      | Ntext        | Checked     | Ghi chú       |

## Danh mục khách hàng - Customer

| Column Name   | Data Type     | Allow Nulls | Explain                                                  |
| ------------- | ------------- | ----------- | -------------------------------------------------------- |
| Id            | int           | Unchecked   | ID khách hàng                                            |
| CustomerCode  | varchar(50)   | Unchecked   | Mã khách hàng                                            |
| Name          | varchar(200)  | Unchecked   | Tên khách hàng                                           |
| CustomerType  | int           | Unchecked   | Loại khách hàng (0: Cá nhân, 1: Doanh nghiệp, 2: Đại lý) |
| Address       | varchar(300)  | Checked     | Địa chỉ                                                  |
| Phone         | varchar(20)   | Checked     | Số điện thoại                                            |
| Email         | varchar(100)  | Checked     | Email                                                    |
| TaxCode       | varchar(50)   | Checked     | Mã số thuế                                               |
| ContactPerson | varchar(100)  | Checked     | Người liên hệ                                            |
| BankAccount   | varchar(50)   | Checked     | Số tài khoản                                             |
| BankName      | varchar(100)  | Checked     | Tên ngân hàng                                            |
| BankBranch    | varchar(100)  | Checked     | Chi nhánh ngân hàng                                      |
| PaymentTerms  | varchar(200)  | Checked     | Điều khoản thanh toán                                    |
| CurrentDebt   | decimal(18,2) | Unchecked   | Công nợ hiện tại                                         |
| DebtLimit     | decimal(18,2) | Unchecked   | Hạn mức công nợ                                          |
| IsActive      | bit           | Unchecked   | Còn hoạt động                                            |
| CreatedDate   | datetime      | Unchecked   | Ngày tạo                                                 |
| CreatedBy     | varchar(50)   | Unchecked   | Người tạo                                                |
| UpdatedDate   | datetime      | Checked     | Ngày cập nhật                                            |
| UpdatedBy     | varchar(50)   | Checked     | Người cập nhật                                           |

## Danh mục sản phẩm - Product

| Column Name       | Data Type     | Allow Nulls | Explain                    |
| ----------------- | ------------- | ----------- | -------------------------- |
| Id                | int           | Unchecked   | ID sản phẩm                |
| ProductCode       | varchar(50)   | Unchecked   | Mã sản phẩm                |
| Name              | varchar(200)  | Unchecked   | Tên sản phẩm               |
| Description       | varchar(500)  | Checked     | Mô tả                      |
| ProductCategoryId | int           | Unchecked   | ID loại sản phẩm           |
| SupplierId        | int           | Checked     | ID nhà cung cấp            |
| DefaultSupplierId | int           | Checked     | ID nhà cung cấp mặc định   |
| PurchasePrice     | decimal(18,2) | Unchecked   | Giá nhập                   |
| SellingPrice      | decimal(18,2) | Unchecked   | Giá bán                    |
| DiscountPrice     | decimal(18,2) | Checked     | Giá khuyến mãi             |
| Unit              | varchar(20)   | Unchecked   | Đơn vị tính                |
| StockQuantity     | int           | Unchecked   | Số lượng tồn kho           |
| MinimumStockLevel | int           | Unchecked   | Số lượng tối thiểu         |
| ReorderLevel      | int           | Unchecked   | Số lượng đặt hàng lại      |
| LeadTime          | int           | Unchecked   | Thời gian giao hàng (ngày) |
| Brand             | varchar(100)  | Checked     | Thương hiệu                |
| Origin            | varchar(100)  | Checked     | Xuất xứ                    |
| WarehouseLocation | varchar(100)  | Checked     | Vị trí kho                 |
| Barcode           | varchar(50)   | Checked     | Mã vạch                    |
| IsSellable        | bit           | Unchecked   | Có thể bán                 |
| IsActive          | bit           | Unchecked   | Còn hoạt động              |
| IsService         | bit           | Unchecked   | Là sản phẩm dịch vụ        |
| TrackInventory    | bit           | Unchecked   | Có theo dõi tồn kho        |
| Notes             | varchar(500)  | Checked     | Ghi chú                    |
| CreatedDate       | datetime      | Unchecked   | Ngày tạo                   |
| CreatedBy         | varchar(50)   | Unchecked   | Người tạo                  |
| UpdatedDate       | datetime      | Checked     | Ngày cập nhật              |
| UpdatedBy         | varchar(50)   | Checked     | Người cập nhật             |

## Danh mục nhà cung cấp - Supplier

| Column Name   | Data Type    | Allow Nulls | Explain                      |
| ------------- | ------------ | ----------- | ---------------------------- |
| Id            | int          | Unchecked   | ID nhà cung cấp              |
| SupplierCode  | varchar(50)  | Unchecked   | Mã nhà cung cấp              |
| Name          | varchar(200) | Unchecked   | Tên nhà cung cấp             |
| EnglishName   | varchar(200) | Checked     | Tên tiếng Anh                |
| TaxCode       | varchar(20)  | Checked     | Mã số thuế                   |
| Address       | varchar(500) | Checked     | Địa chỉ                      |
| Country       | varchar(100) | Unchecked   | Quốc gia                     |
| Province      | varchar(100) | Checked     | Tỉnh/Thành phố               |
| District      | varchar(100) | Checked     | Quận/Huyện                   |
| Phone         | varchar(20)  | Checked     | Số điện thoại                |
| Email         | varchar(100) | Checked     | Email                        |
| Website       | varchar(200) | Checked     | Website                      |
| ContactPerson | varchar(100) | Checked     | Người liên hệ                |
| ContactPhone  | varchar(20)  | Checked     | Số điện thoại liên hệ        |
| ContactEmail  | varchar(100) | Checked     | Email liên hệ                |
| PaymentMethod | varchar(50)  | Checked     | Phương thức thanh toán       |
| PaymentTerms  | int          | Unchecked   | Điều khoản thanh toán (ngày) |
| Notes         | varchar(500) | Checked     | Ghi chú                      |
| IsActive      | bit          | Unchecked   | Còn hoạt động                |
| CreatedDate   | datetime     | Unchecked   | Ngày tạo                     |
| CreatedBy     | varchar(50)  | Unchecked   | Người tạo                    |
| UpdatedDate   | datetime     | Checked     | Ngày cập nhật                |
| UpdatedBy     | varchar(50)  | Checked     | Người cập nhật               |

## Danh mục loại sản phẩm - ProductCategory

| Column Name      | Data Type    | Allow Nulls | Explain         |
| ---------------- | ------------ | ----------- | --------------- |
| Id               | int          | Unchecked   | ID danh mục     |
| CategoryCode     | varchar(50)  | Unchecked   | Mã danh mục     |
| Name             | varchar(100) | Unchecked   | Tên danh mục    |
| Description      | varchar(500) | Checked     | Mô tả           |
| ParentCategoryId | int          | Checked     | ID danh mục cha |
| DisplayOrder     | int          | Unchecked   | Thứ tự hiển thị |
| IsActive         | bit          | Unchecked   | Còn hoạt động   |
| CreatedDate      | datetime     | Unchecked   | Ngày tạo        |
| CreatedBy        | varchar(50)  | Unchecked   | Người tạo       |
| UpdatedDate      | datetime     | Checked     | Ngày cập nhật   |
| UpdatedBy        | varchar(50)  | Checked     | Người cập nhật  |

## Chi tiết đơn mua hàng - PurchaseOrderDetail

| Column Name        | Data Type     | Allow Nulls | Explain           |
| ------------------ | ------------- | ----------- | ----------------- |
| Id                 | int           | Unchecked   | ID chi tiết       |
| PurchaseOrderId    | int           | Unchecked   | ID đơn mua hàng   |
| ProductId          | int           | Unchecked   | ID sản phẩm       |
| Quantity           | int           | Unchecked   | Số lượng          |
| UnitPrice          | decimal(18,2) | Unchecked   | Đơn giá           |
| DiscountPercentage | decimal(18,2) | Unchecked   | Chiết khấu (%)    |
| DiscountAmount     | decimal(18,2) | Unchecked   | Chiết khấu (tiền) |
| TaxRate            | decimal(18,2) | Unchecked   | Thuế (%)          |
| TaxAmount          | decimal(18,2) | Unchecked   | Thuế (tiền)       |
| SubTotal           | decimal(18,2) | Unchecked   | Thành tiền        |
| Total              | decimal(18,2) | Unchecked   | Tổng cộng         |
| ReceivedQuantity   | int           | Unchecked   | Số lượng đã nhận  |
| WarehouseLocation  | varchar(100)  | Checked     | Vị trí kho        |
| Notes              | varchar(500)  | Checked     | Ghi chú           |
| CreatedDate        | datetime      | Unchecked   | Ngày tạo          |
| CreatedBy          | varchar(50)   | Unchecked   | Người tạo         |
| UpdatedDate        | datetime      | Checked     | Ngày cập nhật     |
| UpdatedBy          | varchar(50)   | Checked     | Người cập nhật    |

## Đơn mua hàng - PurchaseOrder

| Column Name          | Data Type     | Allow Nulls | Explain                                                                                                                                       |
| -------------------- | ------------- | ----------- | --------------------------------------------------------------------------------------------------------------------------------------------- |
| Id                   | int           | Unchecked   | ID đơn mua hàng                                                                                                                               |
| OrderNumber          | varchar(50)   | Unchecked   | Mã đơn mua hàng                                                                                                                               |
| OrderDate            | datetime      | Unchecked   | Ngày đặt hàng                                                                                                                                 |
| ExpectedDeliveryDate | datetime      | Checked     | Ngày dự kiến nhận                                                                                                                             |
| SupplierId           | int           | Unchecked   | ID nhà cung cấp                                                                                                                               |
| Status               | int           | Unchecked   | Trạng thái (0: Đơn mới, 1: Đã duyệt, 2: Đã nhận hàng một phần, 3: Đã nhận hàng đủ, 4: Đã thanh toán một phần, 5: Đã thanh toán đủ, 6: Đã hủy) |
| ReferenceNumber      | varchar(50)   | Checked     | Số tham chiếu                                                                                                                                 |
| PaymentMethod        | varchar(100)  | Checked     | Phương thức thanh toán                                                                                                                        |
| PaymentTerms         | varchar(200)  | Checked     | Điều khoản thanh toán                                                                                                                         |
| ReceivedDate         | datetime      | Checked     | Ngày nhận hàng                                                                                                                                |
| DeliveryAddress      | varchar(200)  | Checked     | Địa chỉ giao hàng                                                                                                                             |
| Notes                | varchar(500)  | Checked     | Ghi chú                                                                                                                                       |
| SubTotal             | decimal(18,2) | Unchecked   | Tổng tiền hàng                                                                                                                                |
| Discount             | decimal(18,2) | Unchecked   | Chiết khấu                                                                                                                                    |
| Tax                  | decimal(18,2) | Unchecked   | Thuế                                                                                                                                          |
| ShippingFee          | decimal(18,2) | Unchecked   | Phí vận chuyển                                                                                                                                |
| GrandTotal           | decimal(18,2) | Unchecked   | Tổng cộng                                                                                                                                     |
| AmountPaid           | decimal(18,2) | Unchecked   | Đã thanh toán                                                                                                                                 |
| CreatedDate          | datetime      | Unchecked   | Ngày tạo                                                                                                                                      |
| CreatedBy            | varchar(50)   | Unchecked   | Người tạo                                                                                                                                     |
| UpdatedDate          | datetime      | Checked     | Ngày cập nhật                                                                                                                                 |
| UpdatedBy            | varchar(50)   | Checked     | Người cập nhật                                                                                                                                |
| ApprovedDate         | datetime      | Checked     | Ngày duyệt                                                                                                                                    |
| ApprovedBy           | varchar(50)   | Checked     | Người duyệt                                                                                                                                   |

## Đơn bán hàng - SalesOrder

| Column Name             | Data Type     | Allow Nulls | Explain                                                                                                                                                                                           |
| ----------------------- | ------------- | ----------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Id                      | int           | Unchecked   | ID đơn bán hàng                                                                                                                                                                                   |
| OrderNumber             | varchar(50)   | Unchecked   | Mã đơn bán hàng                                                                                                                                                                                   |
| OrderDate               | datetime      | Unchecked   | Ngày đặt hàng                                                                                                                                                                                     |
| ExpectedShippingDate    | datetime      | Checked     | Ngày dự kiến giao                                                                                                                                                                                 |
| CustomerId              | int           | Unchecked   | ID khách hàng                                                                                                                                                                                     |
| Status                  | int           | Unchecked   | Trạng thái (0: Đơn nháp, 1: Đơn mới, 2: Đã xác nhận, 3: Đã giao hàng một phần, 4: Đã giao hàng đủ, 5: Đã xuất hóa đơn, 6: Đã thanh toán một phần, 7: Đã thanh toán đủ, 8: Đã hủy, 9: Đã trả hàng) |
| CustomerReferenceNumber | varchar(50)   | Checked     | Số tham chiếu khách hàng                                                                                                                                                                          |
| PaymentMethod           | varchar(100)  | Checked     | Phương thức thanh toán                                                                                                                                                                            |
| PaymentTerms            | varchar(200)  | Checked     | Điều khoản thanh toán                                                                                                                                                                             |
| ShippingDate            | datetime      | Checked     | Ngày giao hàng                                                                                                                                                                                    |
| SalesPerson             | varchar(100)  | Checked     | Nhân viên bán hàng                                                                                                                                                                                |
| ShippingAddress         | varchar(200)  | Checked     | Địa chỉ giao hàng                                                                                                                                                                                 |
| Notes                   | varchar(500)  | Checked     | Ghi chú                                                                                                                                                                                           |
| SubTotal                | decimal(18,2) | Unchecked   | Tổng tiền hàng                                                                                                                                                                                    |
| Discount                | decimal(18,2) | Unchecked   | Chiết khấu                                                                                                                                                                                        |
| Tax                     | decimal(18,2) | Unchecked   | Thuế                                                                                                                                                                                              |
| ShippingFee             | decimal(18,2) | Unchecked   | Phí vận chuyển                                                                                                                                                                                    |
| GrandTotal              | decimal(18,2) | Unchecked   | Tổng cộng                                                                                                                                                                                         |
| AmountPaid              | decimal(18,2) | Unchecked   | Đã thanh toán                                                                                                                                                                                     |
| IsInvoiced              | bit           | Unchecked   | Đã xuất hóa đơn                                                                                                                                                                                   |
| InvoiceId               | int           | Checked     | ID hóa đơn                                                                                                                                                                                        |
| CreatedDate             | datetime      | Unchecked   | Ngày tạo                                                                                                                                                                                          |
| CreatedBy               | varchar(50)   | Unchecked   | Người tạo                                                                                                                                                                                         |
| UpdatedDate             | datetime      | Checked     | Ngày cập nhật                                                                                                                                                                                     |
| UpdatedBy               | varchar(50)   | Checked     | Người cập nhật                                                                                                                                                                                    |
| ConfirmedDate           | datetime      | Checked     | Ngày xác nhận                                                                                                                                                                                     |
| ConfirmedBy             | varchar(50)   | Checked     | Người xác nhận                                                                                                                                                                                    |

## Chi tiết đơn bán hàng - SalesOrderDetail

| Column Name        | Data Type     | Allow Nulls | Explain           |
| ------------------ | ------------- | ----------- | ----------------- |
| Id                 | int           | Unchecked   | ID chi tiết       |
| SalesOrderId       | int           | Unchecked   | ID đơn bán hàng   |
| ProductId          | int           | Unchecked   | ID sản phẩm       |
| Quantity           | int           | Unchecked   | Số lượng          |
| UnitPrice          | decimal(18,2) | Unchecked   | Đơn giá           |
| DiscountPercentage | decimal(18,2) | Unchecked   | Chiết khấu (%)    |
| DiscountAmount     | decimal(18,2) | Unchecked   | Chiết khấu (tiền) |
| TaxRate            | decimal(18,2) | Unchecked   | Thuế (%)          |
| TaxAmount          | decimal(18,2) | Unchecked   | Thuế (tiền)       |
| SubTotal           | decimal(18,2) | Unchecked   | Thành tiền        |
| Total              | decimal(18,2) | Unchecked   | Tổng cộng         |
| ShippedQuantity    | int           | Unchecked   | Số lượng đã giao  |
| Notes              | varchar(500)  | Checked     | Ghi chú           |
| CreatedDate        | datetime      | Unchecked   | Ngày tạo          |
| CreatedBy          | varchar(50)   | Unchecked   | Người tạo         |
| UpdatedDate        | datetime      | Checked     | Ngày cập nhật     |
| UpdatedBy          | varchar(50)   | Checked     | Người cập nhật    |

## Hóa đơn - Invoice

| Column Name     | Data Type     | Allow Nulls | Explain                                                                                                  |
| --------------- | ------------- | ----------- | -------------------------------------------------------------------------------------------------------- |
| Id              | int           | Unchecked   | ID hóa đơn                                                                                               |
| InvoiceNumber   | varchar(50)   | Unchecked   | Mã hóa đơn                                                                                               |
| InvoiceDate     | datetime      | Unchecked   | Ngày hóa đơn                                                                                             |
| DueDate         | datetime      | Unchecked   | Ngày đến hạn                                                                                             |
| CustomerId      | int           | Unchecked   | ID khách hàng                                                                                            |
| SalesOrderId    | int           | Checked     | ID đơn hàng                                                                                              |
| Status          | int           | Unchecked   | Trạng thái (0: Đã tạo, 1: Đã gửi, 2: Đã thanh toán một phần, 3: Đã thanh toán đủ, 4: Quá hạn, 5: Đã hủy) |
| ReferenceNumber | varchar(50)   | Checked     | Mã tham chiếu                                                                                            |
| PaymentMethod   | varchar(100)  | Checked     | Phương thức thanh toán                                                                                   |
| PaymentTerms    | varchar(200)  | Checked     | Điều khoản thanh toán                                                                                    |
| Notes           | varchar(500)  | Checked     | Ghi chú                                                                                                  |
| SubTotal        | decimal(18,2) | Unchecked   | Tổng tiền hàng                                                                                           |
| Discount        | decimal(18,2) | Unchecked   | Chiết khấu                                                                                               |
| Tax             | decimal(18,2) | Unchecked   | Thuế                                                                                                     |
| ShippingFee     | decimal(18,2) | Unchecked   | Phí vận chuyển                                                                                           |
| GrandTotal      | decimal(18,2) | Unchecked   | Tổng cộng                                                                                                |
| AmountPaid      | decimal(18,2) | Unchecked   | Đã thanh toán                                                                                            |
| SentDate        | datetime      | Checked     | Ngày gửi                                                                                                 |
| PaymentDate     | datetime      | Checked     | Ngày thanh toán                                                                                          |
| VoucherNumber   | varchar(50)   | Checked     | Mã chứng từ                                                                                              |
| CreatedDate     | datetime      | Unchecked   | Ngày tạo                                                                                                 |
| CreatedBy       | varchar(50)   | Unchecked   | Người tạo                                                                                                |
| UpdatedDate     | datetime      | Checked     | Ngày cập nhật                                                                                            |
| UpdatedBy       | varchar(50)   | Checked     | Người cập nhật                                                                                           |

## Chi tiết hóa đơn - InvoiceDetail

| Column Name        | Data Type     | Allow Nulls | Explain           |
| ------------------ | ------------- | ----------- | ----------------- |
| Id                 | int           | Unchecked   | ID chi tiết       |
| InvoiceId          | int           | Unchecked   | ID hóa đơn        |
| ProductId          | int           | Unchecked   | ID sản phẩm       |
| Quantity           | int           | Unchecked   | Số lượng          |
| UnitPrice          | decimal(18,2) | Unchecked   | Đơn giá           |
| DiscountPercentage | decimal(18,2) | Unchecked   | Chiết khấu (%)    |
| DiscountAmount     | decimal(18,2) | Unchecked   | Chiết khấu (tiền) |
| TaxRate            | decimal(18,2) | Unchecked   | Thuế (%)          |
| TaxAmount          | decimal(18,2) | Unchecked   | Thuế (tiền)       |
| SubTotal           | decimal(18,2) | Unchecked   | Thành tiền        |
| Total              | decimal(18,2) | Unchecked   | Tổng cộng         |
| Notes              | varchar(500)  | Checked     | Ghi chú           |
| CreatedDate        | datetime      | Unchecked   | Ngày tạo          |
| CreatedBy          | varchar(50)   | Unchecked   | Người tạo         |
| UpdatedDate        | datetime      | Checked     | Ngày cập nhật     |
| UpdatedBy          | varchar(50)   | Checked     | Người cập nhật    |

## Thanh toán - Payment

| Column Name          | Data Type     | Allow Nulls | Explain                                                                                                        |
| -------------------- | ------------- | ----------- | -------------------------------------------------------------------------------------------------------------- |
| Id                   | int           | Unchecked   | ID thanh toán                                                                                                  |
| PaymentCode          | varchar(50)   | Unchecked   | Mã thanh toán                                                                                                  |
| PaymentDate          | datetime      | Unchecked   | Ngày thanh toán                                                                                                |
| PaymentType          | int           | Unchecked   | Loại thanh toán (0: Tiền mặt, 1: Chuyển khoản, 2: Thẻ tín dụng, 3: Ví điện tử, 4: Thẻ ghi nợ, 5: Séc, 6: Khác) |
| Direction            | int           | Unchecked   | Hướng thanh toán (0: Tiền vào, 1: Tiền ra)                                                                     |
| CustomerId           | int           | Checked     | ID khách hàng                                                                                                  |
| SupplierId           | int           | Checked     | ID nhà cung cấp                                                                                                |
| InvoiceId            | int           | Checked     | ID hóa đơn                                                                                                     |
| PurchaseOrderId      | int           | Checked     | ID đơn mua hàng                                                                                                |
| Amount               | decimal(18,2) | Unchecked   | Số tiền                                                                                                        |
| PayeeName            | varchar(100)  | Checked     | Người nhận/Chi trả                                                                                             |
| AccountNumber        | varchar(50)   | Checked     | Số tài khoản                                                                                                   |
| BankName             | varchar(100)  | Checked     | Ngân hàng                                                                                                      |
| TransactionReference | varchar(200)  | Checked     | Nội dung chuyển khoản                                                                                          |
| Notes                | varchar(500)  | Checked     | Ghi chú                                                                                                        |
| IsVerified           | bit           | Unchecked   | Đã chứng thực                                                                                                  |
| VerificationDate     | datetime      | Checked     | Ngày chứng thực                                                                                                |
| VerifiedDate         | datetime      | Checked     | Ngày xác minh                                                                                                  |
| VerifiedBy           | varchar(50)   | Checked     | Người xác minh                                                                                                 |
| CreatedDate          | datetime      | Unchecked   | Ngày tạo                                                                                                       |
| CreatedBy            | varchar(50)   | Unchecked   | Người tạo                                                                                                      |
| UpdatedDate          | datetime      | Checked     | Ngày cập nhật                                                                                                  |
| UpdatedBy            | varchar(50)   | Checked     | Người cập nhật                                                                                                 |

## Chuyển động kho - InventoryMovement

| Column Name           | Data Type    | Allow Nulls | Explain                                                                                                                                                                                        |
| --------------------- | ------------ | ----------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Id                    | int          | Unchecked   | ID chuyển động kho                                                                                                                                                                             |
| MovementCode          | varchar(50)  | Unchecked   | Mã phiếu                                                                                                                                                                                       |
| MovementDate          | datetime     | Unchecked   | Ngày thực hiện                                                                                                                                                                                 |
| MovementType          | int          | Unchecked   | Loại di chuyển (0: Nhập kho từ nhà cung cấp, 1: Xuất kho bán hàng, 2: Điều chỉnh tăng, 3: Điều chỉnh giảm, 4: Trả hàng cho nhà cung cấp, 5: Nhận hàng trả từ khách, 6: Chuyển kho, 7: Kiểm kê) |
| ProductId             | int          | Unchecked   | ID sản phẩm                                                                                                                                                                                    |
| Quantity              | int          | Unchecked   | Số lượng                                                                                                                                                                                       |
| PurchaseOrderId       | int          | Checked     | ID đơn mua hàng                                                                                                                                                                                |
| PurchaseOrderDetailId | int          | Checked     | ID chi tiết đơn mua                                                                                                                                                                            |
| SalesOrderId          | int          | Checked     | ID đơn bán hàng                                                                                                                                                                                |
| SalesOrderDetailId    | int          | Checked     | ID chi tiết đơn bán                                                                                                                                                                            |
| BeforeQuantity        | int          | Unchecked   | Tồn kho trước                                                                                                                                                                                  |
| AfterQuantity         | int          | Unchecked   | Tồn kho sau                                                                                                                                                                                    |
| WarehouseLocation     | varchar(100) | Checked     | Vị trí kho                                                                                                                                                                                     |
| Notes                 | varchar(500) | Checked     | Ghi chú                                                                                                                                                                                        |
| CreatedDate           | datetime     | Unchecked   | Ngày tạo                                                                                                                                                                                       |
| CreatedBy             | varchar(50)  | Unchecked   | Người tạo                                                                                                                                                                                      |
| UpdatedDate           | datetime     | Checked     | Ngày cập nhật                                                                                                                                                                                  |
| UpdatedBy             | varchar(50)  | Checked     | Người cập nhật                                                                                                                                                                                 |
