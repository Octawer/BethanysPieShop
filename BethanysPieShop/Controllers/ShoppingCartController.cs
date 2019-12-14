using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BethanysPieShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IPieRepository pieRepository, ShoppingCart shoppingCart)
        {
            _pieRepository = pieRepository;
            _shoppingCart = shoppingCart;
        }

        public ViewResult Index() 
        {
            var viewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                Total = _shoppingCart.CartTotal
            };

            return View(viewModel);
        }

        public RedirectToActionResult AddToShoppingCart(int pieId) 
        {
            Pie pie = _pieRepository.AllPies.SingleOrDefault(p => p.PieId == pieId);
            if (pie != null)
            {
                _shoppingCart.AddToCart(pie);
            }
            return RedirectToAction(nameof(Index));
        }

        public RedirectToActionResult RemoveFromShoppingCart(int pieId) 
        {
            Pie pie = _pieRepository.AllPies.SingleOrDefault(p => p.PieId == pieId);
            if (pie != null)
            {
                _shoppingCart.RemoveFromCart(pie);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
