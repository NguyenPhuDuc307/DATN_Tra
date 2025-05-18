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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Supplier)
                .ToListAsync();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            await PrepareDropdownLists();
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductCode,Name,Description,ProductCategoryId,SupplierId,DefaultSupplierId,PurchasePrice,SellingPrice,DiscountPrice,Unit,StockQuantity,MinimumStockLevel,ReorderLevel,LeadTime,Brand,Origin,WarehouseLocation,Barcode,IsSellable,IsActive,IsService,TrackInventory,Notes")] Product product)
        {
            // Debug - ghi log tất cả các giá trị
            Console.WriteLine("=============== FORM VALUES ===============");
            foreach (var key in Request.Form.Keys)
            {
                Console.WriteLine($"{key}: {Request.Form[key]}");
            }
            Console.WriteLine($"ProductCategoryId trong model: {product.ProductCategoryId}");
            
            // Debug - kiểm tra ModelState
            Console.WriteLine("=============== MODEL STATE ===============");
            foreach (var state in ModelState)
            {
                Console.WriteLine($"Key: {state.Key}, Valid: {state.Value.ValidationState}");
                if (state.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"  - Error: {error.ErrorMessage}");
                    }
                }
            }
            
            // Xử lý trường hợp ProductCategoryId có format không đúng (ví dụ: "1,1")
            string productCategoryIdStr = Request.Form["ProductCategoryId"].ToString();
            Console.WriteLine($"Raw ProductCategoryId from form: '{productCategoryIdStr}'");
            
            if (!string.IsNullOrEmpty(productCategoryIdStr))
            {
                if (productCategoryIdStr.Contains(","))
                {
                    // Lấy giá trị đầu tiên trước dấu phẩy
                    productCategoryIdStr = productCategoryIdStr.Split(',')[0];
                    Console.WriteLine($"Fixed ProductCategoryId after split: '{productCategoryIdStr}'");
                }
                
                if (int.TryParse(productCategoryIdStr, out int categoryId) && categoryId > 0)
                {
                    product.ProductCategoryId = categoryId;
                    Console.WriteLine($"Updated model ProductCategoryId: {product.ProductCategoryId}");
                }
            }
            
            // Xóa tất cả lỗi liên quan đến ProductCategory và ProductCategoryId
            ModelState.Remove("ProductCategory");
            ModelState.Remove("ProductCategoryId");
            
            // Thêm lại lỗi cho ProductCategoryId nếu cần thiết
            if (product.ProductCategoryId <= 0)
            {
                ModelState.AddModelError("ProductCategoryId", "Vui lòng chọn danh mục sản phẩm.");
            }
            
            // Kiểm tra danh mục có tồn tại không khi ProductCategoryId > 0
            if (product.ProductCategoryId > 0)
            {
                var categoryExists = await _context.ProductCategories.AnyAsync(c => c.Id == product.ProductCategoryId);
                if (!categoryExists)
                {
                    ModelState.AddModelError("ProductCategoryId", "Danh mục sản phẩm không tồn tại.");
                    Console.WriteLine($"Danh mục với ID={product.ProductCategoryId} không tồn tại");
                }
                else
                {
                    Console.WriteLine($"Đã xác thực danh mục ID={product.ProductCategoryId} tồn tại");
                }
            }
            
            // Xử lý ModelState
            if (ModelState.IsValid)
            {
                // Gán giá trị mặc định cho một số trường
                product.CreatedDate = DateTime.Now;
                product.CreatedBy = User.Identity.Name ?? "System";
                
                // Debug - thông báo trạng thái tạo sản phẩm
                Console.WriteLine($"Creating product: {product.Name} with CategoryId: {product.ProductCategoryId}");
                
                try 
                {
                    // Không cần tải ProductCategory trước khi tạo
                    // Entity Framework sẽ xử lý mối quan hệ dựa trên khóa ngoại
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sản phẩm đã được tạo thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra: {ex.Message}");
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
            
            // In các lỗi để debug
            Console.WriteLine("========== MODEL STATE ERRORS ==========");
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Field: {state.Key}, Error: {error.ErrorMessage}");
                }
            }

            // Khởi tạo lại các dropdown list
            await PrepareDropdownLists(product);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await PrepareDropdownLists(product);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductCode,Name,Description,ProductCategoryId,SupplierId,DefaultSupplierId,PurchasePrice,SellingPrice,DiscountPrice,Unit,StockQuantity,MinimumStockLevel,ReorderLevel,LeadTime,Brand,Origin,WarehouseLocation,Barcode,IsSellable,IsActive,IsService,TrackInventory,Notes,CreatedDate,CreatedBy")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.UpdatedDate = DateTime.Now;
                    product.UpdatedBy = User.Identity.Name;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

            await PrepareDropdownLists(product);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem sản phẩm có dữ liệu liên quan không
            bool hasRelatedSalesOrders = await _context.SalesOrderDetails.AnyAsync(s => s.ProductId == id);
            bool hasRelatedPurchaseOrders = await _context.PurchaseOrderDetails.AnyAsync(p => p.ProductId == id);
            bool hasRelatedInvoices = await _context.InvoiceDetails.AnyAsync(i => i.ProductId == id);
            bool hasRelatedInventoryMovements = await _context.InventoryMovements.AnyAsync(i => i.ProductId == id);

            if (hasRelatedSalesOrders || hasRelatedPurchaseOrders || hasRelatedInvoices || hasRelatedInventoryMovements)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa sản phẩm này vì đã có dữ liệu liên quan.");
                var product = await _context.Products
                    .Include(p => p.ProductCategory)
                    .Include(p => p.Supplier)
                    .FirstOrDefaultAsync(m => m.Id == id);
                return View("Delete", product);
            }

            var productToDelete = await _context.Products.FindAsync(id);
            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private async Task PrepareDropdownLists(Product product = null)
        {
            // Lấy danh sách danh mục sản phẩm từ database
            var categories = await _context.ProductCategories
                .Where(c => c.IsActive) // Chỉ lấy danh mục đang hoạt động
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
                
            // Debug: Kiểm tra danh sách danh mục
            Console.WriteLine($"Loaded {categories.Count} product categories");
            foreach (var cat in categories.Take(5)) // In ra 5 danh mục đầu tiên để debug
            {
                Console.WriteLine($"Category: Id={cat.Id}, Name={cat.Name}");
            }
            
            // Tạo SelectList với selectedValue là product?.ProductCategoryId
            // Sử dụng trực tiếp Id thay vì chuyển sang string để tránh vấn đề dấu phẩy
            ViewBag.ProductCategories = new SelectList(
                categories,
                "Id", 
                "Name", 
                product?.ProductCategoryId);

            // Danh sách nhà cung cấp
            var suppliers = await _context.Suppliers
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
                
            ViewBag.Suppliers = new SelectList(
                suppliers,
                "Id", 
                "Name", 
                product?.SupplierId);

            // Danh sách loại đơn vị
            ViewBag.UnitTypes = Enum.GetValues(typeof(UnitType))
                .Cast<UnitType>()
                .Select(u => new SelectListItem
                {
                    Value = GetEnumDisplayName(u),
                    Text = GetEnumDisplayName(u),
                    Selected = product != null && GetEnumDisplayName(u) == product.Unit
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
    }
}