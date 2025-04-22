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
