using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DehaAccountingMvc.Models.Accounting
{
    public class ProductCategory
    {
        public ProductCategory()
        {
            CategoryCode = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            CreatedBy = "System";
            ChildCategories = new List<ProductCategory>();
            Products = new List<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã danh mục")]
        public string CategoryCode { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Danh mục cha")]
        public int? ParentCategoryId { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Còn hoạt động")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual ProductCategory? ParentCategory { get; set; }
        public virtual ICollection<ProductCategory> ChildCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}