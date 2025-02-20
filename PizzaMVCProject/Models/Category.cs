using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PizzaMVCProject.Models
{
    public class Category
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime DateOfPublication { get; set; }
    }
}
