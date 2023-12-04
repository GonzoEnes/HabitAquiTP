using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HabitAqui.ViewModels;
using Microsoft.Extensions.Hosting;

namespace HabitAqui.Controllers
{
    public class HabitacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _environment;

        public HabitacoesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Habitacoes
        public async Task<IActionResult> Index()
        {
              return _context.Habitacoes != null ? 
                          View(await _context.Habitacoes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Habitacoes'  is null.");
        }

        // GET: Habitacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Habitacoes == null)
            {
                return NotFound();
            }

            var habitacao = await _context.Habitacoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habitacao == null)
            {
                return NotFound();
            }

            return View(habitacao);
        }

        // GET: Habitacoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Habitacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,IdContrato,Disponivel,Localizacao,IdArrendamento,IdTipo,IdLocador,Avaliacao,Danos,Observacoes,Image,IdCategoria,NBath,NBedroom,Area,Estado,Tipo")] Habitacao habitacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habitacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ListaDeHabitacoes"] = new SelectList(_context.Habitacoes.OrderBy(c => c.Disponivel).ToList(), "Id", "Nome");

            return View(habitacao);
        }

        // GET: Habitacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Habitacoes == null)
            {
                return NotFound();
            }

            var habitacao = await _context.Habitacoes.FindAsync(id);
            if (habitacao == null)
            {
                return NotFound();
            }
            return View(habitacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([Bind("TextoAPesquisar")] HabitacoesViewModel pesquisaHabitacoes)
        {
            ViewData["Title"] = "Pesquisar Habitações";
            
            if (string.IsNullOrEmpty(pesquisaHabitacoes.TextoAPesquisar))
            {
                pesquisaHabitacoes.ListaHabitacoes = await _context.Habitacoes.OrderBy(c => c.Nome).ToListAsync();
                pesquisaHabitacoes.NResults = pesquisaHabitacoes.ListaHabitacoes.Count();
            }
            else
            {
                pesquisaHabitacoes.ListaHabitacoes =
                    await _context.Habitacoes.Where(c => c.Nome.Contains(pesquisaHabitacoes.TextoAPesquisar)
                                                || c.Danos.Contains(pesquisaHabitacoes.TextoAPesquisar)
                                                || c.Estado.Contains(pesquisaHabitacoes.TextoAPesquisar) 
                                                || c.Localizacao.Contains(pesquisaHabitacoes.TextoAPesquisar)
                                                ).OrderBy(c => c.Nome).ToListAsync();
                pesquisaHabitacoes.NResults = pesquisaHabitacoes.ListaHabitacoes.Count();
            }
            return View(pesquisaHabitacoes);
        }

        public async Task<IActionResult> Search(string? TextoAPesquisar)
        {
            HabitacoesViewModel pesquisaHabit = new HabitacoesViewModel();
            
            ViewData["Title"] = "Pesquisar Habitações";

            if (string.IsNullOrWhiteSpace(TextoAPesquisar))
                pesquisaHabit.ListaHabitacoes = await _context.Habitacoes.OrderBy(c => c.Nome).ToListAsync();
            else
            {
                pesquisaHabit.ListaHabitacoes =
                    await _context.Habitacoes.Where(c => c.Nome.Contains(TextoAPesquisar)
                                                || c.Observacoes.Contains(TextoAPesquisar)
                                                || c.Estado.Contains(TextoAPesquisar)
                                                || c.Localizacao.Contains(TextoAPesquisar)
                                                ).ToListAsync();
                pesquisaHabit.TextoAPesquisar = TextoAPesquisar;
 
            }
            pesquisaHabit.NResults = pesquisaHabit.ListaHabitacoes.Count();

            return View(pesquisaHabit);
        }
        // POST: Habitacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,IdContrato,Disponivel,Localizacao,IdArrendamento,IdTipo,IdLocador,Avaliacao,IdEstado,Danos,Observacoes")] Habitacao habitacao)
        {
            if (id != habitacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habitacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitacaoExists(habitacao.Id))
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
            return View(habitacao);
        }

        // GET: Habitacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Habitacoes == null)
            {
                return NotFound();
            }

            var habitacao = await _context.Habitacoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habitacao == null)
            {
                return NotFound();
            }

            return View(habitacao);
        }

        // POST: Habitacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Habitacoes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Habitacoes'  is null.");
            }
            var habitacao = await _context.Habitacoes.FindAsync(id);
            if (habitacao != null)
            {
                _context.Habitacoes.Remove(habitacao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabitacaoExists(int id)
        {
          return (_context.Habitacoes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
