using System;
using System.Collections.Generic;
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
    public class SalesOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SalesOrders
        public async Task<IActionResult> Index()
        {
            var salesOrders = await _context.SalesOrders
                .Include(s => s.Customer)
                .OrderByDescending(s => s.OrderDate)
                .ToListAsync();
            return View(salesOrders);
        }

        // GET: SalesOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.SalesOrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // GET: SalesOrders/Create
        public async Task<IActionResult> Create()
        {
            // Tạo mã đơn hàng tự động format: SO-yyyyMMdd-nnn
            string prefix = "SO-" + DateTime.Now.ToString("yyyyMMdd");

            // Tìm số thứ tự lớn nhất trong ngày
            var lastOrder = await _context.SalesOrders
                .Where(s => s.OrderNumber.StartsWith(prefix))
                .OrderByDescending(s => s.OrderNumber)
                .FirstOrDefaultAsync();

            int sequenceNumber = 1;
            if (lastOrder != null)
            {
                string sequencePart = lastOrder.OrderNumber.Substring(prefix.Length + 1);
                if (int.TryParse(sequencePart, out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }

            var salesOrder = new SalesOrder
            {
                OrderNumber = $"{prefix}-{sequenceNumber:D3}",
                OrderDate = DateTime.Now,
                Status = SalesOrderStatus.Draft
            };

            await PrepareDropdownLists();
            return View(salesOrder);
        }

        // POST: SalesOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderNumber,OrderDate,ExpectedShippingDate,CustomerId,Status,CustomerReferenceNumber,PaymentMethod,PaymentTerms,ShippingAddress,Notes")] SalesOrder salesOrder, int[] productId, int[] quantity, decimal[] unitPrice, decimal[] discountPercentage)
        {
            if (ModelState.IsValid)
            {
                // Thiết lập thông tin cơ bản cho đơn hàng
                salesOrder.CreatedDate = DateTime.Now;
                salesOrder.CreatedBy = User.Identity?.Name ?? "System";
                salesOrder.Status = SalesOrderStatus.New;

                // Tạo danh sách chi tiết đơn hàng
                salesOrder.SalesOrderDetails = new List<SalesOrderDetail>();
                decimal subTotal = 0;

                if (productId != null && productId.Length > 0)
                {
                    for (int i = 0; i < productId.Length; i++)
                    {
                        if (productId[i] > 0 && quantity[i] > 0)
                        {
                            decimal itemSubTotal = quantity[i] * unitPrice[i];
                            decimal itemDiscount = itemSubTotal * (discountPercentage[i] / 100);
                            decimal itemTotal = itemSubTotal - itemDiscount;

                            var detail = new SalesOrderDetail
                            {
                                ProductId = productId[i],
                                Quantity = quantity[i],
                                UnitPrice = unitPrice[i],
                                DiscountPercentage = discountPercentage[i],
                                DiscountAmount = itemDiscount,
                                SubTotal = itemSubTotal,
                                Total = itemTotal,
                                CreatedDate = DateTime.Now,
                                CreatedBy = User.Identity?.Name ?? "System"
                            };

                            salesOrder.SalesOrderDetails.Add(detail);
                            subTotal += itemTotal;
                        }
                    }
                }

                // Cập nhật tổng tiền
                salesOrder.SubTotal = subTotal;
                salesOrder.GrandTotal = subTotal;

                _context.Add(salesOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PrepareDropdownLists();
            return View(salesOrder);
        }

        // GET: SalesOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(s => s.SalesOrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            // Chỉ cho phép chỉnh sửa nếu đơn hàng ở trạng thái Draft hoặc New
            if (salesOrder.Status != SalesOrderStatus.Draft && salesOrder.Status != SalesOrderStatus.New)
            {
                TempData["ErrorMessage"] = "Không thể chỉnh sửa đơn hàng ở trạng thái hiện tại";
                return RedirectToAction(nameof(Details), new { id = salesOrder.Id });
            }

            await PrepareDropdownLists(salesOrder);
            return View(salesOrder);
        }

        // POST: SalesOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderNumber,OrderDate,ExpectedShippingDate,CustomerId,Status,CustomerReferenceNumber,PaymentMethod,PaymentTerms,ShippingAddress,Notes,CreatedDate,CreatedBy")] SalesOrder salesOrder, int[] detailId, int[] productId, int[] quantity, decimal[] unitPrice, decimal[] discountPercentage)
        {
            if (id != salesOrder.Id)
            {
                return NotFound();
            }

            // Kiểm tra trạng thái đơn hàng
            var existingOrder = await _context.SalesOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingOrder != null && existingOrder.Status != SalesOrderStatus.Draft && existingOrder.Status != SalesOrderStatus.New)
            {
                TempData["ErrorMessage"] = "Không thể chỉnh sửa đơn hàng ở trạng thái hiện tại";
                return RedirectToAction(nameof(Details), new { id = salesOrder.Id });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin đơn hàng
                    salesOrder.UpdatedDate = DateTime.Now;
                    salesOrder.UpdatedBy = User.Identity?.Name ?? "System";

                    // Xử lý chi tiết đơn hàng
                    var existingDetails = await _context.SalesOrderDetails
                        .Where(d => d.SalesOrderId == id)
                        .ToListAsync();

                    // Xóa các chi tiết không còn trong form
                    var detailsToDelete = existingDetails
                        .Where(d => detailId == null || !detailId.Contains(d.Id))
                        .ToList();

                    foreach (var detail in detailsToDelete)
                    {
                        _context.SalesOrderDetails.Remove(detail);
                    }

                    // Cập nhật hoặc thêm mới chi tiết
                    decimal subTotal = 0;

                    if (productId != null && productId.Length > 0)
                    {
                        for (int i = 0; i < productId.Length; i++)
                        {
                            if (productId[i] > 0 && quantity[i] > 0)
                            {
                                decimal itemSubTotal = quantity[i] * unitPrice[i];
                                decimal itemDiscount = itemSubTotal * (discountPercentage[i] / 100);
                                decimal itemTotal = itemSubTotal - itemDiscount;

                                if (i < (detailId?.Length ?? 0) && detailId[i] > 0)
                                {
                                    // Cập nhật chi tiết hiện có
                                    var existingDetail = existingDetails.FirstOrDefault(d => d.Id == detailId[i]);
                                    if (existingDetail != null)
                                    {
                                        existingDetail.ProductId = productId[i];
                                        existingDetail.Quantity = quantity[i];
                                        existingDetail.UnitPrice = unitPrice[i];
                                        existingDetail.DiscountPercentage = discountPercentage[i];
                                        existingDetail.DiscountAmount = itemDiscount;
                                        existingDetail.SubTotal = itemSubTotal;
                                        existingDetail.Total = itemTotal;
                                        existingDetail.UpdatedDate = DateTime.Now;
                                        existingDetail.UpdatedBy = User.Identity?.Name ?? "System";

                                        _context.Update(existingDetail);
                                    }
                                }
                                else
                                {
                                    // Thêm chi tiết mới
                                    var newDetail = new SalesOrderDetail
                                    {
                                        SalesOrderId = id,
                                        ProductId = productId[i],
                                        Quantity = quantity[i],
                                        UnitPrice = unitPrice[i],
                                        DiscountPercentage = discountPercentage[i],
                                        DiscountAmount = itemDiscount,
                                        SubTotal = itemSubTotal,
                                        Total = itemTotal,
                                        CreatedDate = DateTime.Now,
                                        CreatedBy = User.Identity?.Name ?? "System"
                                    };

                                    _context.Add(newDetail);
                                }

                                subTotal += itemTotal;
                            }
                        }
                    }

                    // Cập nhật tổng tiền
                    salesOrder.SubTotal = subTotal;
                    salesOrder.GrandTotal = subTotal;

                    _context.Update(salesOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderExists(salesOrder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = salesOrder.Id });
            }

            await PrepareDropdownLists(salesOrder);
            return View(salesOrder);
        }

        // GET: SalesOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.SalesOrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // POST: SalesOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem đơn hàng đã được xử lý chưa
            var salesOrder = await _context.SalesOrders
                .Include(s => s.SalesOrderDetails)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            // Chỉ cho phép xóa nếu trạng thái là Draft hoặc New
            if (salesOrder.Status != SalesOrderStatus.Draft && salesOrder.Status != SalesOrderStatus.New)
            {
                TempData["ErrorMessage"] = "Không thể xóa đơn hàng đã được xử lý";
                return RedirectToAction(nameof(Details), new { id = salesOrder.Id });
            }

            // Xóa các chi tiết trước
            _context.SalesOrderDetails.RemoveRange(salesOrder.SalesOrderDetails);

            // Xóa đơn hàng
            _context.SalesOrders.Remove(salesOrder);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: SalesOrders/ConfirmOrder/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var salesOrder = await _context.SalesOrders
                .Include(s => s.SalesOrderDetails)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            // Chỉ có thể xác nhận đơn hàng ở trạng thái New
            if (salesOrder.Status != SalesOrderStatus.New)
            {
                TempData["ErrorMessage"] = "Chỉ có thể xác nhận đơn hàng ở trạng thái 'Đơn mới'";
                return RedirectToAction(nameof(Details), new { id = salesOrder.Id });
            }

            // Cập nhật trạng thái đơn hàng
            salesOrder.Status = SalesOrderStatus.Confirmed;
            salesOrder.ConfirmedDate = DateTime.Now;
            salesOrder.ConfirmedBy = User.Identity?.Name ?? "System";
            salesOrder.UpdatedDate = DateTime.Now;
            salesOrder.UpdatedBy = User.Identity?.Name ?? "System";

            _context.Update(salesOrder);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đơn hàng đã được xác nhận thành công";
            return RedirectToAction(nameof(Details), new { id = salesOrder.Id });
        }

        // POST: SalesOrders/CreateShipment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShipment(int id, int[] detailId, int[] quantityToShip)
        {
            var salesOrder = await _context.SalesOrders
                .Include(s => s.SalesOrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            // Chỉ có thể tạo phiếu xuất kho nếu đơn hàng đã được xác nhận
            if (salesOrder.Status != SalesOrderStatus.Confirmed &&
                salesOrder.Status != SalesOrderStatus.PartiallyShipped)
            {
                TempData["ErrorMessage"] = "Chỉ có thể tạo phiếu xuất kho khi đơn hàng đã xác nhận";
                return RedirectToAction(nameof(Details), new { id = salesOrder.Id });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Tạo phiếu xuất kho cho từng sản phẩm
                bool anyShipped = false;
                bool allShipped = true;

                for (int i = 0; i < detailId.Length; i++)
                {
                    if (quantityToShip[i] <= 0)
                    {
                        continue;
                    }

                    var detail = salesOrder.SalesOrderDetails.FirstOrDefault(d => d.Id == detailId[i]);
                    if (detail == null)
                    {
                        continue;
                    }

                    var product = await _context.Products.FindAsync(detail.ProductId);
                    if (product == null)
                    {
                        continue;
                    }

                    // Kiểm tra số lượng có thể xuất
                    int remainingQty = detail.Quantity - detail.ShippedQuantity;
                    if (quantityToShip[i] > remainingQty)
                    {
                        TempData["ErrorMessage"] = $"Số lượng xuất vượt quá số lượng còn lại: {product.Name}";
                        return RedirectToAction(nameof(Details), new { id = salesOrder.Id });
                    }

                    // Kiểm tra tồn kho
                    if (product.TrackInventory && !product.IsService && quantityToShip[i] > product.StockQuantity)
                    {
                        TempData["ErrorMessage"] = $"Không đủ tồn kho cho sản phẩm: {product.Name}";
                        return RedirectToAction(nameof(Details), new { id = salesOrder.Id });
                    }

                    // Cập nhật số lượng đã giao
                    detail.ShippedQuantity += quantityToShip[i];
                    detail.UpdatedDate = DateTime.Now;
                    detail.UpdatedBy = User.Identity?.Name ?? "System";
                    _context.Update(detail);

                    // Cập nhật tồn kho
                    if (product.TrackInventory && !product.IsService)
                    {
                        // Tạo phiếu xuất kho
                        var inventoryMovement = new InventoryMovement
                        {
                            MovementCode = $"SHIP-{DateTime.Now:yyyyMMdd}-{id}",
                            MovementDate = DateTime.Now,
                            MovementType = MovementType.SalesShipment,
                            ProductId = detail.ProductId,
                            Quantity = quantityToShip[i],
                            BeforeQuantity = product.StockQuantity,
                            AfterQuantity = product.StockQuantity - quantityToShip[i],
                            WarehouseLocation = product.WarehouseLocation,
                            Notes = $"Xuất kho cho đơn hàng {salesOrder.OrderNumber}",
                            CreatedDate = DateTime.Now,
                            CreatedBy = User.Identity?.Name ?? "System"
                        };

                        _context.InventoryMovements.Add(inventoryMovement);

                        // Cập nhật số lượng tồn kho của sản phẩm
                        product.StockQuantity -= quantityToShip[i];
                        product.UpdatedDate = DateTime.Now;
                        product.UpdatedBy = User.Identity?.Name ?? "System";
                        _context.Update(product);
                    }

                    anyShipped = true;

                    // Kiểm tra xem đã giao đủ số lượng chưa
                    if (detail.ShippedQuantity < detail.Quantity)
                    {
                        allShipped = false;
                    }
                }

                // Cập nhật trạng thái đơn hàng
                if (anyShipped)
                {
                    salesOrder.Status = allShipped ? SalesOrderStatus.FullyShipped : SalesOrderStatus.PartiallyShipped;
                    salesOrder.ShippingDate = DateTime.Now;
                    salesOrder.UpdatedDate = DateTime.Now;
                    salesOrder.UpdatedBy = User.Identity?.Name ?? "System";
                    _context.Update(salesOrder);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Xuất kho thành công";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = $"Lỗi khi tạo phiếu xuất kho: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id = salesOrder.Id });
        }

        private bool SalesOrderExists(int id)
        {
            return _context.SalesOrders.Any(e => e.Id == id);
        }

        private async Task PrepareDropdownLists(SalesOrder salesOrder = null)
        {
            // Danh sách khách hàng
            ViewBag.Customers = new SelectList(
                await _context.Customers.OrderBy(c => c.Name).ToListAsync(),
                "Id", "Name", salesOrder?.CustomerId);

            // Danh sách phương thức thanh toán
            ViewBag.PaymentMethods = new List<SelectListItem>
            {
                new SelectListItem { Value = "Tiền mặt", Text = "Tiền mặt" },
                new SelectListItem { Value = "Chuyển khoản", Text = "Chuyển khoản" },
                new SelectListItem { Value = "Thẻ tín dụng", Text = "Thẻ tín dụng" },
                new SelectListItem { Value = "Ví điện tử", Text = "Ví điện tử" }
            };

            // Danh sách sản phẩm
            ViewBag.Products = await _context.Products
                .Where(p => p.IsActive && p.IsSellable)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.ProductCode,
                    p.SellingPrice,
                    p.Unit,
                    p.StockQuantity,
                    p.IsService
                })
                .ToListAsync();
        }
    }
}