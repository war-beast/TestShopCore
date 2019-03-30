using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestShopCore.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Наименование")]
        [Required]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}