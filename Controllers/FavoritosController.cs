using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecetasApp.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecetasApp.Controllers
{
    [Authorize]
    public class FavoritosController : Controller
    {
        private readonly Supabase.Client _supabase;

        public FavoritosController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        [HttpPost]
        public async Task<IActionResult> MarcarFavorito(Guid recetaId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var existe = await _supabase.From<Favorito>()
                .Where(x => x.UsuarioId == userId && x.RecetaId == recetaId)
                .Get();

            if (!existe.Models.Any())
            {
                var favorito = new Favorito
                {
                    UsuarioId = userId,
                    RecetaId = recetaId
                };
                await _supabase.From<Favorito>().Insert(favorito);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> QuitarFavorito(Guid id) 
        {
            await _supabase.From<Favorito>().Where(x => x.Id == id).Delete();
            return RedirectToAction("MisFavoritos");
        }

        public async Task<IActionResult> MisFavoritos()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var favoritos = await _supabase.From<Favorito>()
                .Where(x => x.UsuarioId == userId)
                .Get();

            return View(favoritos.Models);
        }
    }
}
