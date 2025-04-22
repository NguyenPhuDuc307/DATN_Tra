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
        public async Task<IActionResult> Create([Bind("ProductCode,Name,Description,ProductCategoryId,SupplierId,PurchasePrice,SellingPrice,DiscountPrice,Unit,StockQuantity,MinimumStockLevel,ReorderLevel,LeadTime,Brand,Origin,WarehouseLocation,Barcode,IsSellable,IsActive,IsService,TrackInventory,Notes")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedDate = DateTime.Now;
                product.CreatedBy = User.Identity.Name;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductCode,Name,Description,ProductCategoryId,SupplierId,PurchasePrice,SellingPrice,DiscountPrice,Unit,StockQuantity,MinimumStockLevel,ReorderLevel,LeadTime,Brand,Origin,WarehouseLocation,Barcode,IsSellable,IsActive,IsService,TrackInventory,Notes,CreatedDate,CreatedBy")] Product product)
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
            // Danh sách danh mục sản phẩm
            ViewBag.ProductCategories = new SelectList(
                await _context.ProductCategories.OrderBy(c => c.Name).ToListAsync(),
                "Id", "Name", product?.ProductCategoryId);

            // Danh sách nhà cung cấp
            ViewBag.Suppliers = new SelectList(
                await _context.Suppliers.OrderBy(s => s.Name).ToListAsync(),
                "Id", "Name", product?.SupplierId);

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