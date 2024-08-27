﻿using Microsoft.AspNetCore.Mvc;
using NexTrends.Models;

namespace NexTrends.Controllers
{
    public class AdminController : Controller
    {
        NexTrendsContext context = new NexTrendsContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Category()
        {
            Category cat = new Category();
            var context = new NexTrendsContext();
            IEnumerable<Category> categories=context.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        public IActionResult AddCategory(string CName)
        {
            string name = CName;
            Category category = new Category();
            var context = new NexTrendsContext();
            category.Name = name;
            context.Add(category);
            var re=context.SaveChanges();
            if (re > 0)
            {
                TempData["SuccessMessage"] = "Category added successfully!";
                return RedirectToAction("Category");
            }
            return RedirectToAction("Category");
        }

        public IActionResult Products(int Id)
        {
            int Cid = Convert.ToInt32(Id);
            Category cat=new Category();
            var context=new NexTrendsContext();
            cat = context.Categories.Find(Cid);
            return View(cat);
        }

        public IActionResult AddProduct(string Name,int Price,string Desc,int Qunatity,int Cid, IFormFile Image) 
        {
            Product pr = new Product();   
            var context = new NexTrendsContext();
            pr.Name = Name;
            pr.Price = Price;
            pr.Description = Desc;
            pr.Quantity = Qunatity;
            pr.CategoryId = Cid;
            if (Image != null && Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    Image.CopyTo(memoryStream);
                    pr.Image = memoryStream.ToArray(); 
                }
            }
            context.Products.Add(pr);
            var re=context.SaveChanges();
            if (re > 0)
            {
                TempData["SuccessMessage"] = "Product added successfully!";
                return RedirectToAction("Category");
            }
            return RedirectToAction("Category");
        }
        public IActionResult ProductsList(int id)
        {
            IEnumerable<Product> products = context.Products.Where(p => p.CategoryId == id).ToList();
            if (products.Any())
            {
                return View(products); 
            }
            else
            {
                ViewBag.Result = "There are no products in this category.";
                return View(); 
            }
        }

        public IActionResult GetImage(int id)
        {
            var product = context.Products
                .Where(p => p.Id == id)
                .Select(p => p.Image)
                .FirstOrDefault();

            if (product != null)
            {
                return File(product, "image/jpeg"); 
            }

            return NotFound();
        }

    }
}
