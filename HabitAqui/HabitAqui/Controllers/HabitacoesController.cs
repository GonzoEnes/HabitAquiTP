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
using System.Security.Cryptography.Xml;
using System.Security.Claims;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Hosting;

namespace HabitAqui.Controllers
{
    public class HabitacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public HabitacoesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Habitacoes
        public async Task<IActionResult> Index()
        {
            
            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");

            ViewData["ListaEmpresas"] = new SelectList(_context.Empresa.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");

            var habitacoesViewModel = new HabitacoesViewModel();

            habitacoesViewModel.ListaHabitacoes = await _context.Habitacoes.Include("Categoria").Include("Arrendamentos").Include("Tipologia").Include("Empresa").Where(c => c.Disponivel == true).ToListAsync();

            return View(habitacoesViewModel);
        }

        // GET: Habitacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Habitacoes == null)
            {
                return NotFound();
            }

            var habitacao = await _context.Habitacoes.Include("Categoria").Include("Arrendamentos").Include("Tipologia").Include("Empresa")
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (habitacao == null)
            {
                return NotFound();
            }

            return View(habitacao);
        }

        // GET: Habitacoes/Create
        [Authorize(Roles = "Admin,Funcionario,Gestor")]
        public IActionResult Create()
        {
            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");

            ViewData["ListaTipologias"] = new SelectList(_context.Tipologia.ToList(), "Id", "Nome");

            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Gestor"))
            {
                var gestor = _context.Gestores.Where(c => c.ApplicationUser.Id == appUserId).FirstOrDefault();

                ViewData["ListaEmpresas"] = new SelectList(_context.Empresa.Where(c => c.Id == gestor.EmpresaId).ToList(), "Id", "Nome");
            }
            else if (User.IsInRole("Funcionario"))
            {
                var funcionario = _context.Funcionarios.Where(c => c.ApplicationUser.Id == appUserId).FirstOrDefault();

                ViewData["ListaEmpresas"] = new SelectList(_context.Empresa.Where(c => c.Id == funcionario.EmpresaId).ToList(), "Id", "Nome");
            }
            else if (User.IsInRole("Admin"))
            {
                ViewData["ListaEmpresas"] = new SelectList(_context.Empresa.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");
            }

            return View();
        }

        private async Task<string> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return new string("Erro, não há fotografia.");

            
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");

            // Garante que o diretório de destino existe, caso contrário, cria-o
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Gera um nome de arquivo único para evitar colisões
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

            // Caminho completo para o arquivo no sistema de arquivos
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Salva a imagem no sistema de arquivos
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Retorna a URL da imagem (assumindo que 'img' seja o diretório virtual para as imagens)
            return uniqueFileName;
        }

        // POST: Habitacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Funcionario,Gestor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Custo,Disponivel,Localizacao,ArrendamentoId,TipologiaId,EstadoId,EmpresaId,Avaliacao,PeriodoMinimoArrendamento,CategoriaId,NBath,NBedroom,Area,ImagePath, Fotografia")] Habitacao habitacao)
        {

            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");
            
           if (ModelState.IsValid)
            {

                if (habitacao.Fotografia != null)
                {
                    habitacao.ImagePath = await UploadImage(habitacao.Fotografia);
                }
                _context.Add(habitacao);
                await _context.SaveChangesAsync();
                if (User.IsInRole("Admin")) {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(ListaHabitacoesByEmpresaId));

            }

            ViewData["ListaDeHabitacoes"] = new SelectList(_context.Habitacoes.OrderBy(c => c.Disponivel).ToList(), "Id", "Nome");
            return View(habitacao);
        }

        // GET: Habitacoes/Edit/5
        [Authorize(Roles = "Admin,Funcionario,Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");

            ViewData["ListaEmpresas"] = new SelectList(_context.Empresa.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");

            ViewData["ListaTipologias"] = new SelectList(_context.Tipologia.ToList(), "Id", "Nome");

            if (id == null || _context.Habitacoes == null)
            {
                return NotFound();
            }

            var habitacao = await _context.Habitacoes.FindAsync(id);
            
            if (habitacao == null)
            {
                return NotFound();
            }

            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Gestor"))
            {
                var gestor = _context.Gestores.Where(c => c.ApplicationUser.Id == appUserId).FirstOrDefault();

                if (habitacao.EmpresaId != gestor.EmpresaId) {
                    return Problem("Não pode editar uma habitação que não é da sua empresa.");
                }

                
            }
            else if (User.IsInRole("Funcionario"))
            {
                var funcionario = _context.Funcionarios.Where(c => c.ApplicationUser.Id == appUserId).FirstOrDefault();

                if (habitacao.EmpresaId != funcionario.EmpresaId)
                {
                    return Problem("Não pode editar uma habitação que não é da sua empresa.");
                }
            }

            
            return View(habitacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([Bind("TextoAPesquisar,DataInicioPesquisa,DataFinalPesquisa,Localizacao,Tipologia,Empresa,Categoria,Ordenar")] HabitacoesViewModel pesquisaHabitacoes)
        {
            ViewData["Title"] = "Pesquisar Habitações";

            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");

            ViewData["ListaEmpresas"] = new SelectList(_context.Empresa.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");

            IQueryable<Habitacao> query = _context.Habitacoes.Include("Categoria").Include("Arrendamentos").Include("Tipologia").Include("Empresa");
            
            if (!string.IsNullOrEmpty(pesquisaHabitacoes.TextoAPesquisar))
            {
                query = query.Where(c =>
                    c.Nome.Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Localizacao.Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Tipologia.Nome.Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Custo.ToString().Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Empresa.Nome.Contains(pesquisaHabitacoes.TextoAPesquisar) ||
                    c.Categoria.Nome.Contains(pesquisaHabitacoes.TextoAPesquisar));
            }

            IQueryable<Habitacao> ListaFiltrada = query;
            
                

                query = ListaFiltrada;

                pesquisaHabitacoes.ListaHabitacoes = await query.ToListAsync();

                pesquisaHabitacoes.NResults = pesquisaHabitacoes.ListaHabitacoes.Count();

            //string ordenarValue = Request.Form["Ordenar"]; // get value from select in search

            /*if (int.TryParse(ordenarValue, out int ordenar))
            {*/
            // pesquisaHabitacoes.Ordenar = ordenar;

            //string ordenarValue = Request.Form["Ordenar"];

            //pesquisaHabitacoes.Ordenar = int.Parse(ordenarValue);

                Console.WriteLine("AIJDAIDWAODJI \n\n\n\n\n\n\n" + pesquisaHabitacoes.Ordenar);

            switch (pesquisaHabitacoes.Ordenar)
                {
                    case 1:
                        pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes  
                            .OrderBy(c => c.Custo)
                            .ToList();
                        break;
                    case 2:
                        pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                            .OrderByDescending(c => c.Custo)
                            .ToList();
                        break;
                    case 3:
                        pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                            .Where(c => c.MediaAvaliacoes.HasValue)  
                            .OrderBy(c => c.MediaAvaliacoes)
                            .ToList();
                        break;
                    case 4:
                        pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                            .Where(c => c.MediaAvaliacoes.HasValue) 
                            .OrderByDescending(c => c.MediaAvaliacoes)
                            .ToList();
                        break;
                    default:
                        break;
                }

            if (!string.IsNullOrEmpty(pesquisaHabitacoes.Categoria))
            {
                int valor = int.Parse(pesquisaHabitacoes.Categoria);
                pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes.Where(c => c.CategoriaId == valor).ToList();
            }

            if (!string.IsNullOrEmpty(pesquisaHabitacoes.Empresa))
            {
                int valor = int.Parse(pesquisaHabitacoes.Empresa);
                pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes.Where(c => c.EmpresaId == valor).ToList();
            }

            if (!string.IsNullOrEmpty(pesquisaHabitacoes.PeriodoMinimoArrendamento))
            {
                int valor = int.Parse(pesquisaHabitacoes.PeriodoMinimoArrendamento);
                pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes.Where(c => c.PeriodoMinimoArrendamento == valor).ToList();
            }


            return View(pesquisaHabitacoes);
        }

        public async Task<IActionResult> SearchEmpresa([Bind("TextoAPesquisar,DataInicioPesquisa,DataFinalPesquisa,Localizacao,Tipologia,Empresa,Categoria")] HabitacoesViewModel pesquisaHabitacoes)
        {

            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.Where(c => c.Disponivel == true), "Id", "Nome");

            string ordenarValue = Request.Form["Ordenar"]; // get value from select in search

            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Funcionario")) {
                var funcionario = _context.Funcionarios.Where(c => c.ApplicationUser.Id == appUserId).FirstOrDefault();

                pesquisaHabitacoes.ListaHabitacoes = await _context.Habitacoes.Include("Categoria").Include("Tipologia").Include("Arrendamentos").Include("Empresa").Where(c => c.EmpresaId == funcionario.EmpresaId).ToListAsync();

                if (pesquisaHabitacoes.Categoria != null)
                {
                    pesquisaHabitacoes.ListaHabitacoes = await _context.Habitacoes.Include("Categoria").Include("Tipologia").Include("Arrendamentos").Include("Empresa").Where(c => c.EmpresaId == funcionario.EmpresaId && c.Categoria.Id == int.Parse(pesquisaHabitacoes.Categoria)).ToListAsync();
                }

                if (int.TryParse(ordenarValue, out int ordenar))
                {
                    pesquisaHabitacoes.Ordenar = ordenar;

                    switch (pesquisaHabitacoes.Ordenar)
                    {
                        case 1:
                            pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                                .OrderBy(c => c.Custo)
                                .ToList();
                            break;
                        case 2:
                            pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                                .OrderByDescending(c => c.Custo)
                                .ToList();
                            break;
                        case 3:
                            pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                                .Where(c => c.MediaAvaliacoes.HasValue)
                                .OrderBy(c => c.MediaAvaliacoes)
                                .ToList();
                            break;
                        case 4:
                            pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                                .Where(c => c.MediaAvaliacoes.HasValue)
                                .OrderByDescending(c => c.MediaAvaliacoes)
                                .ToList();
                            break;
                        default:
                            break;
                    }

                }
            }

            else if (User.IsInRole("Gestor")) {
                var gestor = _context.Gestores.Where(c => c.ApplicationUser.Id == appUserId).FirstOrDefault();

                pesquisaHabitacoes.ListaHabitacoes = _context.Habitacoes.Include("Categoria").Include("Tipologia").Include("Arrendamentos").Include("Empresa").Where(c => c.EmpresaId == gestor.EmpresaId).ToList();

                if (pesquisaHabitacoes.Categoria != null)
                {
                    pesquisaHabitacoes.ListaHabitacoes = await _context.Habitacoes.Include("Categoria").Include("Tipologia").Include("Arrendamentos").Include("Empresa").Where(c => c.EmpresaId == gestor.EmpresaId && c.Categoria.Id == int.Parse(pesquisaHabitacoes.Categoria)).ToListAsync();
                }

                if (int.TryParse(ordenarValue, out int ordenar))
                {
                    pesquisaHabitacoes.Ordenar = ordenar;

                    switch (pesquisaHabitacoes.Ordenar)
                    {
                        case 1:
                            pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                                .OrderBy(c => c.Custo)
                                .ToList();
                            break;
                        case 2:
                            pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                                .OrderByDescending(c => c.Custo)
                                .ToList();
                            break;
                        case 3:
                            pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                                .Where(c => c.MediaAvaliacoes.HasValue)
                                .OrderBy(c => c.MediaAvaliacoes)
                                .ToList();
                            break;
                        case 4:
                            pesquisaHabitacoes.ListaHabitacoes = pesquisaHabitacoes.ListaHabitacoes
                                .Where(c => c.MediaAvaliacoes.HasValue)
                                .OrderByDescending(c => c.MediaAvaliacoes)
                                .ToList();
                            break;
                        default:
                            break;
                    }

                }
            }


            return View(pesquisaHabitacoes);
        }

        public async Task<IActionResult> Search([Bind("TextoAPesquisar,DataInicioPesquisa,DataFinalPesquisa,Localizacao,Tipologia,Empresa,Categoria,PeriodoMinimoArrendamento,Ordenar")] HabitacoesViewModel pesquisaHabit,
            [Bind("Id,Nome,Custo,NBath,NBedroom,Area,Disponivel,Localizacao,ArrendamentoId,TipologiaId,MediaAvaliacoes,PeriodoMinimoArrendamento,EstadoId,EmpresaId,CategoriaId")] Habitacao habitacao, string? TextoAPesquisar)
        {

            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");

            ViewData["ListaEmpresas"] = new SelectList(_context.Empresa.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");


            ViewData["Title"] = "Pesquisar Habitações";

            if ( 
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

            IQueryable<Habitacao> listaHabitacao = _context.Habitacoes.Include("Categoria").Include("Tipologia").Include("Arrendamentos").Include("Empresa");


            if (string.IsNullOrWhiteSpace(TextoAPesquisar)) {
                listaHabitacao = _context.Habitacoes.Include("Categoria").Include("Arrendamentos").Include("Tipologia").Include("Empresa");
                pesquisaHabit.ListaHabitacoes = await listaHabitacao.ToListAsync();
            }
            else
            { 
                listaHabitacao =
                    _context.Habitacoes.Include("Categoria").Include("Tipologia").Include("Arrendamentos").Include("Empresa").Where(
                        c => c.Nome.Contains(TextoAPesquisar)
                                                || c.Localizacao.Contains(TextoAPesquisar)
                                                || c.Tipologia.Nome.Contains(TextoAPesquisar)
                                                || c.Custo.ToString().Contains(TextoAPesquisar)
                                                || c.Categoria.Nome.Contains(TextoAPesquisar)
                                                );

                pesquisaHabit.TextoAPesquisar = TextoAPesquisar;

            }

            if (habitacao.Localizacao != null)
            {
                var localizacao = _context.Habitacoes.Find(Convert.ToInt32(habitacao.Localizacao)).Localizacao;

                listaHabitacao = listaHabitacao.Where(l => l.Localizacao.Equals(localizacao));
            }

            if (habitacao.TipologiaId != 0 && habitacao.TipologiaId != null)
            {
                listaHabitacao = listaHabitacao.Where(c => c.TipologiaId == habitacao.TipologiaId);
            }

            if (habitacao.EmpresaId != 0 && habitacao.EmpresaId != null)
            {
                listaHabitacao = listaHabitacao.Where(c => c.EmpresaId == habitacao.EmpresaId);
            }

            if (habitacao.CategoriaId != 0 && habitacao.CategoriaId != null)
            {
                listaHabitacao = listaHabitacao.Where(c => c.CategoriaId == habitacao.CategoriaId);
            }

            if (habitacao.PeriodoMinimoArrendamento != 0 && habitacao.PeriodoMinimoArrendamento != null) {
                listaHabitacao = listaHabitacao.Where(c => c.PeriodoMinimoArrendamento == habitacao.PeriodoMinimoArrendamento);
            }

            IQueryable<Habitacao> listaFiltrada = listaHabitacao;

            foreach (var habitacaoForLoop in listaHabitacao)
            {
                bool disponivel = true;

                foreach (var arrendamento in habitacaoForLoop.Arrendamentos)
                {
                    if ((arrendamento.DataInicio <= pesquisaHabit.DataFinalPesquisa && arrendamento.DataFinal >= pesquisaHabit.DataInicioPesquisa) || (arrendamento.DataFinal >= pesquisaHabit.DataInicioPesquisa && arrendamento.DataInicio <= pesquisaHabit.DataFinalPesquisa))
                    {
                        disponivel = false;
                        break;
                    }
                }

                if (!disponivel)
                {
                    listaFiltrada = listaFiltrada.Where(c => c.Id != habitacao.Id);
                }
            }

            listaHabitacao = listaFiltrada;

            pesquisaHabit.ListaHabitacoes = await listaHabitacao.ToListAsync();

            pesquisaHabit.NResults = pesquisaHabit.ListaHabitacoes.Count();

            /*if (!string.IsNullOrEmpty(pesquisaHabit.Categoria))
            {
                int valor = int.Parse(pesquisaHabit.Categoria);
                listaHabitacao = listaHabitacao.Where(c => c.CategoriaId == valor);
            }

            if (!string.IsNullOrEmpty(pesquisaHabit.Empresa))
            {
                int valor = int.Parse(pesquisaHabit.Empresa);
                listaHabitacao = listaHabitacao.Where(c => c.EmpresaId == valor);
            }

            if (!string.IsNullOrEmpty(pesquisaHabit.PeriodoMinimoArrendamento))
            {
                int valor = int.Parse(pesquisaHabit.PeriodoMinimoArrendamento);
                listaHabitacao = listaHabitacao.Where(c => c.PeriodoMinimoArrendamento == valor);
            }*/


            switch (pesquisaHabit.Ordenar)
                {
                    case 1:
                        pesquisaHabit.ListaHabitacoes = pesquisaHabit.ListaHabitacoes
                            .OrderBy(c => c.Custo)
                            .ToList();
                        break;
                    case 2:
                        pesquisaHabit.ListaHabitacoes = pesquisaHabit.ListaHabitacoes
                            .OrderByDescending(c => c.Custo)
                            .ToList();
                        break;
                    case 3:
                        pesquisaHabit.ListaHabitacoes = pesquisaHabit.ListaHabitacoes
                            .Where(c => c.MediaAvaliacoes.HasValue)
                            .OrderBy(c => c.MediaAvaliacoes)
                            .ToList();
                        break;
                    case 4:
                        pesquisaHabit.ListaHabitacoes = pesquisaHabit.ListaHabitacoes
                            .Where(c => c.MediaAvaliacoes.HasValue)
                            .OrderByDescending(c => c.MediaAvaliacoes)
                            .ToList();
                        break;
                    default:
                        break;
                }
            

            

            return View(pesquisaHabit);
        }

        // POST: Habitacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Custo,NBath,NBedroom,Area,PeriodoMinimoArrendamento,Disponivel,Localizacao,ArrendamentoId,TipologiaId,Avaliacao,EstadoId,EmpresaId,CategoriaId,ImagePath,Fotografia")] Habitacao habitacao)
        {
            if (id != habitacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (habitacao.Fotografia != null)
                {
                    habitacao.ImagePath = await UploadImage(habitacao.Fotografia);
                }

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

            if (habitacao.Arrendamentos != null)
            {
                return Problem("Esta habitação possui arrendamentos. Impossível apagar.");
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

        [Authorize(Roles = "Gestor,Funcionario")]
        public async Task<IActionResult> ListaHabitacoesByEmpresaId()
        {
            ViewData["ListaCategorias"] = new SelectList(_context.Categorias.Where(c => c.Disponivel == true).ToList(), "Id", "Nome");

            var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (User.IsInRole("Funcionario"))
            {
                var funcionario = _context.Funcionarios.Where(e => e.ApplicationUser.Id == applicationUserId).FirstOrDefault();
                var habitacoesFunc = await _context.Habitacoes.Include("Empresa").Include("Categoria").Include("Tipologia").Where(v => v.EmpresaId == funcionario.EmpresaId).ToListAsync();
                return View(habitacoesFunc);
            }
            var gestor = _context.Gestores.Where(e => e.ApplicationUser.Id == applicationUserId).FirstOrDefault();
            var habitacoes = await _context.Habitacoes.Include("Empresa").Include("Categoria").Include("Tipologia").Where(v => v.EmpresaId == gestor.EmpresaId).ToListAsync();

            var habitacoesViewModel = new HabitacoesViewModel();

            habitacoesViewModel.ListaHabitacoes = habitacoes;
            
            return View(habitacoesViewModel);
        }
    }
}
