using System.ComponentModel.DataAnnotations;

namespace PizzaMVCProject.ViewModels
{
    public class CategoryViewModel
    {
        [Key]
        public int? Id {  get; set; }
        [Display(Name = " Название")]
        [Required (ErrorMessage = "Не указано название категории")]
        public string Name { get; set; }
        [Display(Name = " Описание")]
        public string? Description { get; set; }
        [Display(Name ="Изображение")]
        public IFormFile? file { get; set; }
        public string? Image {  get; set; }
        public DateTime DateOfPublication {  get; set; }
    }
}
