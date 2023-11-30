using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HabitAqui.Data;
using HabitAqui.Models;
using HabitAqui.ViewModels;

namespace HabitAqui.Controllers
{
    public class HabitacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HabitacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Habitacoes
        public async Task<IActionResult> Index(bool? disponivel)
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Habitacoes.OrderBy(c => c.Nome).ToList(), "Id", "Nome");

            if (disponivel != null)
            {
                if (disponivel == true)
                    ViewData["Title"] = "Lista de Habitacoes Activas";
                else
                    ViewData["Title"] = "Lista de Habitacoes Inactivas";

                return View(await _context.Habitacoes.
                    Where(c => c.Disponivel == disponivel).OrderBy(c => c.Nome).ToListAsync()
                    );
            }
            else
            {
                ViewData["Title"] = "Lista de cursos";
                return View(await _context.Habitacoes.OrderBy(c => c.Nome).ToListAsync());
            }
        }


        [HttpPost]
        public IActionResult Index(string TextoAPesquisar, int HabitacaoId)
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Habitacoes.OrderBy(c => c.Nome).ToList(), "Id", "Nome");

            if (string.IsNullOrWhiteSpace(TextoAPesquisar))
                return View(_context.Habitacoes.Where(c => c.Id == HabitacaoId));
            else
            {
                var resultado = from c in _context.Habitacoes
                                where c.Nome.Contains(TextoAPesquisar) && c.Id == HabitacaoId
                                select c;
                return View(resultado);
            }
        }

        public async Task<IActionResult> Search(string? TextoAPesquisar)
        {
            PesquisaHabitacaoViewModel pesquisaVM = new PesquisaHabitacaoViewModel();
            ViewData["Title"] = "Pesquisar cursos";

            if (string.IsNullOrWhiteSpace(TextoAPesquisar))
                pesquisaVM.ListaDeHabitacoes = await _context.Habitacoes.Include("categoria").OrderBy(c => c.Nome).ToListAsync();
            else
            {
                pesquisaVM.ListaDeHabitacoes =
                    await _context.Habitacoes.Where(c => c.Nome.Contains(TextoAPesquisar)
                                                || c.Localizacao.Contains(TextoAPesquisar)
                                                || c.Observacoes.Contains(TextoAPesquisar)
                                                ).ToListAsync();
                pesquisaVM.TextoAPesquisar = TextoAPesquisar;
                foreach (Habitacao c in pesquisaVM.ListaDeHabitacoes)
                {
                    c.Nome = AltCorSubSTR(c.Nome, pesquisaVM.TextoAPesquisar);
                    c.Localizacao = AltCorSubSTR(c.Localizacao, pesquisaVM.TextoAPesquisar);
                }
            }
            pesquisaVM.NumResultados = pesquisaVM.ListaDeHabitacoes.Count();

            return View(pesquisaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(
            [Bind("TextoAPesquisar")] PesquisaHabitacaoViewModel pesquisaHabitacao
            )
        {
            ViewData["Title"] = "Pesquisar cursos";
            if (string.IsNullOrEmpty(pesquisaHabitacao.TextoAPesquisar))
            {
                pesquisaHabitacao.ListaDeHabitacoes = await _context.Habitacoes.OrderBy(c => c.Nome).ToListAsync();
                pesquisaHabitacao.NumResultados = pesquisaHabitacao.ListaDeHabitacoes.Count();
            }
            else
            {
                pesquisaHabitacao.ListaDeHabitacoes =
                    await _context.Habitacoes.Where(c => c.Nome.Contains(pesquisaHabitacao.TextoAPesquisar)
                                                || c.Localizacao.Contains(pesquisaHabitacao.TextoAPesquisar)
                                                || c.Observacoes.Contains(pesquisaHabitacao.TextoAPesquisar)
                                                ).OrderBy(c => c.Nome).ToListAsync();
                pesquisaHabitacao.NumResultados = pesquisaHabitacao.ListaDeHabitacoes.Count();

                foreach (Habitacao c in pesquisaHabitacao.ListaDeHabitacoes)
                {
                    c.Nome = AltCorSubSTR(c.Nome, pesquisaHabitacao.TextoAPesquisar);
                    c.Localizacao = AltCorSubSTR(c.Localizacao, pesquisaHabitacao.TextoAPesquisar);
                }
            }
            return View(pesquisaHabitacao);
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
        public async Task<IActionResult> Create([Bind("Id,Nome,IdContrato,Disponivel,Localizacao,IdArrendamento,IdTipo,IdLocador,Avaliacao,IdEstado,Danos,Observacoes")] Habitacao habitacao)
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
        public string AltCorSubSTR(string txtOriginal, string txtPesquisa)
        {
            string txtAlterado = string.Empty;

            if (!string.IsNullOrEmpty(txtOriginal))
            {
                string[] split = txtOriginal.Split(" ");

                foreach (string str in split)
                {
                    if (str.ToLower().Contains(txtPesquisa.ToLower()))
                    {
                        string a = string.Empty;
                        int b = 0;

                        for (int i = 0; i < str.Length; i++)
                        {
                            if (str.ToLower().Substring(i, txtPesquisa.Length) == txtPesquisa.ToLower())
                            {
                                a = str.Substring(i, txtPesquisa.Length);
                                b = i;
                                break;
                            }
                        }

                        txtAlterado += str + " ";

                        txtAlterado = txtAlterado.Replace(str.Substring(b, txtPesquisa.Length),
                            "<span class=\"bg-warning\">" + a + "</span>");
                    }
                    else
                        txtAlterado += str + " ";
                }
            }
            else
                txtAlterado = txtOriginal;

            return txtAlterado;
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
