using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestShopCore.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}