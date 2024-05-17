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
    public class ProvidedServicesController : Controller
    {
        private readonly DetailingCenterDBContext _context;

        public ProvidedServicesController(DetailingCenterDBContext context)
        {
            _context = context;
        }

        // GET: ProvidedServices
        public async Task<IActionResult> Index()
        {
            var detailingCenterDBContext = _context.ProvidedServices.Include(p => p.Automobile).Include(p => p.Employer).Include(p => p.Service);
            return View(await detailingCenterDBContext.ToListAsync());
        }

        // GET: ProvidedServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProvidedServices == null)
            {
                return NotFound();
            }

            var providedService = await _context.ProvidedServices
                .Include(p => p.Automobile)
                .Include(p => p.Employer)
                .Include(p => p.Service)
                .FirstOrDefaultAsync(m => m.ProvidedServiceID == id);
            if (providedService == null)
            {
                return NotFound();
            }

            return View(providedService);
        }

        // GET: ProvidedServices/Create
        public IActionResult Create()
        {
            ViewData["AutomobileId"] = new SelectList(_context.Automobiles, "AutomobileID", "AutomobileID");
            ViewData["EmployerId"] = new SelectList(_context.Employers, "EmployerID", "EmployerID");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceID", "ServiceID");
            return View();
        }

        // POST: ProvidedServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProvidedServiceID,ServiceId,EmployerId,AutomobileId,dateTime")] ProvidedService providedService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(providedService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutomobileId"] = new SelectList(_context.Automobiles, "AutomobileID", "AutomobileID", providedService.AutomobileId);
            ViewData["EmployerId"] = new SelectList(_context.Employers, "EmployerID", "EmployerID", providedService.EmployerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceID", "ServiceID", providedService.ServiceId);
            return View(providedService);
        }

        // GET: ProvidedServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProvidedServices == null)
            {
                return NotFound();
            }

            var providedService = await _context.ProvidedServices.FindAsync(id);
            if (providedService == null)
            {
                return NotFound();
            }
            ViewData["AutomobileId"] = new SelectList(_context.Automobiles, "AutomobileID", "AutomobileID", providedService.AutomobileId);
            ViewData["EmployerId"] = new SelectList(_context.Employers, "EmployerID", "EmployerID", providedService.EmployerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceID", "ServiceID", providedService.ServiceId);
            return View(providedService);
        }

        // POST: ProvidedServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProvidedServiceID,ServiceId,EmployerId,AutomobileId,dateTime")] ProvidedService providedService)
        {
            if (id != providedService.ProvidedServiceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(providedService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvidedServiceExists(providedService.ProvidedServiceID))
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
            ViewData["AutomobileId"] = new SelectList(_context.Automobiles, "AutomobileID", "AutomobileID", providedService.AutomobileId);
            ViewData["EmployerId"] = new SelectList(_context.Employers, "EmployerID", "EmployerID", providedService.EmployerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceID", "ServiceID", providedService.ServiceId);
            return View(providedService);
        }

        // GET: ProvidedServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProvidedServices == null)
            {
                return NotFound();
            }

            var providedService = await _context.ProvidedServices
                .Include(p => p.Automobile)
                .Include(p => p.Employer)
                .Include(p => p.Service)
                .FirstOrDefaultAsync(m => m.ProvidedServiceID == id);
            if (providedService == null)
            {
                return NotFound();
            }

            return View(providedService);
        }

        // POST: ProvidedServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProvidedServices == null)
            {
                return Problem("Entity set 'DetailingCenterDBContext.ProvidedServices'  is null.");
            }
            var providedService = await _context.ProvidedServices.FindAsync(id);
            if (providedService != null)
            {
                _context.ProvidedServices.Remove(providedService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProvidedServiceExists(int id)
        {
          return (_context.ProvidedServices?.Any(e => e.ProvidedServiceID == id)).GetValueOrDefault();
        }
    }
}
