-- Script para crear la tabla Destinatarios

-- Opcional: Especifica la base de datos donde quieres crear la tabla.
-- Si no especificas una base de datos, se creará en la base de datos en la que estés conectado.
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
    Email NVARCHAR(255) NOT NULL UNIQUE, -- Cadena de texto para email, no nulo y único
    Nombre NVARCHAR(255),               -- Cadena de texto para nombre
    Edad INT,                           -- Número entero para edad
    Genero NVARCHAR(50),                -- Cadena de texto para género
    Ubicacion NVARCHAR(255)             -- Cadena de texto para ubicación
);
GO

-- Opcional: Crear un índice no agrupado en la columna Email
-- para búsquedas más rápidas si vas a consultar mucho por email.
CREATE NONCLUSTERED INDEX IX_Destinatarios_Email
ON Destinatarios (Email);
GO

-- Opcional: Insertar algunos datos de ejemplo (descomenta para usar)

INSERT INTO Destinatarios (Email, Nombre, Edad, Genero, Ubicacion) VALUES
('Santiago.g.miranda@gmail.com', 'Ana García', 30, 'F', 'Madrid'),
('Santiago.miranda@gmail.com', 'Juan Pérez', 32, 'M', 'Barcelona'),
('Santiago.ga.miranda@gmail.com', 'Maria Lopez', 25, 'F', 'Argentina');
GO


select * from Destinatarios