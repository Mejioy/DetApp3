//using DetailingCenterApp.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DetailingCenterApp.Models;
using System.Numerics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace DetApp3.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmployersController : Controller
    {

        private readonly DetailingCenterDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public EmployersController(DetailingCenterDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        

        //public EmployersController(DetailingCenterDBContext context)
        //{
        //    _context = context;
        //}

        // GET: Employers
        public async Task<IActionResult> Index()
        {
              return _context.Employers != null ? 
                          View(await _context.Employers.ToListAsync()) :
                          Problem("Entity set 'DetailingCenterDBContext.Employers'  is null.");
        }

        // GET: Employers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employers == null)
            {
                return NotFound();
            }

            var employer = await _context.Employers
                .FirstOrDefaultAsync(m => m.EmployerID == id);
            if (employer == null)
            {
                return NotFound();
            }

            return View(employer);
        }

        // GET: Employers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployerID,Surname,Name,Patronym,Phone,City,Street,Housenumber,Appartmentnumber")] Employer employer, IFormFile upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    string path = "/Files/" + upload.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await upload.CopyToAsync(fileStream);
                    }
                    employer.Photo = path;
                }
                _context.Add(employer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", player.TeamId);
            return View(employer);
        }

        // GET: Employers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employers == null)
            {
                return NotFound();
            }
            var employer = await _context.Employers.FindAsync(id);
            if (!employer.Photo.IsNullOrEmpty())
            {
                byte[] photodata =
               System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + employer.Photo);
                ViewBag.Photodata = photodata;
            }
            else
            {
                ViewBag.Photodata = null;
            }
            if (employer == null)
            {
                return NotFound();
            }
            return View(employer);
        }

        // POST: Employers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployerID,Surname,Name,Patronym,Phone,City,Street,Housenumber,Appartmentnumber, Photo")] Employer employer, IFormFile? upload)
        {
            if (id != employer.EmployerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    string path = "/Files/" + upload.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await upload.CopyToAsync(fileStream);
                    }
                    if (!employer.Photo.IsNullOrEmpty())
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath +
                       employer.Photo);
                    }
                    employer.Photo = path;
                }
                try
                {
                    _context.Update(employer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployerExists(employer.EmployerID))
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
            return View(employer);
        }

        // GET: Employers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employers == null)
            {
                return NotFound();
            }

            var employer = await _context.Employers
                .FirstOrDefaultAsync(m => m.EmployerID == id);
            if (employer == null)
            {
                return NotFound();
            }

            return View(employer);
        }

        // POST: Employers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employers == null)
            {
                return Problem("Entity set 'DetailingCenterDBContext.Employers'  is null.");
            }
            var employer = await _context.Employers.FindAsync(id);
            if (employer != null)
            {
                _context.Employers.Remove(employer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployerExists(int id)
        {
          return (_context.Employers?.Any(e => e.EmployerID == id)).GetValueOrDefault();
        }
    }
}
