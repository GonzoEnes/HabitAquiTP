using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace HabitAqui.Controllers
{
    public class ArrendamentosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArrendamentosController(ApplicationDbContext context,UserManager<ApplicationUser>usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }


        //[Authorize(Roles = "Client,Admin")]
        public IActionResult Request()
        {
            ViewData["ListaHabitacoes"] = new SelectList(_context.Habitacoes.Where(v => v.Disponivel == true).ToList(), "Id", "Nome");
            return View();
        }

        // GET: Arrendamentos
        public async Task<IActionResult> Index()
        {
            var reservations = _context.Arrendamentos.
            Include(a => a.Habitacao).
            Include(a => a.ApplicationUser).
            Where(a => a.ApplicationUserId == _userManager.GetUserId(User));
            return View(await reservations.ToListAsync());
        }

        // GET: Arrendamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Arrendamentos == null)
            {
                return NotFound();
            }

            var arrendamento = await _context.Arrendamentos
                .Include(a => a.ApplicationUser)
                .Include(a => a.Estado)
                .Include(a => a.Habitacao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arrendamento == null)
            {
                return NotFound();
            }

            return View(arrendamento);
        }

        // GET: Arrendamentos/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["EstadoId"] = new SelectList(_context.Set<Estado>(), "Id", "Id");
            ViewData["HabitacaoId"] = new SelectList(_context.Habitacoes, "Id", "Id");
            return View();
        }

        // POST: Arrendamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustoArrendamento,DataInicio,DataFinal,DataPedido,HabitacaoId,ApplicationUserId,EstadoId")] Arrendamento arrendamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arrendamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", arrendamento.ApplicationUserId);
            ViewData["EstadoId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", arrendamento.EstadoId);
            ViewData["HabitacaoId"] = new SelectList(_context.Habitacoes, "Id", "Id", arrendamento.HabitacaoId);
            return View(arrendamento);
        }

        // GET: Arrendamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Arrendamentos == null)
            {
                return NotFound();
            }

            var arrendamento = await _context.Arrendamentos.FindAsync(id);
            if (arrendamento == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", arrendamento.ApplicationUserId);
            ViewData["EstadoId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", arrendamento.EstadoId);
            ViewData["HabitacaoId"] = new SelectList(_context.Habitacoes, "Id", "Id", arrendamento.HabitacaoId);
            return View(arrendamento);
        }

        // POST: Arrendamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustoArrendamento,DataInicio,DataFinal,DataPedido,HabitacaoId,ApplicationUserId,EstadoId")] Arrendamento arrendamento)
        {
            if (id != arrendamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(arrendamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArrendamentoExists(arrendamento.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", arrendamento.ApplicationUserId);
            ViewData["EstadoId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", arrendamento.EstadoId);
            ViewData["HabitacaoId"] = new SelectList(_context.Habitacoes, "Id", "Id", arrendamento.HabitacaoId);
            return View(arrendamento);
        }

        // GET: Arrendamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Arrendamentos == null)
            {
                return NotFound();
            }

            var arrendamento = await _context.Arrendamentos
                .Include(a => a.ApplicationUser)
                .Include(a => a.Estado)
                .Include(a => a.Habitacao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arrendamento == null)
            {
                return NotFound();
            }

            return View(arrendamento);
        }

        // POST: Arrendamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Arrendamentos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Arrendamentos'  is null.");
            }
            var arrendamento = await _context.Arrendamentos.FindAsync(id);
            if (arrendamento != null)
            {
                _context.Arrendamentos.Remove(arrendamento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArrendamentoExists(int id)
        {
          return (_context.Arrendamentos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
