using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private ApplicationDbContext _context;
        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Company> companies = _context.Company.ToList();
            return View(companies);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(Company company)
        {
            if(ModelState.IsValid)
            {
                _context.Company.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(company);
        }
        public async Task <IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return RedirectToAction("Index");
            }
            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(Company company)
        {
            if(ModelState.IsValid)
            {
                _context.Company.Update(company);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Company company = _context.Company.Single(model => model.Id == id);
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Company company = _context.Company.Single(model => model.Id == id);
            _context.Company.Remove(company);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FirstOrDefaultAsync(model => model.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }
    }
}