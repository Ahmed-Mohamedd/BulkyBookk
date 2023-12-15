using AutoMapper;
using BulkyBook.BLL.Interfaces;
using BulkyBook.DAL.Entities;
using BulkyBook.PL.Models;
using BulkyBook.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBook.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger , IUnitOfWork unitOfWork , IMapper mapper)
        {
            _logger = logger;
            _unitOfWork=unitOfWork;
            _mapper=mapper;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> AllPoducts = _unitOfWork.Products.GetAll();
            var products = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductCardViewModel>>(AllPoducts);
           
            return View(products);
        }

        public IActionResult Details(int productId)
        {
            if(productId == null || productId==0)
                return NotFound();

            //var product = _unitOfWork.Products.GetByIdSpecification(p => p.Id==id);
            //var CastedProduct = _mapper.Map<Product,ProductCardViewModel>(product);
            var ShoppingCart = new ShoppingCart()
            {
                Count = 1,
                Product = _unitOfWork.Products.GetByIdSpecification(p => p.Id == productId),
                ProductId = productId
            };

            return View(ShoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var UserId = claims.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = UserId.Value;

            var shoppingCartFromDb = _unitOfWork.ShoppingCarts.GetAll().Where(s => s.ApplicationUserId==UserId.Value && s.ProductId == shoppingCart.ProductId).FirstOrDefault();
            if(shoppingCartFromDb == null)
            {
            _unitOfWork.ShoppingCarts.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCarts.IncrementCount(shoppingCartFromDb, shoppingCart.Count);
            }
            _unitOfWork.ShoppingCarts.Save();
            

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}