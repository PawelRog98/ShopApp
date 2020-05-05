using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Pagination;

namespace Shop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _iWebHost;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _iWebHost = webHost;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, string filter ,int? pageNumber)
        {
            var applicationDbContext = _context.Product.Include(p => p.Company);

            ViewData["Filter"] = searchString;
            ViewData["PriceSort"] = sortOrder == "Price" ? "Price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = filter;
            }

            var productFilter = from p in _context.Product
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                productFilter = productFilter.Where(s => s.Name.Contains(searchString));
            }
            switch(sortOrder)
            {
                case "Price":
                    productFilter = productFilter.OrderBy(o => o.Price);
                    break;
                case "Price_desc":
                    productFilter = productFilter.OrderByDescending(o => o.Price);
                    break;
                default:
                    productFilter = productFilter.OrderBy(o => o.Name);
                    break;

            }
            int pageSize = 5;
            return View(await PaginationList<Product>.CreateAsync(productFilter.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "CompanyName");
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Quantity,Description,ImagePath,ProductType,CompanyId")] Product product, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var imagePath = @"\img\images\";
                var uploadPath = _iWebHost.WebRootPath + imagePath;

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var newFileName = Guid.NewGuid().ToString();
                var fileName = Path.GetFileName(newFileName + "." + file.FileName.Split(".")[1].ToLower());
                string fullPath = uploadPath + fileName;
                imagePath = imagePath + @"\";
                var filePath = @".." + Path.Combine(imagePath, fileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                product.ImagePath = fileName;
            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "CompanyName", product.CompanyId);
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "CompanyName", product.CompanyId);
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Price,Quantity,Description,ImagePath,ProductType,CompanyId")] Product product, IFormFile Editfile)
        {
            //product = _context.Product.Include(i => i.Company).Where(i => i.Id == id).Single();
            if (Editfile != null)
            {
                if (id != product.Id)
                {
                    return NotFound();
                }
                product = await _context.Product.FindAsync(id);
                if (product.ImagePath != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\images", product.ImagePath);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                if (Editfile != null && Editfile.Length > 0)
                {
                    var imagePath = @"\img\images\";
                    var uploadPath = _iWebHost.WebRootPath + imagePath;

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var newFileName = Guid.NewGuid().ToString();
                    var fileName = Path.GetFileName(newFileName + "." + Editfile.FileName.Split(".")[1].ToLower());
                    string fullPath = uploadPath + fileName;
                    imagePath = imagePath + @"\";
                    var filePath = @".." + Path.Combine(imagePath, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await Editfile.CopyToAsync(fileStream);
                    }
                    product.ImagePath = fileName;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    //var prodHelper = new Product();
                    //prodHelper = _context.Product.Include(i => i.Company).Where(i => i.Id == id).Single();
                    //product.ImagePath = prodHelper.ImagePath;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "CompanyName", product.CompanyId);
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var product = await _context.Product
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product.ImagePath != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\images", product.ImagePath);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

    }
}
