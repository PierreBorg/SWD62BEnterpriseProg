using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class ProductsService : IProductsService
    {
        private IProductsRepository _productsRepo;

        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepo = productsRepository;
        }

        public ProductViewModel GetProduct(Guid id)
        {
            var myProduct = _productsRepo.GetProduct(id);
            ProductViewModel myModel = new ProductViewModel();
            myModel.Description = myProduct.Description;
            myModel.ImageUrl = myProduct.ImageUrl;
            myModel.Name = myProduct.Name;
            myModel.Price = myProduct.Price;
            myModel.Id = myProduct.Id;
            myModel.Category = new CategoryViewModel()
            {
                Id = myProduct.Category.Id,
                Name = myProduct.Category.Name
            };

            return myModel;
        }
        public IQueryable<ProductViewModel> GetProducts()
        {
            var list = from p in _productsRepo.GetProducts()
                       select new ProductViewModel()
                       {
                           Id = p.Id,
                           Description = p.Description,
                           Name = p.Name,
                           Price = p.Price,
                           Category = new CategoryViewModel() { Id = p.Category.Id, Name = p.Category.Name },
                           ImageUrl = p.ImageUrl
                       };
            return list;
        }

        public IQueryable<ProductViewModel> GetProducts(int category)
        {
            var list = from p in _productsRepo.GetProducts().Where(x => x.Category.Id == category)
                       select new ProductViewModel()
                       {
                           Id = p.Id,
                           Description = p.Description,
                           Name = p.Name,
                           Price = p.Price,
                           Category = new CategoryViewModel() { Id = p.Category.Id, Name = p.Category.Name }
                       };
            return list;
        }
    }
}
