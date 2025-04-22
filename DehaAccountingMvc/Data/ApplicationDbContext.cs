using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DehaAccountingMvc.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DehaAccountingMvc.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Khách hàng và nhà cung cấp
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    // Sản phẩm
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }

    // Đơn mua hàng
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

    // Đơn bán hàng
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }

    // Hóa đơn
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    // Thanh toán và kho
    public DbSet<Payment> Payments { get; set; }
    public DbSet<InventoryMovement> InventoryMovements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ProductCategory tự tham chiếu
        modelBuilder.Entity<ProductCategory>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.ChildCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // Product và ProductCategory
        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductCategory)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.ProductCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product và Supplier
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // PurchaseOrder và Supplier
        modelBuilder.Entity<PurchaseOrder>()
            .HasOne(po => po.Supplier)
            .WithMany()
            .HasForeignKey(po => po.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // PurchaseOrderDetail và PurchaseOrder
        modelBuilder.Entity<PurchaseOrderDetail>()
            .HasOne(pod => pod.PurchaseOrder)
            .WithMany(po => po.PurchaseOrderDetails)
            .HasForeignKey(pod => pod.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // PurchaseOrderDetail và Product
        modelBuilder.Entity<PurchaseOrderDetail>()
            .HasOne(pod => pod.Product)
            .WithMany(p => p.PurchaseOrderDetails)
            .HasForeignKey(pod => pod.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // SalesOrder và Customer
        modelBuilder.Entity<SalesOrder>()
            .HasOne(so => so.Customer)
            .WithMany()
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // SalesOrderDetail và SalesOrder
        modelBuilder.Entity<SalesOrderDetail>()
            .HasOne(sod => sod.SalesOrder)
            .WithMany(so => so.SalesOrderDetails)
            .HasForeignKey(sod => sod.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // SalesOrderDetail và Product
        modelBuilder.Entity<SalesOrderDetail>()
            .HasOne(sod => sod.Product)
            .WithMany(p => p.SalesOrderDetails)
            .HasForeignKey(sod => sod.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Invoice và Customer
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Customer)
            .WithMany()
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Invoice và SalesOrder
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.SalesOrder)
            .WithMany()
            .HasForeignKey(i => i.SalesOrderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // InvoiceDetail và Invoice
        modelBuilder.Entity<InvoiceDetail>()
            .HasOne(id => id.Invoice)
            .WithMany(i => i.InvoiceDetails)
            .HasForeignKey(id => id.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // InvoiceDetail và Product
        modelBuilder.Entity<InvoiceDetail>()
            .HasOne(id => id.Product)
            .WithMany()
            .HasForeignKey(id => id.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Payment và các bảng liên quan
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Customer)
            .WithMany()
            .HasForeignKey(p => p.CustomerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Supplier)
            .WithMany()
            .HasForeignKey(p => p.SupplierId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Invoice)
            .WithMany()
            .HasForeignKey(p => p.InvoiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.PurchaseOrder)
            .WithMany()
            .HasForeignKey(p => p.PurchaseOrderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // InventoryMovement và các bảng liên quan
        modelBuilder.Entity<InventoryMovement>()
            .HasOne(im => im.Product)
            .WithMany()
            .HasForeignKey(im => im.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryMovement>()
            .HasOne(im => im.PurchaseOrder)
            .WithMany()
            .HasForeignKey(im => im.PurchaseOrderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryMovement>()
            .HasOne(im => im.PurchaseOrderDetail)
            .WithMany()
            .HasForeignKey(im => im.PurchaseOrderDetailId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryMovement>()
            .HasOne(im => im.SalesOrder)
            .WithMany()
            .HasForeignKey(im => im.SalesOrderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryMovement>()
            .HasOne(im => im.SalesOrderDetail)
            .WithMany()
            .HasForeignKey(im => im.SalesOrderDetailId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }

    // Phương thức tạo dữ liệu mẫu
    public void Seed()
    {
        // Kiểm tra nếu đã có dữ liệu trong DB thì không seed nữa
        if (Customers.Any() || Suppliers.Any() || Products.Any())
        {
            return;
        }

        // 1. Seed danh mục sản phẩm
        var categories = new List<ProductCategory>
        {
            new ProductCategory
            {
                Name = "Phần cứng",
                Description = "Các thiết bị phần cứng máy tính",
                CategoryCode = "PC",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = string.Empty
            },
            new ProductCategory
            {
                Name = "Phần mềm",
                Description = "Các sản phẩm phần mềm và giấy phép",
                CategoryCode = "PM",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = string.Empty
            },
            new ProductCategory
            {
                Name = "Dịch vụ CNTT",
                Description = "Các dịch vụ về công nghệ thông tin",
                CategoryCode = "DV",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = string.Empty
            }
        };
        ProductCategories.AddRange(categories);
        SaveChanges();

        // 2. Seed nhà cung cấp
        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                SupplierCode = "DELL-VN",
                Name = "Dell Việt Nam",
                EnglishName = "Dell Vietnam",
                TaxCode = "0123456789",
                Address = "Tầng 19, Keangnam Landmark 72, Phạm Hùng, Hà Nội",
                Country = "Việt Nam",
                Province = "Hà Nội",
                District = "Nam Từ Liêm",
                Phone = "02473078889",
                Email = "contact@dell.com.vn",
                Website = "https://www.dell.com/vn",
                ContactPerson = "Nguyễn Văn A",
                ContactPhone = "0987654321",
                ContactEmail = "nguyenvana@dell.com.vn",
                PaymentMethod = "Chuyển khoản",
                PaymentTerms = 30,
                Notes = "Nhà cung cấp thiết bị Dell chính hãng",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = string.Empty
            },
            new Supplier
            {
                SupplierCode = "MICROSOFT-VN",
                Name = "Microsoft Việt Nam",
                EnglishName = "Microsoft Vietnam",
                TaxCode = "0123456788",
                Address = "Tầng 10, Tòa nhà CornerStone, 16 Phan Chu Trinh, Hoàn Kiếm, Hà Nội",
                Country = "Việt Nam",
                Province = "Hà Nội",
                District = "Hoàn Kiếm",
                Phone = "02439335000",
                Email = "contact@microsoft.com.vn",
                Website = "https://www.microsoft.com/vi-vn",
                ContactPerson = "Trần Thị B",
                ContactPhone = "0987654322",
                ContactEmail = "tranthib@microsoft.com.vn",
                PaymentMethod = "Chuyển khoản",
                PaymentTerms = 45,
                Notes = "Nhà cung cấp phần mềm và dịch vụ Microsoft",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = string.Empty
            }
        };
        Suppliers.AddRange(suppliers);
        SaveChanges();

        // 3. Seed khách hàng
        var customers = new List<Customer>
        {
            new Customer
            {
                CustomerCode = "CTY-ABC",
                Name = "Công ty TNHH ABC",
                CustomerType = CustomerType.Business,
                Address = "123 Đường Lê Lợi, Quận 1, TP.HCM",
                Phone = "02838123456",
                Email = "contact@abc.com.vn",
                TaxCode = "0312345678",
                ContactPerson = "Lê Văn C",
                BankAccount = "0123456789",
                BankName = "Vietcombank",
                BankBranch = "Chi nhánh Sở giao dịch",
                PaymentTerms = "Thanh toán trong vòng 30 ngày",
                DebtLimit = 100000000,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = string.Empty
            },
            new Customer
            {
                CustomerCode = "CTY-XYZ",
                Name = "Công ty Cổ phần XYZ",
                CustomerType = CustomerType.Business,
                Address = "456 Đường Nguyễn Huệ, Quận 1, TP.HCM",
                Phone = "02838123457",
                Email = "contact@xyz.com.vn",
                TaxCode = "0312345679",
                ContactPerson = "Phạm Thị D",
                BankAccount = "0123456790",
                BankName = "BIDV",
                BankBranch = "Chi nhánh TP.HCM",
                PaymentTerms = "Thanh toán trong vòng 15 ngày",
                DebtLimit = 50000000,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = string.Empty
            }
        };
        Customers.AddRange(customers);
        SaveChanges();

        // 4. Seed sản phẩm
        var products = new List<Product>
        {
            new Product
            {
                ProductCode = "LT-DELL-XPS13",
                Name = "Laptop Dell XPS 13",
                Description = "Laptop Dell XPS 13 mới nhất, Core i7, 16GB RAM, 512GB SSD",
                ProductCategoryId = categories[0].Id, // Phần cứng
                SupplierId = suppliers[0].Id, // Dell
                PurchasePrice = 28000000,
                SellingPrice = 32000000,
                DiscountPrice = 30500000,
                Unit = "Chiếc",
                StockQuantity = 10,
                MinimumStockLevel = 2,
                ReorderLevel = 5,
                LeadTime = 14,
                Brand = "Dell",
                Origin = "Mỹ",
                WarehouseLocation = "Kho HN-A1-01",
                Barcode = "8935236789012",
                IsSellable = true,
                IsActive = true,
                IsService = false,
                TrackInventory = true,
                Notes = "Sản phẩm cao cấp, bảo hành 24 tháng",
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = string.Empty
            },
            new Product
            {
                ProductCode = "MS-OFFICE-365",
                Name = "Microsoft Office 365 Business",
                Description = "Bộ phần mềm văn phòng Microsoft Office 365 cho doanh nghiệp, 1 năm",
                ProductCategoryId = categories[1].Id, // Phần mềm
                SupplierId = suppliers[1].Id, // Microsoft
                PurchasePrice = 2800000,
                SellingPrice = 3500000,
                DiscountPrice = 3300000,
                Unit = "Bản quyền",
                StockQuantity = 50,
                MinimumStockLevel = 10,
                ReorderLevel = 20,
                LeadTime = 7,
                Brand = "Microsoft",
                Origin = "Mỹ",
                WarehouseLocation = "Kho HN-B2-02",
                Barcode = "8935236789013",
                IsSellable = true,
                IsActive = true,
                IsService = true,
                TrackInventory = true,
                Notes = "Giấy phép sử dụng 1 năm cho 5 người dùng",
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = string.Empty
            },
            new Product
            {
                ProductCode = "DV-CNTT-HTKT",
                Name = "Dịch vụ hỗ trợ kỹ thuật CNTT",
                Description = "Dịch vụ hỗ trợ kỹ thuật và bảo trì hệ thống CNTT",
                ProductCategoryId = categories[2].Id, // Dịch vụ
                PurchasePrice = 0,
                SellingPrice = 500000,
                DiscountPrice = 450000,
                Unit = "Giờ",
                StockQuantity = 0,
                MinimumStockLevel = 0,
                ReorderLevel = 0,
                LeadTime = 1,
                Brand = "DEHA",
                Origin = "Việt Nam",
                IsSellable = true,
                IsActive = true,
                IsService = true,
                TrackInventory = false,
                Notes = "Dịch vụ hỗ trợ kỹ thuật tính theo giờ",
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = string.Empty
            }
        };
        Products.AddRange(products);
        SaveChanges();

        // 5. Seed phiếu nhập kho
        var inventoryMovements = new List<InventoryMovement>
        {
            new InventoryMovement
            {
                MovementCode = "IM-20230401-001",
                MovementDate = DateTime.Now.AddDays(-30),
                MovementType = MovementType.PurchaseReceive,
                ProductId = products[0].Id, // Laptop Dell
                Quantity = 10,
                BeforeQuantity = 0,
                AfterQuantity = 10,
                WarehouseLocation = "Kho HN-A1-01",
                Notes = "Nhập kho lần đầu từ Dell Vietnam",
                CreatedDate = DateTime.Now.AddDays(-30),
                CreatedBy = "System"
            },
            new InventoryMovement
            {
                MovementCode = "IM-20230401-002",
                MovementDate = DateTime.Now.AddDays(-30),
                MovementType = MovementType.PurchaseReceive,
                ProductId = products[1].Id, // Microsoft Office
                Quantity = 50,
                BeforeQuantity = 0,
                AfterQuantity = 50,
                WarehouseLocation = "Kho HN-B2-02",
                Notes = "Nhập kho lần đầu từ Microsoft Vietnam",
                CreatedDate = DateTime.Now.AddDays(-30),
                CreatedBy = "System"
            },
            new InventoryMovement
            {
                MovementCode = "IM-20230410-001",
                MovementDate = DateTime.Now.AddDays(-20),
                MovementType = MovementType.SalesShipment,
                ProductId = products[0].Id, // Laptop Dell
                Quantity = 2,
                BeforeQuantity = 10,
                AfterQuantity = 8,
                WarehouseLocation = "Kho HN-A1-01",
                Notes = "Xuất kho bán hàng cho Công ty TNHH ABC",
                CreatedDate = DateTime.Now.AddDays(-20),
                CreatedBy = "System"
            },
            new InventoryMovement
            {
                MovementCode = "IM-20230410-002",
                MovementDate = DateTime.Now.AddDays(-20),
                MovementType = MovementType.SalesShipment,
                ProductId = products[1].Id, // Microsoft Office
                Quantity = 5,
                BeforeQuantity = 50,
                AfterQuantity = 45,
                WarehouseLocation = "Kho HN-B2-02",
                Notes = "Xuất kho bán hàng cho Công ty TNHH ABC",
                CreatedDate = DateTime.Now.AddDays(-20),
                CreatedBy = "System"
            }
        };
        InventoryMovements.AddRange(inventoryMovements);
        SaveChanges();
    }
}
