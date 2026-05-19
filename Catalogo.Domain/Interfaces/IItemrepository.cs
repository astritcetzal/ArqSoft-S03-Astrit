using Catalogo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalogo.Domain.Interfaces
{
    internal interface IItemRepository
    {
        List<Item> ObtenerTodos();
        Item? ObtenerPorId(int id);
        void Agregar(Item item);
        void Eliminar(int id);
    }
}
