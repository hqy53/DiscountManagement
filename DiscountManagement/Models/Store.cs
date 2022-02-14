using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DiscountManagement.Models
{
    public class Store
    {
        [Key]
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountPercentage { get; set; }

        //A store can sell many products
        public ICollection<Product> Products { get; set; }

    }
    public class StoreDto
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountPercentage { get; set; }
    }
}