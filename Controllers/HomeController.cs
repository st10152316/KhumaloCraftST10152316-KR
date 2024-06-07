using KhumaloCraftST10152316.Models;
using KhumaloCraftST10152316.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KhumaloCraftST10152316.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sterm="",int ProductTypeId=0)
        {
            IEnumerable<Product> Products = await _homeRepository.GetProducts(sterm, ProductTypeId);
            IEnumerable<ProductType> ProductTypes = await _homeRepository.ProductTypes();
            ProductDisplayModel ProductModel = new ProductDisplayModel
            {
              Products=Products,
              ProductTypes=ProductTypes,
              STerm=sterm,
              ProductTypeId=ProductTypeId
            };
            return View(ProductModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}