using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;

namespace PresentationWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private IWebHostEnvironment _env;
        public ProductsController(IProductsService productsService, ICategoriesService categoriesService, IWebHostEnvironment env)
        {
            _productsService = productsService;
            _categoriesService = categoriesService;
            _env = env;
        }

        public IActionResult Index()
        {
            var list = _productsService.GetProducts();
            return View(list);
        }

        public IActionResult Details(Guid id)
        {
            var p = _productsService.GetProduct(id);
            return View(p);
        }
    
        //the engine will load a page with empty fields
        [HttpGet]
        [Authorize (Roles ="Admin")] //is going to be accessed only by authenticated users
        public IActionResult Create()
        {
            //fetch a list of categories
            var listOfCategories = _categoriesService.GetCategories();

            //we pass the categories to the page
            ViewBag.Categories = listOfCategories;

            return View();
        }

        //here details inputted by the user will be received
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductViewModel data, IFormFile f)
        {
            try
            {
                if(f != null)
                {
                    if(f.Length > 0)
                    {
                        //the newFileName is going to be stored in the database
                        string newFilename = Guid.NewGuid() + System.IO.Path.GetExtension(f.FileName);
                        
                        string newFilenameWithAbsolutePath = _env.WebRootPath + @"\Images\" + newFilename;
                        
                        using (var stream = System.IO.File.Create(newFilenameWithAbsolutePath))
                        {
                            f.CopyTo(stream);
                        }

                        data.ImageUrl = @"\Images\" + newFilename;
                    }
                }

                _productsService.AddProduct(data);

                TempData["feedback"] = "Product was added successfully";
            }
            catch(Exception)
            {
                //log error
                TempData["Error"] = "Product was not added!! ";
                return RedirectToAction("Error", "Home");
            }

            var listOfCategories = _categoriesService.GetCategories();

            ViewBag.Categories = listOfCategories;

            return View(data);

        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult Hide(Guid id)
        {
            try
            {
                _productsService.HideProduct(id);
                TempData["feedback"] = "Product was hidden";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Product was not hidden!! ";
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Show(Guid id)
        {
            try
            {
                _productsService.UnHideProduct(id);
                TempData["feedback"] = "Product was unhidden";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Product was not hidden!! ";
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _productsService.DeleteProduct(id);
                TempData["feedback"] = "Product was deleted";
            }
            catch (Exception ex)
            {
                //log your error
                TempData["Error"] = "Product was not deleted!! ";
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index");
        }
    }
}
