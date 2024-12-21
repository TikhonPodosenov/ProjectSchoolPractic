using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSchool.DB;
using ProjectSchool.Models;


namespace ProjectSchool.Controllers
{
    public class PersonalAccountController:Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public PersonalAccountController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }

        public IActionResult Index()
        {
            int id_aut = _globalDataService.Id_aut;
            var data_student = _context.GetPersonalAccount(id_aut);
            return View(data_student);
        }
    }
}
