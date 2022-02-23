using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountManagement.Models.ViewModels
{
    public class DetailsStore
    {
        public StoreDto SelectedStore { get; set; }
        public IEnumerable<ProductDto> SellingProducts { get; set; }

        public IEnumerable<ProductDto> AvailableProducts { get; set; }
    }
}