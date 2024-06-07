using KhumaloCraftST10152316.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KhumaloCraftST10152316.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class ProductController : Controller
{
    private readonly IProductRepository _ProductRepo;
    private readonly IProductTypeRepository _ProductTypeRepo;
    private readonly IFileService _fileService;

    public ProductController(IProductRepository ProductRepo, IProductTypeRepository ProductTypeRepo, IFileService fileService)
    {
        _ProductRepo = ProductRepo;
        _ProductTypeRepo = ProductTypeRepo;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var Products = await _ProductRepo.GetProducts();
        return View(Products);
    }

    public async Task<IActionResult> AddProduct()
    {
        var ProductTypeSelectList = (await _ProductTypeRepo.GetProductTypes()).Select(ProductType => new SelectListItem
        {
            Text = ProductType.ProductTypeName,
            Value = ProductType.Id.ToString(),
        });
        ProductDTO ProductToAdd = new() { ProductTypeList = ProductTypeSelectList };
        return View(ProductToAdd);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductDTO ProductToAdd)
    {
        var ProductTypeSelectList = (await _ProductTypeRepo.GetProductTypes()).Select(ProductType => new SelectListItem
        {
            Text = ProductType.ProductTypeName,
            Value = ProductType.Id.ToString(),
        });
        ProductToAdd.ProductTypeList = ProductTypeSelectList;

        if (!ModelState.IsValid)
            return View(ProductToAdd);

        try
        {
            if (ProductToAdd.ImageFile != null)
            {
                if(ProductToAdd.ImageFile.Length> 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file can not exceed 1 MB");
                }
                string[] allowedExtensions = [".jpeg",".jpg",".png"];
                string imageName=await _fileService.SaveFile(ProductToAdd.ImageFile, allowedExtensions);
                ProductToAdd.Image = imageName;
            }
            // manual mapping of ProductDTO -> Product
            Product Product = new()
            {
                Id = ProductToAdd.Id,
                ProductName = ProductToAdd.ProductName,
                ProductDescription = ProductToAdd.ProductDescription,
                Image = ProductToAdd.Image,
                ProductTypeId = ProductToAdd.ProductTypeId,
                Price = ProductToAdd.Price
            };
            await _ProductRepo.AddProduct(Product);
            TempData["successMessage"] = "Product is added successfully";
            return RedirectToAction(nameof(AddProduct));
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"]= ex.Message;
            return View(ProductToAdd);
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(ProductToAdd);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on saving data";
            return View(ProductToAdd);
        }
    }

    public async Task<IActionResult> UpdateProduct(int id)
    {
        var Product = await _ProductRepo.GetProductById(id);
        if(Product==null)
        {
            TempData["errorMessage"] = $"Product with the id: {id} does not found";
            return RedirectToAction(nameof(Index));
        }
        var ProductTypeSelectList = (await _ProductTypeRepo.GetProductTypes()).Select(ProductType => new SelectListItem
        {
            Text = ProductType.ProductTypeName,
            Value = ProductType.Id.ToString(),
            Selected=ProductType.Id==Product.ProductTypeId
        });
        ProductDTO ProductToUpdate = new() 
        { 
            ProductTypeList = ProductTypeSelectList,
            ProductName=Product.ProductName,
            ProductDescription=Product.ProductDescription,
            ProductTypeId=Product.ProductTypeId,
            Price=Product.Price,
            Image=Product.Image 
        };
        return View(ProductToUpdate);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(ProductDTO ProductToUpdate)
    {
        var ProductTypeSelectList = (await _ProductTypeRepo.GetProductTypes()).Select(ProductType => new SelectListItem
        {
            Text = ProductType.ProductTypeName,
            Value = ProductType.Id.ToString(),
            Selected=ProductType.Id==ProductToUpdate.ProductTypeId
        });
        ProductToUpdate.ProductTypeList = ProductTypeSelectList;

        if (!ModelState.IsValid)
            return View(ProductToUpdate);

        try
        {
            string oldImage = "";
            if (ProductToUpdate.ImageFile != null)
            {
                if (ProductToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file can not exceed 1 MB");
                }
                string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
                string imageName = await _fileService.SaveFile(ProductToUpdate.ImageFile, allowedExtensions);
                // hold the old image name. Because we will delete this image after updating the new
                oldImage = ProductToUpdate.Image;
                ProductToUpdate.Image = imageName;
            }
            // manual mapping of ProductDTO -> Product
            Product Product = new()
            {
                Id=ProductToUpdate.Id,
                ProductName = ProductToUpdate.ProductName,
                ProductDescription = ProductToUpdate.ProductDescription,
                ProductTypeId = ProductToUpdate.ProductTypeId,
                Price = ProductToUpdate.Price,
                Image = ProductToUpdate.Image
            };
            await _ProductRepo.UpdateProduct(Product);
            // if image is updated, then delete it from the folder too
            if(!string.IsNullOrWhiteSpace(oldImage))
            {
                _fileService.DeleteFile(oldImage);
            }
            TempData["successMessage"] = "Product is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(ProductToUpdate);
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(ProductToUpdate);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on saving data";
            return View(ProductToUpdate);
        }
    }

    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var Product = await _ProductRepo.GetProductById(id);
            if (Product == null)
            {
                TempData["errorMessage"] = $"Product with the id: {id} does not found";
            }
            else
            {
                await _ProductRepo.DeleteProduct(Product);
                if (!string.IsNullOrWhiteSpace(Product.Image))
                {
                    _fileService.DeleteFile(Product.Image);
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on deleting the data";
        }
        return RedirectToAction(nameof(Index));
    }

}
