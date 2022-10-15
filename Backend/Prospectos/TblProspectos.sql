CREATE TABLE [dbo].[TblProspectos]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Nombres] NVARCHAR(50) NOT NULL, 
    [ApellidoPaterno] NVARCHAR(50) NULL, 
    [ApellidoMaterno] NVARCHAR(50) NULL, 
    [FechaNacimiento] DATE NULL, 
    [Curp] NVARCHAR(20) NULL, 
    [Rfc] NVARCHAR(20) NULL, 
    [CorreoElectronico] NVARCHAR(50) NOT NULL, 
    [Telefono] NVARCHAR(10) NOT NULL, 
    [Domicilio] NVARCHAR(100) NULL, 
    [Estatus] INT NOT NULL, 
    [FechaCreacion] DATETIME NULL DEFAULT GETDATE()
)
