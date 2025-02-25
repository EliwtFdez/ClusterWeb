using System.Text.Json.Serialization;
using System.Reflection;
using ClusterWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace ClusterWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de la base de datos
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // Configurar CORS para permitir cualquier origen
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirTodos", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Agregar controladores con conversión de enums a cadena
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // Configurar Swagger con comentarios XML
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            var app = builder.Build();

            // Aplicar migraciones automáticamente con control de errores
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en la migración de la base de datos: {ex.Message}");
                }
            }

            // Manejo de errores global en producción
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            // Configurar Swagger solo en desarrollo
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("PermitirTodos"); // Aplicar CORS
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
