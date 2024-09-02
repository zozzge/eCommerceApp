using Microsoft.AspNetCore.Mvc;
using eCommerceApp.Data;
using eCommerceApp.Models;
using System.Threading.Tasks;
using eCommerceApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using eCommerceApp.Services;
using eCommerceApp.ViewModels;
using System.Web;

namespace eCommerceApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string Username = "A49A8F35C812BD1CDEEB";
        private const string Token = "A49A8F35C812BD1CDEEBAF96992F145A";
        private const int KeywordId = 2;
        private const string RequestUrl = "https://testpos.payby.me/webpayment/Request.aspx";
        private const string PaymentPageUrl = "https://testpos.payby.me/webpayment/Pay.aspx";

        public PaymentController(ApplicationDbContext context,IPaymentService paymentService, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _paymentService = paymentService;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PaymentOptions()
        {
            var paymentOptions = await _paymentService.GetPaymentOptionsAsync();
            var totalPriceFromTempData = TempData["TotalPrice"] != null
        ? decimal.Parse(TempData["TotalPrice"].ToString().Trim('$')) // Remove currency symbol if needed
        : (decimal?)null;

            var viewModel = new PaymentOptionsViewModel
            {
                TotalPrice = totalPriceFromTempData,
                PaymentOptions = paymentOptions
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentWidget(PaymentRequestModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var requestUrl = "https://testpos.payby.me/webpayment/Request.aspx"; // Replace with actual PayByMe request URL

            var parameters = new Dictionary<string, string>
            {
                { "username", Username },
                { "token", Token },
                { "syncId", model.SyncId.ToString() },
                { "keywordId", KeywordId.ToString() },
                { "subCompany", model.SubCompany },
                { "assetName", model.AssetName },
                { "assetPrice", model.AssetPrice.ToString() },
                { "clientIp", model.ClientIp },
                { "countryCode", model.CountryCode },
                { "currencyCode", model.CurrencyCode },
                { "languageCode", model.LanguageCode },
                { "notifyPage", model.NotifyPage },
                { "redirectPage", model.RedirectPage },
                { "errorPage", model.ErrorPage },
                { "paymentType", model.PaymentType }
            };

            var encodedContent = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(requestUrl, encodedContent);
            var responseText = await response.Content.ReadAsStringAsync();

            var responseParams = HttpUtility.ParseQueryString(responseText);

            if (responseParams["Status"] == "1")
            {
                // Redirect to PaymentPage with the hash value
                var hash = responseParams["ErrorDesc"];
                return Redirect($"{PaymentPageUrl}?hash={hash}");
            }
            else
            {
                // Redirect to ErrorPage with error details
                return RedirectToAction("ErrorPage", new { errorCode = responseParams["ErrorCode"], errorDesc = responseParams["ErrorDesc"] });
            }
        }

        // Method to display the payment page
        public IActionResult PaymentPage(string hash)
        {
            // Pass the hash to the view for PayByMe payment page
            ViewBag.Hash = hash;
            return View();
        }

        // Method to handle the payment result
        public IActionResult RedirectPage()
        {
            // Handle successful payment result
            return View();
        }

        // Method to handle payment errors
        public IActionResult ErrorPage(string errorCode, string errorDesc)
        {
            ViewBag.ErrorCode = errorCode;
            ViewBag.ErrorDesc = errorDesc;
            return View();
        }

        // Method to handle payment notifications
        [HttpPost]
        public IActionResult NotifyPage()
        {
            // Acknowledge receipt of notification
            return Content("OK");
        }
        //public IActionResult WhiteLabel()
        //{
        //    var viewModel = new WhiteLabelViewModel(); // Adjust as needed
        //    return View("WhiteLabel", viewModel);
        //}

        //public IActionResult Widget()
        //{
        //    var viewModel = new WidgetViewModel(); // Adjust as needed
        //    return View("Widget", viewModel);
        //}


    }
}
