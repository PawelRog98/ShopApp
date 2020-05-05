using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Helpers;
using Shop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shop.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ShopUser> _userManager;
        public CartController(ApplicationDbContext context,UserManager<ShopUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            try
            {
                ViewBag.total = cart.Sum(item => item.product.Price * item.CartQuantity);
            }
            catch
            {
                ViewBag.total = 0;
            }
            return View();
        }
        [Authorize]
        public IActionResult Buying(int id)
        {
            var Product = _context.Product.FirstOrDefault(p => p.Id == id);
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { product = Product, CartQuantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].CartQuantity++;
                }
                else
                {
                    cart.Add(new Item { product = Product, CartQuantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Remove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        [Authorize]
        public IActionResult HasBeenBought(Product product, int? id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            var Item = _context.Item.Include(p => p.product);

            Orders order = new Orders();
            order.OrderName= Guid.NewGuid().ToString();

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            order.UserName = userIdClaim.Value;
            order.dateTime = DateTime.Now;
            order.TransportCost = 0;
            foreach(var item in cart)
            {
                order.TransportCost = order.TransportCost + (item.product.Price*item.CartQuantity);
            }
            _context.Orders.Add(order);

            _context.SaveChanges();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            int index = 0;
            foreach (var item in cart)
            {
                var ProductItem = new Item();
                {
                    ProductItem.CartQuantity = item.CartQuantity;
                    ProductItem.productId = item.product.Id;
                    ProductItem.ordersId = order.Id;
                    ProductItem.UserName = userIdClaim.Value;
                }
                _context.Item.Add(ProductItem);
                _context.SaveChanges();
                var ProductUpdate = new Product();

                product = _context.Product.Include(i => i.Company).Where(i => i.Id == item.product.Id).Single();
                item.product.Quantity = item.product.Quantity - item.CartQuantity;
                product.Quantity = item.product.Quantity;

                _context.Product.Update(product);
                _context.SaveChanges();
                index++;
            }
            cart.Clear();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            _context.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        [Authorize]
        public async Task<IActionResult> ConfirmAddress()
        {
            var user = await _userManager.GetUserAsync(User);
            ShopUser shopUser = new ShopUser();

            shopUser.FirstName = user.FirstName;
            shopUser.LastName = user.LastName;
            shopUser.City = user.City;
            shopUser.Street = user.Street;
            shopUser.PostalCode = user.PostalCode;
            shopUser.State = user.State;
            shopUser.Country = user.Country;

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmAddress(ShopUser shopUser)
        {
            var user = await _userManager.GetUserAsync(User);

            user.FirstName = shopUser.FirstName;
            user.LastName = shopUser.LastName;
            user.City = shopUser.City;
            user.Street = shopUser.Street;
            user.PostalCode = shopUser.PostalCode;
            user.State = shopUser.State;
            user.Country = shopUser.Country;

            await _userManager.UpdateAsync(user);
            return View(user);
        }
    }
}