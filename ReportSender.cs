using System.Net.Mail;
using System.Net;
using DetApp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using OfficeOpenXml;
using Quartz;
using System;
using DetailingCenterApp.Models;

namespace DetApp3
{
    public class ReportSender : IJob
    {
        string file_path_template;
        string file_path_report;
        private readonly DetailingCenterDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public ReportSender(DetailingCenterDBContext context, IWebHostEnvironment
       appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public void PrepareReport()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            using (ExcelPackage excelPackage = new ExcelPackage(file_path_template))
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
                    Service service = _context.Services.Find(providedservice.ServiceId);
                    Automobile automobile = _context.Automobiles.Find(providedservice.AutomobileId);
                    Client client = _context.Clients.Find(automobile.AutomobileID);
                    Employer employer = _context.Employers.Find(providedservice.EmployerId);
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = service.Servicename;
                    worksheet.Cells[startLine, 3].Value = client.Surname + ' ' + client.Name + ' ' + client.Patronym;
                    worksheet.Cells[startLine, 4].Value = automobile.Mark + ' ' + automobile.Model + ' ' + automobile.Gosnumber;
                    worksheet.Cells[startLine, 5].Value = employer.Surname + ' ' + employer.Name + ' ' + employer.Patronym;
                    worksheet.Cells[startLine, 6].Value = service.Price.ToString();
                    //worksheet.Cells[startLine, 2].Value = providedservice.Service.Servicename;
                    //worksheet.Cells[startLine, 3].Value = providedservice.Automobile.Client.Surname + ' ' + providedservice.Automobile.Client.Name + ' ' + providedservice.Automobile.Client.Patronym;
                    //worksheet.Cells[startLine, 4].Value = providedservice.Automobile.Mark + ' ' + providedservice.Automobile.Model + ' ' + providedservice.Automobile.Gosnumber;
                    //worksheet.Cells[startLine, 5].Value = providedservice.Employer.Surname + ' ' + providedservice.Employer.Name + ' ' + providedservice.Employer.Patronym;
                    //worksheet.Cells[startLine, 6].Value = providedservice.Service.Price.ToString();
                    worksheet.Cells[startLine, 7].Value = providedservice.dateTime.ToString();
                    startLine++;
                }
                //созраняем в новое место
                excelPackage.SaveAs(file_path_report);
            }            
        }
        public async Task Execute(IJobExecutionContext context)
        {
            // Путь к файлу с шаблоном
            string path = @"/Reports/report-template.xlsx";
            //Путь к файлу с результатом
            string result = @"/Reports/report.xlsx";
            file_path_template = _appEnvironment.WebRootPath + path;
            file_path_report = _appEnvironment.WebRootPath + result;
            try
            {
                if (File.Exists(file_path_report))
                    File.Delete(file_path_report);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            PrepareReport();
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("dgprivezencev@gmail.com", "Den");
            // кому отправляем
            MailAddress to = new MailAddress("dgprivezencev@mail.ru");
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Тест";
            // текст письма
            m.Body = "<h2>Письмо-тест работы smtp-клиента</h2>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("dgprivezencev@gmail.com",
            "ЗДЕСЬДОЛЖЕНБЫТЬПАРОЛЬ");
            smtp.EnableSsl = true;
            // вкладываем файл в письмо
            m.Attachments.Add(new Attachment(file_path_report));
            // отправляем асинхронно
            await smtp.SendMailAsync(m);
            m.Dispose();
        }

    }
}
