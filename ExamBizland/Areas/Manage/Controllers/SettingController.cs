using ExamBizland.DAL;
using ExamBizland.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamBizland.Areas.Manage.Controllers
{
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var setting = await _context.Settings.Take(5).ToListAsync();
            return View(setting);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Setting? setting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (setting == null) { NotFound(); return View(); }
            return View(setting);
        }

        [HttpPost]
        public async Task<IActionResult>Edit(Setting setting)
        {
            if (setting == null) { NotFound(); return View(); }
            Setting? exists = await _context.Settings.FirstOrDefaultAsync(x => x.Id == setting.Id);
            if (exists == null) { NotFound(); return View(); }
            exists.Value = setting.Value;
            return RedirectToAction("Index", "Home");
        }
    }
}
