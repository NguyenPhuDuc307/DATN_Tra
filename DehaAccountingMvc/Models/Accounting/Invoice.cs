using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DehaAccountingMvc.Models.Accounting
{
    public enum InvoiceStatus
    {
        [Display(Name = "Đã tạo")]
        Created = 0,

        [Display(Name = "Đã gửi")]
        Sent = 1,

        [Display(Name = "Đã thanh toán một phần")]
        PartiallyPaid = 2,

        [Display(Name = "Đã thanh toán đủ")]
        FullyPaid = 3,

        [Display(Name = "Quá hạn")]
        Overdue = 4,

        [Display(Name = "Đã hủy")]
        Cancelled = 5
    }

    public class Invoice
    {
        public Invoice()
        {
            InvoiceNumber = string.Empty;
            ReferenceNumber = string.Empty;
            PaymentMethod = string.Empty;
            PaymentTerms = string.Empty;
            Notes = string.Empty;
            VoucherNumber = string.Empty;
            CreatedBy = "System";
            UpdatedBy = string.Empty;
            InvoiceDetails = new List<InvoiceDetail>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã hóa đơn")]
        public string InvoiceNumber { get; set; }

        [Required]
        [Display(Name = "Ngày hóa đơn")]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Display(Name = "Ngày đến hạn")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        [Display(Name = "Khách hàng")]
        public int CustomerId { get; set; }

        [Display(Name = "Đơn hàng")]
        public int? SalesOrderId { get; set; }

        [Display(Name = "Trạng thái")]
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Created;

        [Display(Name = "Mã tham chiếu")]
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [StringLength(200)]
        [Display(Name = "Điều khoản thanh toán")]
        public string PaymentTerms { get; set; }

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

        [Display(Name = "Ngày gửi")]
        public DateTime? SentDate { get; set; }

        [Display(Name = "Ngày thanh toán")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Mã chứng từ")]
        [StringLength(50)]
        public string VoucherNumber { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey("SalesOrderId")]
        public virtual SalesOrder? SalesOrder { get; set; }

        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}