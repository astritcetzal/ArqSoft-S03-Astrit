using CatalogoApp.Application.Service;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Runtime.InteropServices;
using System.Text.Unicode;

namespace Catalogo.Presentation.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ItemService _service;

        // El servicio llega por inyección de dependencias
        public CatalogoController(ItemService service)
        {
            _service = service;
        }

        // Lista con filtro opcional por género

        public IActionResult Index(string? lanzamiento)
        {
            var items = string.IsNullOrEmpty(lanzamiento)
                ? _service.ObtenerTodos()
                : _service.ObtenerPorTiposLanzamiento(lanzamiento);

            ViewBag.Lanzamientos = _service.ObtenerTiposLanzamiento();
            ViewBag.LanzamientoActual = lanzamiento;

            return View(items);
        }

        // Detalle de un item

        public IActionResult Detalle(int id)
        {

            var item = _service.ObtenerPorId(id);
            return item == null ? NotFound() : View(item);
        }

        // Formulario — GET
        public IActionResult Agregar()
        {
            return View();
        }

        // Formulario — POST
        [HttpPost]
        public IActionResult Agregar(Item item, IFormFile? ArchivoPortada, IFormFile? ArchivoCanciones)
        {
            if (ArchivoPortada != null && ArchivoPortada.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    ArchivoPortada.CopyTo(memoryStream);
                    item.ImagenBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    item.TipoImagen = ArchivoPortada.ContentType;
                }       
            }
            if (ArchivoCanciones != null && ArchivoCanciones.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    ArchivoCanciones.CopyTo(memoryStream);
                    item.CancionesListaBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    item.TipoCanciones = ArchivoCanciones.ContentType;
                }
            }
            _service.Agregar(item);
            return RedirectToAction("Index");
        }

        // Eliminar
        public IActionResult Eliminar(int id)
        {
            _service.Eliminar(id);
            return RedirectToAction("Index");
        }
    }

}