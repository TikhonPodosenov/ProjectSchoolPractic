using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectSchool.DB;
using ProjectSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;


namespace ProjectSchool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public HomeController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }
        

        public IActionResult Index()
        {
            return View();
        }

        public static string HashPassword(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var sha512 = SHA512.Create())
            {
                byte[] hashBytes = sha512.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckPersonData(Home aut)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", aut); 
            }

            string hashcode = HashPassword(aut.Password);
            var data_person = _context.VerificationOfAuthorization(aut.Login, hashcode);
            if (data_person == null || data_person.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Неправильные данные");
                return View("Index", aut);
            }

            _globalDataService.Post = data_person[0].Post;
            _globalDataService.Id_aut = data_person[0].Id_aut;

            
            switch (data_person[0].Post)
            {
                case "Учитель":
                    _globalDataService.PathProfile = "/PersonalAccountTeacher";
                    _globalDataService.PathElectronicemagazine = "/ElectronicMagazineTeacher";
                    _globalDataService.PathNews = "/News";
                    _globalDataService.PathSchedule = "/Schedule";
                    return RedirectToAction("Index", "PersonalAccountTeacher");
                case "Ученик":
                    _globalDataService.PathProfile = "/PersonalAccount";
                    _globalDataService.PathElectronicemagazine = "/ElectronicMagazine";
                    _globalDataService.PathNews = "/News";
                    _globalDataService.PathSchedule = "/Schedule";
                    return RedirectToAction("Index", "PersonalAccount", new { id_aut = data_person[0].Id_aut });
                case "Администратор":
                    _globalDataService.PathProfile = "/PersonalAccountAdministration";
                    _globalDataService.PathElectronicemagazine = "/ElectronicMagazineTeacher";
                    _globalDataService.PathNews = "/NewsAdministration";
                    _globalDataService.PathSchedule = "/ScheduleAdministration";
                    return RedirectToAction("Index", "PersonalAccountAdministration", new { id_aut = data_person[0].Id_aut });
                default: break;
            }
            
            return RedirectToAction("Index", "PersonalAccountController");
        }

    }
}
