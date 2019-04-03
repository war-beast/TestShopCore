using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestShopCore.Models;

namespace TestShopCore.Areas.Admin.Models
{
    public class Parametr
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class AdminProductListViewModel
    {
        public int? Id { get; set; }
        public string CategoryName { get; set; } = "";
        public ICollection<Product> Products { get; set; }
    }

    public class OrderViewModel
    {
        public string CustomerEmail { get; set; }
        public Order Order { get; set; }
    }
}