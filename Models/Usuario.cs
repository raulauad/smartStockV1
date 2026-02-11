using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SmartStockV1.Models
{
    public enum EstadoUsuario
    {
        Inactivo = 0,
        ActivoDesconectado = 1,
        ActivoConectado = 2
    }
    public class Usuario
    {

        public int UsuarioId {  get; set; }
        public int RolId {  get; set; }
        public string UsuarioNombre { get; set; } = null!;
        public string? UsuarioDireccion { get; set; }
        public string? UsuarioTelefono { get; set; }
        public DateTime AltaUsuario { get; set; } = DateTime.UtcNow; //Momento en el que el admin da de alta un usuario
        public DateTime? UltConexion { get; set; } //Momento en el que el usuario desconecta de la aplicacion
        public DateTime? HoraConexion { get; set; } //Momento en el que el usuario conecta a la aplicacion
        public byte[] ContraseñaHash { get; set; } = Array.Empty<byte>();
        public byte[] ContraseñaSalt { get; set; } = Array.Empty<byte>();

        public EstadoUsuario EstadoUsuario { get; set; } = EstadoUsuario.ActivoDesconectado; // Estado usuario activo/desconectado, activo/conectado, inactivo


        public Rol Rol { get; set; } = null!;
        public ICollection<Categoria> CategoriasCreadas { get; set; } = new List<Categoria>(); //El admin puede crear muchas categorias
        public ICollection<Proveedor> ProveedoresCreados { get; set; } = new List<Proveedor>(); //El admin puede crear muchos proveedores
        public ICollection<Producto> ProductosCreados { get; set; } = new List<Producto>(); //Un usuario puede crear varios productos
        public ICollection<DetalleCompra> DetallesCompra { get; set; } = new List<DetalleCompra>(); //Un usuario puede generar muchos detallescompra
        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>(); //Un usuario puede generar muchos detallesventa

    }
}
