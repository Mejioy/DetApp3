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
    public class AutomobilesController : Controller
    {
        private readonly DetailingCenterDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public AutomobilesController(DetailingCenterDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
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
            var clients = _context.Clients
        .Select(c => new
        {
            c.ClientID,
            DisplayValue = c.Surname + ' ' + c.Name + ' ' + c.Patronym
        })
        .ToList();
            ViewData["ClientId"] = new SelectList(clients, "ClientID", "DisplayValue");
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
            var clients = _context.Clients
        .Select(c => new
        {
            c.ClientID,
            DisplayValue = c.Surname + ' ' + c.Name + ' ' + c.Patronym
        })
        .ToList();
            ViewData["ClientId"] = new SelectList(clients, "ClientID", "DisplayValue", automobile.ClientId);
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
        public FileResult GetReport(int id)
        {
            // Путь к файлу с шаблоном
            string path = "/reports/providedserviceofauto-report.xlsx";
            //Путь к файлу с результатом
            string result = "/reports/providedservicesofauto/providedserviceofauto.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //открываем файл с шаблоном
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
                Automobile auto = _context.Automobiles.Find(id);
                worksheet.Cells[1, 2].Value = $"для автомобиля {auto.Mark} {auto.Model} {auto.Gosnumber}";
                int startLine = 3;
                List<ProvidedService> providedServices = _context.ProvidedServices.ToList();
                List<ProvidedService> providedServicesOfAuto=new List<ProvidedService>();
                foreach (ProvidedService providedService in providedServices)
                {
                    if(providedService.AutomobileId == id)
                    {
                        providedServicesOfAuto.Add(providedService);
                    }
                }
                int sum = 0;
                foreach (ProvidedService providedservice in providedServicesOfAuto)
                {
                    Service service = _context.Services.Find(providedservice.ServiceId);
                    Employer employer = _context.Employers.Find(providedservice.EmployerId);
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = service.Servicename;
                    //worksheet.Cells[startLine, 3].Value = client.Surname + ' ' + client.Name + ' ' + client.Patronym;
                    //worksheet.Cells[startLine, 4].Value = automobile.Mark + ' ' + automobile.Model + ' ' + automobile.Gosnumber;
                    worksheet.Cells[startLine, 3].Value = employer.Surname + ' ' + employer.Name + ' ' + employer.Patronym;
                    worksheet.Cells[startLine, 4].Value = service.Price.ToString();
                    sum += service.Price;
                    //worksheet.Cells[startLine, 2].Value = providedservice.Service.Servicename;
                    //worksheet.Cells[startLine, 3].Value = providedservice.Automobile.Client.Surname + ' ' + providedservice.Automobile.Client.Name + ' ' + providedservice.Automobile.Client.Patronym;
                    //worksheet.Cells[startLine, 4].Value = providedservice.Automobile.Mark + ' ' + providedservice.Automobile.Model + ' ' + providedservice.Automobile.Gosnumber;
                    //worksheet.Cells[startLine, 5].Value = providedservice.Employer.Surname + ' ' + providedservice.Employer.Name + ' ' + providedservice.Employer.Patronym;
                    //worksheet.Cells[startLine, 6].Value = providedservice.Service.Price.ToString();
                    worksheet.Cells[startLine, 5].Value = providedservice.dateTime.ToString();
                    startLine++;
                }
                worksheet.Cells[startLine, 1].Value = "Стоимость:";
                worksheet.Cells[startLine, 4].Value = sum.ToString();
                //созраняем в новое место
                excelPackage.SaveAs(fr);
            }
            // Тип файла - content-type
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            // Имя файла - необязательно
            string file_name = "providedserviceofauto.xlsx";
            return File(result, file_type, file_name);
        }
    }
}
