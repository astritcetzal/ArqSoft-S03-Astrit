using CatalogoApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogoApp.Domain.Interfaces
{
    public interface IReviewRepository
    {
        List<Review> ObtenerPorAlbum(int albumId);
        void Agregar(Review review);
        Review? ObtenerPorId(int id);
    }
}
