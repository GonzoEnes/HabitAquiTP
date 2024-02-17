using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using HabitAqui.Data;
using HabitAqui.Models;
using HabitAqui.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.Linq;

namespace HabitAqui.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly IUserStore<ApplicationUser> _userStore;
        public EmpresasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
        {
            _context = context;
            _userManager = userManager;
            _userStore = userStore;
            // _emailStore = GetEmailStore();
        }

        // GET: Companies
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empresa.Include("Habitacoes").ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(string? TextoAPesquisar, bool? disponivel)
        {
            SearchEmpresasViewModel searchEmpresa = new SearchEmpresasViewModel();

            if (string.IsNullOrWhiteSpace(TextoAPesquisar) && disponivel == null)
            {
                searchEmpresa.ListaEmpresas = await _context.Empresa.Include("Habitacoes").ToListAsync();
            }

            if (disponivel != null && TextoAPesquisar != null)
            {
                searchEmpresa.ListaEmpresas = await _context.Empresa.Include("Habitacoes").Where(c => c.Disponivel == disponivel && c.Nome.Contains(TextoAPesquisar)).ToListAsync();
            }

            if (TextoAPesquisar != null && disponivel == null)
            {
                searchEmpresa.ListaEmpresas = await _context.Empresa.Include("Habitacoes").Where(c => c.Nome.Contains(TextoAPesquisar)).ToListAsync();
            }

            if (disponivel != null && string.IsNullOrEmpty(TextoAPesquisar))
            {
                searchEmpresa.ListaEmpresas = await _context.Empresa.Include("Habitacoes").Where(c => c.Disponivel == disponivel).ToListAsync();
            }

            searchEmpresa.ListaEmpresas = searchEmpresa.ListaEmpresas.OrderBy(c => c.Nome).ToList();
            searchEmpresa.NResultados = await _context.Empresa.CountAsync();
            return View(searchEmpresa);
        }

        // GET: Empresas/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa.Include("Habitacoes")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id, Nome, Avaliacao, Disponivel")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                var email = "gestor" + empresa.Nome.ToLower() + "@gmail.com";

                // Create a new user
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    PrimeiroNome = "Gestor",
                    UltimoNome = empresa.Nome.ToLower(),
                    NIF = 0,
                    DataNascimento = DateTime.Today,
                    EmailConfirmed = true,
                    Disponivel = true
                };

                var result = await _userManager.CreateAsync(user, "Gestor123!");

                if (result.Succeeded)
                {
                    // Add the new user to the "Gestor" role
                    await _userManager.AddToRoleAsync(user, "Gestor");

                    // Add the new empresa and associated gestor to the context
                    _context.Add(empresa);

                    var gestor = new Gestor
                    {
                        EmpresaId = empresa.Id,
                        Empresa = empresa,
                        ApplicationUser = user
                    };

                    _context.Add(gestor);

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                // If user creation fails, add errors to ModelState and return to the view
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If ModelState is not valid, return to the view with the existing data
            return View(empresa);
        }

        [Authorize(Roles = "Gestor,Admin")]
        public async Task<IActionResult> ListEmpresaFuncionarios()
        {
            var applicationUserId = _userManager.GetUserId(User);
            var gestor = _context.Gestores.Where(m => m.ApplicationUser.Id == applicationUserId).FirstOrDefault();

            // Check if gestor is null before accessing its properties
            if (gestor == null)
            {
                // Handle the case where gestor is null, maybe return a NotFound or BadRequest result
                return NotFound();
            }

            // Make sure gestor.EmpresaId is not null before using it
            var funcionario = _context.Funcionarios.Include("ApplicationUser").Include("Empresa").Where(e => e.EmpresaId == gestor.EmpresaId);

            return View(await funcionario.ToListAsync());
        }


        //aqui
        // GET: Empresas/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa.FindAsync(id);

            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }



        [Authorize(Roles = "Gestor,Admin")]
        public async Task<IActionResult> FuncionarioAvailableUnavailable(int? id)
        {
            if (id == null || _context.Funcionarios == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios.Include("ApplicationUser").Where(e => e.Id == id).FirstOrDefaultAsync();
            if (funcionario == null)
            {
                return NotFound();
            }
            if (funcionario.ApplicationUser.Disponivel == true)
            {

                funcionario.ApplicationUser.Disponivel = false;
                var userTask = _userManager.FindByEmailAsync(funcionario.ApplicationUser.Email);
                userTask.Wait();
                var user = userTask.Result;
                var lockUserTask = _userManager.SetLockoutEnabledAsync(user, true);
                lockUserTask.Wait();
                var lockDateTask = _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);
                lockDateTask.Wait();
            }
            else
            {
                funcionario.ApplicationUser.Disponivel = true;
                var userTask = _userManager.FindByEmailAsync(funcionario.ApplicationUser.Email);
                userTask.Wait();
                var user = userTask.Result;
                var lockDisabledTask = _userManager.SetLockoutEnabledAsync(user, false);
                lockDisabledTask.Wait();
                var setLockoutEndDateTask = _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));
                setLockoutEndDateTask.Wait();
            }
            try
            {
                _context.Update(funcionario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(funcionario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ListEmpresaFuncionarios));
        }

       


        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Nome, Avaliacao, Disponivel")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            
                try
                {
                
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Log the exception or handle it as needed
                    // For example, you can log the error using a logging framework
                    // logger.LogError(ex, "Concurrency error while updating the empresa with ID {EmpresaId}", empresa.Id);

                    if (!EmpresaExists(empresa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
           

            // Log ModelState errors
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                // Log or print the error messages
            }

            return View(empresa);
        }


        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteFunc(int? id)
        {
            if (id == null || _context.Habitacoes == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(m => m.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            

            return View(funcionario);
        }

        [HttpPost, ActionName("DeleteFunc")]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmedFunc(int id)
        {

            if (_context.Funcionarios == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Habitacoes'  is null.");
            }
 
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario != null)
            {
                _context.Funcionarios.Remove(funcionario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListEmpresaFuncionarios));
        }


          


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Retrieve the Empresa along with its associated entities
            var empresa = await _context.Empresa
                .Include(e => e.Habitacoes)
                    .ThenInclude(h => h.Arrendamentos)
                .Include(e => e.Gestores)
                    .ThenInclude(g => g.ApplicationUser)
                .Include(e => e.Funcionarios)
                    .ThenInclude(f => f.ApplicationUser)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (empresa == null)
            {
                return NotFound();
            }

            if (empresa.Habitacoes.Any())
            {
                return Problem("Não é possível eliminar empresas que contêm habitações.\n");
            }

            // Remove associated Funcionarios
            if (empresa.Funcionarios != null && empresa.Funcionarios.Any())
            {
                foreach (var funcionario in empresa.Funcionarios)
                {
                    var user = funcionario.ApplicationUser;
                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);
                    }
                }

                _context.Funcionarios.RemoveRange(empresa.Funcionarios);
            }

            // Remove associated Gestores
            if (empresa.Gestores != null && empresa.Gestores.Any())
            {
                foreach (var gestor in empresa.Gestores)
                {
                    var user = gestor.ApplicationUser;
                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);
                    }
                }

                _context.Gestores.RemoveRange(empresa.Gestores);
            }

            // Remove Empresa
            _context.Empresa.Remove(empresa);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> makeGestorAvailableUnavailable(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gestores = await _context.Gestores
                .Include(g => g.ApplicationUser)  // Assuming ApplicationUser is a navigation property
                .Where(g => g.EmpresaId == id)
                .ToListAsync();

            var funcionarios = await _context.Funcionarios
                .Include(g => g.ApplicationUser)  // Assuming ApplicationUser is a navigation property
                .Where(g => g.EmpresaId == id)
                .ToListAsync();

            var empresa = await _context.Empresa
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (gestores == null || empresa == null)
            {
                return NotFound();
            }

            foreach (var gestor in gestores)
            {
                // Toggle the Disponivel property
                empresa.Disponivel = !empresa.Disponivel;
                gestor.ApplicationUser.Disponivel = !gestor.ApplicationUser.Disponivel;

                _context.Update(empresa);
                _context.Update(gestor);
            }
            if (funcionarios == null || empresa == null)
            {
                return NotFound();
            }

            foreach (var func in funcionarios)
            {
                empresa.Disponivel = !empresa.Disponivel;
                func.ApplicationUser.Disponivel = !func.ApplicationUser.Disponivel;

                _context.Update(empresa);
                _context.Update(func);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (roles.Count > 0)
                {
                    foreach (var role in roles.ToList())
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }
                await _userManager.DeleteAsync(user);
                transaction.Commit();
            }
            return Ok();
        }

        private bool EmpresaExists(int id)
        {
            return (_context.Empresa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

    }
}