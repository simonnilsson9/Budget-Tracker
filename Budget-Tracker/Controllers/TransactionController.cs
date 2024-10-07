﻿using Budget_Tracker.Data;
using Budget_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        //Get: Transaction
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Transactions.Include(t => t.Category);
            return View(await appDbContext.ToListAsync());
        }

        //Get: Transaction/Details

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(m => m.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        //Get: Transaction/AddOrEdit
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            PopulateCategories();
            if (id == 0)
            {
                return View(new Transaction());
            }
            else
            {
                return View(_context.Transactions.Find(id));
            }
        }

        //Post: Transaction/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId, CategoryId, Amount, Note, Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (transaction.TransactionId == 0)
                {
                    _context.Add(transaction);
                }
                else
                {
                    _context.Update(transaction);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            PopulateCategories();
            return View(transaction);
        }

        //Get: Transaction/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(m => m.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);

        }

        //Post Transaction/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public void PopulateCategories()
        {
            var CategoryCollection = _context.Categories.ToList();
            Category defaultCategory = new Category()
            {
                CategoryId = 0,
                Title = "Choose a category"
            };

            CategoryCollection.Insert(0, defaultCategory);
            ViewBag.Categories = CategoryCollection;
        }
    }
}
