using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KhumaloCraftST10152316.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeRepository _ProductTypeRepo;

        public ProductTypeController(IProductTypeRepository ProductTypeRepo)
        {
            _ProductTypeRepo = ProductTypeRepo;
        }

        public async Task<IActionResult> Index()
        {
            var ProductTypes = await _ProductTypeRepo.GetProductTypes();
            return View(ProductTypes);
        }

        public IActionResult AddProductType()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductType(ProductTypeDTO ProductType)
        {
            if(!ModelState.IsValid)
            {
                return View(ProductType);
            }
            try
            {
                var ProductTypeToAdd = new ProductType { ProductTypeName = ProductType.ProductTypeName, Id = ProductType.Id };
                await _ProductTypeRepo.AddProductType(ProductTypeToAdd);
                TempData["successMessage"] = "ProductType added successfully";
                return RedirectToAction(nameof(AddProductType));
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = "ProductType could not added!";
                return View(ProductType);
            }

        }

        public async Task<IActionResult> UpdateProductType(int id)
        {
            var ProductType = await _ProductTypeRepo.GetProductTypeById(id);
            if (ProductType is null)
                throw new InvalidOperationException($"ProductType with id: {id} does not found");
            var ProductTypeToUpdate = new ProductTypeDTO
            {
                Id = ProductType.Id,
                ProductTypeName = ProductType.ProductTypeName
            };
            return View(ProductTypeToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductType(ProductTypeDTO ProductTypeToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(ProductTypeToUpdate);
            }
            try
            {
                var ProductType = new ProductType { ProductTypeName = ProductTypeToUpdate.ProductTypeName, Id = ProductTypeToUpdate.Id };
                await _ProductTypeRepo.UpdateProductType(ProductType);
                TempData["successMessage"] = "ProductType is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "ProductType could not updated!";
                return View(ProductTypeToUpdate);
            }

        }

        public async Task<IActionResult> DeleteProductType(int id)
        {
            var ProductType = await _ProductTypeRepo.GetProductTypeById(id);
            if (ProductType is null)
                throw new InvalidOperationException($"ProductType with id: {id} does not found");
            await _ProductTypeRepo.DeleteProductType(ProductType);
            return RedirectToAction(nameof(Index));

        }

    }
}
