using ExamBizland.DAL;
using ExamBizland.View_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamBizland.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Teams = await _context.Teams.Include(x => x.Profession).Take(4).ToListAsync()
            };
            return View(homeVM);
        }
    }
}
