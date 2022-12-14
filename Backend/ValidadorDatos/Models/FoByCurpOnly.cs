// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using AltergoAPI.Nss.Core.Models;
//
//    var foByCurpOnly = FoByCurpOnly.FromJson(jsonString);

namespace AltergoAPI.Nss.Core.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class FoByCurpOnly
    {
        [JsonProperty("statusOper", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusOper { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("tipoError", NullValueHandling = NullValueHandling.Ignore)]
        public string TipoError { get; set; }

        [JsonProperty("codigoError", NullValueHandling = NullValueHandling.Ignore)]
        public string CodigoError { get; set; }

        [JsonProperty("sessionID", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionId { get; set; }

        [JsonProperty("apellido1", NullValueHandling = NullValueHandling.Ignore)]
        public string Apellido1 { get; set; }

        [JsonProperty("apellido2", NullValueHandling = NullValueHandling.Ignore)]
        public string Apellido2 { get; set; }

        [JsonProperty("nombres", NullValueHandling = NullValueHandling.Ignore)]
        public string Nombres { get; set; }

        [JsonProperty("sexo", NullValueHandling = NullValueHandling.Ignore)]
        public string Sexo { get; set; }

        [JsonProperty("fechNac", NullValueHandling = NullValueHandling.Ignore)]
        public string FechNac { get; set; }

        [JsonProperty("nacionalidad", NullValueHandling = NullValueHandling.Ignore)]
        public string Nacionalidad { get; set; }

        [JsonProperty("docProbatorio", NullValueHandling = NullValueHandling.Ignore)]
        public string DocProbatorio { get; set; }

        [JsonProperty("anioReg", NullValueHandling = NullValueHandling.Ignore)]
        public string AnioReg { get; set; }

        [JsonProperty("foja", NullValueHandling = NullValueHandling.Ignore)]
        public string Foja { get; set; }

        [JsonProperty("tomo", NullValueHandling = NullValueHandling.Ignore)]
        public string Tomo { get; set; }

        [JsonProperty("libro", NullValueHandling = NullValueHandling.Ignore)]
        public string Libro { get; set; }

        [JsonProperty("numActa", NullValueHandling = NullValueHandling.Ignore)]
        public string NumActa { get; set; }

        [JsonProperty("numEntidadReg", NullValueHandling = NullValueHandling.Ignore)]
        public string NumEntidadReg { get; set; }

        [JsonProperty("cveMunicipioReg", NullValueHandling = NullValueHandling.Ignore)]
        public string CveMunicipioReg { get; set; }

        [JsonProperty("cveEntidadNac", NullValueHandling = NullValueHandling.Ignore)]
        public string CveEntidadNac { get; set; }

        [JsonProperty("cveEntidadEmisora", NullValueHandling = NullValueHandling.Ignore)]
        public string CveEntidadEmisora { get; set; }

        [JsonProperty("statusCurp", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusCurp { get; set; }

        [JsonProperty("numRegExtranjeros", NullValueHandling = NullValueHandling.Ignore)]
        public string NumRegExtranjeros { get; set; }

        [JsonProperty("curp", NullValueHandling = NullValueHandling.Ignore)]
        public string Curp { get; set; }

        [JsonProperty("crip", NullValueHandling = NullValueHandling.Ignore)]
        public string Crip { get; set; }

        [JsonProperty("folioCarta", NullValueHandling = NullValueHandling.Ignore)]
        public string FolioCarta { get; set; }
    }

    public partial class FoByCurpOnly
    {
        public static FoByCurpOnly FromJson(string json) => JsonConvert.DeserializeObject<FoByCurpOnly>(json, AltergoAPI.Nss.Core.Models.ConverterFoByCurpOnly.Settings);
    }

    public static class SerializeFoByCurpOnly
    {
        public static string ToJson(this FoByCurpOnly self) => JsonConvert.SerializeObject(self, AltergoAPI.Nss.Core.Models.ConverterFoByCurpOnly.Settings);
    }

    internal static class ConverterFoByCurpOnly
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
