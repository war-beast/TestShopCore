using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestShopCore.Models
{
    public class Parametr
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ShopingCardItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }

    public class ShopingCardViewModel
    {
        public ICollection<ShopingCardItem> Items { get; set; }
        public string Adress { get; set; }
    }

    public class FilterViewModels
    {
        public List<Parametr> Categories { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int Sort { get; set; }
    }

    public class SortType
    {
        private List<Parametr> SortList { get; set; }
        public SortType()
        {
            SortList = new List<Parametr> {
                new Parametr { Id = 0, Name = "Умолчанию" },
                new Parametr { Id = 1, Name= "Цене" },
                new Parametr { Id = 2, Name = "Рейтингу" } };
        }

        public List<Parametr> GetSortTypes()
        {
            return SortList;
        }

        public Parametr Get(int Id)
        {
            return SortList.FirstOrDefault(st => st.Id == Id);
        }
    }

    public class ReviewViewModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string ReviewText { get; set; }
        [Required]
        public int Value { get; set; }
        [Required]
        public int ProductId { get; set; }
    }

    public class ProductListViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}