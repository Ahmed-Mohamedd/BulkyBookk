using BulkyBook.BLL.Interfaces;
using BulkyBook.DAL.Entities;
using BulkyBook.PL.Utilities;
using BulkyBook.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyBook.PL.Areas.Customer.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;


        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }
        public double OrderTotalPrice { get; set; }


        public CartController(IUnitOfWork unitOfWork , IConfiguration configuration)
        {
                _unitOfWork = unitOfWork;
                _configuration=configuration;
        }


        public IActionResult Index()
        {
            var claims = (ClaimsIdentity)User.Identity;
            var UserId = claims.FindFirst(ClaimTypes.NameIdentifier);
            var shoppingCarts = _unitOfWork.ShoppingCarts.GetAll().Where(c => c.ApplicationUserId == UserId.Value);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                ListCart = shoppingCarts,
                OrderHeader = new()
            };
            foreach (var cart in ShoppingCartViewModel.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price5, cart.Product.Price10);
                ShoppingCartViewModel.OrderHeader.OrderTotal+= (cart.Price*cart.Count);
            }
            return View(ShoppingCartViewModel);
        }

        public IActionResult Summary() 
        {
			var claims = (ClaimsIdentity)User.Identity;
			var UserId = claims.FindFirst(ClaimTypes.NameIdentifier);
			var shoppingCarts = _unitOfWork.ShoppingCarts.GetAll().Where(c => c.ApplicationUserId == UserId.Value);

			ShoppingCartViewModel = new ShoppingCartViewModel()
			{
				ListCart = shoppingCarts,
				OrderHeader = new()
			};

            ShoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUsers.GetById(u => u.Id == UserId.Value);

            ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel.OrderHeader.ApplicationUser.UserName;
            ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartViewModel.OrderHeader.State = ShoppingCartViewModel.OrderHeader.ApplicationUser.State;
            ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.ApplicationUser.City;
            ShoppingCartViewModel.OrderHeader.StreetAddress = ShoppingCartViewModel.OrderHeader.ApplicationUser.StreetName;
            ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.ApplicationUser.PostalCode;


			foreach (var cart in ShoppingCartViewModel.ListCart)
			{
				cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price5, cart.Product.Price10);
				ShoppingCartViewModel.OrderHeader.OrderTotal+= (cart.Price*cart.Count);
			}
			return View(ShoppingCartViewModel);
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Summary(ShoppingCartViewModel shoppingCartViewModel)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var UserId = claims.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartViewModel.ListCart = _unitOfWork.ShoppingCarts.GetAll().Where(c => c.ApplicationUserId == UserId.Value);

            shoppingCartViewModel.OrderHeader.OrderStatus = OrderPaymentStatus.OrderStatusPending;
            shoppingCartViewModel.OrderHeader.PaymentStatus = OrderPaymentStatus.PaymentStatusPending;
            shoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
            shoppingCartViewModel.OrderHeader.ApplicationUserId = UserId.Value;
            


            foreach (var cart in shoppingCartViewModel.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price5, cart.Product.Price10);
                shoppingCartViewModel.OrderHeader.OrderTotal+= (cart.Price*cart.Count);
            }


            _unitOfWork.OrderHeaders.Add(shoppingCartViewModel.OrderHeader);
            _unitOfWork.OrderHeaders.Save();

            foreach (var cart in shoppingCartViewModel.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = shoppingCartViewModel.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                _unitOfWork.OrderDetails.Add(orderDetail);
                _unitOfWork.OrderDetails.Save();
            }

            var domain = "https://localhost:7279/";
            // Stripe Setting
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                
                Mode = "payment",
                SuccessUrl = $"{domain}Customer/Cart/OrderConfirmation?id={shoppingCartViewModel.OrderHeader.Id}",
                CancelUrl = $"{domain}Customer/Cart/Index",
            };

            foreach(var item in shoppingCartViewModel.ListCart)
            {

               var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price*100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.OrderHeaders.UpdateStripePaymentId(shoppingCartViewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.OrderHeaders.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);



            //_unitOfWork.ShoppingCarts.RemoveRange(shoppingCartViewModel.ListCart);
            //_unitOfWork.ShoppingCarts.Save();

            //return RedirectToAction(nameof(Index) , "Home");
        }



        public IActionResult OrderConfirmation(int id)
        {
            var OrderHeader = _unitOfWork.OrderHeaders.GetById(h => h.Id == id);

            // Update Stripe Status

            var service = new SessionService();
            Session session = service.Get(OrderHeader.SessionId);

            if(session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeaders.UpdateStatus(OrderHeader.Id, OrderPaymentStatus.OrderStatusApproved, OrderPaymentStatus.PaymentStatusApproved);
                _unitOfWork.OrderHeaders.Save();
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCarts.GetAll().Where(c => c.ApplicationUserId == OrderHeader.ApplicationUserId).ToList();


            _unitOfWork.ShoppingCarts.RemoveRange(shoppingCarts);
            _unitOfWork.ShoppingCarts.Save();

            return View(id);

        }



        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCarts.GetById(c => c.Id==cartId);
            _unitOfWork.ShoppingCarts.IncrementCount(cart, 1);
            _unitOfWork.ShoppingCarts.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCarts.GetById(c => c.Id==cartId);
            if(cart.Count <= 1)
            {
                _unitOfWork.ShoppingCarts.Delete(cart);
            }
            else
            {
                _unitOfWork.ShoppingCarts.DecrementCount(cart, 1);
            }
            _unitOfWork.ShoppingCarts.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCarts.GetById(c => c.Id==cartId);
            _unitOfWork.ShoppingCarts.Delete(cart);
            _unitOfWork.ShoppingCarts.Save();
            return RedirectToAction(nameof(Index));
        }


        private double GetPriceBasedOnQuantity(double quantity , double price , double price5 , double price10)
        {
            if(quantity <=5)
            {
                return price;
            }
            else
            {
                if(quantity <= 10)
                    return price5;
                return price10;
            }
        }
    }
}
