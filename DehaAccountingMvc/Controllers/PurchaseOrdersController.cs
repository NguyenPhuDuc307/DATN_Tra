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
    public class PurchaseOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseOrders
        public async Task<IActionResult> Index()
        {
            var purchaseOrders = await _context.PurchaseOrders
                .Include(p => p.Supplier)
                .OrderByDescending(p => p.OrderDate)
                .ToListAsync();
            return View(purchaseOrders);
        }

        // GET: PurchaseOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseOrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/CreateForProduct
        public async Task<IActionResult> CreateForProduct(int? productId)
        {
            if (productId == null)
            {
                return RedirectToAction(nameof(Create));
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Tạo mã đơn hàng tự động format: PO-yyyyMMdd-nnn
            string prefix = "PO-" + DateTime.Now.ToString("yyyyMMdd");

            // Tìm số thứ tự lớn nhất trong ngày
            var lastOrder = await _context.PurchaseOrders
                .Where(p => p.OrderNumber.StartsWith(prefix))
                .OrderByDescending(p => p.OrderNumber)
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

            var purchaseOrder = new PurchaseOrder
            {
                OrderNumber = $"{prefix}-{sequenceNumber:D3}",
                OrderDate = DateTime.Now,
                Status = PurchaseOrderStatus.New,
                Notes = $"Tạo tự động cho sản phẩm {product.Name} (tồn kho thấp)"
            };

            // Thiết lập nhà cung cấp mặc định nếu sản phẩm có nhà cung cấp mặc định
            if (product.DefaultSupplierId.HasValue)
            {
                purchaseOrder.SupplierId = product.DefaultSupplierId.Value;
            }

            await PrepareDropdownLists(purchaseOrder);

            // Truyền thông tin sản phẩm để tự động thêm vào form
            ViewBag.PreselectedProduct = new
            {
                product.Id,
                product.Name,
                product.ProductCode,
                product.PurchasePrice,
                product.Unit,
                product.StockQuantity,
                product.IsService,
                product.WarehouseLocation,
                RecommendedQuantity = Math.Max(20 - product.StockQuantity, 10) // Đề xuất số lượng đặt hàng
            };

            return View("Create", purchaseOrder);
        }

        // GET: PurchaseOrders/Create
        public async Task<IActionResult> Create()
        {
            // Tạo mã đơn hàng tự động format: PO-yyyyMMdd-nnn
            string prefix = "PO-" + DateTime.Now.ToString("yyyyMMdd");

            // Tìm số thứ tự lớn nhất trong ngày
            var lastOrder = await _context.PurchaseOrders
                .Where(p => p.OrderNumber.StartsWith(prefix))
                .OrderByDescending(p => p.OrderNumber)
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

            var purchaseOrder = new PurchaseOrder
            {
                OrderNumber = $"{prefix}-{sequenceNumber:D3}",
                OrderDate = DateTime.Now,
                Status = PurchaseOrderStatus.New
            };

            await PrepareDropdownLists();
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderNumber,OrderDate,ExpectedDeliveryDate,SupplierId,Status,ReferenceNumber,PaymentMethod,PaymentTerms,DeliveryAddress,Notes")] PurchaseOrder purchaseOrder, int[] productId, int[] quantity, decimal[] unitPrice, decimal[] discountPercentage)
        {
            if (ModelState.IsValid)
            {
                // Thiết lập thông tin cơ bản cho đơn hàng
                purchaseOrder.CreatedDate = DateTime.Now;
                purchaseOrder.CreatedBy = User.Identity?.Name ?? "System";

                // Tạo danh sách chi tiết đơn hàng
                purchaseOrder.PurchaseOrderDetails = new List<PurchaseOrderDetail>();
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

                            var product = await _context.Products.FindAsync(productId[i]);
                            string warehouseLocation = product?.WarehouseLocation ?? "";

                            var detail = new PurchaseOrderDetail
                            {
                                ProductId = productId[i],
                                Quantity = quantity[i],
                                UnitPrice = unitPrice[i],
                                DiscountPercentage = discountPercentage[i],
                                DiscountAmount = itemDiscount,
                                SubTotal = itemSubTotal,
                                Total = itemTotal,
                                WarehouseLocation = warehouseLocation,
                                CreatedDate = DateTime.Now,
                                CreatedBy = User.Identity?.Name ?? "System"
                            };

                            purchaseOrder.PurchaseOrderDetails.Add(detail);
                            subTotal += itemTotal;
                        }
                    }
                }

                // Cập nhật tổng tiền
                purchaseOrder.SubTotal = subTotal;
                purchaseOrder.GrandTotal = subTotal;

                _context.Add(purchaseOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PrepareDropdownLists();
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.PurchaseOrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            // Chỉ cho phép chỉnh sửa nếu đơn hàng ở trạng thái New
            if (purchaseOrder.Status != PurchaseOrderStatus.New)
            {
                TempData["ErrorMessage"] = "Không thể chỉnh sửa đơn hàng ở trạng thái hiện tại";
                return RedirectToAction(nameof(Details), new { id = purchaseOrder.Id });
            }

            await PrepareDropdownLists(purchaseOrder);
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderNumber,OrderDate,ExpectedDeliveryDate,SupplierId,Status,ReferenceNumber,PaymentMethod,PaymentTerms,DeliveryAddress,Notes,CreatedDate,CreatedBy")] PurchaseOrder purchaseOrder, int[] detailId, int[] productId, int[] quantity, decimal[] unitPrice, decimal[] discountPercentage, string[] warehouseLocation)
        {
            if (id != purchaseOrder.Id)
            {
                return NotFound();
            }

            // Kiểm tra trạng thái đơn hàng
            var existingOrder = await _context.PurchaseOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingOrder != null && existingOrder.Status != PurchaseOrderStatus.New)
            {
                TempData["ErrorMessage"] = "Không thể chỉnh sửa đơn hàng ở trạng thái hiện tại";
                return RedirectToAction(nameof(Details), new { id = purchaseOrder.Id });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin đơn hàng
                    purchaseOrder.UpdatedDate = DateTime.Now;
                    purchaseOrder.UpdatedBy = User.Identity?.Name ?? "System";

                    // Xử lý chi tiết đơn hàng
                    var existingDetails = await _context.PurchaseOrderDetails
                        .Where(d => d.PurchaseOrderId == id)
                        .ToListAsync();

                    // Xóa các chi tiết không còn trong form
                    var detailsToDelete = existingDetails
                        .Where(d => detailId == null || !detailId.Contains(d.Id))
                        .ToList();

                    foreach (var detail in detailsToDelete)
                    {
                        _context.PurchaseOrderDetails.Remove(detail);
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

                                string itemWarehouseLocation = (warehouseLocation != null && i < warehouseLocation.Length)
                                    ? warehouseLocation[i]
                                    : "";

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
                                        existingDetail.WarehouseLocation = itemWarehouseLocation;
                                        existingDetail.UpdatedDate = DateTime.Now;
                                        existingDetail.UpdatedBy = User.Identity?.Name ?? "System";

                                        _context.Update(existingDetail);
                                    }
                                }
                                else
                                {
                                    // Thêm chi tiết mới
                                    var newDetail = new PurchaseOrderDetail
                                    {
                                        PurchaseOrderId = id,
                                        ProductId = productId[i],
                                        Quantity = quantity[i],
                                        UnitPrice = unitPrice[i],
                                        DiscountPercentage = discountPercentage[i],
                                        DiscountAmount = itemDiscount,
                                        SubTotal = itemSubTotal,
                                        Total = itemTotal,
                                        WarehouseLocation = itemWarehouseLocation,
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
                    purchaseOrder.SubTotal = subTotal;
                    purchaseOrder.GrandTotal = subTotal;

                    _context.Update(purchaseOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseOrderExists(purchaseOrder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = purchaseOrder.Id });
            }

            await PrepareDropdownLists(purchaseOrder);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseOrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem đơn hàng đã được xử lý chưa
            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.PurchaseOrderDetails)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            // Chỉ cho phép xóa nếu trạng thái là New
            if (purchaseOrder.Status != PurchaseOrderStatus.New)
            {
                TempData["ErrorMessage"] = "Không thể xóa đơn hàng đã được xử lý";
                return RedirectToAction(nameof(Details), new { id = purchaseOrder.Id });
            }

            // Xóa các chi tiết trước
            _context.PurchaseOrderDetails.RemoveRange(purchaseOrder.PurchaseOrderDetails);

            // Xóa đơn hàng
            _context.PurchaseOrders.Remove(purchaseOrder);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: PurchaseOrders/ApproveOrder/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders
                .FirstOrDefaultAsync(m => m.Id == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            // Chỉ có thể duyệt đơn hàng ở trạng thái New
            if (purchaseOrder.Status != PurchaseOrderStatus.New)
            {
                TempData["ErrorMessage"] = "Chỉ có thể duyệt đơn hàng ở trạng thái 'Đơn mới'";
                return RedirectToAction(nameof(Details), new { id = purchaseOrder.Id });
            }

            // Cập nhật trạng thái đơn hàng
            purchaseOrder.Status = PurchaseOrderStatus.Approved;
            purchaseOrder.ApprovedDate = DateTime.Now;
            purchaseOrder.ApprovedBy = User.Identity?.Name ?? "System";
            purchaseOrder.UpdatedDate = DateTime.Now;
            purchaseOrder.UpdatedBy = User.Identity?.Name ?? "System";

            _context.Update(purchaseOrder);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đơn hàng đã được duyệt thành công";
            return RedirectToAction(nameof(Details), new { id = purchaseOrder.Id });
        }

        // POST: PurchaseOrders/ReceiveItems/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceiveItems(int id, int[] detailId, int[] quantityToReceive)
        {
            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.PurchaseOrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            // Chỉ có thể nhận hàng nếu đơn hàng đã được duyệt
            if (purchaseOrder.Status != PurchaseOrderStatus.Approved &&
                purchaseOrder.Status != PurchaseOrderStatus.PartiallyReceived)
            {
                TempData["ErrorMessage"] = "Chỉ có thể nhận hàng khi đơn hàng đã được duyệt";
                return RedirectToAction(nameof(Details), new { id = purchaseOrder.Id });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Tạo phiếu nhập kho cho từng sản phẩm
                bool anyReceived = false;
                bool allReceived = true;

                for (int i = 0; i < detailId.Length; i++)
                {
                    if (quantityToReceive[i] <= 0)
                    {
                        continue;
                    }

                    var detail = purchaseOrder.PurchaseOrderDetails.FirstOrDefault(d => d.Id == detailId[i]);
                    if (detail == null)
                    {
                        continue;
                    }

                    var product = await _context.Products.FindAsync(detail.ProductId);
                    if (product == null)
                    {
                        continue;
                    }

                    // Kiểm tra số lượng có thể nhận
                    int remainingQty = detail.Quantity - detail.ReceivedQuantity;
                    if (quantityToReceive[i] > remainingQty)
                    {
                        TempData["ErrorMessage"] = $"Số lượng nhận vượt quá số lượng còn lại: {product.Name}";
                        return RedirectToAction(nameof(Details), new { id = purchaseOrder.Id });
                    }

                    // Cập nhật số lượng đã nhận
                    detail.ReceivedQuantity += quantityToReceive[i];
                    detail.UpdatedDate = DateTime.Now;
                    detail.UpdatedBy = User.Identity?.Name ?? "System";
                    _context.Update(detail);

                    // Cập nhật tồn kho
                    if (product.TrackInventory && !product.IsService)
                    {
                        // Tạo phiếu nhập kho
                        var inventoryMovement = new InventoryMovement
                        {
                            MovementCode = $"RECV-{DateTime.Now:yyyyMMdd}-{id}",
                            MovementDate = DateTime.Now,
                            MovementType = MovementType.PurchaseReceive,
                            ProductId = detail.ProductId,
                            Quantity = quantityToReceive[i],
                            BeforeQuantity = product.StockQuantity,
                            AfterQuantity = product.StockQuantity + quantityToReceive[i],
                            WarehouseLocation = detail.WarehouseLocation,
                            Notes = $"Nhập kho từ đơn hàng {purchaseOrder.OrderNumber}",
                            CreatedDate = DateTime.Now,
                            CreatedBy = User.Identity?.Name ?? "System"
                        };

                        _context.InventoryMovements.Add(inventoryMovement);

                        // Cập nhật số lượng tồn kho của sản phẩm
                        product.StockQuantity += quantityToReceive[i];
                        product.UpdatedDate = DateTime.Now;
                        product.UpdatedBy = User.Identity?.Name ?? "System";
                        _context.Update(product);
                    }

                    anyReceived = true;

                    // Kiểm tra xem đã nhận đủ số lượng chưa
                    if (detail.ReceivedQuantity < detail.Quantity)
                    {
                        allReceived = false;
                    }
                }

                // Cập nhật trạng thái đơn hàng
                if (anyReceived)
                {
                    purchaseOrder.Status = allReceived ? PurchaseOrderStatus.FullyReceived : PurchaseOrderStatus.PartiallyReceived;
                    purchaseOrder.ReceivedDate = DateTime.Now;
                    purchaseOrder.UpdatedDate = DateTime.Now;
                    purchaseOrder.UpdatedBy = User.Identity?.Name ?? "System";
                    _context.Update(purchaseOrder);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Nhập kho thành công";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = $"Lỗi khi tạo phiếu nhập kho: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id = purchaseOrder.Id });
        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrders.Any(e => e.Id == id);
        }

        private async Task PrepareDropdownLists(PurchaseOrder purchaseOrder = null)
        {
            // Danh sách nhà cung cấp
            ViewBag.Suppliers = new SelectList(
                await _context.Suppliers.OrderBy(s => s.Name).ToListAsync(),
                "Id", "Name", purchaseOrder?.SupplierId);

            // Danh sách phương thức thanh toán
            ViewBag.PaymentMethods = new List<SelectListItem>
            {
                new SelectListItem { Value = "Tiền mặt", Text = "Tiền mặt" },
                new SelectListItem { Value = "Chuyển khoản", Text = "Chuyển khoản" },
                new SelectListItem { Value = "Thẻ tín dụng", Text = "Thẻ tín dụng" },
                new SelectListItem { Value = "Công nợ 30 ngày", Text = "Công nợ 30 ngày" },
                new SelectListItem { Value = "Công nợ 45 ngày", Text = "Công nợ 45 ngày" },
                new SelectListItem { Value = "Công nợ 60 ngày", Text = "Công nợ 60 ngày" }
            };

            // Danh sách sản phẩm
            ViewBag.Products = await _context.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.ProductCode,
                    p.PurchasePrice,
                    p.Unit,
                    p.StockQuantity,
                    p.IsService,
                    p.WarehouseLocation
                })
                .ToListAsync();
        }
    }
}