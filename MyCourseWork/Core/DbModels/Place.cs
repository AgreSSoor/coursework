using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.DbModels
{
    public class Place : BaseEntity
    {
        [Required(ErrorMessage = "Данные не введены")]
        [StringLength(50, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Данные не введены")]
        [StringLength(50, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 6)]
        [DataType(DataType.PhoneNumber)]
        public string Phones { get; set; }

        [Required(ErrorMessage = "Данные не введены")]
        [StringLength(15, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 3)]
        public string Specialisation { get; set; }

        [Required(ErrorMessage = "Данные не введены")]
        [StringLength(10, ErrorMessage = "{0} длинна должна быть между {2} и {1}.", MinimumLength = 0)]
        public string OwnerShipType { get; set; }

        [Required(ErrorMessage = "Данные не введены")]
        public string WorkTime { get; set; }

        [Required(ErrorMessage = "Данные не введены")]
        public string URL { get; set; }
        
        public ICollection<Product> Places { get; set; }
    }
}
