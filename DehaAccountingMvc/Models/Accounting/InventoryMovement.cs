using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DehaAccountingMvc.Models.Accounting
{
    public enum MovementType
    {
        [Display(Name = "Nhập kho từ nhà cung cấp")]
        PurchaseReceive = 0,

        [Display(Name = "Xuất kho bán hàng")]
        SalesShipment = 1,

        [Display(Name = "Điều chỉnh tăng")]
        AdjustmentIn = 2,

        [Display(Name = "Điều chỉnh giảm")]
        AdjustmentOut = 3,

        [Display(Name = "Trả hàng cho nhà cung cấp")]
        ReturnToVendor = 4,

        [Display(Name = "Nhận hàng trả từ khách")]
        CustomerReturn = 5,

        [Display(Name = "Chuyển kho")]
        Transfer = 6,

        [Display(Name = "Kiểm kê")]
        Stocktaking = 7
    }

    public class InventoryMovement
    {
        public InventoryMovement()
        {
            MovementCode = string.Empty;
            WarehouseLocation = string.Empty;
            Notes = string.Empty;
            CreatedBy = "System";
            UpdatedBy = string.Empty;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã phiếu")]
        public string MovementCode { get; set; }

        [Required]
        [Display(Name = "Ngày thực hiện")]
        [DataType(DataType.Date)]
        public DateTime MovementDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Loại di chuyển")]
        public MovementType MovementType { get; set; }

        [Display(Name = "Sản phẩm")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Đơn mua hàng")]
        public int? PurchaseOrderId { get; set; }

        [Display(Name = "Chi tiết đơn mua")]
        public int? PurchaseOrderDetailId { get; set; }

        [Display(Name = "Đơn bán hàng")]
        public int? SalesOrderId { get; set; }

        [Display(Name = "Chi tiết đơn bán")]
        public int? SalesOrderDetailId { get; set; }

        [Display(Name = "Tồn kho trước")]
        public int BeforeQuantity { get; set; }

        [Display(Name = "Tồn kho sau")]
        public int AfterQuantity { get; set; }

        [StringLength(100)]
        [Display(Name = "Vị trí kho")]
        public string WarehouseLocation { get; set; }

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
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder? PurchaseOrder { get; set; }

        [ForeignKey("PurchaseOrderDetailId")]
        public virtual PurchaseOrderDetail? PurchaseOrderDetail { get; set; }

        [ForeignKey("SalesOrderId")]
        public virtual SalesOrder? SalesOrder { get; set; }

        [ForeignKey("SalesOrderDetailId")]
        public virtual SalesOrderDetail? SalesOrderDetail { get; set; }
    }
}