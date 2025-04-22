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
    public class ProductCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductCategories
        public async Task<IActionResult> Index()
        {
            var productCategories = await _context.ProductCategories
                .Include(c => c.ParentCategory)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
            return View(productCategories);
        }

        // GET: ProductCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .Include(c => c.ParentCategory)
                .Include(c => c.ChildCategories)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Create
        public async Task<IActionResult> Create()
        {
            // Chuẩn bị danh sách danh mục cha cho dropdown
            var categories = await _context.ProductCategories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.ParentCategories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: ProductCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryCode,Name,Description,ParentCategoryId,DisplayOrder,IsActive")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                productCategory.CreatedDate = DateTime.Now;
                productCategory.CreatedBy = User.Identity.Name;
                _context.Add(productCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Chuẩn bị danh sách danh mục cha cho dropdown nếu ModelState không hợp lệ
            var categories = await _context.ProductCategories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.ParentCategories = new SelectList(categories, "Id", "Name", productCategory.ParentCategoryId);
            return View(productCategory);
        }

        // GET: ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            // Chuẩn bị danh sách danh mục cha cho dropdown, loại trừ danh mục hiện tại và con của nó
            var allCategories = await _context.ProductCategories.ToListAsync();
            var childCategories = GetAllChildCategories(allCategories, id.Value);
            var availableParentCategories = allCategories
                .Where(c => c.Id != id.Value && !childCategories.Contains(c.Id))
                .OrderBy(c => c.Name);

            ViewBag.ParentCategories = new SelectList(availableParentCategories, "Id", "Name", productCategory.ParentCategoryId);
            return View(productCategory);
        }

        // POST: ProductCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryCode,Name,Description,ParentCategoryId,DisplayOrder,IsActive,CreatedDate,CreatedBy")] ProductCategory productCategory)
        {
            if (id != productCategory.Id)
            {
                return NotFound();
            }

            // Kiểm tra xem danh mục cha có hợp lệ không (không phải là con của danh mục hiện tại)
            if (productCategory.ParentCategoryId.HasValue)
            {
                var allCategories = await _context.ProductCategories.ToListAsync();
                var childCategories = GetAllChildCategories(allCategories, id);

                if (childCategories.Contains(productCategory.ParentCategoryId.Value))
                {
                    ModelState.AddModelError("ParentCategoryId", "Không thể chọn danh mục con làm danh mục cha.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    productCategory.UpdatedDate = DateTime.Now;
                    productCategory.UpdatedBy = User.Identity.Name;
                    _context.Update(productCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(productCategory.Id))
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

            // Chuẩn bị danh sách danh mục cha cho dropdown nếu ModelState không hợp lệ
            var availableCategories = await _context.ProductCategories
                .Where(c => c.Id != id)
                .OrderBy(c => c.Name)
                .ToListAsync();
            ViewBag.ParentCategories = new SelectList(availableCategories, "Id", "Name", productCategory.ParentCategoryId);
            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .Include(c => c.ParentCategory)
                .Include(c => c.ChildCategories)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem danh mục có chứa danh mục con hoặc sản phẩm không
            var hasChildCategories = await _context.ProductCategories.AnyAsync(c => c.ParentCategoryId == id);
            var hasProducts = await _context.Products.AnyAsync(p => p.ProductCategoryId == id);

            if (hasChildCategories || hasProducts)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa danh mục này vì chứa danh mục con hoặc sản phẩm.");
                var productCategory = await _context.ProductCategories
                    .Include(c => c.ParentCategory)
                    .Include(c => c.ChildCategories)
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(m => m.Id == id);
                return View("Delete", productCategory);
            }

            var categoryToDelete = await _context.ProductCategories.FindAsync(id);
            if (categoryToDelete != null)
            {
                _context.ProductCategories.Remove(categoryToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExists(int id)
        {
            return _context.ProductCategories.Any(e => e.Id == id);
        }

        // Lấy tất cả các ID của danh mục con (bao gồm cả con của con)
        private HashSet<int> GetAllChildCategories(List<ProductCategory> allCategories, int parentId)
        {
            var result = new HashSet<int>();
            var directChildren = allCategories.Where(c => c.ParentCategoryId == parentId).Select(c => c.Id).ToList();

            foreach (var childId in directChildren)
            {
                result.Add(childId);
                var grandchildren = GetAllChildCategories(allCategories, childId);
                foreach (var grandchildId in grandchildren)
                {
                    result.Add(grandchildId);
                }
            }

            return result;
        }
    }
}