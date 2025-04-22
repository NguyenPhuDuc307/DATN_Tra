using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DehaAccountingMvc.Models.Accounting
{
    public enum PurchaseOrderStatus
    {
        [Display(Name = "Đơn mới")]
        New = 0,

        [Display(Name = "Đã duyệt")]
        Approved = 1,

        [Display(Name = "Đã nhận hàng một phần")]
        PartiallyReceived = 2,

        [Display(Name = "Đã nhận hàng đủ")]
        FullyReceived = 3,

        [Display(Name = "Đã thanh toán một phần")]
        PartiallyPaid = 4,

        [Display(Name = "Đã thanh toán đủ")]
        FullyPaid = 5,

        [Display(Name = "Đã hủy")]
        Cancelled = 6
    }

    public class PurchaseOrder
    {
        public PurchaseOrder()
        {
            OrderNumber = string.Empty;
            ReferenceNumber = string.Empty;
            PaymentMethod = string.Empty;
            PaymentTerms = string.Empty;
            DeliveryAddress = string.Empty;
            Notes = string.Empty;
            CreatedBy = "System";
            UpdatedBy = string.Empty;
            ApprovedBy = string.Empty;
            PurchaseOrderDetails = new List<PurchaseOrderDetail>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã đơn mua hàng")]
        public string OrderNumber { get; set; }

        [Required]
        [Display(Name = "Ngày đặt hàng")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Ngày dự kiến nhận")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [Display(Name = "Nhà cung cấp")]
        public int SupplierId { get; set; }

        [Display(Name = "Trạng thái")]
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.New;

        [Display(Name = "Số tham chiếu")]
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [StringLength(200)]
        [Display(Name = "Điều khoản thanh toán")]
        public string PaymentTerms { get; set; }

        [Display(Name = "Ngày nhận hàng")]
        [DataType(DataType.Date)]
        public DateTime? ReceivedDate { get; set; }

        [StringLength(200)]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DeliveryAddress { get; set; }

        [StringLength(500)]
        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng tiền hàng")]
        public decimal SubTotal { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Chiết khấu")]
        public decimal Discount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Thuế")]
        public decimal Tax { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Phí vận chuyển")]
        public decimal ShippingFee { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng cộng")]
        public decimal GrandTotal { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Đã thanh toán")]
        public decimal AmountPaid { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Còn nợ")]
        public decimal AmountDue { get { return GrandTotal - AmountPaid; } }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Ngày duyệt")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Người duyệt")]
        public string ApprovedBy { get; set; }

        // Navigation properties
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; } = null!;

        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}