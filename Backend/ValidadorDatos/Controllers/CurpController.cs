using AltergoAPI.Nss.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AltergoAPI.Nss.Core.Controllers
{
    [SwaggerTag("APIS RENAPO")]
    [Produces("application/json")]
    [Route("Renapo")]
    [ApiController]
    public class CurpController : ControllerBase
    {
        private const string agente = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36 Edg/94.0.992.50";

        private readonly IConfiguration _configuration;

        public CurpController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Valida la clave única de registro de población de una persona y devuelve sus datos
        /// </summary>
        /// <param name="curp" example="HEBG830708HDFRSR03">Clave Única de Registro de Población a 18 caracteres alfanuméricos</param>
        /// <returns>Datos de la persona registrada con esa CURP</returns>
        /// <response code="200">Respuesta Valida</response>
        /// <response code="204">Respuesta Inválida: Sin contenido</response>
        /// <response code="400">Respuesta Inválida: El curp no tiene el formato correcto</response>
        /// <response code="404">Respuesta Inválida: No fue posible validar el curp</response>
        [HttpGet("ValidarCurp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ValidarCurpRenapo([FromQuery, Required] string curp)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(curp))
                {
                    const string err = "El CURP es requerido.";
                    return BadRequest(err);
                }

                const string pattern = @"^([A-Z&]|[a-z&]{1})([AEIOU]|[aeiou]{1})([A-Z&]|[a-z&]{1})([A-Z&]|[a-z&]{1})([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])([HM]|[hm]{1})([AS|as|BC|bc|BS|bs|CC|cc|CS|cs|CH|ch|CL|cl|CM|cm|DF|df|DG|dg|GT|gt|GR|gr|HG|hg|JC|jc|MC|mc|MN|mn|MS|ms|NT|nt|NL|nl|OC|oc|PL|pl|QT|qt|QR|qr|SP|sp|SL|sl|SR|sr|TC|tc|TS|ts|TL|tl|VZ|vz|YN|yn|ZS|zs|NE|ne]{2})([^A|a|E|e|I|i|O|o|U|u]{1})([^A|a|E|e|I|i|O|o|U|u]{1})([^A|a|E|e|I|i|O|o|U|u]{1})([A-Z]|[a-z]|[0-9]{1})([0-9]{1})$";
                var m = Regex.Match(curp, pattern, RegexOptions.IgnoreCase);

                if (!m.Success)
                {
                    const string err = "El CURP no tiene el formato correcto";
                    return BadRequest(err);
                }

                var client = new RestClient($"{_configuration["renapourl"]}/consultaCurp")
                {
                    Timeout = -1,
                    UserAgent = agente
                };
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"cveCurp\": \"" + curp.ToUpperInvariant().Trim() + "\"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var curpData = FoByCurpOnly.FromJson(response.Content);
                    if (curpData.StatusOper.ToUpperInvariant() == "EXITOSO")
                    {
                        object o = new
                        {
                            apellidoPaterno = curpData.Apellido1,
                            apellidoMaterno = curpData.Apellido2,
                            nombres = curpData.Nombres,
                            genero = curpData.Sexo,
                            fechaNacimiento = curpData.FechNac,
                            nacionalidad = curpData.Nacionalidad,
                            añoRegistro = curpData.AnioReg,
                            foja = curpData.Foja,
                            tomo = curpData.Tomo,
                            libro = curpData.Libro,
                            numeroActa = curpData.NumActa,
                            numeroEntidadRegistro = curpData.NumEntidadReg,
                            claveMunicipioRegistro = curpData.CveMunicipioReg,
                            claveEntidadNacimiento = curpData.CveEntidadNac
                        };

                        return Ok(o);
                    }

                    if (curpData.StatusOper.ToUpperInvariant() == "NO EXITOSO")
                    {
                        object o = new
                        {
                            message = curpData.Message
                        };

                        return Ok(o);
                    }

                    return NoContent();
                }
            }
            catch
            {
                return NotFound("No fue posible validar el curp");
            }

            return NotFound("No fue posible validar el curp");
        } //Fin

        /// <summary>
        /// Obtiene la clave única de registro de población de una persona mediante sus datos personales.
        /// Usar el método: ObtenerEstadosAcronimo para obtener la cventidadNacimiento
        /// </summary>
        /// <param name="renapoRequest">Datos personales</param>
        /// <returns>CURP de la persona registrada con esos datos</returns>
        /// <response code="200">Respuesta Valida</response>
        /// <response code="204">Respuesta Inválida: Sin contenido</response>
        /// <response code="400">Respuesta Inválida</response>
        /// <response code="404">Respuesta Inválida: No fue posible obtener el curp</response>
        [HttpPost("ValidarCurpPorDatos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ValidarCurpRenapoDatos([FromBody, Required] CurpByDataRequest renapoRequest)
        {
            try
            {
                var client = new RestClient($"{_configuration["renapourl"]}/consultaCurpPorDetalle")
                {
                    Timeout = -1,
                    UserAgent = agente
                };
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = renapoRequest.ToJson();
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var curpData = FoByDataOnly.FromJson(response.Content);
                    if (curpData.StatusOper.ToUpperInvariant() == "EXITOSO")
                    {
                        var curpStruct = curpData.CurpStruct[0];
                        object o = new
                        {
                            curp = curpStruct?.Curp,
                            apellidoPaterno = curpStruct?.Apellido1,
                            apellidoMaterno = curpStruct?.Apellido2,
                            nombres = curpStruct?.Nombres,
                            genero = curpStruct?.Sexo,
                            fechaNacimiento = curpStruct?.FechNac,
                            nacionalidad = curpStruct?.Nacionalidad,
                            añoRegistro = curpStruct?.AnioReg,
                            foja = curpStruct?.Foja,
                            tomo = curpStruct?.Tomo,
                            libro = curpStruct?.Libro,
                            numeroActa = curpStruct?.NumActa,
                            numeroEntidadRegistro = curpStruct?.NumEntidadReg,
                            claveMunicipioRegistro = curpStruct?.CveMunicipioReg,
                            claveEntidadNacimiento = curpStruct?.CveEntidadNac
                        };

                        return Ok(o);
                    }

                    if (curpData.StatusOper.ToUpperInvariant() == "NO EXITOSO")
                    {
                        object o = new
                        {
                            message = curpData.Message
                        };

                        return Ok(o);
                    }

                    return NoContent();
                }
            }
            catch
            {
                return NotFound("No fue posible obtener el curp");
            }

            return NotFound("No fue posible obtener el curp");
        } //Fin

    }
}
