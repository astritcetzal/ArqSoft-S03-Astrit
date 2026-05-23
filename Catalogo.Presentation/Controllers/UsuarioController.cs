using CatalogoApp.Application.Service;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Runtime.InteropServices;
using System.Text.Unicode;

namespace CatalogoApp.Presentation.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserService _service;

        // El servicio llega por inyección de dependencias
        public UsuarioController(UserService service)
        {
            _service = service;
        }

        
        
        public IActionResult IniciarSesion()
        {
            return View();
        }

        // Formulario — POST
        [HttpPost]
        public IActionResult IniciarSesion(User user)
        {
            _service.Agregar(user);
            return RedirectToAction("Index");
        }

    }
}
