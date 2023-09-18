using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudParisina.Models;

namespace CrudParisina.Controllers
{
    public class ProductoesController : Controller
    {
        private readonly ParisinaNetContext _context;

        public ProductoesController(ParisinaNetContext context)
        {
            _context = context;
        }

        // GET: Productoes
        public async Task<IActionResult> Index(string buscar, string filtro)
        {
            var productos = from Producto in _context.Productos select Producto;
            var productosConCategorias = _context.Productos.Include(p => p.IdCategoriaNavigation);


            if (!String.IsNullOrEmpty(buscar))
            {
                productos = productos.Where(s => s.NombreProducto!.Contains(buscar));
            }
            ViewData["FiltroNombre"] = String.IsNullOrEmpty(filtro) ? "NombreDescendente" : "";
            ViewData["FiltroPrecio"] = filtro == "PrecioAscendente" ? "PrecionDescendente" : "PrecioAscendente";
            switch (filtro)
            {
                case "NombreDescendente":
                    productos = productos.OrderByDescending(producto => producto.NombreProducto);
                    break;
                case "PrecioDescendente":
                    productos = productos.OrderByDescending(productos => productos.PrecioProducto);
                    break;
                case "PrecionAscendente":
                    productos = productos.OrderBy(productos => productos.PrecioProducto);
                    break;
                default:
                    productos = productos.OrderBy(producto => producto.NombreProducto);
                    break;
            }
            //Retornamos la lista de productos encontrados
            return View(await productos.ToListAsync());
        }

        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productoes/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria");
            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,IdCategoria,NombreProducto,DescripcionProducto,PrecioProducto,ImagenProducto,EstadoProducto")] Producto producto, IFormFile imagen)
        {
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.Length > 0)
                {
                    // Verificar la extensión del archivo
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(imagen.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ImagenProducto", "Solo se permiten archivos de imagen JPEG, PNG o GIF.");
                        return View(producto);
                    }

                    // Procesar la imagen si se ha subido
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        producto.ImagenProducto = memoryStream.ToArray();
                    }
                }

                //Lógica para guardar producto
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria", producto.IdCategoria);
            // Si hay errores de validación, muestra la vista nuevamente
            return View(producto);
        }


        // GET: Productoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria", producto.IdCategoria);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,IdCategoria,NombreProducto,DescripcionProducto,PrecioProducto,ImagenProducto,EstadoProducto")] Producto producto, IFormFile nuevaImagen)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (nuevaImagen != null && nuevaImagen.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(nuevaImagen.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("ImagenProducto", "Solo se permiten archivos de imagen JPEG, PNG o GIF.");
                            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria", producto.IdCategoria);
                            return View(producto);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            await nuevaImagen.CopyToAsync(memoryStream);
                            producto.ImagenProducto = memoryStream.ToArray();
                        }
                    }

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
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
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria", producto.IdCategoria);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'ParisinaNetContext.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
          return (_context.Productos?.Any(e => e.IdProducto == id)).GetValueOrDefault();
        }
    }
}
