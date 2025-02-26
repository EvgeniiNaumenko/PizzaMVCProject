﻿using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzaMVCProject.Data;
using PizzaMVCProject.Interfaces;
using PizzaMVCProject.Models;
using PizzaMVCProject.Models.Pages;
using PizzaMVCProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace PizzaMVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProduct _products;
        private readonly ApplicationContext _context;
        private readonly ICategory _categories;
        public HomeController(IProduct product, ApplicationContext context, ICategory categories)
        {
            _products = product;
            _context = context;
            _categories = categories;
        }


        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Index(QueryOptions options, int categoryId)
        {
            if (categoryId != 0)
            {
                ViewBag.CategoryId = categoryId;

                var currentCategory = await _categories.GetCategoryAsync(categoryId);
                if (currentCategory != null)
                {
                    ViewData["Title"] = currentCategory.Name;
                }

                return View(_products.GetAllProductsByCategoryWithRatings(options, categoryId));

            }
            else
            {
                ViewData["Title"] = "Ãëàâíàÿ";
                return View(_products.GetAllProductsWithRelations(options));
            }
        }



        [Route("/product")]
        [HttpGet]
        public async Task<IActionResult> GetProduct(int productId, string? returnUrl)
        {
            if (productId != 0)
            {
                var currentProduct = await _products.GetProductWithCategoryAndRatingAsync(productId);
                if (currentProduct != null)
                {
                    ViewBag.CategoryId = double.NaN;
                    return View(new CurrentProductViewModel
                    {
                        Product = currentProduct,
                        ReturnUrl = returnUrl
                    });
                }
            }
            return NotFound();
        }


        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Rate(int productId, int rating)
        {
            var product = await _context.Products
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingRating = product.Ratings
                .FirstOrDefault(r => r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.RatingValue = rating;
                existingRating.RatedAt = DateTime.UtcNow;
            }
            else
            {
                product.Ratings.Add(new Rating
                {
                    ProductId = productId,
                    UserId = userId,
                    RatingValue = rating,
                    RatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            var newAverage = product.Ratings.Any()
                ? Math.Round(product.Ratings.Average(r => r.RatingValue), 1)
                : 0;

            return Json(new
            {
                success = true,
                newRating = newAverage,
                ratingCount = product.Ratings.Count
            });
        }
    }
}
