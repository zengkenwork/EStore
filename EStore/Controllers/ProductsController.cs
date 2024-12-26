using EStore.Models.EFModels;
using EStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace EStore.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(ProductFilterVm vm)
        {
            IQueryable<Product> products;
            List<Product> data;
            using (var db = new AppDbContext())
            {
                products = db.Products.AsNoTracking().Include(x => x.Category);

                if (string.IsNullOrEmpty(vm.Name) == false) products = products.Where(x => x.Name.Contains(vm.Name));
                if (vm.PriceStart.HasValue) products = products.Where(x => x.Price >= vm.PriceStart);
                if (vm.PriceEnd.HasValue) products = products.Where(x => x.Price <= vm.PriceEnd);

                data = products.ToList();
            }

            List<ProductIndexVm> model = new List<ProductIndexVm>();
            foreach (var product in data)
            {
                var p = new ProductIndexVm
                {
                    Id = product.Id,
                    CategoryName = product.Category.Name,
                    Name = product.Name,
                    Price = product.Price
                };
                model.Add(p);
            }

           vm.Data = model;

            return View(vm);
        }
    }
}