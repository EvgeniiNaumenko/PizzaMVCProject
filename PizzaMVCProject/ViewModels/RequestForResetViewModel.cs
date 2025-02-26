﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PizzaMVCProject.ViewModels
{
    public class RequestForResetViewModel
    {
        [Required(ErrorMessage = " Укажите имейл адрес")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailExists", controller: "Account", ErrorMessage = "Имейл адрес не найден")]
        public string Email { get; set; }
    }
}
