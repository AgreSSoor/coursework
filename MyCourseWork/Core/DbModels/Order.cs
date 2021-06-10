using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyCourseWork.Models
{
    public class Order
    {
        [BindNever]
        public int Id { get; set; }
        
        [Display(Name = "Введите ваше имя")]
        [StringLength(25, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 2)]
        [Required(ErrorMessage = "Длинна имени не более 20 символов")]
        public string Name { get; set; }
        
        [Display(Name = "Введите вашу фамилию")]
        [StringLength(25, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 2)]
        [Required(ErrorMessage = "Длинна фамилии не более 20 символов")]
        public string Surname { get; set; }
        
        [Display(Name = "Введите ваш адресс")]
        [StringLength(25, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 6)]
        [Required(ErrorMessage = "Введите адресс")]
        public string Adress { get; set; }
        
        [Display(Name = "Введите ваш телефон")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(25, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 6)]
        [Required(ErrorMessage = "Вы неправильно ввели телефон")]
        public string Phone { get; set; }
         
        [Display(Name = "Введите ваш почтовый адресс(Email)")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Вы неправильно ввели email, попробуйте ещё раз")]
        [DataType(DataType.EmailAddress)]
        [StringLength(25, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 6)]
        [Required(ErrorMessage = "Введите email")]
        public string Email { get; set; }
        
        [BindNever]
        [ScaffoldColumn(false)]
        public DateTime OrderTime { get; set; }
        
        public List<OrderDetail> OrderDetails { get; set; }
    }
}