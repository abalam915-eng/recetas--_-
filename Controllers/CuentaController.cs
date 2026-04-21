using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Supabase.Gotrue;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RecetasApp.Models;

namespace RecetasApp.Controllers
{
    public class SessionDto
    {
        public string? access_token { get; set; }
        public string? refresh_token { get; set; }
    }

    public class CuentaController : Controller
    {
        private readonly Supabase.Client _supabase;

        public CuentaController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginGithub()
        {
            var redirectUrl = $"{Request.Scheme}://{Request.Host}/Cuenta/Callback";
            var authState = await _supabase.Auth.SignIn(Supabase.Gotrue.Constants.Provider.Github, new SignInOptions { RedirectTo = redirectUrl });
            if (authState?.Uri != null)
            {
                return Redirect(authState.Uri.ToString());
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Callback()
        {
            // Mostramos una vista que leerá el "#access_token=" usando JS
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetSession([FromBody] SessionDto dto)
        {
            if (string.IsNullOrEmpty(dto.access_token) || string.IsNullOrEmpty(dto.refresh_token)) 
                return BadRequest();

            // Setea manualmente la sesión en el cliente SDK con los tokens obtenidos
            var session = await _supabase.Auth.SetSession(dto.access_token, dto.refresh_token);
            
            if (session?.User != null)
            {
                // Extracción segura de Metadata
                string nombreMetadato = "Usuario Github";
                if (session.User.UserMetadata != null && session.User.UserMetadata.ContainsKey("full_name"))
                {
                    nombreMetadato = session.User.UserMetadata["full_name"]?.ToString() ?? "Usuario Github";
                }
                
                string? githubIdMetadato = null;
                if (session.User.UserMetadata != null && session.User.UserMetadata.ContainsKey("user_name"))
                {
                    githubIdMetadato = session.User.UserMetadata["user_name"]?.ToString();
                }

                // Sincronizar el usuario con nuestra tabla pública
                var userIdStr = session.User.Id;
                if (!string.IsNullOrEmpty(userIdStr))
                {
                    var userId = Guid.Parse(userIdStr);
                    var checkUser = await _supabase.From<Usuario>().Where(x => x.Id == userId).Get();
                    if (!checkUser.Models.Any())
                    {
                        var nuevoUsuario = new Usuario 
                        {
                            Id = userId,
                            Email = session.User.Email,
                            Nombre = nombreMetadato,
                            GitHubId = githubIdMetadato
                        };
                        await _supabase.From<Usuario>().Insert(nuevoUsuario);
                    }
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, session.User.Id?.ToString() ?? ""),
                    new Claim(ClaimTypes.Email, session.User.Email?.ToString() ?? ""),
                    new Claim(ClaimTypes.Name, nombreMetadato)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity));

                return Ok();
            }
            return BadRequest();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _supabase.Auth.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
