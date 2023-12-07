using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SQLitePCL;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace HabitAqui.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var propertyList = _context.Habitacoes.GroupBy(c => c.Nome).Select(group => group.FirstOrDefault());

            ViewData["PropriedadeList"] = new SelectList(propertyList.ToList(), "Id", "Nome");

            var locationList = _context.Habitacoes.GroupBy(c => c.Localizacao).Select(group => group.FirstOrDefault());

            ViewData["LocalizacaoList"] = new SelectList(locationList.ToList(), "Id", "Localizacao");

            var tipologiaIds = _context.Habitacoes
    .Select(c => c.Tipologia.Id)
    .Distinct()
    .ToList();

            var tipologiaList = _context.Tipologia
                .Where(t => tipologiaIds.Contains(t.Id))
                .Select(t => t.Nome)
                .ToList();

            ViewData["TipologiaList"] = new SelectList(tipologiaList);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}