using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;

namespace Shop.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HistoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task <IActionResult> Index(int id, string orderName, string userName)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            userName = userIdClaim.Value;
            var Item = await _context.Item.Include(p => p.product).Include(o=>o.orders)
                .Where(item=>item.orders.OrderName==orderName && item.orders.UserName==userName && item.ordersId==id).ToListAsync();
            return View(Item);
        }
        [Authorize]
        public async Task<IActionResult> Orders(string userName)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            userName = userIdClaim.Value;
            var Order = await _context.Orders.Where(o=>o.UserName==userName).ToListAsync();
            return View(Order);
        }
    }
}