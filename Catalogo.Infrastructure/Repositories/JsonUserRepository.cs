
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Necesario para usar .FirstOrDefault() y .Max()
using System.Text.Json;
namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonUserRepository : IUserRepository
    {
        // Ruta del archivo JSON, relativa a donde corre la app
        private readonly string _filePath;

        public JsonUserRepository(string filePath)
        {
            _filePath = filePath;

            // Si la carpeta no existe, crearla
            var carpeta2 = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta2))
                Directory.CreateDirectory(carpeta2);
        }

        public List<User> ObtenerTodos()
        {
            if (!File.Exists(_filePath))
                return new List<User>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        // Inicia sesión y guarda la lista completa en el JSON
        public void IniciarSesion(User user)
        {
            var users = ObtenerTodos();

            // Auto-incrementar el Id
            user.Id = users.Count > 0
                      ? users.Max(u => u.Id) + 1
                      : 1;

            users.Add(user);
            Guardar(users);
        }


        public User? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(u => u.Id == id);
        }

        // Método privado: serializa y escribe el archivo
        private void Guardar(List<User> users)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true   // JSON legible para humanos
            };
            var json = JsonSerializer.Serialize(users, opciones);
            File.WriteAllText(_filePath, json);
        }

    }
}
