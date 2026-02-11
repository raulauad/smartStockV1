using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartStockV1.Data;
using SmartStockV1.Middlewares;
using SmartStockV1.Interfaces.Usuarios;
using SmartStockV1.Repositories.Usuarios;
using SmartStockV1.Services.Usuarios;
using System.Text;

namespace Smart_Stock_V1
{
    public class Program
    {
        private static byte[] GetJwtKeyBytes(IConfiguration config)
        {
            var key = config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("Falta Jwt:Key. En PROD debe venir por variable de entorno JWT__KEY.");

            // Preferimos Base64 en producción
            try
            {
                var bytes = Convert.FromBase64String(key);
                if (bytes.Length < 32) // 256 bits mínimo
                    throw new InvalidOperationException("Jwt:Key demasiado corta. Usá mínimo 32 bytes (256 bits).");
                return bytes;
            }
            catch (FormatException)
            {
                // Fallback (por si llega como string normal)
                var bytes = Encoding.UTF8.GetBytes(key);
                if (bytes.Length < 32)
                    throw new InvalidOperationException("Jwt:Key demasiado corta. Usá mínimo 32 caracteres.");
                return bytes;
            }
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Falta ConnectionStrings:DefaultConnection (en PROD debe venir por variable de entorno).");

            builder.Services.AddDbContext<SmartStockDbContext>(options =>
                options.UseSqlServer(connectionString));

            // DI
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IUsuarioService, AdminUsuarioService>();
            builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();

            // JWT
            var issuer = builder.Configuration["Jwt:Issuer"] ?? "SmartStockV1";
            var audience = builder.Configuration["Jwt:Audience"] ?? "SmartStockV1";
            var keyBytes = GetJwtKeyBytes(builder.Configuration);

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        ValidateAudience = true,
                        ValidAudience = audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            builder.Services.AddAuthorization();

            // Swagger SOLO en Development
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartStockV1 API", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Ingrese: Bearer {token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } });
            });

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
