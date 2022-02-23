using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountManagement.Models.ViewModels
{
    public class DetailsProduct
    {
        public ProductDto SelectedProduct { get; set; }
        public IEnumerable<StoreDto> SellingStores { get; set; }
    }
}