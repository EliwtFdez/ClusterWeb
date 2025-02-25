-- Trigger para actualizar el saldo pendiente y estado de la deuda después de un pago
CREATE TRIGGER tr_ActualizarDeudaDespuesDePago
ON pagos
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Actualizar el saldo pendiente
    UPDATE d
    SET saldo_pendiente = d.monto - (
        SELECT COALESCE(SUM(monto_pagado), 0)
        FROM pagos
        WHERE deuda_id = d.deuda_id
    ),
    estado = CASE 
        WHEN d.monto - (
            SELECT COALESCE(SUM(monto_pagado), 0)
            FROM pagos
            WHERE deuda_id = d.deuda_id
        ) = 0 THEN 'pagado'
        WHEN d.monto - (
            SELECT COALESCE(SUM(monto_pagado), 0)
            FROM pagos
            WHERE deuda_id = d.deuda_id
        ) < d.monto THEN 'parcial'
        ELSE 'pendiente'
    END
    FROM deudas d
    INNER JOIN inserted i ON d.deuda_id = i.deuda_id;
END;

-- Trigger para actualizar el estado de deudas vencidas
CREATE TRIGGER tr_ActualizarDeudasVencidas
ON deudas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE deudas
    SET estado = 'vencida'
    WHERE fecha_vencimiento < GETDATE()
    AND estado = 'pendiente'
    AND saldo_pendiente > 0;
END;

-- Trigger para validar que el monto del pago no exceda el saldo pendiente
CREATE TRIGGER tr_ValidarMontoPago
ON pagos
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN deudas d ON i.deuda_id = d.deuda_id
        WHERE i.monto_pagado > d.saldo_pendiente
    )
    BEGIN
        RAISERROR ('El monto del pago no puede ser mayor al saldo pendiente de la deuda.', 16, 1);
        RETURN;
    END

    -- Si la validación pasa, insertar el pago
    INSERT INTO pagos (deuda_id, residente_id, monto_pagado, fecha_pago, metodo_pago)
    SELECT deuda_id, residente_id, monto_pagado, fecha_pago, metodo_pago
    FROM inserted;
END;

-- Trigger para validar que no se eliminen pagos ya procesados
CREATE TRIGGER tr_PrevenirEliminacionPagos
ON pagos
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (
        SELECT 1
        FROM deleted d
        WHERE DATEDIFF(day, d.fecha_pago, GETDATE()) > 3
    )
    BEGIN
        RAISERROR ('No se pueden eliminar pagos con más de 3 días de antigüedad.', 16, 1);
        RETURN;
    END

    -- Si la validación pasa, eliminar el pago
    DELETE p
    FROM pagos p
    INNER JOIN deleted d ON p.pago_id = d.pago_id
    WHERE DATEDIFF(day, p.fecha_pago, GETDATE()) <= 3;
END;

-- Trigger para registrar historial de cambios en deudas
CREATE TABLE historial_deudas (
    historial_id INT IDENTITY(1,1) PRIMARY KEY,
    deuda_id INT,
    accion VARCHAR(20),
    estado_anterior VARCHAR(20),
    estado_nuevo VARCHAR(20),
    saldo_anterior DECIMAL(10,2),
    saldo_nuevo DECIMAL(10,2),
    fecha_cambio DATETIME DEFAULT GETDATE(),
    usuario VARCHAR(100)
);

CREATE TRIGGER tr_RegistrarCambiosDeuda
ON deudas
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO historial_deudas (
        deuda_id,
        accion,
        estado_anterior,
        estado_nuevo,
        saldo_anterior,
        saldo_nuevo,
        usuario
    )
    SELECT 
        i.deuda_id,
        'UPDATE',
        d.estado,
        i.estado,
        d.saldo_pendiente,
        i.saldo_pendiente,
        SYSTEM_USER
    FROM inserted i
    INNER JOIN deleted d ON i.deuda_id = d.deuda_id
    WHERE d.estado != i.estado
    OR d.saldo_pendiente != i.saldo_pendiente;
END;

-- Trigger para actualizar el estado de la deuda
CREATE TRIGGER trg_UpdateDeudaEstado
ON pagos
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE d
    SET estado = CASE 
        WHEN (SELECT ISNULL(SUM(monto_pagado), 0) FROM pagos WHERE deuda_id = d.deuda_id) >= d.monto
             THEN 'pagado'
        ELSE 'pendiente'
    END
    FROM deudas d
    WHERE d.deuda_id IN (
        SELECT DISTINCT deuda_id FROM inserted
        UNION
        SELECT DISTINCT deuda_id FROM deleted
    );
END; 