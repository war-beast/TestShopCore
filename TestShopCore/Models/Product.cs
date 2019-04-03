using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestShopCore.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Цена")]
        [Required]
        public decimal Price { get; set; }

        [Display(Name = "Рейтинг")]
        [Required]
        public int Rating { get; set; }

        [Required]
        [Display(Name = "Фото")]
        public string Photo { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}