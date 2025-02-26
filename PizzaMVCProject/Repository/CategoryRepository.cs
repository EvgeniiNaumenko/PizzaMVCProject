﻿using Microsoft.EntityFrameworkCore;
using PizzaMVCProject.Data;
using PizzaMVCProject.Interfaces;
using PizzaMVCProject.Models;
using PizzaMVCProject.Models.Pages;

namespace PizzaMVCProject.Repository
{
    public class CategoryRepository : ICategory
    {
        private readonly ApplicationContext _context;
        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }


        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }

        public PagedList<Category> GetAllCategories(QueryOptions options)
        {
            return new PagedList<Category>(_context.Categories, options);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }


        public async Task<Category?> GetCategoryAsync(int categoryId)
        {
            return await _context.Categories
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

    }
}
