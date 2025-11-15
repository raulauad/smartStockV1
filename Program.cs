using Microsoft.EntityFrameworkCore;
using SmartStockV1.Data;
using SmartStockV1.Interfaces.Usuarios;
using SmartStockV1.Repositories.Usuarios;
using SmartStockV1.Services.Usuarios;

namespace Smart_Stock_V1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1) DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<SmartStockDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 2) Repositorios
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // 3) Servicios (CU01 - gestión de usuarios desde panel admin)
            builder.Services.AddScoped<IUsuarioService, AdminUsuarioService>();

            // 4) Infra API + Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
