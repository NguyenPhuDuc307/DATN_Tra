using System;
using System.ComponentModel.DataAnnotations;

namespace DehaAccountingMvc.Models.Accounting
{
    public class StockReportViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; } = string.Empty;

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Danh mục")]
        public string CategoryName { get; set; } = string.Empty;

        [Display(Name = "Đơn giá")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Số lượng tồn")]
        public int StockQuantity { get; set; }

        [Display(Name = "Giá trị tồn kho")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal StockValue { get; set; }

        [Display(Name = "Tồn kho tối thiểu")]
        public int MinimumStockLevel { get; set; }

        [Display(Name = "Tồn kho thấp")]
        public bool IsLowStock { get; set; }
    }
}