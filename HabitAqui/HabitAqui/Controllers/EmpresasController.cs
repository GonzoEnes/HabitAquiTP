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
        public async Task<IActionResult> Create([Bind("Id,Nome,Avaliacao,Disponivel")] Empresa empresa)
        {
         
            if (ModelState.IsValid)
            {
                
                var user = CreateUser();
                var email = "gestor" + empresa.Nome.ToLower() + "@gmail.com";
                await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
                //await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
                user.PrimeiroNome = "Gestor";
                user.UltimoNome = empresa.Nome.ToLower();
                user.NIF = 0;
                user.DataNascimento = DateTime.Today;
                user.EmailConfirmed = true;
                user.Disponivel = true;

                var result = await _userManager.CreateAsync(user, "Gestor123!");

                if (result.Succeeded)
                {
                    _context.Add(empresa);
                    //   await _context.SaveChangesAsync();
                    var gestor = new Gestor
                    {
                        Id = empresa.Id,
                        Empresa = empresa,
                        ApplicationUser = user

                    };

                    _context.Update(gestor);
                    await _context.SaveChangesAsync();
                    await _userManager.AddToRoleAsync(user, "Gestor");
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(empresa);
        }

        [Authorize(Roles = "Gestor,Admin")]
        public async Task<IActionResult> ListEmpresaFuncionarios()
        {
            var applicationUserId = _userManager.GetUserId(User);
            var gestor = _context.Gestores.Where(m => m.ApplicationUser.Id == applicationUserId).FirstOrDefault();
            var funcionario = _context.Funcionarios.Include("ApplicationUser").Include("Empresa").Where(e => e.EmpresaId == gestor.EmpresaId);
            return View(await funcionario.ToListAsync());
        }

        //aqui
        // GET: Empresas/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empresa == null)
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
        public async Task<IActionResult> makeEmployeeAvailableUnavailable(int? id)
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

        //MUDAR VARIAVEIS
        [Authorize(Roles = "Gestor,Admin")]
        public async Task<IActionResult> makeManagerAvailableUnavailable(int? id)
        {
            if (id == null || _context.Gestores == null)
            {
                return NotFound();
            }

            var manager = await _context.Gestores.Include("ApplicationUser").Where(e => e.Id == id).FirstOrDefaultAsync();
            if (manager == null)
            {
                return NotFound();
            }
            if (manager.ApplicationUser.Disponivel == true)
            {

                manager.ApplicationUser.Disponivel = false;
                var userTask = _userManager.FindByEmailAsync(manager.ApplicationUser.Email);
                userTask.Wait();
                var user = userTask.Result;
                var lockUserTask = _userManager.SetLockoutEnabledAsync(user, true);
                lockUserTask.Wait();
                var lockDateTask = _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);
                lockDateTask.Wait();
            }
            else
            {
                manager.ApplicationUser.Disponivel = true;
                var userTask = _userManager.FindByEmailAsync(manager.ApplicationUser.Email);
                userTask.Wait();
                var user = userTask.Result;
                var lockDisabledTask = _userManager.SetLockoutEnabledAsync(user, false);
                lockDisabledTask.Wait();
                var setLockoutEndDateTask = _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));
                setLockoutEndDateTask.Wait();
            }
            try
            {
                _context.Update(manager);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(manager.Id))
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
        public async Task<IActionResult> Edit(int id, [Bind("Nome,Avaliacao,Disponivel")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }
            var gestors = await _context.Gestores.Where(e => e.EmpresaId == empresa.Id).ToListAsync();
            var funcionarios = await _context.Funcionarios.Where(e => e.EmpresaId == empresa.Id).ToListAsync();
            var habitacoes = await _context.Habitacoes.Where(e => e.EmpresaId == empresa.Id).ToListAsync();
            var arrendamentos = await _context.Arrendamentos.Include("Habitacao").ToListAsync();
            var check = false;

            if (arrendamentos == null)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var habi in habitacoes)
            {
                foreach (var arrenda in arrendamentos)
                {
                    if (arrenda.Habitacao.Id == habi.Id)
                    {
                        check = true;
                    }
                }
            }

            if (check == true)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                    if (empresa.Disponivel == false)
                    {

                        if (funcionarios != null)
                        {
                            foreach (var item in funcionarios)
                            {
                                await makeEmployeeAvailableUnavailable(item.Id);
                            }
                        }
                        if (gestors != null)
                        {
                            foreach (var item in gestors)
                            {
                                await makeManagerAvailableUnavailable(item.Id);
                            }
                        }
                        if (habitacoes != null)
                        {
                            foreach (var item in habitacoes)
                            {
                                item.Disponivel = false;
                                _context.Update(item);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {
                        if (funcionarios != null)
                        {
                            foreach (var item in funcionarios)
                            {
                                await makeEmployeeAvailableUnavailable(item.Id);
                            }
                        }
                        if (gestors != null)
                        {
                            foreach (var item in gestors)
                            {
                                await makeManagerAvailableUnavailable(item.Id);
                            }
                        }
                        if (habitacoes != null)
                        {
                            foreach (var item in habitacoes)
                            {
                                item.Disponivel = true;
                                _context.Update(item);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
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
            return View(empresa);
        }

        // GET: Empresas/Delete/5
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
                .Include(e => e.Funcionarios)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (empresa == null)
            {
                return NotFound();
            }

            if (empresa.Habitacoes.Any())
            {
                return Problem("Não é possível eliminar empresas que contêm habitações.\n");
            }

            // Remove associated Gestores
            foreach (var user in empresa.Gestores.Select(g => g.ApplicationUser).Where(u => u != null))
            {
                var result = await _userManager.DeleteAsync(user);
            }

            // Remove associated Funcionarios
            if (empresa.Funcionarios != null && empresa.Funcionarios.Any())
            {
                foreach (var user in empresa.Funcionarios.Select(g => g.ApplicationUser).Where(u => u != null))
                {
                    await _userManager.DeleteAsync(user);
                }

                _context.Funcionarios.RemoveRange(empresa.Funcionarios);
            }

            // Remove Empresa
            _context.Empresa.Remove(empresa);

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