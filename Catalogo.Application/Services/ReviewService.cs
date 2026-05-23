using CatalogoApp.Domain.Models;
using CatalogoApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogoApp.Application.Service
{
    public class ReviewService
    {
        private readonly IReviewRepository _repo;

        // El servicio recibe el repositorio por constructor
        // No sabe si es JSON, SQL, memoria, etc.
        public  ReviewService(IReviewRepository repo)
        {
            _repo = repo;
        }
        public List<Review> ObtenerPorAlbum(int albumId)
        {
            return _repo.ObtenerPorAlbum(albumId);
        }

        public Review? ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }

        public void Agregar(Review review)
        {
            // Aquí podrías agregar validaciones de negocio
            // Por ejemplo: if (string.IsNullOrEmpty(review.Comentario)) throw...
            _repo.Agregar(review);
        }

    }
}
