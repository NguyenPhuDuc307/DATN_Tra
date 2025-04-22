using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DehaAccountingMvc.Models.Accounting
{
    public enum PaymentType
    {
        [Display(Name = "Tiền mặt")]
        Cash = 0,

        [Display(Name = "Chuyển khoản")]
        BankTransfer = 1,

        [Display(Name = "Thẻ tín dụng")]
        CreditCard = 2,

        [Display(Name = "Ví điện tử")]
        EWallet = 3,

        [Display(Name = "Thẻ ghi nợ")]
        DebitCard = 4,

        [Display(Name = "Séc")]
        Cheque = 5,

        [Display(Name = "Khác")]
        Other = 6
    }

    public enum PaymentDirection
    {
        [Display(Name = "Tiền vào")]
        Incoming = 0,

        [Display(Name = "Tiền ra")]
        Outgoing = 1
    }

    public class Payment
    {
        public Payment()
        {
            PaymentCode = string.Empty;
            PayeeName = string.Empty;
            AccountNumber = string.Empty;
            BankName = string.Empty;
            TransactionReference = string.Empty;
            Notes = string.Empty;
            CreatedBy = "System";
            UpdatedBy = string.Empty;
            VerifiedBy = string.Empty;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã thanh toán")]
        public string PaymentCode { get; set; }

        [Required]
        [Display(Name = "Ngày thanh toán")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Display(Name = "Loại thanh toán")]
        public PaymentType PaymentType { get; set; } = PaymentType.Cash;

        [Display(Name = "Hướng thanh toán")]
        public PaymentDirection Direction { get; set; } = PaymentDirection.Incoming;

        [Display(Name = "Khách hàng")]
        public int? CustomerId { get; set; }

        [Display(Name = "Nhà cung cấp")]
        public int? SupplierId { get; set; }

        [Display(Name = "Hóa đơn")]
        public int? InvoiceId { get; set; }

        [Display(Name = "Đơn mua hàng")]
        public int? PurchaseOrderId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Số tiền")]
        public decimal Amount { get; set; }

        [StringLength(100)]
        [Display(Name = "Người nhận/Chi trả")]
        public string PayeeName { get; set; }

        [StringLength(50)]
        [Display(Name = "Số tài khoản")]
        public string AccountNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Ngân hàng")]
        public string BankName { get; set; }

        [StringLength(200)]
        [Display(Name = "Nội dung chuyển khoản")]
        public string TransactionReference { get; set; }

        [StringLength(500)]
        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        [Display(Name = "Đã chứng thực")]
        public bool IsVerified { get; set; } = false;

        [Display(Name = "Ngày chứng thực")]
        [DataType(DataType.Date)]
        public DateTime? VerificationDate { get; set; }

        [Display(Name = "Ngày xác minh")]
        public DateTime? VerifiedDate { get; set; }

        [Display(Name = "Người xác minh")]
        public string VerifiedBy { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; }

        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder? PurchaseOrder { get; set; }
    }
}