using Microsoft.AspNetCore.Mvc;
using eCommerceApp.Data;
using eCommerceApp.Models;
using System.Threading.Tasks;
using eCommerceApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using eCommerceApp.Services;

namespace eCommerceApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PaymentService _paymentService;

        public PaymentController(ApplicationDbContext context,PaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;


        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PaymentOptions()
        {
            var paymentOptions = await _paymentService.GetPaymentOptionsAsync();
            var totalPrice = await _paymentService.GetTotalPriceAsync();

            ViewBag.TotalPrice = totalPrice;
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
