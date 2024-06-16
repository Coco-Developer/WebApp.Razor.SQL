using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppLazaro.Models;


namespace AppLazaro.Controllers
{
    public class ItemsController : Controller
    {
        private readonly CrudDbContext _context;

        public ItemsController(CrudDbContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10; // Número de ítems por página
            int pageNumber = page ?? 1;

            var items = await _context.Items
                .OrderByDescending(i => i.ID) // Ordenar por ID descendente
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking() // Optimización: no rastrear entidades para lectura
                .ToListAsync();

            return View(items);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .AsNoTracking() // Optimización: no rastrear entidades para lectura
                .FirstOrDefaultAsync(m => m.ID == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Descripcion")] Items item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(item);
                    await _context.SaveChangesAsync();

                    // SweetAlert para mostrar mensaje al usuario
                    TempData["SweetAlertMessage"] = "¡El ítem se ha creado exitosamente!";
                    TempData["SweetAlertType"] = "success";

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                // Loguear la excepción o manejarla según sea necesario
                ModelState.AddModelError("", "No se pudo guardar el nuevo item. Por favor, inténtelo de nuevo.");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Descripcion")] Items item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();

                    // SweetAlert para mostrar mensaje al usuario
                    TempData["SweetAlertMessage"] = "¡Los cambios se han guardado exitosamente!";
                    TempData["SweetAlertType"] = "success";

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                // Loguear la excepción o manejarla según sea necesario
                ModelState.AddModelError("", "Error al intentar guardar los cambios. Por favor, inténtelo de nuevo.");
            }

            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .AsNoTracking() // Optimización: no rastrear entidades para lectura
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Hubo un error al intentar eliminar el item. Inténtalo nuevamente.";
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            try
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();

                // SweetAlert para mostrar mensaje al usuario
                TempData["SweetAlertMessage"] = "¡El ítem se ha eliminado exitosamente!";
                TempData["SweetAlertType"] = "success";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Loguear la excepción o manejarla según sea necesario
                Console.WriteLine($"Error al eliminar el item: {ex.Message}");

                // Redireccionar a la vista de Delete con error
                return RedirectToAction(nameof(Delete), new { id, saveChangesError = true });
            }
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ID == id);
        }
    }
}
