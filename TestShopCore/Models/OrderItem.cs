using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestShopCore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Display(Name = "Товар")]
        public Product Product { get; set; }
        [Display(Name = "Количество")]
        public int Count { get; set; }
    }
}