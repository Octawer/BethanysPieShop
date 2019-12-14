using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BethanysPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
        }

        public ViewResult List(string category)
        {
            var viewModel = new PiesListViewModel();

            if (string.IsNullOrEmpty(category))
            {
                viewModel.Pies = _pieRepository.AllPies.OrderBy(p => p.PieId);
                viewModel.CurrentCategory = "All pies";
            }
            else
            {
                viewModel.Pies = _pieRepository.AllPies.Where(p => p.Category.CategoryName == category).OrderBy(p => p.PieId);
                viewModel.CurrentCategory = _categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryName == category)?.CategoryName;
            }

            return View(viewModel);
        }

        public IActionResult Details(int id) 
        {
            var pie = _pieRepository.GetById(id);
            if (pie == null)
            {
                return NotFound();
            }

            return View(pie);
        }
    }
}
