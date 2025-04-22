using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DehaAccountingMvc.Models.Accounting
{
    public enum SalesOrderStatus
    {
        [Display(Name = "Đơn nháp")]
        Draft = 0,

        [Display(Name = "Đơn mới")]
        New = 1,

        [Display(Name = "Đã xác nhận")]
        Confirmed = 2,

        [Display(Name = "Đã giao hàng một phần")]
        PartiallyShipped = 3,

        [Display(Name = "Đã giao hàng đủ")]
        FullyShipped = 4,

        [Display(Name = "Đã xuất hóa đơn")]
        Invoiced = 5,

        [Display(Name = "Đã thanh toán một phần")]
        PartiallyPaid = 6,

        [Display(Name = "Đã thanh toán đủ")]
        FullyPaid = 7,

        [Display(Name = "Đã hủy")]
        Cancelled = 8,

        [Display(Name = "Đã trả hàng")]
        Returned = 9
    }

    public class SalesOrder
    {
        public SalesOrder()
        {
            OrderNumber = string.Empty;
            CustomerReferenceNumber = string.Empty;
            PaymentMethod = string.Empty;
            PaymentTerms = string.Empty;
            SalesPerson = string.Empty;
            ShippingAddress = string.Empty;
            Notes = string.Empty;
            CreatedBy = "System";
            UpdatedBy = string.Empty;
            ConfirmedBy = string.Empty;
            SalesOrderDetails = new List<SalesOrderDetail>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã đơn bán hàng")]
        public string OrderNumber { get; set; }

        [Required]
        [Display(Name = "Ngày đặt hàng")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Ngày dự kiến giao")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedShippingDate { get; set; }

        [Required]
        [Display(Name = "Khách hàng")]
        public int CustomerId { get; set; }

        [Display(Name = "Trạng thái")]
        public SalesOrderStatus Status { get; set; } = SalesOrderStatus.Draft;

        [Display(Name = "Số tham chiếu khách hàng")]
        [StringLength(50)]
        public string CustomerReferenceNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [StringLength(200)]
        [Display(Name = "Điều khoản thanh toán")]
        public string PaymentTerms { get; set; }

        [Display(Name = "Ngày giao hàng")]
        [DataType(DataType.Date)]
        public DateTime? ShippingDate { get; set; }

        [Display(Name = "Nhân viên bán hàng")]
        [StringLength(100)]
        public string SalesPerson { get; set; }

        [StringLength(200)]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

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

        [Display(Name = "Đã xuất hóa đơn")]
        public bool IsInvoiced { get; set; } = false;

        [Display(Name = "Mã hóa đơn")]
        public int? InvoiceId { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Ngày xác nhận")]
        public DateTime? ConfirmedDate { get; set; }

        [Display(Name = "Người xác nhận")]
        public string ConfirmedBy { get; set; }

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;

        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}