using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using WhiteLabel.Helpers;
using WhiteLabel.Models;
using WhiteLabel.Services.Database;
using Xamarin.Forms;

namespace WhiteLabel.ViewModels
{
    internal sealed class Retrys
    {
        private readonly Repository<PersonData> _repositoryDocs = Repository<PersonData>.Instance();

        #region URL Secretas
        private const string _urlCore = "https://dt01.azurewebsites.net";
        private const string _urlIneMilitante = "https://servicios.ine.mx";
        #endregion

        private static Retrys _instance;
        internal static Retrys Instance()
        {
            return _instance ??= new Retrys();
        }

        public async Task UpdateIneAsync(ValidaIneRequest ineRequest, PersonData person)
        {
            try
            {
                var factory = new PerBaseUrlFlurlClientFactory();
                var flIne = factory.Get(new Url(_urlCore));
                flIne.Settings.HttpClientFactory = new UntrustedCertClientFactory();

                var preine = await flIne
                    .Request("Ine/ValidarIdTipoE")
                    .PostJsonAsync(ineRequest)
                    .ReceiveJson<ValidaIneResponse>();

                if (preine != null)
                {
                    person.WasValidated = true;
                    person.ValidationResult = preine.EstaVigente == "Si" ? "VIGENTE COMO IDENTIFICACION Y PUEDE VOTAR" : preine.Motivo;

                    if (preine.Details != null)
                    {
                        person.ValidatedElectorKey = preine.Details.ClaveDeElector;
                        person.ValidatedEmission = preine.Details.NumeroDeEmision;
                        person.ValidatedRecordYear = preine.Details.AnioDeRegistro;
                        person.ValidatedEmissionYear = preine.Details.AnioDeEmision;
                        person.ValidatedOcrNumber = preine.Details.NumeroOcr;
                    }

                    var result = await _repositoryDocs.UpdateItemAsync(person);
                    MessagingCenter.Send<object, bool>(this, $"updatelbl_{person.Id}", result);

                    //como se obtuvo la clave de elector se puede validar partido
                    person.EnableAfiliationValidation = true;
                    var afiliationRequest = new AfiliationRequest
                    {
                        Clave = person.ValidatedElectorKey
                    };
                    _ = Task.Factory.StartNew(async () => await UpdateAfiliationAsync(afiliationRequest, person));

                    //como se obtuvo la clave de elector se puede generar la curp
                    _ = Task.Factory.StartNew(async () => await UpdateCurpFromElectorKeyAsync(person));
                }
            }
            catch (Exception fhte)
            {
            }
        }

        public async Task UpdateAfiliationAsync(AfiliationRequest afiliationRequest, PersonData person)
        {
            try
            {
                var factory = new PerBaseUrlFlurlClientFactory();
                var flAfil = factory.Get(new Url(_urlIneMilitante));
                flAfil.Settings.HttpClientFactory = new UntrustedCertClientFactory();

                var afi = await flAfil
                    .Request("militantesService/rest/service/detalleMilitante")
                    .WithHeader("Content-Type", "text/plain")
                    .PostJsonAsync(afiliationRequest)
                    .ReceiveString();

                var afiliation = AfiliationResponse.FromJson(afi);
                if (afiliation != null)
                {
                    if (afiliation.ListaMilitantes.Count > 0)
                    {
                        var afiliationResume = afiliation.ListaMilitantes[0];
                        person.AfiliationResult = $"AFILIADO AL {afiliationResume.Siglas} DESDE EL {afiliationResume.FechaAfiliacion}";
                    }
                    else
                    {
                        person.AfiliationResult = "NO ESTA AFILIADO A UN PARTIDO POLITICO"; //afiliation.Message;
                    }

                    var result = await _repositoryDocs.UpdateItemAsync(person);
                    MessagingCenter.Send<object, bool>(this, $"updatelbl_{person.Id}", result);
                }
            }
            catch (Exception fhte)
            {
            }
        }

        public async Task UpdateNssAsync(PersonData person)
        {
            try
            {
                var ims = await _urlCore
                         .AppendPathSegment("Imss/ExtraerNss")
                         .SetQueryParams(new { person.Curp })
                         .GetStringAsync();

                var infoImss = InfoImssResponse.FromJson(ims);
                if (infoImss != null)
                {
                    person.SecurityNumber = infoImss.DatosPersonales?.Nss.ToUpperInvariant();
                    person.BornState = infoImss.DatosPersonales?.EntidadNacimiento;
                    person.CurrentAge = infoImss.DatosPersonales?.EdadCumplida;
                    person.MedicalUnit = infoImss.UnidadMedica?.Nombre;
                    person.MedicalServiceActive = infoImss.Vigencia?.ConDerechoInc;

                    var result = await _repositoryDocs.UpdateItemAsync(person);
                    MessagingCenter.Send<object, bool>(this, $"updatelbl_{person.Id}", result);
                }
            }
            catch (Exception fhte)
            {
            }
        }

        public async Task UpdateRfcAsync(PersonData person)
        {
            try
            {
                //var nombres = person.Name.Split(' ');
                //var fullName = string.Empty;
                //if (nombres.Length >= 3)
                //{
                //    var rest = nombres.Skip(2);
                //    foreach (var r in rest)
                //    {
                //        fullName += $"{r} ";
                //    }
                //}

                var dt = DateTime.Parse(person.BirthDate);
                var rfcGenerated = await _urlCore
                         .AppendPathSegment("Sat/GenerarRfc")
                         .SetQueryParams(new
                         {
                             Name = person.Name.Trim(), //fullName.Trim(),
                             FirstLastName = person.ApellidoPaterno, //nombres[0].Trim(),
                             SecondLastName = person.ApellidoMaterno, //nombres[1].Trim(),
                             dt.Year,
                             dt.Month,
                             dt.Day
                         }).GetStringAsync();

                var infoRfc = RfcResponse.FromJson(rfcGenerated);
                if (infoRfc != null)
                {
                    person.Rfc = infoRfc.Rfc.Trim();

                    //consulta en SAT
                    //var rfcStatus = await _urlCore
                    //        .AppendPathSegment("Sat/ValidarRfc")
                    //        .SetQueryParams(new
                    //        {
                    //            rfc = person.Rfc
                    //        }).PostStringAsync("");
                    //var preValidation = await rfcStatus.Content.ReadAsStringAsync();
                    //person.RfcValidationResult = preValidation.Replace("\"", "").ToUpperInvariant();

                    var result = await _repositoryDocs.UpdateItemAsync(person);
                    MessagingCenter.Send<object, bool>(this, $"updatelbl_{person.Id}", result);
                }
            }
            catch (Exception fhte)
            {
            }
        }

        public async Task UpdateCedulaAsync(PersonData person)
        {
            try
            {
                var cedulaRequest = new CedulaRequest { Curp = person.Curp };

                var factory = new PerBaseUrlFlurlClientFactory();
                var flCedula = factory.Get(new Url(_urlCore));
                flCedula.Settings.HttpClientFactory = new UntrustedCertClientFactory();

                var precedula = await flCedula
                    .Request("Sep/VerificarPorCurp")
                    .PostJsonAsync(cedulaRequest)
                    .ReceiveJson<CedulaResponse>();

                if (precedula != null)
                {
                    person.CedulaValidated = true;
                    person.EdoNacimiento = precedula.EdoNacimiento;
                    person.CedulaProfesional = precedula.CedulaProf;
                    person.Escuela = precedula.Escuela;
                    person.Carrera = precedula.Carrera;

                    var result = await _repositoryDocs.UpdateItemAsync(person);
                    MessagingCenter.Send<object, bool>(this, $"updatelbl_{person.Id}", result);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task UpdateCurpFromElectorKeyAsync(PersonData person)
        {
            try
            {
                var nombres = person.Name.Split(' ');
                var fullName = string.Empty;
                if (nombres.Length >= 3)
                {
                    var rest = nombres.Skip(2);
                    foreach (var r in rest)
                    {
                        fullName += $"{r} ";
                    }
                }

                var dt = DateTime.Parse(person.BirthDate);
                var genre = person.ValidatedElectorKey.Substring(14, 1);
                var state = person.ValidatedElectorKey.Substring(12, 2);
                var curpGenerated = await _urlCore
                         .AppendPathSegment("Renapo/GenerarCurp")
                         .SetQueryParams(new
                         {
                             Names = fullName.Trim(),
                             FirstLastName = nombres[0].Trim(),
                             SecondLastName = nombres[1].Trim(),
                             dt.Year,
                             dt.Month,
                             dt.Day,
                             Genre = genre,
                             State = state
                         }).GetStringAsync();

                var infoCurp = curpGenerated.Replace("\"", "").ToUpperInvariant();
                if (infoCurp.Length == 18)
                {
                    person.Curp = infoCurp;
                    person.EnableImssValidation = true;
                    person.EnableCedulaValidation = true;

                    var result = await _repositoryDocs.UpdateItemAsync(person);
                    MessagingCenter.Send<object, bool>(this, $"updatelbl_{person.Id}", result);

                    _ = Task.Factory.StartNew(async () => await UpdateNssAsync(person));
                    _ = Task.Factory.StartNew(async () => await UpdateCedulaAsync(person));
                }
            }
            catch (Exception fhte)
            {
            }
        }

    }
}
