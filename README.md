# ClusterWeb
Para montar/Migrar a una Bd de produccion:
    dotnet ef migrations add NombreDeLaMigracion
    dotnet ef database update



ejemplo de api funcionando:
{
  "casas": [
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
  ],
  "residentes": [
    {
      "ResidenteId": 1,
      "Nombre": "Juan Pérez",
      "Telefono": "555-0101",
      "Email": "juan.perez@example.com",
      "FechaIngreso": "2025-02-01T00:00:00",
      "FechaRegistro": "2025-02-20T12:10:00",
      "CasaId": 1
    },
    {
      "ResidenteId": 2,
      "Nombre": "María López",
      "Telefono": "555-0102",
      "Email": "maria.lopez@example.com",
      "FechaIngreso": "2025-02-05T00:00:00",
      "FechaRegistro": "2025-02-20T12:15:00",
      "CasaId": 2
    }
  ],
  "deudas": [
    {
      "DeudaId": 1,
      "ResidenteId": 1,
      "CasaId": 1,
      "Monto": 200.00,
      "SaldoPendiente": 200.00,
      "FechaVencimiento": "2025-03-01T00:00:00",
      "Estado": "Pendiente",
      "Descripcion": "Cuota de mantenimiento",
      "FechaRegistro": "2025-02-20T12:20:00"
    },
    {
      "DeudaId": 2,
      "ResidenteId": 2,
      "CasaId": 2,
      "Monto": 150.50,
      "SaldoPendiente": 150.50,
      "FechaVencimiento": "2025-03-05T00:00:00",
      "Estado": "Pendiente",
      "Descripcion": "Gastos de agua y luz",
      "FechaRegistro": "2025-02-20T12:25:00"
    }
  ],
  "pagos": [
    {
      "PagoId": 1,
      "DeudaId": 1,
      "MontoPagado": 200.00,
      "FechaPago": "2025-02-21T12:00:00",
      "MetodoPago": "Efectivo"
    },
    {
      "PagoId": 2,
      "DeudaId": 2,
      "MontoPagado": 150.50,
      "FechaPago": "2025-02-22T12:00:00",
      "MetodoPago": "TarjetaCredito"
    }
  ]
}
