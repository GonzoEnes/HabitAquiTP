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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var habitacoesViewModel = new HabitacoesViewModel();
            habitacoesViewModel.ListaHabitacoes = await _context.Habitacoes.Include("Categoria").Include("Arrendamentos").Include("Tipologia").ToListAsync();

            return View(habitacoesViewModel);
        }

        // GET: Habitacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Habitacoes == null)
            {
                return NotFound();
            }

            var habitacao = await _context.Habitacoes.Include("Categoria").Include("Arrendamentos").Include("Tipologia")
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (habitacao == null)
            {
                return NotFound();
            }

            return View(habitacao);
        }

        // GET: Habitacoes/Create
        [Authorize(Roles = "Admin,Funcionario")]
        public IActionResult Create()
        {
            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");

            //  ViewData["ListaLocadores"] = new SelectList(_context.Locadores.ToList(), "Id", "Nome");

            ViewData["ListaTipologias"] = new SelectList(_context.Tipologia.ToList(), "Id", "Nome");

            return View();
        }

        // POST: Habitacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Custo,Disponivel,Localizacao,ArrendamentoId,TipologiaId,EstadoId,Avaliacao,CategoriaId,NBath,NBedroom,Area,Image")] Habitacao habitacao)
        {
            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            
           // ViewData["ListaLocadores"] = new SelectList(_context.Locadores.ToList(), "Id", "Nome");

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
        public async Task<IActionResult> Search([Bind("TextoAPesquisar,DataInicioPesquisa,DataFinalPesquisa,Localizacao,Tipologia")] HabitacoesViewModel pesquisaHabitacoes)
        {
            ViewData["Title"] = "Pesquisar Habitações";

            IQueryable<Habitacao> query = _context.Habitacoes.Include("Categoria").Include("Arrendamentos").Include("Tipologia");
            
            if (!string.IsNullOrEmpty(pesquisaHabitacoes.TextoAPesquisar))
            {
                query = query.Where(c =>
                    c.Nome.Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Localizacao.Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Tipologia.Nome.Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Custo.ToString().Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Categoria.Nome.Contains(pesquisaHabitacoes.TextoAPesquisar));
            }

            IQueryable<Habitacao> ListaFiltrada = query;
            
                foreach (var habitacao in query)
                {
                    bool disponivel = true;

                    foreach (var arrendamento in habitacao.Arrendamentos)
                    {
                        if ((arrendamento.DataInicio <= pesquisaHabitacoes.DataFinalPesquisa && arrendamento.DataFinal >= pesquisaHabitacoes.DataInicioPesquisa) || (arrendamento.DataFinal >= pesquisaHabitacoes.DataInicioPesquisa && arrendamento.DataInicio <= pesquisaHabitacoes.DataFinalPesquisa))
                        {
                            disponivel = false;
                            break;
                        }
                    }

                    if (!disponivel)
                    {
                        ListaFiltrada = ListaFiltrada.Where(c => c.Id != habitacao.Id);
                    }
                }

                query = ListaFiltrada;

                pesquisaHabitacoes.ListaHabitacoes = await query.ToListAsync();

                pesquisaHabitacoes.NResults = pesquisaHabitacoes.ListaHabitacoes.Count();

                string ordenarValue = Request.Form["Ordenar"]; // get value from select in search

            if (int.TryParse(ordenarValue, out int ordenar))
            {
                pesquisaHabitacoes.Ordenar = ordenar;

                switch (pesquisaHabitacoes.Ordenar)
                {
                    case 1:
                        pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes.OrderBy(c => c.Custo).ToList();
                        break;
                    case 2:
                        pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes.OrderByDescending(c => c.Custo).ToList();
                        break;
                    case 3:
                        pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes.OrderBy(c => c.Avaliacao).ToList();
                        break;
                    case 4:
                        pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes.OrderByDescending(c => c.Avaliacao).ToList();
                        break;
                    default:
                        break;
                }
            }

            //pesquisaHabitacoes.NResults = pesquisaHabitacoes.ListaHabitacoes.Count();

            return View(pesquisaHabitacoes);
        }

        public async Task<IActionResult> Search([Bind("TextoAPesquisar,DataInicioPesquisa,DataFinalPesquisa,Localizacao,Tipologia")] HabitacoesViewModel pesquisaHabit,
            [Bind("Id,Nome,Custo,NBath,NBedroom,Area,Disponivel,Localizacao,ArrendamentoId,TipologiaId,Avaliacao,EstadoId")] Habitacao habitacao, string? TextoAPesquisar)
        {
            //HabitacoesViewModel pesquisaHabit = new HabitacoesViewModel();

            ViewData["Title"] = "Pesquisar Habitações";

            if ( // if not both are filled
                    (pesquisaHabit.DataInicioPesquisa == default(DateTime) && pesquisaHabit.DataFinalPesquisa != default(DateTime)) ||
                    (pesquisaHabit.DataInicioPesquisa != default(DateTime) && pesquisaHabit.DataFinalPesquisa == default(DateTime))
                )
            {
                ModelState.AddModelError("DataInicioPesquisa", "Ambas as datas têm de ser especificadas!");
                ModelState.AddModelError("DataFinalPesquisa", "Ambas as datas têm de ser especificadas!");

                return RedirectToAction("Index", "Home", new { error = "Por favor, especifique ambas as datas!" });
            }

            if (pesquisaHabit.DataFinalPesquisa < pesquisaHabit.DataInicioPesquisa)
            {
                ModelState.AddModelError("DataInicioPesquisa", "Data de Final deve ser após a Data de Início!");
                ModelState.AddModelError("DataFinalPesquisa", "Data de Final deve ser após a Data de Início!");

                return RedirectToAction("Index", "Home", new { error = "Data de Final deve ser após a Data de Início!" });
            }

            IQueryable<Habitacao> listaHabitacao = _context.Habitacoes.Include("Categoria").Include("Tipologia").Include("Arrendamentos");


            if (string.IsNullOrWhiteSpace(TextoAPesquisar)) {
                listaHabitacao = _context.Habitacoes.Include("Categoria").Include("Arrendamentos").Include("Tipologia");
                pesquisaHabit.ListaHabitacoes = await listaHabitacao.ToListAsync();
            }
            else
            { 
                listaHabitacao =
                    _context.Habitacoes.Include("Categoria").Include("Tipologia").Include("Arrendamentos").Where(
                        c => c.Nome.Contains(TextoAPesquisar)
                                                || c.Localizacao.Contains(TextoAPesquisar)
                                                || c.Tipologia.Nome.Contains(TextoAPesquisar)
                                                || c.Custo.ToString().Contains(TextoAPesquisar)
                                                || c.Categoria.Nome.Contains(TextoAPesquisar)
                                                );

                pesquisaHabit.TextoAPesquisar = TextoAPesquisar;

            }

            if (habitacao.Localizacao != null) {
                var localizacao = _context.Habitacoes.Find(Convert.ToInt32(habitacao.Localizacao)).Localizacao;

                listaHabitacao = listaHabitacao.Where(l => l.Localizacao.Equals(localizacao));
            }

            if (habitacao.CategoriaId != 0 && habitacao.CategoriaId != null) {
                listaHabitacao = listaHabitacao.Where(c => c.CategoriaId == habitacao.CategoriaId);
            }

            if (habitacao.TipologiaId != 0 && habitacao.TipologiaId != null) {
                listaHabitacao = listaHabitacao.Where(c => c.TipologiaId == habitacao.TipologiaId);
            }

            pesquisaHabit.ListaHabitacoes = await listaHabitacao.ToListAsync();

            pesquisaHabit.NResults = pesquisaHabit.ListaHabitacoes.Count();

            return View(pesquisaHabit);
        }

        // POST: Habitacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Custo,NBath,NBedroom,Area,Disponivel,Localizacao,ArrendamentoId,TipologiaId,Avaliacao,EstadoId")] Habitacao habitacao)
        {
            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");

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
            if (habitacao != null && habitacao.Arrendamentos == null)
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
