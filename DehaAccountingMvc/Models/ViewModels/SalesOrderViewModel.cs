using System.Collections.Generic;
using DehaAccountingMvc.Models.Accounting;

namespace DehaAccountingMvc.Models.ViewModels
{
    public class SalesOrderViewModel
    {
        public SalesOrder SalesOrder { get; set; }
        public List<SalesOrderDetail> Details { get; set; } = new List<SalesOrderDetail>();
        public List<Product> AvailableProducts { get; set; } = new List<Product>();
    }

    public class CreateShipmentViewModel
    {
        public int SalesOrderId { get; set; }
        public string OrderNumber { get; set; }
        public List<SalesOrderDetailShipment> Details { get; set; } = new List<SalesOrderDetailShipment>();
    }

    public class SalesOrderDetailShipment
    {
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public int ShippedQuantity { get; set; }
        public int RemainingQuantity => Quantity - ShippedQuantity;
        public int QuantityToShip { get; set; }
        public int CurrentStock { get; set; }
        public bool IsService { get; set; }
    }
}