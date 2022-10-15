CREATE TABLE [dbo].[TblCanalesProspectos]
(
    [CanalId] INT NOT NULL,
    [ProspectoId] INT NOT NULL,
	CONSTRAINT Cmp_Llave PRIMARY KEY (CanalId, ProspectoId),
	CONSTRAINT Cmp_FK_Canal FOREIGN KEY (CanalId) REFERENCES TblCanales(Id),
	CONSTRAINT Cmp_FK_Prospecto FOREIGN KEY (ProspectoId) REFERENCES TblProspectos(Id)
)
