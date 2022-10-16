#### 

[Project](../../../../index.md) > [bbva-db-sqlsvr.database.windows.net,1433](../../../index.md) > [User databases](../../index.md) > [bbva-db](../index.md) > [Tables](Tables.md) > dbo.TblCanales

# ![Tables](../../../../Images/Table32.png) [dbo].[TblCanales]

---

## <a name="#description"></a>MS_Description

Tabla de canales de comunicación

## <a name="#properties"></a>Properties

| Property | Value |
|---|---|
| Collation | SQL_Latin1_General_CP1_CI_AS |
| Row Count (~) | 5 |
| Created | 7:03:11 PM Saturday, October 15, 2022 |
| Last Modified | 7:05:13 PM Saturday, October 15, 2022 |


---

## <a name="#columns"></a>Columns

| Key | Name | Data Type | Max Length (Bytes) | Nullability | Identity | Default | Description |
|---|---|---|---|---|---|---|---|
| [![Cluster Primary Key PK__TblCanal__3214EC07BD0BEF50: Id](../../../../Images/pkcluster.png)](#indexes) | Id | int | 4 | NOT NULL | 1 - 1 |  | _Identificador único del registro_ |
|  | Nombre | nvarchar(50) | 100 | NULL allowed |  |  | _Nombre del canal_ |
|  | Estatus | int | 4 | NULL allowed |  |  | _Indicador del estado del registro: 0=Inactivo; 1=Activo_ |
|  | FechaCreacion | datetime | 8 | NULL allowed |  | (getdate()) | _Fecha en que el registro fue creado_ |


---

## <a name="#indexes"></a>Indexes

| Key | Name | Key Columns | Unique |
|---|---|---|---|
| [![Cluster Primary Key PK__TblCanal__3214EC07BD0BEF50: Id](../../../../Images/pkcluster.png)](#indexes) | PK__TblCanal__3214EC07BD0BEF50 | Id | YES |


---

## <a name="#sqlscript"></a>SQL Script

```sql
CREATE TABLE [dbo].[TblCanales]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Nombre] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Estatus] [int] NULL,
[FechaCreacion] [datetime] NULL CONSTRAINT [DF__TblCanale__Fecha__6FE99F9F] DEFAULT (getdate())
)
GO
ALTER TABLE [dbo].[TblCanales] ADD CONSTRAINT [PK__TblCanal__3214EC07BD0BEF50] PRIMARY KEY CLUSTERED  ([Id])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Tabla de canales de comunicación', 'SCHEMA', N'dbo', 'TABLE', N'TblCanales', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Indicador del estado del registro: 0=Inactivo; 1=Activo', 'SCHEMA', N'dbo', 'TABLE', N'TblCanales', 'COLUMN', N'Estatus'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Fecha en que el registro fue creado', 'SCHEMA', N'dbo', 'TABLE', N'TblCanales', 'COLUMN', N'FechaCreacion'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Identificador único del registro', 'SCHEMA', N'dbo', 'TABLE', N'TblCanales', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Nombre del canal', 'SCHEMA', N'dbo', 'TABLE', N'TblCanales', 'COLUMN', N'Nombre'
GO

```


---

## <a name="#usedby"></a>Used By

* [[dbo].[TblCanalesProspectos]](TblCanalesProspectos.md)


---

###### Author:  SmallVille

###### Copyright 2022 - All Rights Reserved

###### Created: Saturday, October 15, 2022 11:10:31 PM

