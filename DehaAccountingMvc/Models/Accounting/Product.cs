using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DehaAccountingMvc.Models.Accounting
{
    public enum ProductType
    {
        [Display(Name = "Hàng hóa")]
        Goods = 0,

        [Display(Name = "Dịch vụ")]
        Service = 1,

        [Display(Name = "Combo")]
        Combo = 2
    }

    public enum UnitType
    {
        [Display(Name = "Cái")]
        Piece = 0,

        [Display(Name = "Hộp")]
        Box = 1,

        [Display(Name = "Chai")]
        Bottle = 2,

        [Display(Name = "Kg")]
        Kilogram = 3,

        [Display(Name = "Gói")]
        Package = 4,

        [Display(Name = "Mét")]
        Meter = 5,

        [Display(Name = "Giờ")]
        Hour = 6
    }

    public class Product
    {
        public Product()
        {
            ProductCode = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Unit = "Cái";
            Brand = string.Empty;
            Origin = string.Empty;
            WarehouseLocation = string.Empty;
            Barcode = string.Empty;
            Notes = string.Empty;
            CreatedBy = "System";
            UpdatedBy = string.Empty;
            PurchaseOrderDetails = new List<PurchaseOrderDetail>();
            SalesOrderDetails = new List<SalesOrderDetail>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm")]
        [Display(Name = "Loại sản phẩm")]
        public int ProductCategoryId { get; set; }

        [Display(Name = "Nhà cung cấp")]
        public int? SupplierId { get; set; }

        [Display(Name = "Nhà cung cấp mặc định")]
        public int? DefaultSupplierId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá nhập")]
        public decimal PurchasePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá bán")]
        public decimal SellingPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá khuyến mãi")]
        public decimal? DiscountPrice { get; set; }

        [Display(Name = "Đơn vị tính")]
        [StringLength(20)]
        public string Unit { get; set; }

        [Display(Name = "Số lượng tồn kho")]
        public int StockQuantity { get; set; } = 0;

        [Display(Name = "Số lượng tối thiểu")]
        public int MinimumStockLevel { get; set; } = 10;

        [Display(Name = "Số lượng đặt hàng lại")]
        public int ReorderLevel { get; set; } = 20;

        [Display(Name = "Thời gian giao hàng (ngày)")]
        public int LeadTime { get; set; } = 7;

        [StringLength(100)]
        [Display(Name = "Thương hiệu")]
        public string Brand { get; set; }

        [StringLength(100)]
        [Display(Name = "Xuất xứ")]
        public string Origin { get; set; }

        [StringLength(100)]
        [Display(Name = "Vị trí kho")]
        public string WarehouseLocation { get; set; }

        [StringLength(50)]
        [Display(Name = "Mã vạch")]
        public string Barcode { get; set; }

        [Display(Name = "Có thể bán")]
        public bool IsSellable { get; set; } = true;

        [Display(Name = "Còn hoạt động")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Là sản phẩm dịch vụ")]
        public bool IsService { get; set; } = false;

        [Display(Name = "Có theo dõi tồn kho")]
        public bool TrackInventory { get; set; } = true;

        [StringLength(500)]
        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("ProductCategoryId")]
        public virtual ProductCategory ProductCategory { get; set; } = null!;

        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }

        // Các collection để liên kết đến các bảng khác
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}