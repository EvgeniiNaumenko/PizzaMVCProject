using PizzaMVCProject.Models;

namespace PizzaMVCProject.ViewModels
{
    public class CurrentProductViewModel
    {
        public Product Product { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
