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

        public IActionResult Create()
        {
            ViewData["ListaHabitacoes"] = new SelectList(_context.Habitacoes.Where(v => v.Disponivel == true).ToList(), "Id", "Nome");
            return View();
        }

        public async Task<IActionResult> Request(
            [Bind("Id,DataInicio,DataFinal,CustoArrendamento,DataPedido,HabitacaoId")] Arrendamento arrendamento)
        {
            
            ModelState.Remove(nameof(arrendamento.Habitacao));
            ModelState.Remove(nameof(arrendamento.ApplicationUserId));
            ModelState.Remove(nameof(arrendamento.ApplicationUser));

            arrendamento.ApplicationUserId = _userManager.GetUserId(User);
            arrendamento.DataPedido = DateTime.Now;
            arrendamento.Confirmado = false;
            if (ModelState.IsValid)
            {
                _context.Add(arrendamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["ListaHabitacao"] = new SelectList(_context.Habitacoes.Where(v => v.Disponivel == true).ToList(), "Id", "Nome", arrendamento.HabitacaoId);
            return View(arrendamento);
        }

        public IActionResult Calculate([Bind("HabitacaoId,DataInicio,DataFinal")] ArrendamentosViewModel request)
        {
            
            ViewData["ListaHabitacoes"] = new SelectList(_context.Habitacoes.Where(v => v.Disponivel == true).ToList(), "Id", "Nome");

            double NrDays = 0;

            if (request.DataInicio < DateTime.Now)
                ModelState.AddModelError("DataInicio", "A data de início não pode ser maior que a data atual!");
            if (request.DataInicio > request.DataFinal)
                ModelState.AddModelError("DataInicio", "A data de início não pode ser maior que a data de final!");


            var habitacao = _context.Habitacoes.Include("Arrendamentos").Include("Tipologia").Include("Categoria").FirstOrDefault(v => v.Id == request.HabitacaoId);
            if (habitacao == null)
            {
                ModelState.AddModelError("HabitacaoId", "Habitação não existe!");
            }

            bool available = true;
            
            foreach (Arrendamento arrendamento in habitacao.Arrendamentos)
            {
               
                if ((arrendamento.DataInicio <= request.DataFinal && arrendamento.DataFinal >= request.DataInicio) ||
                    (arrendamento.DataFinal >= request.DataInicio && arrendamento.DataInicio <= request.DataFinal))
                {
                    available = false;
                    break;
                }
            }
           
            if (!available)
            {
                ModelState.AddModelError("DataInicio", "Habitação já tem uma reserva neste período de tempo, pedimos desculpa.");
            }

            if (ModelState.IsValid)
            {
                NrDays = (request.DataFinal - request.DataInicio).TotalDays;

                Arrendamento x = new Arrendamento();
                x.DataFinal = request.DataFinal;
                x.DataInicio = request.DataInicio;
                x.HabitacaoId = request.HabitacaoId;
                x.CustoArrendamento = Math.Round(habitacao.Custo * (int)NrDays);
                x.Habitacao = habitacao;

                return View("PedidoArrendamento", x);

            }

            return View("Create", request);
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
            //ViewData["EstadoId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", arrendamento.EstadoId);
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
            //ViewData["EstadoId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", arrendamento.EstadoId);
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

        [Authorize(Roles = "Gestor,Funcionario,Admin")]
        public async Task<IActionResult> EstadoHabitacao(int? id) 
        {
            var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

            var gestores = await userManager.GetUsersInRoleAsync("Gestor");

            ViewData["Gestores"] = new SelectList(gestores, "Id", "PrimeiroNome");

            var arrendamento = await _context.Arrendamentos.Include("EstadoEntrega").Include("EstadoRececao").Include(c => c.Habitacao).FirstOrDefaultAsync(c => c.Id == id);

            if (arrendamento == null)
            {
                return NotFound("Não foi possível encontrar esse arrendamento.\n");
            }

            string danosDiretoria = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/fotografias/" + arrendamento.EstadoRececaoId.ToString());

            if (!Directory.Exists(danosDiretoria)) {
                Directory.CreateDirectory(danosDiretoria);
            }

            var ficheiros = from ficheiro in Directory.EnumerateFiles(danosDiretoria)
                            select string.Format("/img/fotografias/{0}/{1}", arrendamento.EstadoRececaoId, Path.GetFileName(ficheiro));

            ViewData["Ficheiros"] = ficheiros;

            if (id == null || _context.Arrendamentos == null) {
                return NotFound();
            }

            var arrendamentoAux = await _context.Arrendamentos.FindAsync(id);

            if (arrendamentoAux == null) {
                return NotFound();
            }

            return View(arrendamentoAux);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor,Funcionario,Admin")]
        public async Task<IActionResult> EstadoHabitacao(
            int id,
            [Bind("Id,DataInicio,DataFinal,CustoArrendamento,DataPedido,HabitacaoId,Habitacao,ApplicationUserId,Confirmado,ApplicationUser,EstadoEntrega,EstadoRececao")] Arrendamento arrendamento,
            [Bind("Id,Nome,Equipamentos,Danos,Observacoes,ApplicationUserId")] Estado estadoEntrega,
            [Bind("Danos,Equipamentos,Observacoes,ApplicationUserId")] Estado estadoRececao,
            [FromForm] List<IFormFile> fotografias
        )
        {
            if (id != arrendamento.Id) {
                return NotFound();
            }

            var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

            var funcionarios = await userManager.GetUsersInRoleAsync("Gestor");

            ViewData["Funcionarios"] = new SelectList(funcionarios, "Id", "PrimeiroNome");

            var arrendamentoComEstado = await _context.Arrendamentos.Include("EstadoEntrega").Include("EstadoRececao").Include("Habitacao").FirstOrDefaultAsync(c => c.Id == id);

            if (arrendamentoComEstado == null) {
                return NotFound();
            }

            string danosDiretoria = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/fotografias/" + arrendamento.EstadoRececao.ToString());

            if (!Directory.Exists(danosDiretoria))
            {
                Directory.CreateDirectory(danosDiretoria);
            }

            var ficheiros = from ficheiro in Directory.EnumerateFiles(danosDiretoria)
                            select string.Format("/img/fotografias/{0}/{1}", arrendamento.EstadoRececaoId, Path.GetFileName(ficheiro));

            ViewData["Ficheiros"] = ficheiros;

            arrendamentoComEstado.EstadoEntrega = estadoEntrega;

            arrendamentoComEstado.EstadoRececao = estadoRececao;

            if (ModelState.IsValid) {
                try {
                    _context.Update(arrendamentoComEstado);

                    await _context.SaveChangesAsync();

                    _context.Habitacoes.Find(arrendamentoComEstado.HabitacaoId).EstadoId = arrendamentoComEstado.EstadoEntregaId;

                    await _context.SaveChangesAsync();

                    if (estadoRececao.Danos) {
                        string diretoria = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/fotografias");

                        if (!Directory.Exists(diretoria)) {
                            Directory.CreateDirectory(diretoria);
                        }

                        foreach (var fotografia in fotografias) {
                            if (fotografia.Length > 0) {
                                var diretoriaFicheiro = Path.Combine(diretoria, Guid.NewGuid().ToString() + Path.GetExtension(fotografia.FileName));

                                while (System.IO.File.Exists(diretoriaFicheiro)) {
                                    diretoriaFicheiro = Path.Combine(diretoria, Guid.NewGuid().ToString() + Path.GetExtension(fotografia.FileName));
                                }

                                using (var stream = System.IO.File.Create(diretoriaFicheiro)) {
                                    await fotografia.CopyToAsync(stream);
                                }
                            }
                        }
                    }
                    _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ArrendamentoExists(arrendamentoComEstado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index)); // trocar para arrendamentos DESTA empresa, criar função de ListarArrendamentosByEmpresa
            }
            ViewData["ListaHabitacoes"] = new SelectList(_context.Habitacoes.ToList(), "Id", "Nome", arrendamentoComEstado.Id);

            return View(arrendamentoComEstado);
        }

            private bool ArrendamentoExists(int id)
        {
            return (_context.Arrendamentos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}