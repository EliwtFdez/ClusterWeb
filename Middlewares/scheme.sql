-- ==================================================
-- 1. Creación de la base de datos (opcional)
-- ==================================================
CREATE DATABASE IF NOT EXISTS residencia_pagos;
USE residencia_pagos;

-- ==================================================
-- 2. Tabla: casas
-- ==================================================
CREATE TABLE casas (
    id_casa INT AUTO_INCREMENT PRIMARY KEY,
    numero_casa VARCHAR(10) NOT NULL,         -- Número o identificador de la casa
    direccion VARCHAR(100) DEFAULT NULL       -- Si quieres almacenar algún dato extra de ubicación
);

-- ==================================================
-- 3. Tabla: residentes
--     (Si cada casa puede tener múltiples residentes o
--      si necesitas datos de contacto de quién paga)
-- ==================================================
CREATE TABLE residentes (
    id_residente INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    telefono VARCHAR(20) DEFAULT NULL,
    email VARCHAR(50) DEFAULT NULL,
    -- Si cada residente está asociado a 1 casa (relación 1 a muchos),
    -- agregamos la FK a casa.
    id_casa INT NOT NULL,
    FOREIGN KEY (id_casa) REFERENCES casas(id_casa)
);

-- ==================================================
-- 4. Tabla: metodos_pago
-- ==================================================
CREATE TABLE metodos_pago (
    id_metodo INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(50) NOT NULL  -- Efectivo, Tarjeta, Transferencia, etc.
);

-- ==================================================
-- 5. Tabla: cuotas
--    Representa los diferentes conceptos a cobrar:
--    Ejemplos: "Mantenimiento mensual", "Agua", "Cuota especial", etc.
-- ==================================================
CREATE TABLE cuotas (
    id_cuota INT AUTO_INCREMENT PRIMARY KEY,
    nombre_cuota VARCHAR(50) NOT NULL,      -- Nombre/Descripción de la cuota
    monto DECIMAL(10,2) NOT NULL,           -- Monto a cobrar
    fecha_vencimiento DATE DEFAULT NULL,    -- Para control de plazos (opcional)
    descripcion VARCHAR(100) DEFAULT NULL   -- Campo extra para más detalles
);

-- ==================================================
-- 6. Tabla: pagos
--    Registra cada pago que se realiza
-- ==================================================
CREATE TABLE pagos (
    id_pago INT AUTO_INCREMENT PRIMARY KEY,
    id_casa INT NOT NULL,                   -- A qué casa se asocia el pago
    id_residente INT DEFAULT NULL,          -- Quién realiza el pago (si se requiere)
    id_cuota INT NOT NULL,                  -- Por cuál cuota se está pagando
    id_metodo INT NOT NULL,                 -- Método de pago usado
    fecha_pago DATE NOT NULL,               -- Fecha en que se realizó el pago
    monto_pagado DECIMAL(10,2) NOT NULL,    -- Cuánto se pagó
    -- Podrías añadir un campo para saber si queda saldo pendiente
    -- saldo_pendiente DECIMAL(10,2) DEFAULT 0.00,
    observaciones VARCHAR(100) DEFAULT NULL,
    
    FOREIGN KEY (id_casa) REFERENCES casas (id_casa),
    FOREIGN KEY (id_residente) REFERENCES residentes (id_residente),
    FOREIGN KEY (id_cuota) REFERENCES cuotas (id_cuota),
    FOREIGN KEY (id_metodo) REFERENCES metodos_pago (id_metodo)
);

-- ==================================================
-- 7. Insertar datos iniciales (Opcional)
-- ==================================================

-- Ejemplo: Insertar las 60 casas
INSERT INTO casas (numero_casa, direccion)
VALUES
('1',  'Casa 1'),
('2',  'Casa 2'),
('3',  'Casa 3'),
-- ...
('60', 'Casa 60');

-- Ejemplo: Métodos de pago
INSERT INTO metodos_pago (descripcion)
VALUES
('Efectivo'),
('Tarjeta de Crédito'),
('Tarjeta de Débito'),
('Transferencia Bancaria');

-- Ejemplo: Cuotas (podrías tener una cuota mensual genérica)
INSERT INTO cuotas (nombre_cuota, monto, fecha_vencimiento, descripcion)
VALUES
('Cuota Mantenimiento Mensual', 100.00, '2025-03-05', 'Cobro Mensual de mantenimiento'),
('Cuota Agua Mensual', 30.00, '2025-03-10', 'Servicio de agua');

-- Ejemplo: Residentes (si cada casa tiene uno por defecto)
INSERT INTO residentes (nombre, telefono, email, id_casa)
VALUES
('Juan Pérez', '555-1111', 'juan@example.com', 1),
('María Gómez', '555-2222', 'maria@example.com', 2),
('Carlos Ruiz', '555-3333', 'carlos@example.com', 3);

-- Ejemplo: Pago
INSERT INTO pagos (id_casa, id_residente, id_cuota, id_metodo, fecha_pago, monto_pagado, observaciones)
VALUES
(1, 1, 1, 1, '2025-03-01', 100.00, 'Pago total en efectivo de mantenimiento');

-- Ajusta, agrega o elimina lo que necesites
