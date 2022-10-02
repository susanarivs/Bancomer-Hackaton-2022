using Newtonsoft.Json;
using SQLite;
using System;
using System.IO;
using System.Runtime.Serialization;
using Xamarin.Forms;

namespace WhiteLabel.Models
{
    [Table("PersonData")]
    public class PersonData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //información del documento
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Curp { get; set; }
        public string Gender { get; set; }
        public string ElectorKey { get; set; }  //desde la credencial
        public string BirthDate { get; set; }
        public string Nationality { get; set; }
        public string FrontImage { get; set; }
        public string FaceImage { get; set; }
        public string State { get; set; }
        public string Municipality { get; set; }
        public string Locality { get; set; }
        public string Section { get; set; }
        public string Emission { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string ExpiryDate { get; set; }

        public string AnioRegistro { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string FullName => $"{Name} {ApellidoPaterno} {ApellidoMaterno}";

        //ine validation info
        public bool EnableIneValidation { get; set; }
        public bool WasValidated { get; set; }
        public string ValidationResult { get; set; }
        public string CitizenId { get; set; }
        public string ValidatedElectorKey { get; set; }
        public string ValidatedEmission { get; set; }
        public string ValidatedRecordYear { get; set; }
        public string ValidatedEmissionYear { get; set; }
        public string ValidatedOcrNumber { get; set; }

        //imss validation info
        //public bool EnableImssValidation { get; set; }
        //public string SecurityNumber { get; set; }
        //public string BornState { get; set; }
        //public int? CurrentAge { get; set; }
        //public string MedicalUnit { get; set; }
        //public string MedicalServiceActive { get; set; }

        //afiliación politica
        //public bool EnableAfiliationValidation { get; set; }
        //public string AfiliationResult { get; set; }

        //Sat
        //public bool EnableRfcGeneration { get; set; }
        //public string Rfc { get; set; }
        //public string RfcValidationResult { get; set; }

        //internal control
        public DateTime CapturedDate { get; set; }

        //cedula profesional
        //public bool EnableCedulaValidation { get; set; }
        //public bool CedulaValidated { get; set; }
        //public string EdoNacimiento { get; set; }
        //public string CedulaProfesional { get; set; }
        //public string Escuela { get; set; }
        //public string Carrera { get; set; }

        public string TramiteSele { get; set; }
        public string Responsable { get; set; }
    }

    public class PersonDataView : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DocumentType { get; set; }
        public string Base64 { get; set; }
        public DateTime CapturedDate { get; set; }

        private ImageSource imageFace;

        [JsonIgnore]
        [IgnoreDataMember]
        public ImageSource ImageFace
        {
            get
            {
                if (imageFace == null)
                {
                    imageFace = ImageSource.FromStream(
                        () => new MemoryStream(Convert.FromBase64String(Base64)));
                }

                return imageFace;
            }
        }
    }
}
