﻿using System;
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
using HabitAqui.ViewModels;

namespace HabitAqui.Controllers
{
    public class ArrendamentosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArrendamentosController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
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
            var arrendamentos = _context.Arrendamentos.
            Include(a => a.Habitacao).
            Include(a => a.Habitacao.Tipologia).
            Include(a => a.ApplicationUser).
            Where(a => a.ApplicationUserId == _userManager.GetUserId(User));
            return View(await arrendamentos.ToListAsync());
        }

        // GET: Arrendamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Arrendamentos == null)
            {
                return NotFound();
            }

            var avaliacao = await _context.Arrendamentos
                .Include(a => a.ApplicationUser)
                .Include(a => a.Habitacao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (avaliacao == null)
            {
                return NotFound();
            }

            return View(avaliacao);
        }


        // GET: Arrendamentos/Create
        /*public IActionResult Create(int? id)
        {
            ViewData["NomeHabitacao"] = new SelectList(_context.Habitacoes.Where(c => c.Id == id).ToList(), "Id", "Nome");
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["EstadoId"] = new SelectList(_context.Set<Estado>(), "Id", "Id");
            ViewData["HabitacaoId"] = new SelectList(_context.Habitacoes, "Id", "Id");
            return View();
        }*/

        public IActionResult Create()
        {
            ViewData["HabitacaoListaBag"] = new SelectList(_context.Habitacoes, "Id", "Nome");

            return View();
        }

        /*public IActionResult CalculaPreco([Bind("DataInicio,DataFinal,HabitacaoId")] ArrendamentosViewModel pedido)
        {
            
            double NrDays = 0;

            if (pedido.DataInicio < DateTime.Now)
                ModelState.AddModelError("DataInicio", "A data de início deve ser depois da data atual!");
            if (pedido.DataInicio > pedido.DataFinal)
                ModelState.AddModelError("DataInicio", "A data de início não pode ser maior que a data final!");

            var habitacao = _context.Habitacoes.Include("Arrendamentos").Include("Tipologia").Include("Categoria").FirstOrDefault(v => v.Id == pedido.HabitacaoId);
            if (habitacao == null)
            {
                ModelState.AddModelError("HabitacaoId", "Habitação não existe!");
            }

            bool disponivel = true;

            /*foreach (Arrendamento arrendamento in habitacao.Arrendamentos)
            {
                
                if ((arrendamento.DataInicio <= pedido.DataFinal && arrendamento.DataFinal >= pedido.DataInicio) ||
                    (arrendamento.DataFinal >= pedido.DataInicio && arrendamento.DataInicio <= pedido.DataFinal))
                {
                    disponivel = false;
                    break;
                }
            }

            /*if (!disponivel)
            {
                ModelState.AddModelError("DataInicio", "Habitação já tem reserva neste período, pedimos desculpa.");
            }

            if (ModelState.IsValid)
            {
                NrDays = (pedido.DataFinal - pedido.DataInicio).TotalDays;

                Arrendamento x = new Arrendamento();
                x.DataFinal = pedido.DataFinal;
                x.DataInicio = pedido.DataInicio;
                x.HabitacaoId = pedido.HabitacaoId;
                x.DataPedido = DateTime.Now;
                x.CustoArrendamento = habitacao.Custo * (decimal?)NrDays;
                x.Habitacao = habitacao;
                x.Confirmado = false;

                pedido.ListaArrendamentos.Add(x);

                return View("Index", pedido.ListaArrendamentos);

            }
            else {
                return Problem("Não foi possível criar Arrendamento.");
            }

            //return View("Index", pedido);

            //return View("Pedido", pedido);
        }*/

        // POST: Arrendamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Cliente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustoArrendamento,DataInicio,DataFinal,DataPedido,HabitacaoId,ApplicationUserId,EstadoId")] Arrendamento arrendamento)
        {

            //ViewData["HabitacaoLista"] = new SelectList(_context.Habitacoes.ToList(), "Id", "Nome");

            if (arrendamento.DataInicio < DateTime.Now)
                ModelState.AddModelError("DataInicio", "A data de início deve ser depois da data atual!");
            if (arrendamento.DataInicio > arrendamento.DataFinal)
                ModelState.AddModelError("DataInicio", "A data de início não pode ser maior que a data final!");

            arrendamento.ApplicationUserId = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault().Id;

            //var habitacao = _context.Habitacoes.Include("Empresa").Include("Arrendamentos").Include("Tipologia").Include("Categoria").FirstOrDefault(v => v.Id == arrendamento.HabitacaoId);
            
            //if (habitacao == null)
            //{
            //    ModelState.AddModelError("HabitacaoId", "Habitação não existe!");
            //}

            double DiasArrendamento = (arrendamento.DataFinal - arrendamento.DataInicio).TotalDays;

            decimal custoPerDay = (decimal)_context.Habitacoes.Where(c => c.Id == arrendamento.HabitacaoId).FirstOrDefault().Custo;

            decimal precoTotal = (decimal)(DiasArrendamento * (double)custoPerDay);

            arrendamento.CustoArrendamento = precoTotal;

            if (ModelState.IsValid)
            {
                _context.Add(arrendamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", arrendamento.ApplicationUserId);
            //ViewData["EstadoId"] = new SelectList(_context., "Id", "Id", arrendamento.EstadoId);
            //ViewData["HabitacaoId"] = new SelectList(_context.Habitacoes.ToList(), "Id", "Nome", arrendamento.HabitacaoId);
            
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