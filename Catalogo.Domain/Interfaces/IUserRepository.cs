using CatalogoApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogoApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        List<User> ObtenerTodos();
        User? ObtenerPorId(int id);
        void IniciarSesion(User user);
    }
}
