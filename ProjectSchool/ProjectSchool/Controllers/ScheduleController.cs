using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSchool.DB;
using ProjectSchool.Models;

namespace ProjectSchool.Controllers
{
    public class ScheduleController:Controller
    {
        private readonly ApplicationDbContext _context;

        private GlobalDataService _globalDataService;

        public ScheduleController(ApplicationDbContext context, GlobalDataService globalDataService)
        {
            _context = context;
            _globalDataService = globalDataService;
        }
        public IActionResult Index(string subjectId, string groupId, string dayId)
        {
            ViewBag.targetUrlElectronicMagazine = _globalDataService.PathElectronicemagazine;
            ViewBag.targetUrlElectronicNews = _globalDataService.PathNews;
            ViewBag.targetUrlElectronicProfile = _globalDataService.PathProfile;
            List<Schedule> allSchedule;
            if (string.IsNullOrEmpty(subjectId) && string.IsNullOrEmpty(groupId) && string.IsNullOrEmpty(dayId))
            {
                allSchedule = _context.GetSchedule();

                HashSet<string> subjects = new HashSet<string>();
                HashSet<string> groups = new HashSet<string>();
                HashSet<string> days = new HashSet<string>();

                if (allSchedule != null)
                {
                    foreach (var item in allSchedule)
                    {
                        if (item.Subject != null) subjects.Add(item.Subject);
                        if (item.Group != null) groups.Add(item.Group);
                        if (item.Day != null) days.Add(item.Day);
                    }
                }

                List<string> Subjects = subjects.ToList();
                List<string> Groups = groups.ToList();
                List<string> Days = days.ToList();

                var dayOrder = new Dictionary<string, int>()
                {
                    { "Понедельник", 1 },
                    { "Вторник", 2 },
                    { "Среда", 3 },
                    { "Четверг", 4 },
                    { "Пятница", 5 },
                    { "Суббота", 6 }
                };

                Days = Days.OrderBy(day => dayOrder[day]).ToList();
                _globalDataService.Subjects = ViewBag.Subjects = Subjects;
                _globalDataService.Groups = ViewBag.Groups = Groups;
                _globalDataService.Days = ViewBag.Days = Days;
                return View(allSchedule);
            }
            else
            {
                ViewBag.Subjects = _globalDataService.Subjects;
                ViewBag.Groups = _globalDataService.Groups;
                ViewBag.Days = _globalDataService.Days;
                if (string.IsNullOrEmpty(subjectId))
                    subjectId = "";
                if (string.IsNullOrEmpty(groupId))
                    groupId = "";
                if (string.IsNullOrEmpty(dayId))
                    dayId = "";

                allSchedule = _context.GetScheduleFilter(subjectId, groupId, dayId);
                return View(allSchedule);
            }
        }
    }
}
