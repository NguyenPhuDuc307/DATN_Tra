using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DehaAccountingMvc.Models.Accounting
{
    public enum CustomerType
    {
        [Display(Name = "Cá nhân")]
        Individual = 0,

        [Display(Name = "Doanh nghiệp")]
        Business = 1,

        [Display(Name = "Đại lý")]
        Distributor = 2
    }

    public class Customer
    {
        public Customer()
        {
            CustomerCode = string.Empty;
            Name = string.Empty;
            Address = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            TaxCode = string.Empty;
            ContactPerson = string.Empty;
            BankAccount = string.Empty;
            BankName = string.Empty;
            BankBranch = string.Empty;
            PaymentTerms = string.Empty;
            CreatedBy = "System";
            UpdatedBy = string.Empty;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã khách hàng")]
        public string CustomerCode { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Tên khách hàng")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Loại khách hàng")]
        public CustomerType CustomerType { get; set; } = CustomerType.Individual;

        [StringLength(300)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Phone]
        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Mã số thuế")]
        public string TaxCode { get; set; }

        [StringLength(100)]
        [Display(Name = "Người liên hệ")]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        [Display(Name = "Số tài khoản")]
        public string BankAccount { get; set; }

        [StringLength(100)]
        [Display(Name = "Tên ngân hàng")]
        public string BankName { get; set; }

        [StringLength(100)]
        [Display(Name = "Chi nhánh ngân hàng")]
        public string BankBranch { get; set; }

        [StringLength(200)]
        [Display(Name = "Điều khoản thanh toán")]
        public string PaymentTerms { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Công nợ hiện tại")]
        public decimal CurrentDebt { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Hạn mức công nợ")]
        public decimal DebtLimit { get; set; } = 0;

        [Display(Name = "Còn hoạt động")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string UpdatedBy { get; set; }

        // Navigation properties for sales orders, invoices, etc. will be added later
    }
}