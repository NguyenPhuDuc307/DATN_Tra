using System.ComponentModel.DataAnnotations;

namespace DehaAccountingMvc.Models.Accounting
{
    public class BatchStocktakingViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; } = string.Empty;

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Danh mục")]
        public string CategoryName { get; set; } = string.Empty;

        [Display(Name = "Số lượng hiện tại")]
        public int CurrentQuantity { get; set; }

        [Display(Name = "Số lượng thực tế")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng thực tế")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm")]
        public int NewQuantity { get; set; }

        [Display(Name = "Chênh lệch")]
        public int Difference => NewQuantity - CurrentQuantity;
    }
}