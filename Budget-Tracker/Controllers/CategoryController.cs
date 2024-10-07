using Budget_Tracker.Data;
using Budget_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        //Get: category
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        //Get: Category/Details

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //Get: Category/AddOrEdit
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Category());
            }
            else
            {
                return View(_context.Categories.Find(id));
            }
        }

        //Post: Category/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId, Title, Icon, Type")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.CategoryId == 0)
                {
                    _context.Add(category);
                }
                else
                {
                    _context.Update(category);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //Get: Category/Delete

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);

        }

        //Post Category/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if(category != null)
            {
                _context.Categories.Remove(category);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
