using Business;
using Business.Services;
using Grpc.AspNetCore.Server;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationUserService _applicationUserService;

        public AuthController(ApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _applicationUserService.AuthenticateUserAsync(model.Username, model.Password);
                if (!string.IsNullOrEmpty(token))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim("jwt", token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                        IsPersistent = false
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );

                    return RedirectToAction("ViewListUsers", "Home");
                }
                else
                {
                    TempData["UserMessage"] = "Credenciales inválidas. Intente nuevamente.";
                    TempData["UserMessageType"] = "danger";
                    return RedirectToAction("ViewLogin", "Home");
                }
            }
            catch (RpcException ex)
            {
                string errorMessage = ex.Status.Detail;
                TempData["ErrorMessage"] = errorMessage;
                TempData["UserMessageType"] = "danger";
                return RedirectToAction("ViewLogin", "Home");
            }
        }

        [Authorize]
        [HttpPost("Register")]        
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var registerRequest = new RegisterRequest
                {
                    Username = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber
                };

                var context = HttpContext.Features.Get<IServerCallContextFeature>()?.ServerCallContext;
                var response = await _applicationUserService.RegisterUser(registerRequest, context);

                if (response.Estado)
                {
                    return RedirectToAction("ViewListUsers", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo registrar el usuario. Intente nuevamente.";
                    TempData["UserMessageType"] = "danger";
                    return RedirectToAction("ViewRegister", "Home");
                }
            }
            catch (RpcException ex)
            {
                string errorMessage = ex.Status.Detail;                
                TempData["ErrorMessage"] = errorMessage;
                TempData["UserMessageType"] = "danger";
                return RedirectToAction("ViewRegister", "Home");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("ViewLogin", "Home");
        }
    }
}


