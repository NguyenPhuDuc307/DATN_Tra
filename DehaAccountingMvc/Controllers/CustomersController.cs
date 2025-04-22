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
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            // Chuẩn bị danh sách các loại khách hàng cho dropdown
            ViewBag.CustomerTypes = Enum.GetValues(typeof(CustomerType))
                .Cast<CustomerType>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = GetEnumDisplayName(c)
                }).ToList();

            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerCode,Name,CustomerType,Address,Phone,Email,TaxCode,ContactPerson,BankAccount,BankName,BankBranch,PaymentTerms,CurrentDebt,DebtLimit,IsActive,Notes")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreatedDate = DateTime.Now;
                customer.CreatedBy = User.Identity.Name;
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Chuẩn bị danh sách các loại khách hàng cho dropdown
            ViewBag.CustomerTypes = Enum.GetValues(typeof(CustomerType))
                .Cast<CustomerType>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = GetEnumDisplayName(c)
                }).ToList();

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            // Chuẩn bị danh sách các loại khách hàng cho dropdown
            ViewBag.CustomerTypes = Enum.GetValues(typeof(CustomerType))
                .Cast<CustomerType>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = GetEnumDisplayName(c),
                    Selected = c == customer.CustomerType
                }).ToList();

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerCode,Name,CustomerType,Address,Phone,Email,TaxCode,ContactPerson,BankAccount,BankName,BankBranch,PaymentTerms,CurrentDebt,DebtLimit,IsActive,Notes,CreatedDate,CreatedBy")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.UpdatedDate = DateTime.Now;
                    customer.UpdatedBy = User.Identity.Name;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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

            // Chuẩn bị danh sách các loại khách hàng cho dropdown
            ViewBag.CustomerTypes = Enum.GetValues(typeof(CustomerType))
                .Cast<CustomerType>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = GetEnumDisplayName(c),
                    Selected = c == customer.CustomerType
                }).ToList();

            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem khách hàng có liên quan đến đơn hàng nào không
            bool hasRelatedOrders = await _context.SalesOrders.AnyAsync(s => s.CustomerId == id);
            bool hasRelatedInvoices = await _context.Invoices.AnyAsync(i => i.CustomerId == id);
            bool hasRelatedPayments = await _context.Payments.AnyAsync(p => p.CustomerId == id);

            if (hasRelatedOrders || hasRelatedInvoices || hasRelatedPayments)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa khách hàng này vì đã có dữ liệu liên quan.");
                var customer = await _context.Customers.FindAsync(id);
                return View("Delete", customer);
            }

            var customerToDelete = await _context.Customers.FindAsync(id);
            if (customerToDelete != null)
            {
                _context.Customers.Remove(customerToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
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