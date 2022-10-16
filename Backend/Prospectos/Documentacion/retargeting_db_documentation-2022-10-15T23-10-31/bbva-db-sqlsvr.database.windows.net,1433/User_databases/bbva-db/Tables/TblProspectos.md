#### 

[Project](../../../../index.md) > [bbva-db-sqlsvr.database.windows.net,1433](../../../index.md) > [User databases](../../index.md) > [bbva-db](../index.md) > [Tables](Tables.md) > dbo.TblProspectos

# ![Tables](../../../../Images/Table32.png) [dbo].[TblProspectos]

---

## <a name="#description"></a>MS_Description

Tabla de prospectos

## <a name="#properties"></a>Properties

| Property | Value |
|---|---|
| Collation | SQL_Latin1_General_CP1_CI_AS |
| Row Count (~) | 300 |
| Created | 7:03:41 PM Saturday, October 15, 2022 |
| Last Modified | 7:05:13 PM Saturday, October 15, 2022 |


---

## <a name="#columns"></a>Columns

| Key | Name | Data Type | Max Length (Bytes) | Nullability | Identity | Default | Description |
|---|---|---|---|---|---|---|---|
| [![Cluster Primary Key PK__TblProsp__3214EC074B62835F: Id](../../../../Images/pkcluster.png)](#indexes) | Id | int | 4 | NOT NULL | 1 - 1 |  |  |
|  | Nombres | nvarchar(50) | 100 | NOT NULL |  |  | _Primer nombre y/o Segundo nombre del prospecto_ |
|  | ApellidoPaterno | nvarchar(50) | 100 | NULL allowed |  |  | _Apellido Paterno_ |
|  | ApellidoMaterno | nvarchar(50) | 100 | NULL allowed |  |  | _Apellido Materno_ |
|  | FechaNacimiento | date | 3 | NULL allowed |  |  | _Fecha de Nacimiento_ |
|  | Curp | nvarchar(20) | 40 | NULL allowed |  |  | _Clave Unica de Registro de Población_ |
|  | Rfc | nvarchar(20) | 40 | NULL allowed |  |  | _Registro Federal de Contribuyentes_ |
|  | CorreoElectronico | nvarchar(50) | 100 | NOT NULL |  |  | _Correo Electrónico_ |
|  | Telefono | nvarchar(10) | 20 | NOT NULL |  |  | _Número Telefónico_ |
|  | Domicilio | nvarchar(100) | 200 | NULL allowed |  |  | _Dirección del domicilio principal_ |
|  | Estatus | int | 4 | NOT NULL |  |  | _Indicador del estado del registro: 0=Inactivo; 1=Activo_ |
|  | FechaCreacion | datetime | 8 | NULL allowed |  | (getdate()) | _Fecha en que el registro fue creado_ |


---

## <a name="#indexes"></a>Indexes

| Key | Name | Key Columns | Unique |
|---|---|---|---|
| [![Cluster Primary Key PK__TblProsp__3214EC074B62835F: Id](../../../../Images/pkcluster.png)](#indexes) | PK__TblProsp__3214EC074B62835F | Id | YES |


---

## <a name="#sqlscript"></a>SQL Script

```sql
CREATE TABLE [dbo].[TblProspectos]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Nombres] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApellidoPaterno] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ApellidoMaterno] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FechaNacimiento] [date] NULL,
[Curp] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Rfc] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CorreoElectronico] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Telefono] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Domicilio] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Estatus] [int] NOT NULL,
[FechaCreacion] [datetime] NULL CONSTRAINT [DF__TblProspe__Fecha__72C60C4A] DEFAULT (getdate())
)
GO
ALTER TABLE [dbo].[TblProspectos] ADD CONSTRAINT [PK__TblProsp__3214EC074B62835F] PRIMARY KEY CLUSTERED  ([Id])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Tabla de prospectos', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Apellido Materno', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'ApellidoMaterno'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Apellido Paterno', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'ApellidoPaterno'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Correo Electrónico', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'CorreoElectronico'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Clave Unica de Registro de Población', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'Curp'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Dirección del domicilio principal', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'Domicilio'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Indicador del estado del registro: 0=Inactivo; 1=Activo', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'Estatus'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Fecha en que el registro fue creado', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'FechaCreacion'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Fecha de Nacimiento', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'FechaNacimiento'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Primer nombre y/o Segundo nombre del prospecto', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'Nombres'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Registro Federal de Contribuyentes', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'Rfc'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Número Telefónico', 'SCHEMA', N'dbo', 'TABLE', N'TblProspectos', 'COLUMN', N'Telefono'
GO

```


---

## <a name="#usedby"></a>Used By

* [[dbo].[TblCanalesProspectos]](TblCanalesProspectos.md)


---

###### Author:  SmallVille

###### Copyright 2022 - All Rights Reserved

###### Created: Saturday, October 15, 2022 11:10:31 PM

