using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecetasApp.Models;
using System.Security.Claims;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace RecetasApp.Controllers
{
    [Authorize]
    public class RecetasController : Controller
    {
        private readonly Supabase.Client _supabase;
        private readonly IWebHostEnvironment _env;

        public RecetasController(Supabase.Client supabase, IWebHostEnvironment env)
        {
            _supabase = supabase;
            _env = env;
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
        public async Task<IActionResult> Crear(Receta modelo, IFormFile? imagenArchivo)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            modelo.UsuarioId = userId;
            modelo.Estatus = "Borrador";
            modelo.FechaCreacion = DateTime.UtcNow;

            if (imagenArchivo != null && imagenArchivo.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagenArchivo.FileName);
                string fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await imagenArchivo.CopyToAsync(stream);
                }
                modelo.Imagen = "/uploads/" + fileName;
            }

            if(string.IsNullOrEmpty(modelo.AutorOriginal))
            {
                // Si nosotros la creamos de 0, somos el autor original
                var authUser = await _supabase.From<Usuario>().Where(x => x.Id == userId).Get();
                modelo.AutorOriginal = authUser.Models.FirstOrDefault()?.Nombre ?? "Yo";
            }

            await _supabase.From<Receta>().Insert(modelo);

            return RedirectToAction(nameof(MisRecetas));
        }

        [HttpPost]
        public async Task<IActionResult> Copiar(Guid recetaId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _supabase.From<Receta>().Where(x => x.Id == recetaId).Get();
            var recetaOriginal = result.Models.FirstOrDefault();

            if (recetaOriginal != null)
            {
                var recetaCopiada = new Receta
                {
                    UsuarioId = userId,
                    Titulo = recetaOriginal.Titulo + " (Copia)",
                    Ingredientes = recetaOriginal.Ingredientes,
                    Pasos = recetaOriginal.Pasos,
                    Imagen = recetaOriginal.Imagen,
                    Estatus = "Borrador",
                    FechaCreacion = DateTime.UtcNow,
                    AutorOriginal = recetaOriginal.AutorOriginal ?? "Anónimo"
                };
                await _supabase.From<Receta>().Insert(recetaCopiada);
            }
            return RedirectToAction(nameof(MisRecetas));
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _supabase.From<Receta>().Where(x => x.Id == id).Get();
            var receta = result.Models.FirstOrDefault();

            if (receta != null && receta.UsuarioId == userId)
            {
                await _supabase.From<Receta>().Where(x => x.Id == id).Delete();
            }
            
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
        public async Task<IActionResult> Editar(Receta modelo, IFormFile? imagenArchivo)
        {
            if (imagenArchivo != null && imagenArchivo.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagenArchivo.FileName);
                string fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await imagenArchivo.CopyToAsync(stream);
                }
                modelo.Imagen = "/uploads/" + fileName;
            }

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
                Imagen = recetaBorrador.Imagen,
                AutorOriginal = recetaBorrador.AutorOriginal,
                Estatus = "Publicada",
                FechaCreacion = recetaBorrador.FechaCreacion,
                FechaPublicacion = DateTime.UtcNow
            };

            await _supabase.From<Receta>().Insert(recetaPublicada);

            return RedirectToAction(nameof(MisRecetas));
        }
    }
}
