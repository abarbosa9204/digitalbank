using Business;
using Business.Services;
using Grpc.AspNetCore.Server;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Diagnostics;
using X.PagedList;
using X.PagedList.Web.Common;
using OfficeOpenXml;
using System.IO;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;        

        public HomeController(ILogger<HomeController> logger, UserService userService, IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;
            _configuration = configuration;            
        }

        public IActionResult ViewLogin()
        {
            if (TempData.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewBag.UserMessage = errorMessage;
            }
            if (TempData.TryGetValue("UserMessageType", out var userMessageType))
            {
                ViewBag.UserMessageType = userMessageType;
            }
            return View();
        }

        [Authorize]
        public IActionResult ViewRegister()
        {
            if (TempData.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewBag.UserMessage = errorMessage;
            }
            if (TempData.TryGetValue("UserMessageType", out var userMessageType))
            {
                ViewBag.UserMessageType = userMessageType;
            }

            return View();
        }


        [Authorize]
        public async Task<IActionResult> ViewListUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {                
                var (users, totalRecords) = await _userService.GetUsersPagedAsync(pageNumber, pageSize);
             
                var pagedList = new StaticPagedList<Data.Models.Users>(users, pageNumber, pageSize, totalRecords);

                var pagedListRenderOptions = new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                    DisplayLinkToLastPage = PagedListDisplayMode.Always,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = true,
                    MaximumPageNumbersToDisplay = 5
                };


                ViewBag.PagedListRenderOptions = new PagedListRenderOptions { DisplayLinkToFirstPage = PagedListDisplayMode.Always, DisplayLinkToLastPage = PagedListDisplayMode.Always, DisplayLinkToPreviousPage = PagedListDisplayMode.Always, DisplayLinkToNextPage = PagedListDisplayMode.Always, DisplayLinkToIndividualPages = false };

                return View("ViewListUsers", pagedList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios");
                return View("ViewListUsers", new List<Data.Models.Users>());
            }
        }


        [Authorize]
        public async Task<IActionResult> ViewUserManageAsync(string operation, Guid uuid)
        {
            var viewModel = new Data.Models.Users();

            if (operation == "Edit" || operation=="Delete")
            {
                var user = await _userService.GetUserByUuid(uuid);

                if (user == null)
                {
                    TempData["UserMessage"] = "El usuario no existe.";
                    TempData["UserMessageType"] = "danger";
                    return RedirectToAction(nameof(ViewListUsers));
                }

                viewModel = new Data.Models.Users
                {
                    Uuid = user.Uuid,
                    Nombre = user.Nombre,
                    FechaNacimiento = user.FechaNacimiento,
                    Sexo = user.Sexo
                };
                if(operation == "Edit")
                {
                    ViewBag.Title = "Editar Usuario";
                    ViewBag.FormAction = "UpdateUser";
                    ViewBag.Action = "Edit";
                    ViewBag.ButtonLabel = "Editar";
                }
                else
                {
                    ViewBag.Title = "Eliminar Usuario";
                    ViewBag.FormAction = "DeleteUser";
                    ViewBag.Action = "Delete";
                    ViewBag.ButtonLabel = "Eliminar";
                }
            }
            else if (operation == "Create")
            {
                ViewBag.Title = "Crear Usuario";
                ViewBag.FormAction = "CreateUser";
                ViewBag.Action = "Create";
                ViewBag.ButtonLabel = "Crear";
            }
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser(Data.Models.Users user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!DateTime.TryParse(user.FechaNacimiento?.ToString(), out DateTime fechaNacimiento))
                    {
                        TempData["UserMessage"] = "La fecha proporcionada no es válida.";
                        TempData["UserMessageType"] = "danger";
                        return RedirectToAction("ViewUserManage", new { operation = "Create", uuid = user.Uuid });
                    }

                    var request = new UserRequest
                    {
                        Uuid = user.Uuid.ToString(),
                        Nombre = user.Nombre,
                        FechaNacimiento = fechaNacimiento.ToString("yyyy-MM-dd"),
                        Sexo = user.Sexo,
                        Estado = true
                    };

                    var context = HttpContext.Features.Get<IServerCallContextFeature>()?.ServerCallContext;
                    var response = await _userService.CreateUser(request, context);

                    TempData["UserMessage"] = "Usuario creado exitosamente!";
                    TempData["UserMessageType"] = "success";
                }
                catch (RpcException ex)
                {
                    _logger.LogError(ex, "Error al crear el usuario");

                    TempData["UserMessage"] = $"Error al crear el usuario: {ex.Message}";
                    TempData["UserMessageType"] = "danger";
                }
            }
            else
            {
                TempData["UserMessage"] = "Los datos ingresados no son válidos";
                TempData["UserMessageType"] = "danger";
            }
            return RedirectToAction("ViewUserManage", new { operation = "Create", Uuid = user.Uuid });
        }
        [Authorize]
        public async Task<IActionResult> UpdateUser(Data.Models.Users user, Guid uuid)
        {
            var viewModel = new Data.Models.Users();
            if (ModelState.IsValid)
            {
                try
                {
                    var userExists = await _userService.GetUserByUuid(uuid);

                    if (userExists == null)
                    {
                        TempData["UserMessage"] = "El usuario no existe.";
                        TempData["UserMessageType"] = "danger";
                        return RedirectToAction("ViewUserManage", new { operation = "Edit", uuid = user.Uuid });
                    }

                    if (!DateTime.TryParse(user.FechaNacimiento?.ToString(), out DateTime fechaNacimiento))
                    {
                        TempData["UserMessage"] = "La fecha proporcionada no es válida.";
                        TempData["UserMessageType"] = "danger";
                        return RedirectToAction("ViewUserManage", new { operation = "Edit", uuid = user.Uuid });
                    }

                    var request = new UserRequest
                    {
                        Uuid = uuid.ToString(),
                        Nombre = user.Nombre,
                        FechaNacimiento = fechaNacimiento.ToString("yyyy-MM-dd"),
                        Sexo = user.Sexo,
                        Estado = true
                    };

                    var context = HttpContext.Features.Get<IServerCallContextFeature>()?.ServerCallContext;
                    var response = await _userService.UpdateUser(request, context);

                    TempData["UserMessage"] = "Usuario actualizado exitosamente!";
                    TempData["UserMessageType"] = "success";
                }
                catch (RpcException ex)
                {
                    _logger.LogError(ex, "Error al actualizar el usuario");

                    TempData["UserMessage"] = $"Error al actualizar el usuario: {ex.Message}";
                    TempData["UserMessageType"] = "danger";
                }
            }
            else
            {
                TempData["UserMessage"] = "Los datos ingresados no son válidos";
                TempData["UserMessageType"] = "danger";
            }
            return RedirectToAction("ViewUserManage", new { operation = "Edit", uuid = user.Uuid });
        }
        [Authorize]
        public async Task<IActionResult> DeleteUser(Data.Models.Users user, Guid uuid)
        {
            var viewModel = new Data.Models.Users();   
                try
                {
                    var userExists = await _userService.GetUserByUuid(uuid);

                    if (userExists == null)
                    {
                        TempData["UserMessage"] = "El usuario no existe.";
                        TempData["UserMessageType"] = "danger";
                        return RedirectToAction(nameof(ViewListUsers), new { msg = "Error", uuid = user.Uuid });
                    }

                    var request = new UserRequest
                    {
                        Uuid = uuid.ToString()                     
                    };

                    var context = HttpContext.Features.Get<IServerCallContextFeature>()?.ServerCallContext;
                    var response = await _userService.DeleteUser(request, context);

                    TempData["UserMessage"] = $"Usuario {userExists.Nombre} eliminado exitosamente!";
                    TempData["UserMessageType"] = "success";
                    return RedirectToAction(nameof(ViewListUsers), new { msg = "Success", uuid = user.Uuid, name = userExists.Nombre });
                }
                catch (RpcException ex)
                {
                    _logger.LogError(ex, "Error al eliminar el usuario");

                    TempData["UserMessage"] = $"Error al eliminar el usuario: {ex.Message}";
                    TempData["UserMessageType"] = "danger";
                }
            return RedirectToAction(nameof(ViewListUsers), new { msg = "Error", uuid = user.Uuid });            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult FirebaseConfig()
        {
            var firebaseConfig = new
            {
                apiKey = _configuration["Firebase:apiKey"],
                authDomain = _configuration["Firebase:authDomain"],
                projectId = _configuration["Firebase:projectId"],
                storageBucket = _configuration["Firebase:storageBucket"],
                messagingSenderId = _configuration["Firebase:messagingSenderId"],
                appId = _configuration["Firebase:appId"],
                measurementId = _configuration["Firebase:measurementId"]
            };
            return Json(firebaseConfig);
        }

        [Authorize]
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {                
                var (users, totalRecords) = await _userService.GetUsersPagedAsync(pageNumber: 1, pageSize: int.MaxValue);

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Usuarios");
                    worksheet.Cells.LoadFromCollection(users, true);

                    var excelFile = package.GetAsByteArray();             
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "usuarios.xlsx");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios para exportar a Excel");
                return View("Error");
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
