﻿using System;
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
        public async Task<IActionResult> Index(int? OrdenarPor)
        {

            var habitacoesViewModel = new HabitacoesViewModel();

            habitacoesViewModel.ListaHabitacoes = await _context.Habitacoes.Include("Categoria").Include("Arrendamentos").Include("Tipologia").ToListAsync();

            if (OrdenarPor.HasValue) {
                
                habitacoesViewModel.Ordenar = (int)OrdenarPor;

                switch (habitacoesViewModel.Ordenar)
                {
                    case 1: // MAIOR AVALIACAO
                        habitacoesViewModel.ListaHabitacoes = habitacoesViewModel.ListaHabitacoes.OrderBy(h => h.Avaliacao).ToList();
                        break;
                    case 2: // MENOR AVALIACAO
                        habitacoesViewModel.ListaHabitacoes = habitacoesViewModel.ListaHabitacoes.OrderByDescending(h => h.Avaliacao).ToList();
                        break;
                    case 3: // MENOR CUSTO
                        habitacoesViewModel.ListaHabitacoes = habitacoesViewModel.ListaHabitacoes.OrderBy(h => h.Custo).ToList();
                        break;
                    case 4: // MAIOR CUSTO
                        habitacoesViewModel.ListaHabitacoes = habitacoesViewModel.ListaHabitacoes.OrderByDescending(h => h.Custo).ToList();
                        break;
                    default:
                        break;

                }
            }

            return View(habitacoesViewModel);
            
            //return _context.Habitacoes != null ? 
            //            View(await _context.Habitacoes.Include("Categoria").ToListAsync()) :
            //            Problem("Entity set 'ApplicationDbContext.Habitacoes'  is null.");
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
        public async Task<IActionResult> Create([Bind("Id,Nome,ContratoId,Custo,Disponivel,Localizacao,ArrendamentoId,TipologiaId,EstadoId,Avaliacao,CategoriaId,NBath,NBedroom,Area,Image")] Habitacao habitacao)
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
        public async Task<IActionResult> Search([Bind("TextoAPesquisar,DataInicioPesquisa,DataFinalPesquisa")] HabitacoesViewModel pesquisaHabitacoes)
        {
            ViewData["Title"] = "Pesquisar Habitações";

            IQueryable<Habitacao> query = _context.Habitacoes.Include("Categoria").Include("Arrendamentos").Include("Tipologia");

            
            if (!string.IsNullOrEmpty(pesquisaHabitacoes.TextoAPesquisar))
            {
                query = query.Where(c =>
                    c.Nome.Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Localizacao.Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Categoria.Nome.Contains(pesquisaHabitacoes.TextoAPesquisar));
            }

            if (pesquisaHabitacoes.DataInicioPesquisa != default && pesquisaHabitacoes.DataFinalPesquisa != default)
            {
                query = query.Where(c => c.Contrato.DataInicio >= pesquisaHabitacoes.DataInicioPesquisa && c.Contrato.DataFim <= pesquisaHabitacoes.DataFinalPesquisa);
            }

            pesquisaHabitacoes.ListaHabitacoes = await query.OrderBy(c => c.Nome).ToListAsync();
            pesquisaHabitacoes.NResults = pesquisaHabitacoes.ListaHabitacoes.Count();

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
                    await _context.Habitacoes.Include("Categoria").Include("Tipologia").Include("Arrendamentos").Where(c => c.Nome.Contains(TextoAPesquisar)
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
