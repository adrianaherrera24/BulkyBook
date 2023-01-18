using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int OrderTotal { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); // User ID

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")
            };

            // To calculate final price in the shopping cart
            foreach(var item in ShoppingCartVM.ListCart)
            {
                item.Price = GetPriceBasedOnQuantity(item.Count, item.Product.Price, item.Product.Price50, item.Product.Price100);
                ShoppingCartVM.CartTotal += (item.Price * item.Count);
            }

            return View(ShoppingCartVM);
        }

        private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if(quantity <= 50)
            {
                return price;
            }else if(quantity <= 100)
            {
                return price50;
            }
            else
            {
                return price100;
            }
        }
    }
}
