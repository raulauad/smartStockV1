using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SmartStockClienteWinForms.Dtos.Usuarios;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SmartStockClienteWinForms.Dtos.Usuarios;

namespace SmartStockClienteWinForms.Servicios
{
    public class ApiAutenticacionCliente
    {
        private readonly HttpClient _http;

      
        private const string ApiBase = "https://localhost:44322";

        public ApiAutenticacionCliente()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(ApiBase)
            };
        }

        public async Task<AdminResponseDto> LoginAdminAsync(LoginAdminRequestDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/Autenticacion/login-admin", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al iniciar sesión: {error}");
            }

            var admin = await response.Content.ReadFromJsonAsync<AdminResponseDto>();

            if (admin == null)
                throw new Exception("La API devolvió una respuesta vacía.");

            return admin;
        }

        // Más adelante agregamos login de usuario y logout
    }
}