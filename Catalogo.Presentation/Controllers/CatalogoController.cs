using CatalogoApp.Application.Service;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authorization;
namespace CatalogoApp.Presentation.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ItemService _service;
        private readonly ReviewService _reviewService;
        // El servicio llega por inyección de dependencias
        public CatalogoController(ItemService service, ReviewService reviewService)
        {
            _service = service;
            _reviewService = reviewService;
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
            var resenas = _reviewService.ObtenerPorAlbum(id);

            // Lógica de promedio:
            double promedio = resenas.Any() ? resenas.Average(r => r.Calificacion) : 0;

            ViewBag.Reseñas = resenas;
            ViewBag.Promedio = Math.Round(promedio, 1); // Redondeamos a 1 decimal
            ViewBag.Cantidad = resenas.Count;

            return View(item);
        }

        // Formulario — GET
        // --- ESTOS SON PRIVADOS (Con cadenero) ---
        [Authorize]
        public IActionResult Agregar()
        {
            return View();
        }

        [Authorize]
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
        [Authorize]
        public IActionResult Eliminar(int id)
        {
            _service.Eliminar(id);
            return RedirectToAction("Index");
        }

        // --- MÉTODO PARA GUARDAR LA RESEÑA ---
        [HttpPost]
        [Authorize] // Solo usuarios registrados pueden publicar
        public IActionResult AgregarReseña(Review review)
        {
            // Validamos que el comentario no esté vacío
            if (string.IsNullOrEmpty(review.Comentario))
            {
                // Si está vacío, regresamos al detalle del álbum con un mensaje
                TempData["Error"] = "El comentario no puede estar vacío.";
                return RedirectToAction("Detalle", new { id = review.AlbumId });
            }

            // Guardamos la reseña usando el servicio
            _reviewService.Agregar(review);

            // Regresamos a la misma página de detalles para que se vea la nueva reseña
            return RedirectToAction("Detalle", new { id = review.AlbumId });
        }
    }

}