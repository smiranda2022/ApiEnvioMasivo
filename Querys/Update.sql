-- Suponiendo que el ID del flujo de bienvenida es 1
UPDATE Destinatarios
SET FlujoId = 1,
    FechaInicioFlujo = GETDATE()
WHERE Email IN (
    'lucia@mail.com',
    'santiago.g.miranda@gmail.com',
    'benja@gmail.com'
);
