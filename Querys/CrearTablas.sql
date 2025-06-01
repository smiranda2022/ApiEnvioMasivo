-- Script para crear la tabla Destinatarios

-- Opcional: Especifica la base de datos donde quieres crear la tabla.
-- Si no especificas una base de datos, se crear� en la base de datos en la que est�s conectado.
-- USE [NombreDeTuBaseDeDatos];
-- GO

IF OBJECT_ID('Destinatarios', 'U') IS NOT NULL
BEGIN
    DROP TABLE Destinatarios;
END
GO

CREATE TABLE Destinatarios
(
    Id INT PRIMARY KEY IDENTITY(1,1), -- Clave primaria auto-incrementable
    Email NVARCHAR(255) NOT NULL UNIQUE, -- Cadena de texto para email, no nulo y �nico
    Nombre NVARCHAR(255),               -- Cadena de texto para nombre
    Edad INT,                           -- N�mero entero para edad
    Genero NVARCHAR(50),                -- Cadena de texto para g�nero
    Ubicacion NVARCHAR(255)             -- Cadena de texto para ubicaci�n
);
GO

-- Opcional: Crear un �ndice no agrupado en la columna Email
-- para b�squedas m�s r�pidas si vas a consultar mucho por email.
CREATE NONCLUSTERED INDEX IX_Destinatarios_Email
ON Destinatarios (Email);
GO

-- Opcional: Insertar algunos datos de ejemplo (descomenta para usar)

INSERT INTO Destinatarios (Email, Nombre, Edad, Genero, Ubicacion) VALUES
('Santiago.g.miranda@gmail.com', 'Ana Garc�a', 30, 'F', 'Madrid'),
('Santiago.miranda@gmail.com', 'Juan P�rez', 32, 'M', 'Barcelona'),
('Santiago.ga.miranda@gmail.com', 'Maria Lopez', 25, 'F', 'Argentina');
GO


select * from Destinatarios