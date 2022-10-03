using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WhiteLabel.Models;
using WhiteLabel.Services.Database;
using Xamarin.Forms;

namespace WhiteLabel
{
    public class ContactDetailViewModel : ObservableObject
    {
        private readonly int _contactId;
        private PersonData _contact;
        private readonly Repository<PersonData> _repositoryPerson = Repository<PersonData>.Instance();

        public ContactDetailViewModel(int contactId)
        {
            if (contactId != -1)
            {
                _contactId = contactId;
                LoadData();

                var ide = $"updatelbl_{contactId}";
                MessagingCenter.Subscribe<object, bool>(this, ide, (s, e) =>
                {
                    if (e)
                    {
                        LoadData();
                    }
                });
            }
        }

        public PersonData Contact
        {
            get => _contact;
            set
            {
                if (SetProperty(ref _contact, value))
                {
                    NotifyPropertyChanged(nameof(Values));
                }
            }
        }

        public IEnumerable<ValueData> Values
        {
            get
            {
                if (Contact == null)
                {
                    yield break;
                }

                if (!string.IsNullOrWhiteSpace(Contact.Name))
                {
                    yield return new ValueData("Nombre Completo", $"{Contact.Name} {Contact.ApellidoPaterno} {Contact.ApellidoMaterno}");
                }

                if (!string.IsNullOrWhiteSpace(Contact.Address))
                {
                    yield return new ValueData("Dirección", Contact.Address);
                }

                if (!string.IsNullOrWhiteSpace(Contact.Curp))
                {
                    yield return new ValueData("Clave Unica de Registro de Población", Contact.Curp);
                }

                if (!string.IsNullOrWhiteSpace(Contact.Gender))
                {
                    yield return new ValueData("Genero", Contact.Gender);
                }

                if (!string.IsNullOrWhiteSpace(Contact.ElectorKey))
                {
                    yield return new ValueData("Clave de Elector", Contact.ElectorKey);
                }

                if (!string.IsNullOrWhiteSpace(Contact.BirthDate))
                {
                    //DateTime dt = DateTime.Parse(Contact.BirthDate.ToString());
                    //string s = dt.ToString("dd/MM/yyyy", _localCulture);
                    yield return new ValueData("Fecha de Nacimiento", Contact.BirthDate);
                }

                if (!string.IsNullOrWhiteSpace(Contact.ExpiryDate))
                {
                    //DateTime dt = DateTime.Parse(Contact.ExpiryDate.ToString());
                    //string s = dt.ToString("dd/MM/yyyy", _localCulture);
                    yield return new ValueData("Vigencia", Contact.ExpiryDate);
                }

                if (!string.IsNullOrWhiteSpace(Contact.Nationality))
                {
                    yield return new ValueData("Nacionalidad", Contact.Nationality);
                }

                if (!string.IsNullOrWhiteSpace(Contact.DocumentNumber))
                {
                    yield return new ValueData("Número de Documento", Contact.DocumentNumber);
                }

                if (!string.IsNullOrWhiteSpace(Contact.State))
                {
                    yield return new ValueData("Estado de Residencia", Contact.State);
                }

                if (!string.IsNullOrWhiteSpace(Contact.Municipality))
                {
                    yield return new ValueData("Municipio de Residencia", Contact.Municipality);
                }

                if (!string.IsNullOrWhiteSpace(Contact.Locality))
                {
                    yield return new ValueData("Localidad de Residencia", Contact.Locality);
                }

                if (!string.IsNullOrWhiteSpace(Contact.Section))
                {
                    yield return new ValueData("Sección Electoral", Contact.Section);
                }

                if (!string.IsNullOrWhiteSpace(Contact.AnioRegistro))
                {
                    yield return new ValueData("Año de Registro", Contact.AnioRegistro);
                }

                if (!string.IsNullOrWhiteSpace(Contact.Emission))
                {
                    yield return new ValueData("Emisión", Contact.Emission);
                }

                /*if (!string.IsNullOrWhiteSpace(Contact.ExpiryDate))
                {
                    string estatus;

                    if (Contact.ExpiryDate == "PERMANENTE")
                    {
                        estatus = "VIGENTE";
                    }
                    else
                    {
                        try
                        {
                            DateTime exp;
                            if (Contact.ExpiryDate.Length == 4)
                            {
                                var year = int.Parse(Contact.ExpiryDate);
                                exp = new DateTime(year, 1, 1);
                            }
                            else
                            {
                                DateTime.TryParse(Contact.ExpiryDate, out exp);
                            }

                            estatus = DateTime.Now > exp ? "VENCIDO" : "VIGENTE";
                        }
                        catch
                        {
                            estatus = "INDETERMINADO";
                        }
                    }

                    yield return new ValueData("El Documento Parece Estar", estatus);
                }*/

                //if (!string.IsNullOrWhiteSpace(Contact.Responsable))
                //{
                //    Responsable = Contact.Responsable;
                //}

                /*if (Contact.EnableRfcGeneration)
                {
                    if (!string.IsNullOrWhiteSpace(Contact.Rfc))
                    {
                        yield return new ValueData("Registro Federal de Contribuyentes", Contact.Rfc);
                    }
                    else
                    {
                        if (DateTime.Now > Contact.CapturedDate.AddMinutes(3))
                        {
                            _ = Task.Factory.StartNew(async () => await _repositoryRetrys.UpdateRfcAsync(Contact));
                        }

                        yield return new ValueData("Registro Federal de Contribuyentes", "", true, false);
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.RfcValidationResult))
                    {
                        yield return new ValueData("Estado en el SAT", Contact.RfcValidationResult);
                    }
                }

                if (Contact.EnableImssValidation)
                {
                    if (!string.IsNullOrWhiteSpace(Contact.SecurityNumber))
                    {
                        yield return new ValueData("Número de Seguridad Social", Contact.SecurityNumber);
                    }
                    else
                    {
                        yield return new ValueData("Número de Seguridad Social", "", true, false);
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.BornState))
                    {
                        yield return new ValueData("Entidad de Nacimiento", Contact.BornState);
                    }

                    if (Contact.CurrentAge != 0)
                    {
                        yield return new ValueData("Edad Cumplida", $"{Contact.CurrentAge} AÑOS");
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.MedicalUnit))
                    {
                        yield return new ValueData("Unidad Médica Asignada", Contact.MedicalUnit);
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.MedicalServiceActive))
                    {
                        yield return new ValueData("Derechos Médicos Vigentes", Contact.MedicalServiceActive);
                    }
                }

                if (Contact.EnableIneValidation)
                {
                    if (!Contact.WasValidated)
                    {
                        if (DateTime.Now > Contact.CapturedDate.AddMinutes(3))
                        {
                            var validaIneRequest = new ValidaIneRequest
                            {
                                IdCiudadano = Contact.CitizenId,
                                Cic = Contact.DocumentNumber
                            };

                            _ = Task.Factory.StartNew(async () => await _repositoryRetrys.UpdateIneAsync(validaIneRequest, Contact));
                        }

                        yield return new ValueData("Validación en INE (verificando)", "", true, false);
                    }
                    else
                    {
                        yield return new ValueData("Validación en INE", Contact.ValidationResult);
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.ValidatedElectorKey))
                    {
                        yield return new ValueData("Clave de Elector", Contact.ValidatedElectorKey);
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.ValidatedOcrNumber))
                    {
                        yield return new ValueData("Número de Ocr", Contact.ValidatedOcrNumber);
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.ValidatedEmission))
                    {
                        yield return new ValueData("Número de Emisión", Contact.ValidatedEmission);
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.ValidatedRecordYear))
                    {
                        yield return new ValueData("Año de Registro", Contact.ValidatedRecordYear);
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.ValidatedEmissionYear))
                    {
                        yield return new ValueData("Año de Emisión", Contact.ValidatedEmissionYear);
                    }
                }

                if (Contact.EnableAfiliationValidation)
                {
                    if (!string.IsNullOrWhiteSpace(Contact.AfiliationResult))
                    {
                        yield return new ValueData("Afiliación Política", Contact.AfiliationResult);
                    }
                    else
                    {
                        if (DateTime.Now > Contact.CapturedDate.AddMinutes(3))
                        {
                            var afiliationRequest = new AfiliationRequest
                            {
                                Clave = Contact.ValidatedElectorKey
                            };

                            _ = Task.Factory.StartNew(async () => await _repositoryRetrys.UpdateAfiliationAsync(afiliationRequest, Contact));
                        }

                        yield return new ValueData("Afiliación Política (verificando)", "", true, false);
                    }
                }

                if (Contact.EnableCedulaValidation)
                {
                    if (!Contact.CedulaValidated)
                    {
                        if (DateTime.Now > Contact.CapturedDate.AddMinutes(3))
                        {
                            _ = Task.Factory.StartNew(async () => await _repositoryRetrys.UpdateCedulaAsync(Contact));
                        }

                        yield return new ValueData("Cédula Profesional (verificando)", "", true, false);
                    }
                    else
                    {
                        yield return new ValueData("Cédula Profesional", Contact.CedulaProfesional);
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.Carrera))
                    {
                        yield return new ValueData("Carrera", Contact.Carrera);
                    }

                    if (!string.IsNullOrWhiteSpace(Contact.Escuela))
                    {
                        yield return new ValueData("Escuela", Contact.Escuela);
                    }
                }*/
            }
        }

        private string imageBase64;
        public string ImageBase64
        {
            get => imageBase64;
            set
            {
                imageBase64 = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Image");
            }
        }

        private ImageSource image;
        public ImageSource Image
        {
            get
            {
                return image ??= ImageSource.FromStream(
                    () => new MemoryStream(Convert.FromBase64String(ImageBase64)));
            }
        }

        public async Task<bool> DeleteContact()
        {
            try
            {
                var result = await _repositoryPerson.DeleteItemAsync(Contact);
                return result;
            }
            catch
            {
                return false;
            }
        }

        private void LoadData()
        {
            //recarga el registro completo
            Contact = _repositoryPerson.GetItem(p => p.Id == _contactId);
            ImageBase64 = string.IsNullOrWhiteSpace(Contact.FaceImage) ? Contact.Avatar : Contact.FaceImage;
        }
    }
}
