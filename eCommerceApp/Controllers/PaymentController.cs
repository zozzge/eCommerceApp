using Microsoft.AspNetCore.Mvc;
using eCommerceApp.Data;
using eCommerceApp.Models;
using System.Threading.Tasks;
using eCommerceApp.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PaymentOptions()
        {
            var paymentOptions = await _context.PaymentOptions.ToListAsync();
            return View(paymentOptions);
        }

        public IActionResult CreditCard()
        {
            return View();
        }

        public IActionResult PayPal()
        {
            return View();
        }

        public IActionResult BankTransfer()
        {
            return View();
        }
    }
}
