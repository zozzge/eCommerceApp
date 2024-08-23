using Microsoft.AspNetCore.Mvc;
using eCommerceApp.Data;
using eCommerceApp.Models;
using System.Threading.Tasks;
using eCommerceApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace eCommerceApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PaymentOptions()
        {
            var paymentOptions = await _context.PaymentOptions.ToListAsync();
            return View(paymentOptions);
        }

        public IActionResult WhiteLabel()
        {
            return View();
        }

        public IActionResult Widget()
        {
            return View();
        }
    }
}
