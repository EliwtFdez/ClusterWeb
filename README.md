# ClusterWeb API

ClusterWeb es una API diseñada para gestionar la información de casas, residentes, deudas y pagos en un entorno residencial. Esta API está construida con .NET 9.0 y utiliza Entity Framework Core para la gestión de la base de datos.

## Requisitos Previos

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)

## Instalación

1. Clona el repositorio:

   ```bash
   git clone https://github.com/tu-usuario/ClusterWeb.git
   cd ClusterWeb
   ```

2. Restaura los paquetes de NuGet:

   ```bash
   dotnet restore
   ```

3. Configura la cadena de conexión a la base de datos en `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=tu-servidor;Database=ClusterWebDb;User Id=tu-usuario;Password=tu-contraseña;"
     }
   }
   ```

4. Aplica las migraciones para crear la base de datos:

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. Ejecuta la aplicación:

   ```bash
   dotnet run
   ```

La API estará disponible en `http://localhost:5000` o `https://localhost:5001`.

## Estructura del Proyecto

- **Casas**: Gestiona la información de las casas.
- **Residentes**: Gestiona la información de los residentes.
- **Deudas**: Gestiona las deudas asociadas a los residentes.
- **Pagos**: Gestiona los pagos realizados por los residentes.

## Ejemplo de Uso

### Obtener todas las casas

**GET** `/api/casas`

```json
[
  {
    "CasaId": 1,
    "Direccion": "Av. Principal 123",
    "NumeroCasa": "101",
    "Habitaciones": 3,
    "Banos": 2,
    "FechaRegistro": "2025-02-20T12:00:00"
  },
  {
    "CasaId": 2,
    "Direccion": "Calle Secundaria 456",
    "NumeroCasa": "202",
    "Habitaciones": 2,
    "Banos": 1,
    "FechaRegistro": "2025-02-20T12:05:00"
  }
]
```

### Crear un nuevo residente

**POST** `/api/residentes`

```json
{
  "Nombre": "Juan Pérez",
  "Telefono": "555-0101",
  "Email": "juan.perez@example.com",
  "FechaIngreso": "2025-02-01T00:00:00",
  "CasaId": 1
}
```

### Registrar un pago

**POST** `/api/pagos`

```json
{
  "DeudaId": 1,
  "MontoPagado": 200.00,
  "FechaPago": "2025-02-21T12:00:00",
  "MetodoPago": "Efectivo"
}
```

## Dependencias

- **BCrypt.Net-Next**: Para el hashing de contraseñas.
- **Microsoft.EntityFrameworkCore**: Para la gestión de la base de datos.
- **Swashbuckle.AspNetCore**: Para la documentación de la API con Swagger.
- **System.IdentityModel.Tokens.Jwt**: Para la autenticación basada en JWT.

## Migraciones

Para crear una nueva migración:

```bash
dotnet ef migrations add NombreDeLaMigracion
```

Para aplicar las migraciones a la base de datos:

```bash
dotnet ef database update
```

## Contribuciones

Si deseas contribuir a este proyecto, por favor sigue los siguientes pasos:

1. Haz un fork del repositorio.
2. Crea una nueva rama (`git checkout -b feature/nueva-funcionalidad`).
3. Realiza tus cambios y haz commit (`git commit -am 'Añade nueva funcionalidad'`).
4. Haz push a la rama (`git push origin feature/nueva-funcionalidad`).
5. Abre un Pull Request.

## Licencia

Este proyecto está bajo la licencia MIT. Consulta el archivo [LICENSE](LICENSE) para más detalles.

## Contacto

Si tienes alguna pregunta o sugerencia, no dudes en contactarme en [tu-email@example.com](mailto:tu-email@example.com).

---

¡Gracias por usar ClusterWeb! Esperamos que esta API sea de gran utilidad para tu proyecto.