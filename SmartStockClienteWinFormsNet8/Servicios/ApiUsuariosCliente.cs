using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using SmartStockClienteWinFormsNet8.Dtos.Usuarios;




namespace SmartStockClienteWinFormsNet8.Servicios
{
    public class ApiUsuariosCliente
    {
        private readonly HttpClient _http;

        // 👉 TU API REAL
        private const string ApiBase = "https://localhost:44322";

        public ApiUsuariosCliente()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(ApiBase)
            };
        }

        public async Task<UsuarioResponseDto> RegistrarAdminAsync(AltaAdminRequestDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/AdminUsuarios/registrar-admin", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al registrar admin: {error}");
            }

            var resultado = await response.Content.ReadFromJsonAsync<UsuarioResponseDto>();

            if (resultado == null)
                throw new Exception("La API devolvió una respuesta vacía.");

            return resultado;
        }
    }
}