using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BethanysPieShop.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _appDbContext;
        private IEnumerable<ShoppingCartItem> _shoppingCartItems;


        public string ShoppingCartId { get; set; }

        public IEnumerable<ShoppingCartItem> ShoppingCartItems
        {
            get
            {
                if (_shoppingCartItems == null)
                {
                    _shoppingCartItems = _appDbContext.ShoppingCartItems.Where(item => item.ShoppingCartId == ShoppingCartId).Include(item => item.Pie);
                }
                return _shoppingCartItems;
            }
        }

        public decimal CartTotal => ShoppingCartItems.Sum(item => item.Pie.Price * item.Amount);

        public ShoppingCart(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public static ShoppingCart GetShoppingCart(IServiceProvider services) 
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;

            AppDbContext context = services.GetService<AppDbContext>();

            var cartId = string.IsNullOrEmpty(session.GetString("CartId")) ? Guid.NewGuid().ToString() : session.GetString("CartId");

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Pie pie) 
        {
            ShoppingCartItem shoppingCartItem = _appDbContext.ShoppingCartItems
                .SingleOrDefault(item => item.Pie.PieId == pie.PieId && item.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = 1
                };

                _appDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            _appDbContext.SaveChanges();
        }

        public void RemoveFromCart(Pie pie) 
        {
            ShoppingCartItem shoppingCartItem = _appDbContext.ShoppingCartItems.SingleOrDefault(item => item.Pie.PieId == pie.PieId);
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 0)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
                _appDbContext.SaveChanges();
            }
        }

        public void ClearCart() 
        {
            _appDbContext.ShoppingCartItems.RemoveRange(ShoppingCartItems);
            _appDbContext.SaveChanges();
        }
    }
}
