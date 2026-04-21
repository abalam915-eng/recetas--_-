using Microsoft.AspNetCore.Mvc;
using RecetasApp.Models;

namespace RecetasApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Supabase.Client _supabase;

        public HomeController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _supabase.From<Receta>()
                .Where(x => x.Estatus == "Publicada")
                .Get();

            var recetasPublicadas = response.Models;

            return View(recetasPublicadas);
        }
    }
}
