using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tp1WebApp.Models;

namespace Tp1WebApp.Controllers
{
    public class LibrosController : Controller
    {
        private readonly appDBcontext _context;
        private readonly IWebHostEnvironment env;  

        public LibrosController(appDBcontext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var appDBcontext = _context.libros.Include(l => l.autor).Include(l => l.genero);
            return View(await appDBcontext.ToListAsync());
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.libros == null)
            {
                return NotFound();
            }

            var libro = await _context.libros
                .Include(l => l.autor)
                .Include(l => l.genero)
                .FirstOrDefaultAsync(m => m.id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {

            ViewData["autorId"] = new SelectList(_context.autores, "id", "id");
            ViewData["generoId"] = new SelectList(_context.generos, "id", "id");
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,titulo,descripcion,fecha_publicacion,portada,autorId,generoId")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];

                    if (archivoFoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(env.WebRootPath, "images/portada");
                        //Generar nombre aleatorio de la imagen
                        var archivoDestino = Guid.NewGuid().ToString();//Obtiene el nombre del archivo
                        archivoDestino = archivoDestino.Replace("-", "");//Reemplaza los guion medio
                        archivoDestino += Path.GetExtension(archivoFoto.FileName);//obtiene la extension del archivo
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            libro.portada = archivoDestino;
                        }

                    }

                }

                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["autorId"] = new SelectList(_context.autores, "id", "id", libro.autorId);
            ViewData["generoId"] = new SelectList(_context.generos, "id", "id", libro.generoId);
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.libros == null)
            {
                return NotFound();
            }

            var libro = await _context.libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            ViewData["autorId"] = new SelectList(_context.autores, "id", "id", libro.autorId);
            ViewData["generoId"] = new SelectList(_context.generos, "id", "id", libro.generoId);
            return View(libro);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,titulo,descripcion,fecha_publicacion,portada,autorId,generoId")] Libro libro)
        {
            if (id != libro.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.id))
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
            ViewData["autorId"] = new SelectList(_context.autores, "id", "id", libro.autorId);
            ViewData["generoId"] = new SelectList(_context.generos, "id", "id", libro.generoId);
            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.libros == null)
            {
                return NotFound();
            }

            var libro = await _context.libros
                .Include(l => l.autor)
                .Include(l => l.genero)
                .FirstOrDefaultAsync(m => m.id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.libros == null)
            {
                return Problem("Entity set 'appDBcontext.libros'  is null.");
            }
            var libro = await _context.libros.FindAsync(id);
            if (libro != null)
            {
                _context.libros.Remove(libro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
          return _context.libros.Any(e => e.id == id);
        }
    }
}
