using EStore.Models.EFModels;
using EStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace EStore.Controllers
{
    public class CartController : Controller
    {
        [Authorize]
        // GET: Cart
        public ActionResult AddItem(int productId)
        {
            string account = User.Identity.Name;
            int qty = 1;
            Add2Cart(account, productId, qty);

            return new EmptyResult();
        }

        private void Add2Cart(string account, int productId, int qty)
        {
            CartVm cart = GetCartInfo(account);
            int cartId = cart.Id;
            AddCartItem(cartId, productId, qty);
        }

        private void AddCartItem(int cartId, int productId, int qty)
        {
            using (var db = new AppDbContext())
            {
                var cartItem = db.CartItems.FirstOrDefault(x => x.CartId == cartId && x.ProductId == productId);
                if (cartItem != null)
                {
                    cartItem.Qty += qty;
                }
                else
                {
                    var newItem = new CartItem
                    {
                        CartId = cartId,
                        ProductId = productId,
                        Qty = qty
                    };
                    db.CartItems.Add(newItem);
                }
                db.SaveChanges();
            }
        }

        private CartVm GetCartInfo(string account)
        {
            using (var db = new AppDbContext())
            {
                var cart = db.Carts.FirstOrDefault(x => x.MemberAccount == account);

                if (cart == null)
                {
                    cart = new Cart { MemberAccount = account };
                    db.Carts.Add(cart);
                    db.SaveChanges();
                }
                // Include 需using System.Data.Entity;
                var cartItems = db.CartItems.AsNoTracking().Include(x => x.Product).Where(x => x.CartId == cart.Id).Select(x => new CartItemVm
                {
                    Id = x.Id,
                    CartId = x.CartId,
                    ProductId = x.ProductId,
                    Product = new ProductIndexVm
                    {
                        Id = x.Product.Id,
                        Name = x.Product.Name,
                        Price = x.Product.Price
                    },
                    Qty = x.Qty
                }).ToList();

                var cartVm = new CartVm
                {
                    Id = cart.Id,
                    MemberAccount = cart.MemberAccount,
                    CartItems = cartItems
                };

                return cartVm;
            }
        }

        [Authorize]
        public ActionResult Info()
        {
            string account = User.Identity.Name;
            var cart = GetCartInfo(account);

            return View(cart);
        }

        [Authorize]
        public ActionResult UpdateItem(int productId, int newQty)
        {
            string account = User.Identity.Name;
            newQty = newQty < 0 ? 0 : newQty;

            UpdateItemQty(account, productId, newQty);

            return new EmptyResult();
        }

        private void UpdateItemQty(string account, int productId, int newQty)
        {
            var cart = GetCartInfo(account);
            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

            using (var db = new AppDbContext())
            {
                var entity = db.CartItems.Find(cartItem.Id);
                if (entity == null) return;

                if(newQty == 0)
                {
                    db.CartItems.Remove(entity);
                }
                else
                {
                    entity.Qty = newQty;
                }

                db.SaveChanges();
            }
        }

        [Authorize]
        public ActionResult Checkout()
        {
            string account = User.Identity.Name;
            var cart = GetCartInfo(account);

            if(cart.AllowCheckout == false)
            {
                return Content("購物車內沒有商品, 無法結帳");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutVm model)
        {
            string account = User.Identity.Name;
            var cart = GetCartInfo(account);

            if (cart.AllowCheckout == false)
            {
                return Content("購物車內沒有商品, 無法結帳");
            }

            CreateOrder(account, model);

            EmptyCart(account);

            return View("ConfirmCheckout");
        }

        private void EmptyCart(string account)
        {
            using (var db = new AppDbContext())
            {
                var cart = db.Carts.FirstOrDefault(x => x.MemberAccount == account);
                if (cart == null) return;

                db.Carts.Remove(cart);
                db.SaveChanges();
            }
        }

        private void CreateOrder(string account, CheckoutVm model)
        {
            using (var db = new AppDbContext())
            {
                var cart = GetCartInfo(account);
                var order = new Order
                {
                    MemberId = db.Members.First(x => x.Account == account).Id,
                    Receiver = model.Receiver,
                    Address = model.Address,
                    CellPhone = model.CellPhone,

                    Total = cart.Total,
                    CreatedTime = DateTime.Now,
                    Status = 1
                };

                foreach (var item in cart.CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        Order = order,
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        Qty = item.Qty,
                        Price = item.Product.Price,
                        SubTotal = item.SubTotal
                    };
                    db.OrderItems.Add(orderItem);
                }

                db.Orders.Add(order);
                db.SaveChanges();
            }
        }
    }
}