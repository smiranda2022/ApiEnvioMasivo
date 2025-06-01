select * from  Destinatarios;
SELECT
    TABLE_SCHEMA AS SchemaName,
    TABLE_NAME AS TableName,
    TABLE_TYPE AS TableType
FROM
    INFORMATION_SCHEMA.TABLES
WHERE
    TABLE_TYPE = 'BASE TABLE'
    -- AND TABLE_SCHEMA = 'dbo' -- Opcional: Descomenta si solo quieres tablas de un esquema específico, como 'dbo'
ORDER BY
    TableName;

	select * from __EFMigrationsHistory
select * from  CorreosEnviados
select * from  Destinatarios
select * from  FlujoHistoriales
select * from  FlujoPasos
select * from  Flujos