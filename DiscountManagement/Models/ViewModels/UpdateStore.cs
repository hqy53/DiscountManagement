using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountManagement.Models.ViewModels
{
    public class UpdateStore
    {
        //This viewmodel is a class which stores information that we need to present to /Store/Update/{}

        //the existing store information

        public StoreDto SelectedStore { get; set; }
    }
}