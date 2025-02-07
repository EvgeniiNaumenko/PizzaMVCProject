using PizzaMVCProject.Models;
using PizzaMVCProject.Models.Pages;

namespace PizzaMVCProject.Interfaces
{
    public interface IProduct
    {
        PagedList<Product> GetAllProducts(QueryOptions options);
        Task<Product?> GetProductAsync(string id);
        Task<Product> GetProductWithCategoryAsync(string id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task EditProductAsync(Product product);
        Task<IEnumerable<Product>> GetSimilarProductsAsync(string categoryName);
    }
}
