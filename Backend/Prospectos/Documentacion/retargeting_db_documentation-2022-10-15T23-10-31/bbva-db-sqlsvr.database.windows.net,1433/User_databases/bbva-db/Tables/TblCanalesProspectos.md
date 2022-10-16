#### 

[Project](../../../../index.md) > [bbva-db-sqlsvr.database.windows.net,1433](../../../index.md) > [User databases](../../index.md) > [bbva-db](../index.md) > [Tables](Tables.md) > dbo.TblCanalesProspectos

# ![Tables](../../../../Images/Table32.png) [dbo].[TblCanalesProspectos]

---

## <a name="#description"></a>MS_Description

Tabla de relación para los prospectos y sus canales asignados

## <a name="#properties"></a>Properties

| Property | Value |
|---|---|
| Row Count (~) | 40 |
| Created | 7:05:13 PM Saturday, October 15, 2022 |
| Last Modified | 7:05:13 PM Saturday, October 15, 2022 |


---

## <a name="#columns"></a>Columns

| Key | Name | Data Type | Max Length (Bytes) | Nullability | Description |
|---|---|---|---|---|---|
| [![Cluster Primary Key Cmp_Llave: CanalId\ProspectoId](../../../../Images/pkcluster.png)](#indexes)[![Foreign Keys Cmp_FK_Canal: [dbo].[TblCanales].CanalId](../../../../Images/fk.png)](#foreignkeys) | CanalId | int | 4 | NOT NULL | _Identificador del Canal_ |
| [![Cluster Primary Key Cmp_Llave: CanalId\ProspectoId](../../../../Images/pkcluster.png)](#indexes)[![Foreign Keys Cmp_FK_Prospecto: [dbo].[TblProspectos].ProspectoId](../../../../Images/fk.png)](#foreignkeys) | ProspectoId | int | 4 | NOT NULL | _Identificador del Prospecto_ |


---

## <a name="#indexes"></a>Indexes

| Key | Name | Key Columns | Unique |
|---|---|---|---|
| [![Cluster Primary Key Cmp_Llave: CanalId\ProspectoId](../../../../Images/pkcluster.png)](#indexes) | Cmp_Llave | CanalId, ProspectoId | YES |


---

## <a name="#foreignkeys"></a>Foreign Keys

| Name | Columns |
|---|---|
| Cmp_FK_Canal | CanalId->[[dbo].[TblCanales].[Id]](TblCanales.md) |
| Cmp_FK_Prospecto | ProspectoId->[[dbo].[TblProspectos].[Id]](TblProspectos.md) |


---

## <a name="#sqlscript"></a>SQL Script

```sql
CREATE TABLE [dbo].[TblCanalesProspectos]
(
[CanalId] [int] NOT NULL,
[ProspectoId] [int] NOT NULL
)
GO
ALTER TABLE [dbo].[TblCanalesProspectos] ADD CONSTRAINT [Cmp_Llave] PRIMARY KEY CLUSTERED  ([CanalId], [ProspectoId])
GO
ALTER TABLE [dbo].[TblCanalesProspectos] ADD CONSTRAINT [Cmp_FK_Canal] FOREIGN KEY ([CanalId]) REFERENCES [dbo].[TblCanales] ([Id])
GO
ALTER TABLE [dbo].[TblCanalesProspectos] ADD CONSTRAINT [Cmp_FK_Prospecto] FOREIGN KEY ([ProspectoId]) REFERENCES [dbo].[TblProspectos] ([Id])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Tabla de relación para los prospectos y sus canales asignados', 'SCHEMA', N'dbo', 'TABLE', N'TblCanalesProspectos', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Identificador del Canal', 'SCHEMA', N'dbo', 'TABLE', N'TblCanalesProspectos', 'COLUMN', N'CanalId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Identificador del Prospecto', 'SCHEMA', N'dbo', 'TABLE', N'TblCanalesProspectos', 'COLUMN', N'ProspectoId'
GO

```


---

## <a name="#uses"></a>Uses

* [[dbo].[TblCanales]](TblCanales.md)
* [[dbo].[TblProspectos]](TblProspectos.md)


---

###### Author:  SmallVille

###### Copyright 2022 - All Rights Reserved

###### Created: Saturday, October 15, 2022 11:10:31 PM

