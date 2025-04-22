using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DehaAccountingMvc.Models.Accounting
{
    public class SalesOrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Đơn bán hàng")]
        public int SalesOrderId { get; set; }

        [Required]
        [Display(Name = "Sản phẩm")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Chiết khấu (%)")]
        public decimal DiscountPercentage { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Chiết khấu (tiền)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Thuế (%)")]
        public decimal TaxRate { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Thuế (tiền)")]
        public decimal TaxAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Thành tiền")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng cộng")]
        public decimal Total { get; set; }

        [Display(Name = "Số lượng đã giao")]
        public int ShippedQuantity { get; set; } = 0;

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
        public string? UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("SalesOrderId")]
        public virtual SalesOrder SalesOrder { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        // Calculated properties for display
        [NotMapped]
        public decimal RemainingQuantity
        {
            get { return Quantity - ShippedQuantity; }
        }
    }
}