using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OData.SmallVille.Models
{
    [Table("TblProspectos")]
    public class Prospecto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Curp { get; set; }
        public string Rfc { get; set; }
        [Required]
        public string CorreoElectronico { get; set; }
        [Required]
        public string Telefono { get; set; }
        public string Domicilio { get; set; }
        [Required]
        public int Estatus { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    [Table("TblCanales")]
    public class Canal
    {
        [Required]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Estatus { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    [Table("TblCanalesProspectos")]
    public class CanalProspecto
    {
        [Required, Key]
        public int CanalId { get; set; }
        [Required, Key]
        public int ProspectoId { get; set; }
    }
}
