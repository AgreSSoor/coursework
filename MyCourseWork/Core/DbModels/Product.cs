using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.CompilerServices;
using MyCourseWork.Models;

namespace Core.DbModels
{
    public class Product : BaseEntity
    {
        [Required(ErrorMessage = "Данные не введены")]
        [StringLength(50, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 3)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Данные не введены")]
        [Range(0, Double.MaxValue,ErrorMessage = "Введено неверное значение")]
        public float? Price { get; set; }
        
        [Required(ErrorMessage = "Данные не введены")]
        [StringLength(50, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 6)]
        public string Description { get; set; }
        
        [Required]
        public int PlaceId { get; set; }
        
        public Place Place { get; set; }
        
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}