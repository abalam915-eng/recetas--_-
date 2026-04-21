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

        public async Task<IActionResult> Index(string? q)
        {
            var query = _supabase.From<Receta>().Where(x => x.Estatus == "Publicada");

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Filter("titulo", Supabase.Postgrest.Constants.Operator.ILike, $"%{q}%");
            }

            var response = await query.Get();

            ViewData["SearchQuery"] = q;
            return View(response.Models);
        }
    }
}
