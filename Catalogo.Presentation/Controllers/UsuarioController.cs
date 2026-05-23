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

        public IActionResult Registrar()
        {
            return View();
        }
        // 2. Guardar el usuario (POST)
        [HttpPost]
        public IActionResult Registrar(User user)
        {
            _service.IniciarSesion(user);
            // Tras registrarse, lo mandamos al catálogo
            return RedirectToAction("Index", "Catalogo");
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        // Formulario — POST
        [HttpPost]
        public IActionResult IniciarSesion(string correo, string contrasena)
        {
            var usuarios = _service.ObtenerTodos();

            // Busca si existe alguien con ese correo y esa contraseña
            var usuarioValido = usuarios.FirstOrDefault(u => u.Correo == correo && u.Contrasena == contrasena);

            if (usuarioValido != null)
                return RedirectToAction("Index", "Catalogo");

            // Si no es válido, mostrar mensaje de error
            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos");
            return View();

        }

    }
}
