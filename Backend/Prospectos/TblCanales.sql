CREATE TABLE [dbo].[TblCanales]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Nombre] NVARCHAR(50) NULL, 
    [Estatus] INT NULL, 
    [FechaCreacion] DATETIME NULL DEFAULT GETDATE()
)
