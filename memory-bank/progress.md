# Tiến độ dự án

## Những gì đã hoàn thành

1. **Phân tích yêu cầu ban đầu**

   - Xác định đối tượng khách hàng là Công ty Cổ phần DEHA Việt Nam
   - Xác định mục tiêu chính: xây dựng phần mềm kế toán bán hàng

2. **Thiết kế module**

   - Đã xác định 8 module chính cho phần mềm:
     - Quản lý khách hàng
     - Quản lý hàng hóa/dịch vụ
     - Quản lý hóa đơn bán hàng
     - Quản lý thanh toán
     - Báo cáo và thống kê
     - Quản lý người dùng và phân quyền
     - Tích hợp hệ thống
     - Thiết lập hệ thống
   - Đã lập tài liệu chi tiết về chức năng của từng module

3. **Thiết kế cơ sở dữ liệu**

   - Đã xác định cấu trúc các bảng dữ liệu cho từng module
   - Đã thiết kế quan hệ giữa các bảng
   - Đã xác định các ràng buộc và chỉ mục cần thiết
   - Đã lập tài liệu về cơ sở dữ liệu chi tiết

4. **Cài đặt môi trường phát triển**

   - Đã chuyển từ .NET 9.0 lên .NET 10 để tránh vấn đề với phiên bản preview
   - Đã tạo dự án ASP.NET Core MVC với .NET 10
   - Đã cấu hình SQLite làm cơ sở dữ liệu phát triển

5. **Xây dựng model dữ liệu**

   - Đã tạo 12 model cơ bản cho hệ thống kế toán bán hàng:
     - Customer (Khách hàng)
     - Supplier (Nhà cung cấp)
     - Product (Sản phẩm)
     - ProductCategory (Danh mục sản phẩm)
     - PurchaseOrder (Đơn mua hàng)
     - PurchaseOrderDetail (Chi tiết đơn mua hàng)
     - SalesOrder (Đơn bán hàng)
     - SalesOrderDetail (Chi tiết đơn bán hàng)
     - Invoice (Hóa đơn)
     - InvoiceDetail (Chi tiết hóa đơn)
     - Payment (Thanh toán)
     - InventoryMovement (Quản lý tồn kho)
   - Đã tạo và cấu hình ApplicationDbContext với các mối quan hệ giữa các entities
   - Đã cải thiện model classes với constructor phù hợp và giá trị mặc định cho các trường bắt buộc

6. **Xây dựng các Controller cơ bản**

   - Đã tạo 5 controller cơ bản cho các entity chính:
     - CustomersController: Quản lý khách hàng
     - SuppliersController: Quản lý nhà cung cấp
     - ProductCategoriesController: Quản lý danh mục sản phẩm
     - ProductsController: Quản lý sản phẩm
     - InventoryMovementsController: Quản lý nhập xuất kho
   - Đã triển khai các chức năng CRUD đầy đủ cho mỗi controller
   - Đã áp dụng xác thực và phân quyền thông qua thuộc tính [Authorize]
   - Đã triển khai logic nghiệp vụ như kiểm tra dữ liệu liên quan trước khi xóa

7. **Thiết kế và phát triển Views**

   - Đã hoàn thành các view cho Products bao gồm Index, Create, Edit, Details và Delete
   - Đã hoàn thành các view cho ProductCategories bao gồm Index, Create, Edit, Details và Delete
   - Đã hoàn thành các view cho Suppliers bao gồm Index, Create, Edit, Details và Delete với thiết kế card-based hiện đại
   - Đã hoàn thành các view cho Customers bao gồm Index, Create, Edit, Details và Delete với thiết kế card-based hiện đại
   - Đã áp dụng Bootstrap 5 và Bootstrap Icons để tạo giao diện người dùng hiện đại và thân thiện
   - Đã tối ưu hóa views cho hiển thị responsive trên nhiều thiết bị
   - Đã thêm các tính năng UX nâng cao như hiển thị thông tin hệ thống, tab navigation và formatting dữ liệu

8. **Cấu hình Database và Migrations**

   - Đã tạo migration đầu tiên (InitialCreate) với Entity Framework Core
   - Đã áp dụng migration để tạo cơ sở dữ liệu ban đầu
   - Đã fix các lỗi cơ bản trong Views để chuẩn bị cho việc dữ liệu thực tế
   - Đã sửa lỗi "NOT NULL constraint failed" trong phương thức Seed() bằng cách thêm giá trị mặc định cho các trường bắt buộc

9. **Sửa lỗi và tối ưu hóa dữ liệu**

   - Đã cập nhật tất cả các model class để thêm constructor khởi tạo giá trị mặc định cho các thuộc tính string
   - Đã thêm trường UpdatedBy vào phương thức Seed() để đảm bảo đáp ứng ràng buộc NOT NULL
   - Đã cập nhật các navigation properties trong các model class để xử lý đúng các mối quan hệ null và non-null
   - Đã sửa lỗi trong các model để đảm bảo tính nhất quán và tuân thủ ràng buộc của cơ sở dữ liệu

10. **Cải thiện trải nghiệm người dùng**

    - Đã sửa lỗi layout trong \_Layout.cshtml để tránh nội dung bị khuất vào footer
    - Đã thêm padding-bottom cho body và margin-bottom cho container chính để tạo khoảng cách phù hợp
    - Đã điều chỉnh vị trí của footer với position: relative để đảm bảo nó luôn hiển thị đúng
    - Đã cải thiện responsive design để đảm bảo giao diện hiển thị tốt trên các thiết bị khác nhau

11. **Hoàn thiện InventoryMovementsController**

    - Đã thêm chức năng StockReport để hiển thị báo cáo tồn kho chi tiết
    - Đã thêm chức năng ProductHistory để xem lịch sử nhập xuất kho của từng sản phẩm
    - Đã thêm chức năng BatchStocktaking để thực hiện kiểm kê hàng loạt
    - Đã cải thiện giao diện Index với thiết kế card-based hiện đại
    - Đã triển khai xử lý transactions cho các thao tác kiểm kê để đảm bảo tính nhất quán dữ liệu
    - Đã thêm cách hiển thị trực quan cho các loại phiếu nhập xuất kho khác nhau
    - Đã cải thiện UX bằng cách hiển thị thay đổi số lượng dưới dạng +/- trực quan

12. **Cải thiện giao diện và sửa lỗi**

    - Đã thêm dropdown "Đơn hàng" vào navbar để quản lý tập trung đơn mua hàng và đơn bán hàng
    - Đã sửa lỗi cú pháp trong file SalesOrders/Delete.cshtml, thêm dấu ngoặc đóng cho ViewData["Title"]
    - Đã sửa lỗi sai enum trong PurchaseOrdersController, sửa từ MovementType.PurchaseReceipt thành MovementType.PurchaseReceive
    - Đã cải thiện hiển thị active state cho các mục menu dựa trên controller và action hiện tại
    - Đã thực hiện các điều chỉnh giao diện để đảm bảo tính nhất quán trên toàn bộ ứng dụng

13. **Hoàn thiện quản lý đơn hàng**

    - Đã tạo đầy đủ các view cho PurchaseOrders: Create.cshtml, Edit.cshtml và Delete.cshtml
    - Đã triển khai chức năng thêm sản phẩm động vào đơn hàng với JavaScript
    - Đã thiết kế giao diện thân thiện và nhất quán giữa các module
    - Đã cải thiện trải nghiệm người dùng với tính năng tự động điền thông tin sản phẩm và tính toán tổng tiền
    - Đã tối ưu hóa các view để phù hợp với workflow của đơn hàng

## Những gì đang làm

1. **Chuẩn bị cho SalesOrdersController**

   - Thiết kế giao diện cho việc thêm nhiều sản phẩm vào đơn hàng
   - Xây dựng các ViewModels cần thiết cho đơn hàng
   - Thiết kế logic tính toán giá trị đơn hàng, thuế và giảm giá

2. **Chuẩn bị cho PurchaseOrdersController**
   - Thiết kế workflow cho quản lý trạng thái đơn hàng
   - Xây dựng cơ chế liên kết giữa đơn mua hàng và nhập kho tự động
   - Thiết kế giao diện quản lý đơn mua hàng

## Những gì cần làm tiếp theo

1. **Hoàn thiện các Views**

   - Tối ưu hóa giao diện người dùng
   - Cải thiện hiển thị biểu đồ và thống kê trên dashboard
   - Thêm validation client-side
   - Thêm sorting và filtering cho các danh sách

2. **Phát triển các Controller nghiệp vụ chính**

   - Xây dựng SalesOrdersController và views liên quan
   - Xây dựng PurchaseOrdersController và views liên quan
   - Xây dựng InvoicesController và views liên quan
   - Xây dựng PaymentsController và views liên quan

3. **Xây dựng bảng điều khiển (Dashboard)**

   - Thiết kế HomeController với chức năng dashboard
   - Hiển thị các thông tin tổng quan về bán hàng
   - Hiển thị biểu đồ và thống kê

4. **Hoàn thiện hệ thống xác thực và phân quyền**

   - Tùy chỉnh Identity framework
   - Tạo và quản lý các roles cụ thể
   - Triển khai authorization policies cho từng chức năng

5. **Xây dựng chức năng báo cáo**
   - Phát triển các báo cáo bán hàng
   - Phát triển các báo cáo tài chính
   - Hỗ trợ xuất báo cáo ra PDF, Excel

## Vấn đề và giải pháp hiện tại

1. **Non-nullable reference types**

   - Vấn đề: Nhiều cảnh báo CS8618 về non-nullable properties chưa được khởi tạo
   - Giải pháp: Đã thêm constructor cho mỗi model class để khởi tạo các string properties thành string.Empty
   - Trạng thái: Đã giải quyết phần lớn, còn một số cảnh báo trong controllers

2. **Null reference exceptions trong Views**

   - Vấn đề: Một số views không xử lý đúng null references
   - Giải pháp: Sử dụng null conditional operators (?.) và null coalescing operators (??) để xử lý null values
   - Trạng thái: Đã có cải thiện đáng kể, tiếp tục hoàn thiện

3. **Vấn đề layout và giao diện người dùng**

   - Vấn đề: Nội dung bị khuất vào footer trên một số trang
   - Giải pháp: Đã điều chỉnh CSS trong \_Layout.cshtml, thêm padding và margin phù hợp
   - Trạng thái: Đã giải quyết vấn đề cơ bản, đang tiếp tục cải thiện

4. **Hiệu năng truy vấn dữ liệu**

   - Vấn đề: Các truy vấn Entity Framework có thể chậm khi dữ liệu lớn
   - Giải pháp: Tối ưu các Include và eager loading, áp dụng cơ chế caching
   - Trạng thái: Đã cải thiện một phần trong InventoryMovementsController với việc tối ưu truy vấn

5. **Tính toàn vẹn dữ liệu**

   - Vấn đề: Đảm bảo dữ liệu kế toán chính xác và nhất quán
   - Giải pháp: Áp dụng transactions cho các thao tác phức tạp, kiểm tra dữ liệu kỹ lưỡng
   - Trạng thái: Đã triển khai trong BatchStocktaking, cần mở rộng cho các chức năng khác

6. **.NET 10 Preview Issues**

   - Vấn đề: .NET 10 đang ở giai đoạn preview, có thể gặp một số vấn đề không tương thích
   - Giải pháp: Theo dõi các cập nhật từ Microsoft và cập nhật các packages cần thiết
   - Trạng thái: Đang theo dõi

7. **Vấn đề dữ liệu seed**

   - Vấn đề: Lỗi "NOT NULL constraint failed" khi seed dữ liệu ban đầu
   - Giải pháp: Đã thêm giá trị mặc định cho tất cả các trường NOT NULL trong phương thức Seed()
   - Trạng thái: Đã giải quyết

8. **Lỗi cú pháp trong views và controllers**
   - Vấn đề: Phát hiện và sửa lỗi cú pháp như thiếu dấu ngoặc trong ViewData["Title"] và sai giá trị enum
   - Giải pháp: Review code cẩn thận và sửa chữa các lỗi phát hiện được
   - Trạng thái: Đã sửa các lỗi được phát hiện, tiếp tục rà soát

## Lịch trình dự kiến

1. **Giai đoạn 1: Phân tích và thiết kế** - Đã hoàn thành 100%

   - Xác định yêu cầu chi tiết - Hoàn thành
   - Thiết kế module - Hoàn thành
   - Thiết kế cơ sở dữ liệu - Hoàn thành
   - Thiết kế model dữ liệu - Hoàn thành

2. **Giai đoạn 2: Phát triển core modules** - Đã hoàn thành 75%

   - Phát triển model dữ liệu - Hoàn thành 100%
   - Phát triển controllers cơ bản - Hoàn thành 85%
   - Phát triển views cơ bản - Hoàn thành 90%
   - Cải thiện trải nghiệm người dùng - Hoàn thành 80%
   - Phát triển module Quản lý khách hàng - Hoàn thành 95%
   - Phát triển module Quản lý nhà cung cấp - Hoàn thành 95%
   - Phát triển module Quản lý hàng hóa/dịch vụ - Hoàn thành 95%
   - Phát triển module Quản lý kho - Hoàn thành 85%
   - Phát triển module Quản lý hóa đơn bán hàng - Chưa bắt đầu
   - Phát triển module Quản lý thanh toán - Chưa bắt đầu
   - Phát triển module Báo cáo cơ bản - Đã hoàn thành 40% (báo cáo tồn kho)

3. **Giai đoạn 3: Phát triển modules bổ sung** - Chưa bắt đầu

   - Phát triển module Quản lý người dùng và phân quyền
   - Phát triển module Tích hợp hệ thống
   - Phát triển module Thiết lập hệ thống
   - Hoàn thiện báo cáo nâng cao

4. **Giai đoạn 4: Kiểm thử và triển khai** - Chưa bắt đầu
   - Kiểm thử tích hợp
   - Kiểm thử hệ thống
   - Triển khai thử nghiệm
   - Đào tạo người dùng
   - Go-live

## Thời gian dự kiến hoàn thành

Dự kiến hoàn thành toàn bộ dự án trong vòng 4-6 tháng tùy thuộc vào phức tạp của yêu cầu và sự phản hồi từ phía khách hàng.
