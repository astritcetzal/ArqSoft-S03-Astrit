using CatalogoApp.Domain.Models;
using CatalogoApp.Domain.Interfaces;

namespace CatalogoApp.Application.Service
{
    public class ItemService
    {
        private readonly IItemRepository _repo;

        // El servicio recibe el repositorio por constructor
        // No sabe si es JSON, SQL, memoria, etc.
        public ItemService(IItemRepository repo)
        {
            _repo = repo;
        }

        public List<Item> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Item? ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }

        public void Agregar(Item item)
        {
            // Aquí podrías agregar validaciones de negocio
            // Por ejemplo: if (string.IsNullOrEmpty(item.Titulo)) throw...
            _repo.Agregar(item);
        }

        public void Eliminar(int id)
        {
            _repo.Eliminar(id);
        }

        // Método útil para el filtro por categoría/género
        public List<Item> ObtenerPorTiposLanzamiento(string tiposLanzamiento)
        {
            return _repo.ObtenerTodos()
                        .Where(i => i.tiposLanzamiento == tiposLanzamiento)
                        .ToList();
        }

        public List<string> ObtenerTiposLanzamiento()
        {
            return _repo.ObtenerTodos()
                        .Select(i => i.tiposLanzamiento)
                        .Distinct()
                        .ToList();
        }
    }
}