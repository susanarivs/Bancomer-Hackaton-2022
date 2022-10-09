using AltergoAPI.Nss.Core.Helpers;
using AltergoAPI.Nss.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AltergoAPI.Nss.Core.Controllers
{
    [SwaggerTag("APIS IMSS")]
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class ImssController : ControllerBase
    {
        private readonly Dictionary<string, string> _entidades;

        private readonly IConfiguration _configuration;

        public ImssController(IConfiguration configuration)
        {
            _configuration = configuration;

            #region Entidades federales

            _entidades = new Dictionary<string, string>(32)
            {
                { "AS", "AGUASCALIENTES" },
                { "BS", "BAJA CALIFORNIA SUR" },
                { "CL", "COAHUILA" },
                { "CS", "CHIAPAS" },
                { "DF", "DISTRITO FEDERAL" },
                { "GT", "GUANAJUATO" },
                { "HG", "HIDALGO" },
                { "MC", "ESTADO DE MEXICO" },
                { "MS", "MORELOS" },
                { "NL", "NUEVO LEON" },
                { "PL", "PUEBLA" },
                { "QR", "QUINTANA ROO" },
                { "SL", "SINALOA" },
                { "TC", "TABASCO" },
                { "TL", "TLAXCALA" },
                { "YN", "YUCATAN" },
                { "NE", "NACIDO EN EL EXTRANJERO" },
                { "BC", "BAJA CALIFORNIA NORTE" },
                { "CC", "CAMPECHE" },
                { "CM", "COLIMA" },
                { "CH", "CHIHUAHUA" },
                { "DG", "DURANGO" },
                { "GR", "GUERRERO" },
                { "JC", "JALISCO" },
                { "MN", "MICHOACAN" },
                { "NT", "NAYARIT" },
                { "OC", "OAXACA" },
                { "QT", "QUERETARO" },
                { "SP", "SAN LUIS POTOSI" },
                { "SR", "SONORA" },
                { "TS", "TAMAULIPAS" },
                { "VZ", "VERACRUZ" },
                { "ZS", "ZACATECAS" }
            };

            #endregion
        }

        /// <summary>
        /// Obtiene el número de seguridad social de una persona inscrita en el imss
        /// mediante su CURP
        /// </summary>
        /// <param name="curp" example="LOSL821028HMCPNS05">Clave Única de Registro de Población a 18 caracteres alfanuméricos</param>
        /// <returns>Número de seguro social, unidad médica asignada y si cuenta con vigencia de derechos</returns>
        /// <response code="200">Respuesta Valida</response>
        /// <response code="400">Respuesta Inválida: El curp no tiene el formato correcto</response>
        [HttpGet("ExtraerNss")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRecord([FromQuery, Required] string curp)
        {
            if (string.IsNullOrWhiteSpace(curp))
            {
                const string err = "El CURP es requerido.";
                return BadRequest(err);
            }

            const string pattern =
                @"^([A-Z&]|[a-z&]{1})([AEIOU]|[aeiou]{1})([A-Z&]|[a-z&]{1})([A-Z&]|[a-z&]{1})([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])([HM]|[hm]{1})([AS|as|BC|bc|BS|bs|CC|cc|CS|cs|CH|ch|CL|cl|CM|cm|DF|df|DG|dg|GT|gt|GR|gr|HG|hg|JC|jc|MC|mc|MN|mn|MS|ms|NT|nt|NL|nl|OC|oc|PL|pl|QT|qt|QR|qr|SP|sp|SL|sl|SR|sr|TC|tc|TS|ts|TL|tl|VZ|vz|YN|yn|ZS|zs|NE|ne]{2})([^A|a|E|e|I|i|O|o|U|u]{1})([^A|a|E|e|I|i|O|o|U|u]{1})([^A|a|E|e|I|i|O|o|U|u]{1})([A-Z]|[a-z]|[0-9]{1})([0-9]{1})$";
            var m = Regex.Match(curp, pattern, RegexOptions.IgnoreCase);

            if (!m.Success)
            {
                const string err = "El CURP no tiene el formato correcto";
                return BadRequest(err);
            }

            var mail = KeyGenerator.GetUniqueKey(10);
            var tempMail = $"{mail}@gmail.com";

            var restClient = new RestClient($"{_configuration["imssapiurl"]}/{curp}?tipoOperacion=consultarNssPorCurp&correo={tempMail}")
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");

            NssResults nssResults = null;
            UmResults umResults = null;
            VgResults vgResults = null;

            var response = await restClient.ExecuteAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                nssResults = NssResults.FromJson(response.Content);

                var iso = curp.Substring(11, 2);
                if (nssResults.Persona != null)
                {
                    nssResults.Persona.EntidadNacimiento = GetEntidad(iso);

                    var dateOfBirth = nssResults.Persona.FechaNacimiento;
                    if (dateOfBirth != null)
                    {
                        var age = GetEdad(dateOfBirth.Value.Date);
                        nssResults.Persona.EdadCumplida = age;
                    }
                }
            }

            if (nssResults?.Persona != null)
            {
                var nss = nssResults.Persona.Nss;
                if (string.IsNullOrWhiteSpace(nss))
                {
                    nssResults.Persona.Nss = "Sin número de seguridad social";
                }
                else
                {
                    restClient = new RestClient($"{_configuration["imssapiurlum"]}/{nss}?tipoOperacion=consultarUMFAsignada")
                    { Timeout = -1 };
                    response = await restClient.ExecuteAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        try
                        {
                            umResults = UmResults.FromJson(response.Content);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    restClient = new RestClient($"{_configuration["imssapiurlvg"]}/{nss}?tipoOperacion=consultarVigencia&curp={curp}")
                    { Timeout = -1 };
                    response = await restClient.ExecuteAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        try
                        {
                            vgResults = VgResults.FromJson(response.Content);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }

            object o = new
            {
                datosPersonales = nssResults?.Persona,
                unidadMedica = umResults,
                vigencia = vgResults
            };

            return Ok(o);
        }

        private string GetEntidad(string iso)
        {
            try
            {
                var res = _entidades.FirstOrDefault(x => x.Key == iso);
                return res.Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static int? GetEdad(DateTime dateOfBirth)
        {
            try
            {
                var age = DateTime.Now.Year - dateOfBirth.Year;
                if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                {
                    age -= 1;
                }

                return age;
            }
            catch
            {
                return null;
            }
        }
    }
}