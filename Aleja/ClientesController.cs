using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudParisina.Models;
using X.PagedList;



namespace CrudParisina.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ParisinaNetContext _context;

        public ClientesController(ParisinaNetContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string buscar, string filtrar, int? page)
        {
            var clientes = from Cliente in _context.Clientes select Cliente;

            if (!string.IsNullOrEmpty(buscar))
            {
                clientes = clientes.Where(s => s.NombreCliente.Contains(buscar));
            }

            ViewData["FiltroNombre"] = string.IsNullOrEmpty(filtrar) ? "NombreDescendente" : "NombreAscendente";

            switch (filtrar)
            {
                case "NombreDescendente":
                    clientes = clientes.OrderByDescending(categoria => categoria.NombreCliente);
                    break;
                case "NombreAscendente":
                    clientes = clientes.OrderBy(categoria => categoria.NombreCliente);
                    break;
                default:
                    clientes = clientes.OrderBy(categoria => categoria.NombreCliente);
                    break;
            }

            int pageSize = 5; // Tamaño de página
            int pageNumber = (page ?? 1); // Número de página actual, predeterminado a 1 si no se proporciona

            // Pagina los resultados y envía un objeto IPagedList a la vista
            var pagedClientes = await clientes.ToPagedListAsync(pageNumber, pageSize);

            ViewData["Buscar"] = buscar;
            ViewData["Page"] = pageNumber;

            return View(pagedClientes);
        }


        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCliente,IdUsuario,NombreCliente,TipoDocumento,NumeroDocumento,Direccion,Telefono,EstadoClientes")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCliente,IdUsuario,NombreCliente,TipoDocumento,NumeroDocumento,Direccion,Telefono,EstadoClientes")] Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.IdCliente))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'ParisinaNetContext.Clientes'  is null.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return (_context.Clientes?.Any(e => e.IdCliente == id)).GetValueOrDefault();
        }
    }
}
