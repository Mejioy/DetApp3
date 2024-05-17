using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DetailingCenterApp.Models;

namespace DetApp3.Controllers
{
    public class AutomobilesController : Controller
    {
        private readonly DetailingCenterDBContext _context;

        public AutomobilesController(DetailingCenterDBContext context)
        {
            _context = context;
        }

        // GET: Automobiles
        public async Task<IActionResult> Index()
        {
            var detailingCenterDBContext = _context.Automobiles.Include(a => a.Client);
            return View(await detailingCenterDBContext.ToListAsync());
        }

        // GET: Automobiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Automobiles == null)
            {
                return NotFound();
            }

            var automobile = await _context.Automobiles
                .Include(a => a.Client)
                .FirstOrDefaultAsync(m => m.AutomobileID == id);
            if (automobile == null)
            {
                return NotFound();
            }

            return View(automobile);
        }

        // GET: Automobiles/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientID", "ClientID");
            return View();
        }

        // POST: Automobiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutomobileID,Mark,Model,Gosnumber,ClientId")] Automobile automobile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(automobile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientID", "ClientID", automobile.ClientId);
            return View(automobile);
        }

        // GET: Automobiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Automobiles == null)
            {
                return NotFound();
            }

            var automobile = await _context.Automobiles.FindAsync(id);
            if (automobile == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientID", "ClientID", automobile.ClientId);
            return View(automobile);
        }

        // POST: Automobiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutomobileID,Mark,Model,Gosnumber,ClientId")] Automobile automobile)
        {
            if (id != automobile.AutomobileID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(automobile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutomobileExists(automobile.AutomobileID))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientID", "ClientID", automobile.ClientId);
            return View(automobile);
        }

        // GET: Automobiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Automobiles == null)
            {
                return NotFound();
            }

            var automobile = await _context.Automobiles
                .Include(a => a.Client)
                .FirstOrDefaultAsync(m => m.AutomobileID == id);
            if (automobile == null)
            {
                return NotFound();
            }

            return View(automobile);
        }

        // POST: Automobiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Automobiles == null)
            {
                return Problem("Entity set 'DetailingCenterDBContext.Automobiles'  is null.");
            }
            var automobile = await _context.Automobiles.FindAsync(id);
            if (automobile != null)
            {
                _context.Automobiles.Remove(automobile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutomobileExists(int id)
        {
          return (_context.Automobiles?.Any(e => e.AutomobileID == id)).GetValueOrDefault();
        }
    }
}
