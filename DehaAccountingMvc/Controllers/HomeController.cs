using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DehaAccountingMvc.Models;
using DehaAccountingMvc.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DehaAccountingMvc.Models.Accounting;

namespace DehaAccountingMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            // Lấy số lượng từng loại dữ liệu
            ViewBag.ProductCount = await _context.Products.CountAsync();
            ViewBag.CustomerCount = await _context.Customers.CountAsync();
            ViewBag.SupplierCount = await _context.Suppliers.CountAsync();
            ViewBag.SalesOrderCount = await _context.SalesOrders.CountAsync();

            // Tính tổng doanh thu từ các đơn hàng đã xác nhận hoặc hoàn thành
            ViewBag.TotalRevenue = await _context.SalesOrders
                .Where(so => so.Status != SalesOrderStatus.Draft && so.Status != SalesOrderStatus.Cancelled)
                .SumAsync(so => so.GrandTotal);

            // Tính tổng chi phí từ các đơn mua hàng đã xác nhận hoặc hoàn thành
            ViewBag.TotalExpenses = await _context.PurchaseOrders
                .Where(po => po.Status != PurchaseOrderStatus.Cancelled)
                .SumAsync(po => po.GrandTotal);

            // Tính toán phần trăm thay đổi doanh thu và chi phí theo tháng
            var currentMonth = DateTime.Now;
            var lastMonth = currentMonth.AddMonths(-1);

            // Doanh thu tháng này
            var currentMonthRevenue = await _context.SalesOrders
                .Where(so => so.Status != SalesOrderStatus.Draft &&
                       so.Status != SalesOrderStatus.Cancelled &&
                       so.OrderDate.Month == currentMonth.Month &&
                       so.OrderDate.Year == currentMonth.Year)
                .SumAsync(so => so.GrandTotal);

            // Doanh thu tháng trước
            var lastMonthRevenue = await _context.SalesOrders
                .Where(so => so.Status != SalesOrderStatus.Draft &&
                       so.Status != SalesOrderStatus.Cancelled &&
                       so.OrderDate.Month == lastMonth.Month &&
                       so.OrderDate.Year == lastMonth.Year)
                .SumAsync(so => so.GrandTotal);

            // Tính phần trăm thay đổi doanh thu
            ViewBag.RevenueChangePercent = lastMonthRevenue > 0
                ? (int)((currentMonthRevenue - lastMonthRevenue) * 100 / lastMonthRevenue)
                : 0;
            ViewBag.RevenueIncreased = currentMonthRevenue >= lastMonthRevenue;

            // Chi phí tháng này
            var currentMonthExpenses = await _context.PurchaseOrders
                .Where(po => po.Status != PurchaseOrderStatus.Cancelled &&
                       po.OrderDate.Month == currentMonth.Month &&
                       po.OrderDate.Year == currentMonth.Year)
                .SumAsync(po => po.GrandTotal);

            // Chi phí tháng trước
            var lastMonthExpenses = await _context.PurchaseOrders
                .Where(po => po.Status != PurchaseOrderStatus.Cancelled &&
                       po.OrderDate.Month == lastMonth.Month &&
                       po.OrderDate.Year == lastMonth.Year)
                .SumAsync(po => po.GrandTotal);

            // Tính phần trăm thay đổi chi phí
            ViewBag.ExpensesChangePercent = lastMonthExpenses > 0
                ? (int)((currentMonthExpenses - lastMonthExpenses) * 100 / lastMonthExpenses)
                : 0;
            ViewBag.ExpensesDecreased = currentMonthExpenses <= lastMonthExpenses;

            // Lấy tổng giá trị tồn kho
            ViewBag.TotalInventoryValue = await _context.Products
                .Where(p => !p.IsService)
                .SumAsync(p => p.PurchasePrice * p.StockQuantity);

            // Lấy 5 sản phẩm có tồn kho nhiều nhất
            ViewBag.TopStockProducts = await _context.Products
                .Where(p => !p.IsService && p.StockQuantity > 0)
                .OrderByDescending(p => p.StockQuantity)
                .Take(5)
                .ToListAsync();

            // Lấy 5 sản phẩm có giá trị tồn kho lớn nhất - Sửa để tránh lỗi SQLite với decimal
            var productsWithStock = await _context.Products
                .Where(p => !p.IsService && p.StockQuantity > 0)
                .ToListAsync();

            ViewBag.TopValueProducts = productsWithStock
                .OrderByDescending(p => p.PurchasePrice * p.StockQuantity)
                .Take(5)
                .ToList();

            // Lấy 5 giao dịch kho gần đây nhất
            ViewBag.RecentInventoryMovements = await _context.InventoryMovements
                .Include(im => im.Product)
                .OrderByDescending(im => im.MovementDate)
                .Take(5)
                .ToListAsync();

            // Lấy 5 đơn hàng gần đây
            try
            {
                ViewBag.RecentSalesOrders = await _context.SalesOrders
                    .Include(so => so.Customer)
                    .OrderByDescending(so => so.OrderDate)
                    .Take(5)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy dữ liệu đơn hàng gần đây");
                ViewBag.RecentSalesOrders = new List<SalesOrder>();
            }

            // Lấy 5 hóa đơn gần đây
            try
            {
                ViewBag.RecentInvoices = await _context.Invoices
                    .Include(i => i.Customer)
                    .OrderByDescending(i => i.InvoiceDate)
                    .Take(5)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy dữ liệu hóa đơn gần đây");
                ViewBag.RecentInvoices = new List<Invoice>();
            }

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tải Dashboard");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
