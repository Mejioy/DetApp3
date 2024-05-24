using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DetailingCenterApp.Models;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;

namespace DetApp3.Controllers
{
    [Authorize(Roles = "employer")]
    public class ProvidedServicesController : Controller
    {
       
        private readonly DetailingCenterDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ProvidedServicesController(DetailingCenterDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
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
            var automobiles = _context.Automobiles
        .Select(c => new
        {
            c.AutomobileID,
            DisplayValue = c.Mark + ' ' + c.Model + ' ' + c.Gosnumber
        })
        .ToList();
            var employers = _context.Employers
        .Select(c => new
        {
            c.EmployerID,
            DisplayValue = c.Surname + ' ' + c.Name + ' ' + c.Patronym
        })
        .ToList();            
            ViewData["AutomobileId"] = new SelectList(automobiles, "AutomobileID", "DisplayValue");
            ViewData["EmployerId"] = new SelectList(employers, "EmployerID", "DisplayValue");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceID", "Servicename");
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
            var automobiles = _context.Automobiles
        .Select(c => new
        {
            c.AutomobileID,
            DisplayValue = c.Mark + ' ' + c.Model + ' ' + c.Gosnumber
        })
        .ToList();
            var employers = _context.Employers
        .Select(c => new
        {
            c.EmployerID,
            DisplayValue = c.Surname + ' ' + c.Name + ' ' + c.Patronym
        })
        .ToList();
            ViewData["AutomobileId"] = new SelectList(automobiles, "AutomobileID", "DisplayValue", providedService.AutomobileId);
            ViewData["EmployerId"] = new SelectList(employers, "EmployerID", "DisplayValue", providedService.EmployerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceID", "Servicename", providedService.ServiceId);
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
        public FileResult GetReport()
        {
            // Путь к файлу с шаблоном
            string path = "/reports/providedserviceall-report.xlsx";
            //Путь к файлу с результатом
            string result = "/reports/providedservicesall/providedservices.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            int sum = 0;
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //устанавливаем поля документа
                excelPackage.Workbook.Properties.Author = "Кокурин Я. Д.";
                excelPackage.Workbook.Properties.Title = "Список оказанных услуг";
                excelPackage.Workbook.Properties.Subject = "Оказанные услуги";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet =
               excelPackage.Workbook.Worksheets["providedServices"];
                //получаем списко пользователей и в цикле заполняем лист данными
                int startLine = 3;
                List<ProvidedService> providedServices = _context.ProvidedServices.ToList();
                foreach (ProvidedService providedservice in providedServices)
                {                    
                    providedservice.Service = _context.Services.Find(providedservice.ServiceId);
                    providedservice.Automobile = _context.Automobiles.Find(providedservice.AutomobileId);
                    Client client = _context.Clients.Find(providedservice.Automobile.ClientId);
                    providedservice.Employer = _context.Employers.Find(providedservice.EmployerId);                
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = providedservice.Service.Servicename;
                    worksheet.Cells[startLine, 3].Value = client.Surname + ' ' + client.Name + ' ' + client.Patronym;
                  
                    worksheet.Cells[startLine, 4].Value = providedservice.Automobile.Mark + ' ' + providedservice.Automobile.Model + ' ' + providedservice.Automobile.Gosnumber;
                    worksheet.Cells[startLine, 5].Value = providedservice.Employer.Surname + ' ' + providedservice.Employer.Name + ' ' + providedservice.Employer.Patronym;
                    worksheet.Cells[startLine, 6].Value = providedservice.Service.Price.ToString();
                    sum+=providedservice.Service.Price;
                    worksheet.Cells[startLine, 7].Value = providedservice.dateTime.ToString();
                    startLine++;
                }
                worksheet.Cells[startLine, 1].Value = "Выручка:";
                worksheet.Cells[startLine, 6].Value = sum.ToString();
                //созраняем в новое место
                excelPackage.SaveAs(fr);
            }            
            // Тип файла - content-type
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            // Имя файла - необязательно
            string file_name = "providedservices.xlsx";
            return File(result, file_type, file_name);
        }
    }
}
