using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonReviewRepository : IReviewRepository
    {
        private readonly string _filePath;

        public JsonReviewRepository(string filePath)
        {
            _filePath = filePath;

            var carpeta2 = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta2))
                Directory.CreateDirectory(carpeta2);
        }

        public List<Review> ObtenerTodos()
        {
            if (!File.Exists(_filePath))
                return new List<Review>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Review>>(json) ?? new List<Review>();
        }

        // Filtra solo las reseñas que pertenecen al álbum que pedimos
        public List<Review> ObtenerPorAlbum(int albumId)
        {
            return ObtenerTodos().Where(r => r.AlbumId == albumId).ToList();
        }

        public Review? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(r => r.Id == id);
        }

        public void Agregar(Review review)
        {
            var reviews = ObtenerTodos();

            review.Id = reviews.Count > 0
                      ? reviews.Max(r => r.Id) + 1
                      : 1;

            reviews.Add(review);
            Guardar(reviews);
        }

        private void Guardar(List<Review> reviews)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(reviews, opciones);
            File.WriteAllText(_filePath, json);
        }
    }
}