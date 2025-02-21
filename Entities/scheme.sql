
-- Crear tabla de casas
CREATE TABLE casas (
    casa_id INT IDENTITY(1,1) PRIMARY KEY,
    direccion VARCHAR(100) NOT NULL,
    numero_casa VARCHAR(10) UNIQUE NOT NULL,
    habitaciones INT CHECK (habitaciones >= 0),
    banos INT CHECK (banos >= 0),
    fecha_registro DATETIME DEFAULT GETDATE(),
    INDEX idx_direccion (direccion)
);

-- Crear tabla de residentes
CREATE TABLE residentes (
    residente_id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(20) UNIQUE,
    email VARCHAR(100) UNIQUE,
    casa_id INT NOT NULL,
    fecha_ingreso DATE NOT NULL,
    fecha_registro DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (casa_id) REFERENCES casas(casa_id) ON DELETE CASCADE,
    INDEX idx_nombre (nombre)
);

-- Crear tabla de deudas
CREATE TABLE deudas (
    deuda_id INT IDENTITY(1,1) PRIMARY KEY,
    residente_id INT NOT NULL,
    casa_id INT NOT NULL,
    monto DECIMAL(10,2) NOT NULL CHECK (monto >= 0),
    saldo_pendiente DECIMAL(10,2) NOT NULL CHECK (saldo_pendiente >= 0),
    fecha_vencimiento DATE NOT NULL,
    estado NVARCHAR(10) CHECK (estado IN ('pendiente', 'pagado', 'parcial')) DEFAULT 'pendiente',
    descripcion NVARCHAR(MAX),
    fecha_registro DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (residente_id) REFERENCES residentes(residente_id) ON DELETE CASCADE,
    FOREIGN KEY (casa_id) REFERENCES casas(casa_id) ON DELETE NO ACTION
);

-- Crear tabla de pagos
CREATE TABLE pagos (
    pago_id INT IDENTITY(1,1) PRIMARY KEY,
    deuda_id INT NOT NULL,
    residente_id INT NOT NULL,
    monto_pagado DECIMAL(10,2) NOT NULL CHECK (monto_pagado > 0),
    fecha_pago DATETIME DEFAULT GETDATE(),
    metodo_pago VARCHAR(50) CHECK (metodo_pago IN ('efectivo', 'transferencia', 'tarjeta')),
    FOREIGN KEY (deuda_id) REFERENCES deudas(deuda_id) ON DELETE CASCADE,
    FOREIGN KEY (residente_id) REFERENCES residentes(residente_id) ON DELETE CASCADE
);


-- Insertar casas
INSERT INTO casas (direccion, NumeroCasa, habitaciones, banos) VALUES
('Calle Ficticia 123', 'A101', 3, 2),
('Avenida Imaginaria 456', 'B202', 4, 3),
('Paseo de la Fantasía 789', 'C303', 2, 1),
('Calle de la Ilusión 321', 'D404', 5, 4),
('Boulevard Inexistente 654', 'E505', 3, 2),
('Carretera Soñada 987', 'F606', 4, 3),
('Avenida Principal 111', 'G707', 2, 1),
('Callejón Misterioso 222', 'H808', 5, 3),
('Plaza Mágica 333', 'I909', 3, 2),
('Ruta Desconocida 444', 'J010', 4, 2);

-- Insertar residentes
INSERT INTO residentes (nombre, telefono, email, casaId, fechaIngreso) VALUES
('Ana Pérez', '555-1234', 'ana.perez@example.com', 2, '2024-01-10'),
('Carlos Gómez', '555-5678', 'carlos.gomez@example.com', 4, '2023-12-15'),
('Lucía Sánchez', '555-4321', 'lucia.sanchez@example.com', 5, '2024-02-05'),
('Jorge Ramírez', '555-8765', 'jorge.ramirez@example.com', 7, '2023-11-20'),
('Marta Torres', '555-1111', 'marta.torres@example.com', 8, '2024-03-01'),
('Diego Herrera', '555-2222', 'diego.herrera@example.com', 9, '2024-01-25'),
('Sofía López', '555-3333', 'sofia.lopez@example.com', 10, '2023-10-30'),
('Andrés Castro', '555-4444', 'andres.castro@example.com', 11, '2024-02-15'),
('Laura Ruiz', '555-5555', 'laura.ruiz@example.com', 12, '2024-04-01'),
('Pedro Fernández', '555-6666', 'pedro.fernandez@example.com', 13, '2023-09-10');

-- Insertar deudas
INSERT INTO deudas (residenteid, casaid, monto, saldo_pendiente, fecha_vencimiento, estado, descripcion) VALUES
(3, 2, 250.00, 250.00, '2024-05-10', 'pendiente', 'Cuota de mantenimiento'),
(4, 4, 150.50, 0.00, '2024-04-15', 'pagado', 'Reparación de plomería'),
(6, 5, 300.75, 150.75, '2024-06-01', 'parcial', 'Reparación eléctrica'),
(7, 7, 400.00, 400.00, '2024-07-01', 'pendiente', 'Mantenimiento de jardín'),
(8, 8, 100.00, 0.00, '2024-03-20', 'pagado', 'Reparación de fachada'),
(9, 9, 500.00, 500.00, '2024-08-15', 'pendiente', 'Instalación de rejas'),
(10, 10, 200.25, 50.25, '2024-05-05', 'parcial', 'Pintura exterior'),
(11,11, 350.60, 0.00, '2024-06-10', 'pagado', 'Cambio de cerradura'),
(12, 12, 450.00, 450.00, '2024-04-25', 'pendiente', 'Limpieza de cisterna'),
(13, 13, 275.80, 75.80, '2024-07-20', 'parcial', 'Reparación del techo');

-- Insertar pagos
INSERT INTO pagos (deudaid, residenteid, monto_pagado, metodo_pago) VALUES
(3, 3, 150.00, 'transferencia'),
(7, 7, 150.00, 'efectivo'),
(10, 10, 200.00, 'tarjeta');
