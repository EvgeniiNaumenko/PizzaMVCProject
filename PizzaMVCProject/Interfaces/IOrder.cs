using PizzaMVCProject.Models.Checkout;
using PizzaMVCProject.Models.Pages;

namespace PizzaMVCProject.Interfaces
{
    public interface IOrder
    {
        PagedList<Order> GetAllOrdersWithDetails(QueryOptions options);
        PagedList<Order> GetAllOrdersByUserWithDetails(QueryOptions options, string userId);
        Task<Order> GetOrderAsync(int id);

        Task AddOrderAsync(Order order);
        Task EditOrderAsync(Order order);
        Task RemoveOrderAsync(Order order);

        PagedList<Order> GetPendingAndProcessingOrders(QueryOptions options);
        Task<Order> GetOrderWithDetailsAsync(int id);
    }
}
