using Demo_PRN231.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;

namespace Demo_PRN231.Controllers
{
	public class ProductController : Controller
	{
        private readonly ILogger<ProductController> _logger;
        MySaleDBContext context = new MySaleDBContext();

        public IActionResult Index()
		{
            List<Product> products = new List<Product>();
            if (products == null || products.Count == 0)
            {
                products = context.Products.Include(p => p.Category).ToList();
            }   
            List<Category> categories = context.Categories.ToList();
			ViewBag.listCate = categories;
            return View(products);
		}

        [HttpGet]
        public IActionResult FilterProducts(int category)
        {
            List<Product> productInDB = new List<Product>();
            if (category == 0)
            {
                productInDB = context.Products.Include(p => p.Category).ToList();
            }
            else
            {
                productInDB = context.Products.Include(p => p.Category).Where(p => p.CategoryId == category).ToList();
            }                     
            List<Category> categories = context.Categories.ToList();
            ViewBag.listCate = categories;
            TempData["listP"] = JsonConvert.SerializeObject(productInDB);
            TempData["selectedCategoryId"] = category;
            return Redirect("Index");
        }

        public IActionResult Delete(int id)
        {
            Product pro = context.Products.FirstOrDefault(p => p.ProductId == id);
            if(pro == null) return Redirect("Index"); 
            context.Products.Remove(pro);
            context.SaveChanges();
            return Redirect("Index");
        }

		public IActionResult SaveProduct()
		{
            string sPid = Request.Form["pid"];
            Console.WriteLine("pid" + sPid);
            int productId = sPid.Equals("") ? 0 : Convert.ToInt32(sPid);
            string productName = Request.Form["pname"];
            string sprice = Request.Form["price"];
            decimal price = sprice.Equals("") ? 0 : Convert.ToDecimal(sprice);
            string sStock = Request.Form["stock"];
            int stock = sStock.Equals("") ? 0 : Convert.ToInt32(sStock);
            string image = Request.Form["image"];
            int categoryId = Convert.ToInt32(Request.Form["categoryId"]);
            Product product = new Product
            {
                ProductId = productId,
                ProductName = productName,
                UnitPrice = price,
                UnitsInStock = stock,
                Image = image,
                CategoryId = categoryId
            };
            if(product.ProductId == 0)
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
            else
            {
                Product productInDB = context.Products.FirstOrDefault(p => p.ProductId == productId);
                if (productInDB == null) return Redirect("Index");
                productInDB.ProductName = productName;
                productInDB.UnitPrice = price;
                productInDB.UnitsInStock = stock;
                productInDB.CategoryId = categoryId;
                productInDB.Image = image;
                context.Products.Update(productInDB);
                context.SaveChanges();
            }
			return Redirect("Index");
		}

        public IActionResult ProductDetails(int id)
        {
            Product p = new Product();
            p = context.Products.FirstOrDefault(p => p.ProductId == id);
            TempData["product"] = JsonConvert.SerializeObject(p);
			return Redirect("Index");
        }
	}
}
