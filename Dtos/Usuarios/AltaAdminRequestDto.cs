namespace SmartStockV1.Dtos.Usuarios
{
    public class AltaAdminRequestDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdRol { get; set; }               // el admin elige rol
        public string ContraseñaPlano { get; set; } = string.Empty;
    }
}
