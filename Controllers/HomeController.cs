using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CurrencyRateApp.Models;

namespace CurrencyRateApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Конструктор для первой загрузки страницы, без указания параметров.
        /// </summary>
        public IActionResult Rates()
        {
            return View(new RatesViewModel());
        }

        /// <summary>
        /// Конструктор для повторной загрузки, с указанием параметров.
        /// </summary>
        [HttpPost]
        public IActionResult Rates(DateTime date, string visible)
        {
            return View(new RatesViewModel(date, visible));
        }

        /// <summary>
        /// Конструктор для первой загрузки страницы, без указания параметров.
        /// </summary>
        public IActionResult Dynamic()
        {
            return View(new DynamicViewModel());
        }

        /// <summary>
        /// Конструктор для повторной загрузки, с указанием параметров.
        /// </summary>
        [HttpPost]
        public IActionResult Dynamic(string code, DateTime startDate, DateTime endDate)
        {
            return View(new DynamicViewModel(code, startDate, endDate));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
