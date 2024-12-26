using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStore.Models.ViewModels
{
    public class ProductFilterVm
    {
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public int? PriceStart { get; set; }

        [DataType(DataType.Currency)]
        public int? PriceEnd { get; set; }

        public List<ProductIndexVm> Data { get; set; }
    }
    public class ProductIndexVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int Price { get; set; }
    }
}