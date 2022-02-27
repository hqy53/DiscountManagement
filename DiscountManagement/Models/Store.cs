using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscountManagement.Models
{
    public class Store
    {
        [Key]
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountPercentage { get; set; }

        //data needed for keeping track of store images uploaded
        //images deposited into /Content/Images/Stores/{id}.{extension}
        public bool StoreHasPic { get; set; }
        public string PicExtension { get; set; }

        //A store can sell many products
        public ICollection<Product> Products { get; set; }

    }
    public class StoreDto
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountPercentage { get; set; }

        //data needed for keeping track of store images uploaded
        //images deposited into /Content/Images/Stores/{id}.{extension}
        public bool StoreHasPic { get; set; }
        public string PicExtension { get; set; }
    }
}