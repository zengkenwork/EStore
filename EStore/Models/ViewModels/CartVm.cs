using EStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EStore.Models.ViewModels
{
    public class CartVm
    {
        public int Id { get; set; }

        public string MemberAccount { get; set; }

        public IEnumerable<CartItemVm> CartItems { get; set; }

        public int Total => CartItems.Sum(x => x.SubTotal);
        public bool AllowCheckout => CartItems.Any(); // 至少一項才能結帳
    }
    public class CartItemVm
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }

        public ProductIndexVm Product { get; set; }

        public int Qty { get; set; }
        public int SubTotal => Product.Price * Qty;
    }
}