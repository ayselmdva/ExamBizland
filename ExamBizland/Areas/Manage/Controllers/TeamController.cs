using ExamBizland.DAL;
using ExamBizland.Models;
using ExamBizland.Utilites;
using ExamBizland.View_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace ExamBizland.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TeamController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index(int page=1, int take=1)
        {
            var team = await _context.Teams.Skip((page-1)*take).Take(take).Include(x => x.Profession).ToListAsync();
            PaginateVM<Team> paginateVM = new PaginateVM<Team>()
            {
                Items = team,
                CurrentPage = page,
                Take = take,
                PageCount = GetPageCount(page)
            };
            return View(paginateVM);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Professions = await _context.Professions.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Team team)
        {
            ViewBag.Professions = await _context.Professions.ToListAsync();
            if (!ModelState.IsValid) { return View(); }
            if(team == null) { ModelState.AddModelError("", "NotFound"); return View(); }
            if (!team.ImageFile.CheckFileType("image")) { ModelState.AddModelError("", "Must be image file"); return View(); }
            if (team.ImageFile.CheckFileSize(2000)) { ModelState.AddModelError("", "Must be less 2000 file size"); return View(); }
            string fileName = await team.ImageFile.SaveFileAsync(_environment.WebRootPath, "faces");
            team.Image = fileName;
            await _context.AddAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Professions = await _context.Professions.ToListAsync();
            Team? team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null) { NotFound(); return View(); }
            return View(team);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Team team)
        {
            ViewBag.Professions = await _context.Professions.ToListAsync();
            if (team == null) { NotFound(); return View(); }
            Team? exists = await _context.Teams.FirstOrDefaultAsync(x => x.Id == team.Id);
            if (exists == null) { NotFound(); return View(); }
            exists.Name=team.Name;
            exists.ProfessionId = team.ProfessionId;
            if (team.ImageFile != null)
            {
                if (!team.ImageFile.CheckFileType("image")) { ModelState.AddModelError("", "Must be image file"); return View(); }
                if (team.ImageFile.CheckFileSize(2000)) { ModelState.AddModelError("", "Must be less 2000 file size"); return View(); }
                exists.ImageFile.DeleteFile(_environment.WebRootPath, "faces", exists.Image);
                string fileName= await team.ImageFile.SaveFileAsync(_environment.WebRootPath, "faces");
                exists.Image = fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            Team? team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null) { NotFound(); return View(); }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public int GetPageCount(int take)
        {
            var pageCount = _context.Teams.Count();
            return (int)Math.Ceiling((decimal)pageCount/take);
        }
    }
}
