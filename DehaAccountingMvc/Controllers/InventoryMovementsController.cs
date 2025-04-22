using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DehaAccountingMvc.Data;
using DehaAccountingMvc.Models.Accounting;
using Microsoft.AspNetCore.Authorization;

namespace DehaAccountingMvc.Controllers
{
    [Authorize]
    public class InventoryMovementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryMovementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InventoryMovements
        public async Task<IActionResult> Index()
        {
            var inventoryMovements = await _context.InventoryMovements
                .Include(i => i.Product)
                .Include(i => i.PurchaseOrder)
                .Include(i => i.SalesOrder)
                .OrderByDescending(i => i.MovementDate)
                .ThenByDescending(i => i.Id)
                .ToListAsync();
            return View(inventoryMovements);
        }

        // GET: InventoryMovements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryMovement = await _context.InventoryMovements
                .Include(i => i.Product)
                .Include(i => i.PurchaseOrder)
                .Include(i => i.PurchaseOrderDetail)
                .Include(i => i.SalesOrder)
                .Include(i => i.SalesOrderDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inventoryMovement == null)
            {
                return NotFound();
            }

            return View(inventoryMovement);
        }

        // GET: InventoryMovements/Create
        public async Task<IActionResult> Create()
        {
            await PrepareDropdownLists();
            var model = new InventoryMovement
            {
                MovementDate = DateTime.Now,
                MovementCode = GenerateMovementCode()
            };
            return View(model);
        }

        // POST: InventoryMovements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryMovement inventoryMovement)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin tồn kho hiện tại
                var product = await _context.Products.FindAsync(inventoryMovement.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError("ProductId", "Sản phẩm không tồn tại");
                    await PrepareDropdownLists(inventoryMovement);
                    return View(inventoryMovement);
                }

                // Lưu lại số lượng tồn trước khi thay đổi
                inventoryMovement.BeforeQuantity = product.StockQuantity;

                // Cập nhật số lượng tồn kho dựa vào loại di chuyển
                switch (inventoryMovement.MovementType)
                {
                    case MovementType.PurchaseReceive:
                    case MovementType.AdjustmentIn:
                    case MovementType.CustomerReturn:
                        product.StockQuantity += inventoryMovement.Quantity;
                        break;
                    case MovementType.SalesShipment:
                    case MovementType.AdjustmentOut:
                    case MovementType.ReturnToVendor:
                        // Kiểm tra nếu số lượng xuất lớn hơn tồn kho
                        if (product.StockQuantity < inventoryMovement.Quantity)
                        {
                            ModelState.AddModelError("Quantity", "Số lượng xuất kho lớn hơn số lượng tồn kho hiện tại");
                            await PrepareDropdownLists(inventoryMovement);
                            return View(inventoryMovement);
                        }
                        product.StockQuantity -= inventoryMovement.Quantity;
                        break;
                    case MovementType.Transfer:
                    case MovementType.Stocktaking:
                        // Kiểm kê hoặc chuyển kho - số lượng là chênh lệch
                        int difference = inventoryMovement.Quantity - product.StockQuantity;
                        product.StockQuantity = inventoryMovement.Quantity;
                        break;
                }

                // Lưu lại số lượng tồn sau khi thay đổi
                inventoryMovement.AfterQuantity = product.StockQuantity;

                // Thêm thông tin người tạo
                inventoryMovement.CreatedBy = User.Identity?.Name ?? "System";
                inventoryMovement.CreatedDate = DateTime.Now;

                _context.Add(inventoryMovement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PrepareDropdownLists(inventoryMovement);
            return View(inventoryMovement);
        }

        // GET: InventoryMovements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryMovement = await _context.InventoryMovements.FindAsync(id);
            if (inventoryMovement == null)
            {
                return NotFound();
            }

            await PrepareDropdownLists(inventoryMovement);
            return View(inventoryMovement);
        }

        // POST: InventoryMovements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InventoryMovement inventoryMovement)
        {
            if (id != inventoryMovement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy bản ghi hiện tại từ cơ sở dữ liệu
                    var existingMovement = await _context.InventoryMovements.FindAsync(id);
                    if (existingMovement == null)
                    {
                        return NotFound();
                    }

                    // Lấy thông tin sản phẩm
                    var product = await _context.Products.FindAsync(inventoryMovement.ProductId);
                    if (product == null)
                    {
                        ModelState.AddModelError("ProductId", "Sản phẩm không tồn tại");
                        await PrepareDropdownLists(inventoryMovement);
                        return View(inventoryMovement);
                    }

                    // Phục hồi số lượng tồn kho về trạng thái trước khi có bản ghi này
                    switch (existingMovement.MovementType)
                    {
                        case MovementType.PurchaseReceive:
                        case MovementType.AdjustmentIn:
                        case MovementType.CustomerReturn:
                            product.StockQuantity -= existingMovement.Quantity;
                            break;
                        case MovementType.SalesShipment:
                        case MovementType.AdjustmentOut:
                        case MovementType.ReturnToVendor:
                            product.StockQuantity += existingMovement.Quantity;
                            break;
                        case MovementType.Transfer:
                        case MovementType.Stocktaking:
                            product.StockQuantity = existingMovement.BeforeQuantity;
                            break;
                    }

                    // Áp dụng thay đổi mới
                    switch (inventoryMovement.MovementType)
                    {
                        case MovementType.PurchaseReceive:
                        case MovementType.AdjustmentIn:
                        case MovementType.CustomerReturn:
                            product.StockQuantity += inventoryMovement.Quantity;
                            break;
                        case MovementType.SalesShipment:
                        case MovementType.AdjustmentOut:
                        case MovementType.ReturnToVendor:
                            if (product.StockQuantity < inventoryMovement.Quantity)
                            {
                                ModelState.AddModelError("Quantity", "Số lượng xuất kho lớn hơn số lượng tồn kho hiện tại");
                                await PrepareDropdownLists(inventoryMovement);
                                return View(inventoryMovement);
                            }
                            product.StockQuantity -= inventoryMovement.Quantity;
                            break;
                        case MovementType.Transfer:
                        case MovementType.Stocktaking:
                            // Số lượng mới là giá trị nhập vào
                            product.StockQuantity = inventoryMovement.Quantity;
                            break;
                    }

                    // Cập nhật thông tin phiếu
                    existingMovement.MovementCode = inventoryMovement.MovementCode;
                    existingMovement.MovementDate = inventoryMovement.MovementDate;
                    existingMovement.MovementType = inventoryMovement.MovementType;
                    existingMovement.ProductId = inventoryMovement.ProductId;
                    existingMovement.Quantity = inventoryMovement.Quantity;
                    existingMovement.PurchaseOrderId = inventoryMovement.PurchaseOrderId;
                    existingMovement.PurchaseOrderDetailId = inventoryMovement.PurchaseOrderDetailId;
                    existingMovement.SalesOrderId = inventoryMovement.SalesOrderId;
                    existingMovement.SalesOrderDetailId = inventoryMovement.SalesOrderDetailId;
                    existingMovement.WarehouseLocation = inventoryMovement.WarehouseLocation;
                    existingMovement.Notes = inventoryMovement.Notes;
                    existingMovement.BeforeQuantity = inventoryMovement.BeforeQuantity;
                    existingMovement.AfterQuantity = product.StockQuantity;
                    existingMovement.UpdatedBy = User.Identity?.Name ?? "System";
                    existingMovement.UpdatedDate = DateTime.Now;

                    _context.Update(existingMovement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryMovementExists(inventoryMovement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            await PrepareDropdownLists(inventoryMovement);
            return View(inventoryMovement);
        }

        // GET: InventoryMovements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryMovement = await _context.InventoryMovements
                .Include(i => i.Product)
                .Include(i => i.PurchaseOrder)
                .Include(i => i.PurchaseOrderDetail)
                .Include(i => i.SalesOrder)
                .Include(i => i.SalesOrderDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inventoryMovement == null)
            {
                return NotFound();
            }

            return View(inventoryMovement);
        }

        // POST: InventoryMovements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventoryMovement = await _context.InventoryMovements.FindAsync(id);
            if (inventoryMovement != null)
            {
                // Lấy thông tin sản phẩm
                var product = await _context.Products.FindAsync(inventoryMovement.ProductId);
                if (product != null)
                {
                    // Hoàn tác thay đổi số lượng tồn kho
                    switch (inventoryMovement.MovementType)
                    {
                        case MovementType.PurchaseReceive:
                        case MovementType.AdjustmentIn:
                        case MovementType.CustomerReturn:
                            product.StockQuantity -= inventoryMovement.Quantity;
                            break;
                        case MovementType.SalesShipment:
                        case MovementType.AdjustmentOut:
                        case MovementType.ReturnToVendor:
                            product.StockQuantity += inventoryMovement.Quantity;
                            break;
                        case MovementType.Transfer:
                        case MovementType.Stocktaking:
                            // Khôi phục về số lượng trước khi thay đổi
                            product.StockQuantity = inventoryMovement.BeforeQuantity;
                            break;
                    }

                    _context.Products.Update(product);
                }

                _context.InventoryMovements.Remove(inventoryMovement);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool InventoryMovementExists(int id)
        {
            return _context.InventoryMovements.Any(e => e.Id == id);
        }

        private async Task PrepareDropdownLists(InventoryMovement movement = null)
        {
            // Danh sách sản phẩm
            ViewBag.Products = new SelectList(
                await _context.Products
                    .OrderBy(p => p.Name)
                    .Select(p => new { Id = p.Id, DisplayName = $"{p.ProductCode} - {p.Name}" })
                    .ToListAsync(),
                "Id", "DisplayName", movement?.ProductId);

            // Danh sách đơn mua hàng
            ViewBag.PurchaseOrders = new SelectList(
                await _context.PurchaseOrders
                    .OrderByDescending(p => p.OrderDate)
                    .Select(p => new { Id = p.Id, DisplayName = $"{p.OrderNumber} - {p.OrderDate:dd/MM/yyyy}" })
                    .ToListAsync(),
                "Id", "DisplayName", movement?.PurchaseOrderId);

            // Danh sách đơn bán hàng
            ViewBag.SalesOrders = new SelectList(
                await _context.SalesOrders
                    .OrderByDescending(s => s.OrderDate)
                    .Select(s => new { Id = s.Id, DisplayName = $"{s.OrderNumber} - {s.OrderDate:dd/MM/yyyy}" })
                    .ToListAsync(),
                "Id", "DisplayName", movement?.SalesOrderId);

            // Danh sách loại di chuyển
            ViewBag.MovementTypes = Enum.GetValues(typeof(MovementType))
                .Cast<MovementType>()
                .Select(m => new SelectListItem
                {
                    Value = ((int)m).ToString(),
                    Text = GetEnumDisplayName(m),
                    Selected = movement != null && m == movement.MovementType
                }).ToList();
        }

        private string GetEnumDisplayName(Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                .SingleOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;

            return displayAttribute?.Name ?? enumValue.ToString();
        }

        private string GenerateMovementCode()
        {
            // Format: IM-{YYYYMMDD}-{sequence}
            string datePrefix = DateTime.Now.ToString("yyyyMMdd");
            string baseCode = $"IM-{datePrefix}-";

            // Lấy số phiếu nhập xuất kho trong ngày
            int sequence = 1;
            var lastMovement = _context.InventoryMovements
                .Where(i => i.MovementCode.StartsWith(baseCode))
                .OrderByDescending(i => i.Id)
                .FirstOrDefault();

            if (lastMovement != null)
            {
                string[] parts = lastMovement.MovementCode.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastSequence))
                {
                    sequence = lastSequence + 1;
                }
            }

            return $"{baseCode}{sequence:D3}";
        }

        // GET: InventoryMovements/StockReport
        public async Task<IActionResult> StockReport()
        {
            var stockReport = await _context.Products
                .Include(p => p.ProductCategory)
                .OrderBy(p => p.ProductCategory.Name)
                .ThenBy(p => p.Name)
                .Select(p => new StockReportViewModel
                {
                    ProductId = p.Id,
                    ProductCode = p.ProductCode,
                    ProductName = p.Name,
                    CategoryName = p.ProductCategory.Name,
                    UnitPrice = p.SellingPrice,
                    StockQuantity = p.StockQuantity,
                    StockValue = p.StockQuantity * p.SellingPrice,
                    MinimumStockLevel = p.MinimumStockLevel,
                    IsLowStock = p.StockQuantity < p.MinimumStockLevel
                })
                .ToListAsync();

            return View(stockReport);
        }

        // GET: InventoryMovements/ProductHistory/5
        public async Task<IActionResult> ProductHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Product = product;

            var movements = await _context.InventoryMovements
                .Where(i => i.ProductId == id)
                .OrderByDescending(i => i.MovementDate)
                .ThenByDescending(i => i.Id)
                .ToListAsync();

            return View(movements);
        }

        // GET: InventoryMovements/BatchStocktaking
        public async Task<IActionResult> BatchStocktaking()
        {
            var products = await _context.Products
                .Include(p => p.ProductCategory)
                .Where(p => p.TrackInventory && !p.IsService)
                .OrderBy(p => p.ProductCategory.Name)
                .ThenBy(p => p.Name)
                .Select(p => new BatchStocktakingViewModel
                {
                    ProductId = p.Id,
                    ProductCode = p.ProductCode,
                    ProductName = p.Name,
                    CategoryName = p.ProductCategory.Name,
                    CurrentQuantity = p.StockQuantity,
                    NewQuantity = p.StockQuantity
                })
                .ToListAsync();

            return View(products);
        }

        // POST: InventoryMovements/BatchStocktaking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchStocktaking(List<BatchStocktakingViewModel> model)
        {
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    string batchCode = $"BKK-{DateTime.Now.ToString("yyyyMMdd")}-{Guid.NewGuid().ToString().Substring(0, 8)}";
                    foreach (var item in model)
                    {
                        if (item.NewQuantity != item.CurrentQuantity)
                        {
                            // Lấy thông tin sản phẩm
                            var product = await _context.Products.FindAsync(item.ProductId);
                            if (product != null)
                            {
                                // Tạo phiếu kiểm kê
                                var movement = new InventoryMovement
                                {
                                    MovementCode = $"{batchCode}-{item.ProductId}",
                                    MovementDate = DateTime.Now,
                                    MovementType = MovementType.Stocktaking,
                                    ProductId = item.ProductId,
                                    Quantity = item.NewQuantity,
                                    BeforeQuantity = product.StockQuantity,
                                    AfterQuantity = item.NewQuantity,
                                    Notes = $"Kiểm kê hàng loạt: {item.CurrentQuantity} -> {item.NewQuantity}",
                                    CreatedBy = User.Identity?.Name ?? "System",
                                    CreatedDate = DateTime.Now
                                };

                                // Cập nhật số lượng tồn kho
                                product.StockQuantity = item.NewQuantity;

                                _context.Products.Update(product);
                                _context.InventoryMovements.Add(movement);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi trong quá trình kiểm kê. Vui lòng thử lại.");
                }
            }

            return View(model);
        }
    }
}