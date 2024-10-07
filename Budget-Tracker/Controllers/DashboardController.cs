using Budget_Tracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Transaction = Budget_Tracker.Models.Transaction;

namespace Budget_Tracker.Controllers
{
    public class DashboardController : Controller
    {

        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            //Last 7 days transactions
            DateTime StartDate = DateTime.Now.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> selectedTransactions = await _context.Transactions.Include(x => x.Category).Where(x => x.Date >= StartDate && x.Date <= EndDate).ToListAsync();


            //Total Income
            int TotalIncome = selectedTransactions.Where(x => x.Category.Type == "Income").Sum(x => x.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            //Total Expense
            int TotalExpense = selectedTransactions.Where(x => x.Category.Type == "Expense").Sum(x => x.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            //Balance
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("sv-SEK");

            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.TotalBalance = String.Format(culture, "{0:C0}", Balance);

            //Donut Chart - Expense by category
            ViewBag.DonutChart = selectedTransactions.Where(x => x.Category.Type == "Expense").GroupBy(x => x.Category.CategoryId)
                .Select(x => new
                {
                    CategoryTitleWithIcon = x.First().Category.Icon + " " + x.First().Category.Title,
                    Amount = x.Sum(x => x.Amount),
                    FormattedAmount = x.Sum(x => x.Amount).ToString("C0")
                }).OrderByDescending(x => x.Amount).ToList();



            return View();
        }
    }
}
