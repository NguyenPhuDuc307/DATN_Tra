using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DehaAccountingMvc.Models.Accounting
{
    public enum SupplierType
    {
        [Display(Name = "Cá nhân")]
        Individual = 0,

        [Display(Name = "Doanh nghiệp")]
        Business = 1,

        [Display(Name = "Nhà sản xuất")]
        Manufacturer = 2
    }

    public class Supplier
    {
        public Supplier()
        {
            SupplierCode = string.Empty;
            Name = string.Empty;
            EnglishName = string.Empty;
            TaxCode = string.Empty;
            Address = string.Empty;
            Province = string.Empty;
            District = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Website = string.Empty;
            ContactPerson = string.Empty;
            ContactPhone = string.Empty;
            ContactEmail = string.Empty;
            PaymentMethod = string.Empty;
            Notes = string.Empty;
            CreatedBy = "System";
            UpdatedBy = string.Empty;
            Products = new List<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã nhà cung cấp")]
        public string SupplierCode { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Tên nhà cung cấp")]
        public string Name { get; set; }

        [StringLength(200)]
        [Display(Name = "Tên tiếng Anh")]
        public string EnglishName { get; set; }

        [StringLength(20)]
        [Display(Name = "Mã số thuế")]
        public string TaxCode { get; set; }

        [StringLength(500)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [StringLength(100)]
        [Display(Name = "Quốc gia")]
        public string Country { get; set; } = "Việt Nam";

        [StringLength(100)]
        [Display(Name = "Tỉnh/Thành phố")]
        public string Province { get; set; }

        [StringLength(100)]
        [Display(Name = "Quận/Huyện")]
        public string District { get; set; }

        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(200)]
        [Display(Name = "Website")]
        [Url]
        public string Website { get; set; }

        [StringLength(100)]
        [Display(Name = "Người liên hệ")]
        public string ContactPerson { get; set; }

        [StringLength(20)]
        [Display(Name = "Số điện thoại liên hệ")]
        public string ContactPhone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email liên hệ")]
        public string ContactEmail { get; set; }

        [StringLength(50)]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Điều khoản thanh toán (ngày)")]
        public int PaymentTerms { get; set; } = 30;

        [StringLength(500)]
        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        [Display(Name = "Còn hoạt động")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string UpdatedBy { get; set; }

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; }
    }
}