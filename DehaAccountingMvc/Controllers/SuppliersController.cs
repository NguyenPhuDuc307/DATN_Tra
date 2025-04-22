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
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Suppliers.ToListAsync());
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .Include(s => s.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            // Chuẩn bị danh sách các loại nhà cung cấp cho dropdown
            ViewBag.SupplierTypes = Enum.GetValues(typeof(SupplierType))
                .Cast<SupplierType>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = GetEnumDisplayName(s)
                }).ToList();

            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierCode,Name,EnglishName,TaxCode,Address,Country,Province,District,Phone,Email,Website,ContactPerson,ContactPhone,ContactEmail,PaymentMethod,PaymentTerms,Notes,IsActive")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.CreatedDate = DateTime.Now;
                supplier.CreatedBy = User.Identity.Name;
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Chuẩn bị danh sách các loại nhà cung cấp cho dropdown
            ViewBag.SupplierTypes = Enum.GetValues(typeof(SupplierType))
                .Cast<SupplierType>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = GetEnumDisplayName(s)
                }).ToList();

            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            // Chuẩn bị danh sách các loại nhà cung cấp cho dropdown
            ViewBag.SupplierTypes = Enum.GetValues(typeof(SupplierType))
                .Cast<SupplierType>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = GetEnumDisplayName(s)
                }).ToList();

            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SupplierCode,Name,EnglishName,TaxCode,Address,Country,Province,District,Phone,Email,Website,ContactPerson,ContactPhone,ContactEmail,PaymentMethod,PaymentTerms,Notes,IsActive,CreatedDate,CreatedBy")] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    supplier.UpdatedDate = DateTime.Now;
                    supplier.UpdatedBy = User.Identity.Name;
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Id))
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

            // Chuẩn bị danh sách các loại nhà cung cấp cho dropdown
            ViewBag.SupplierTypes = Enum.GetValues(typeof(SupplierType))
                .Cast<SupplierType>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = GetEnumDisplayName(s)
                }).ToList();

            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem nhà cung cấp có liên quan đến dữ liệu nào không
            bool hasRelatedProducts = await _context.Products.AnyAsync(p => p.SupplierId == id);
            bool hasRelatedPurchaseOrders = await _context.PurchaseOrders.AnyAsync(p => p.SupplierId == id);
            bool hasRelatedPayments = await _context.Payments.AnyAsync(p => p.SupplierId == id);

            if (hasRelatedProducts || hasRelatedPurchaseOrders || hasRelatedPayments)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa nhà cung cấp này vì đã có dữ liệu liên quan.");
                var supplier = await _context.Suppliers.FindAsync(id);
                return View("Delete", supplier);
            }

            var supplierToDelete = await _context.Suppliers.FindAsync(id);
            if (supplierToDelete != null)
            {
                _context.Suppliers.Remove(supplierToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
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