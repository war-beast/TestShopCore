﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestShopCore.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Сумма заказа")]
        public decimal Sum { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Статус заказа")]
        public string State { get; set; }
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}