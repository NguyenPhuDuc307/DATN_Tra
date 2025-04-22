using System.Collections.Generic;
using DehaAccountingMvc.Models.Accounting;

namespace DehaAccountingMvc.Models.ViewModels
{
    public class PurchaseOrderViewModel
    {
        public PurchaseOrder PurchaseOrder { get; set; }
        public List<PurchaseOrderDetail> Details { get; set; } = new List<PurchaseOrderDetail>();
        public List<Product> AvailableProducts { get; set; } = new List<Product>();
    }

    public class ReceiveItemsViewModel
    {
        public int PurchaseOrderId { get; set; }
        public string OrderNumber { get; set; }
        public List<PurchaseOrderDetailReceipt> Details { get; set; } = new List<PurchaseOrderDetailReceipt>();
    }

    public class PurchaseOrderDetailReceipt
    {
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public int ReceivedQuantity { get; set; }
        public int RemainingQuantity => Quantity - ReceivedQuantity;
        public int QuantityToReceive { get; set; }
        public string WarehouseLocation { get; set; }
    }
}