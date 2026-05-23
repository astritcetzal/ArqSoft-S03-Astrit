using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogoApp.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int AlbumId { get; set; } // Para saber a qué álbum pertenece
        public string Usuario { get; set; }
        public string Comentario { get; set; }
        public int Calificacion { get; set; } // Ejemplo: 1 al 5

    }
}