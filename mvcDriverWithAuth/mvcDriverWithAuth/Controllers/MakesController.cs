﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvcDriverWithAuth.Data;
using mvcDriverWithAuth.Models;

namespace mvcDriverWithAuth.Controllers
{
    [Authorize]
    public class MakesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MakesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Makes
        public async Task<IActionResult> Index()
        {
            var model = new DriverMakeViewModel();
            model.Makes = await _context.Makes.ToListAsync();
            model.ShowModifyLinks = User?.Identity?.IsAuthenticated ?? false;

            return View(model);
        }

        [AllowAnonymous]
        // GET: Makes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Makes == null)
            {
                return NotFound();
            }

            var make = await _context.Makes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (make == null)
            {
                return NotFound();
            }

            if (User?.Identity?.IsAuthenticated ?? false)
            {
                make.ShowModifyLinks = true;
            }

            return View(make);
        }

        [Authorize]
        // GET: Makes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Makes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MakeName")] Make make)
        {
            if (ModelState.IsValid)
            {
                _context.Add(make);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(make);
        }

        [Authorize]
        // GET: Makes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Makes == null)
            {
                return NotFound();
            }

            var make = await _context.Makes.FindAsync(id);
            if (make == null)
            {
                return NotFound();
            }

            if (User?.Identity?.IsAuthenticated ?? false)
            {
                make.ShowModifyLinks = true;
            }

            return View(make);
        }

        // POST: Makes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MakeName")] Make make)
        {
            if (id != make.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(make);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MakeExists(make.Id))
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
            return View(make);
        }

        // GET: Makes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Makes == null)
            {
                return NotFound();
            }

            var make = await _context.Makes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }

        // POST: Makes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Makes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Makes'  is null.");
            }
            var make = await _context.Makes.FindAsync(id);
            if (make != null)
            {
                _context.Makes.Remove(make);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MakeExists(int id)
        {
          return (_context.Makes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
