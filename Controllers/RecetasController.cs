using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecetasApp.Models;
using System.Security.Claims;

namespace RecetasApp.Controllers
{
    [Authorize]
    public class RecetasController : Controller
    {
        private readonly Supabase.Client _supabase;

        public RecetasController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IActionResult> MisRecetas()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Cuenta");
            var userId = Guid.Parse(userIdStr);

            var responseBorradores = await _supabase.From<Receta>()
                .Where(x => x.UsuarioId == userId && x.Estatus == "Borrador")
                .Get();
            
            var responsePublicadas = await _supabase.From<Receta>()
                .Where(x => x.UsuarioId == userId && x.Estatus == "Publicada")
                .Get();

            ViewBag.Borradores = responseBorradores.Models;
            ViewBag.Publicadas = responsePublicadas.Models;

            return View();
        }

        public IActionResult Crear()
        {
            return View(new Receta());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Receta modelo)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            modelo.UsuarioId = userId;
            modelo.Estatus = "Borrador";
            modelo.FechaCreacion = DateTime.UtcNow;

            await _supabase.From<Receta>().Insert(modelo);

            return RedirectToAction(nameof(MisRecetas));
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            var response = await _supabase.From<Receta>().Where(x => x.Id == id).Get();
            var receta = response.Models.FirstOrDefault();
            if (receta == null) return NotFound();

            if (receta.Estatus == "Publicada")
            {
                return RedirectToAction(nameof(MisRecetas));
            }

            return View(receta);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Receta modelo)
        {
            await _supabase.From<Receta>().Update(modelo);
            return RedirectToAction(nameof(MisRecetas));
        }

        [HttpPost]
        public async Task<IActionResult> Publicar(Guid id)
        {
            var response = await _supabase.From<Receta>().Where(x => x.Id == id).Get();
            var recetaBorrador = response.Models.FirstOrDefault();

            if (recetaBorrador == null || recetaBorrador.Estatus != "Borrador") return BadRequest();

            var recetaPublicada = new Receta
            {
                UsuarioId = recetaBorrador.UsuarioId,
                Titulo = recetaBorrador.Titulo,
                Ingredientes = recetaBorrador.Ingredientes,
                Pasos = recetaBorrador.Pasos,
                Estatus = "Publicada",
                FechaCreacion = recetaBorrador.FechaCreacion,
                FechaPublicacion = DateTime.UtcNow
            };

            await _supabase.From<Receta>().Insert(recetaPublicada);

            return RedirectToAction(nameof(MisRecetas));
        }
    }
}
