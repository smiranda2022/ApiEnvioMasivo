SELECT * FROM CorreosEnviados
SELECT * FROM Destinatarios
SELECT *
FROM Destinatarios d
JOIN CorreosEnviados c ON d.Id = c.DestinatarioId
WHERE d.FlujoId IS NOT NULL
  AND d.FechaInicioFlujo <= DATEADD(MINUTE, -5, GETDATE())
  AND c.FlujoPasoId= 1
  AND c.Abierto IS NULL --
  SELECT * FROM CorreosEnviados WHERE Abierto = 1

  SELECT * FROM CorreosEnviados
  update CorreosEnviados set abierto = 0 where id=7
  DELETE FROM CorreosEnviados;
SELECT * FROM [dbo].[FlujoHistoriales]
SELECT * FROM Destinatarios

SELECT * FROM [dbo].[FlujoHistoriales]
SELECT * FROM[dbo].[FlujoPasos]
SELECT * FROM[dbo].[Flujos]



INSERT INTO Destinatarios (Email, Nombre, Edad, Genero, Ubicacion, FechaInicioFlujo, FlujoId)
VALUES ('santiago.g.miranda@mail.com', 'Santgiago', 25, 'F', 'Rosario', GETDATE(), 1);

