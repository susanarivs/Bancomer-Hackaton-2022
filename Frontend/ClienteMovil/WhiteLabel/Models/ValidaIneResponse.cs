// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using WhiteLabel.Models;
//
//    var validaIneResponse = ValidaIneResponse.FromJson(jsonString);

namespace WhiteLabel.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ValidaIneResponse
    {
        [JsonProperty("estaVigente", NullValueHandling = NullValueHandling.Ignore)]
        public string EstaVigente { get; set; }

        [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
        public Details Details { get; set; }

        [JsonProperty("motivo", NullValueHandling = NullValueHandling.Ignore)]
        public string Motivo { get; set; }
    }

    public partial class Details
    {
        [JsonProperty("CIC", NullValueHandling = NullValueHandling.Ignore)]
        public string Cic { get; set; }

        [JsonProperty("Clave de elector", NullValueHandling = NullValueHandling.Ignore)]
        public string ClaveDeElector { get; set; }

        [JsonProperty("Número de emisión", NullValueHandling = NullValueHandling.Ignore)]
        public string NumeroDeEmision { get; set; }

        [JsonProperty("Número OCR", NullValueHandling = NullValueHandling.Ignore)]
        public string NumeroOcr { get; set; }

        [JsonProperty("Año de registro", NullValueHandling = NullValueHandling.Ignore)]
        public string AnioDeRegistro { get; set; }

        [JsonProperty("Año de emisión", NullValueHandling = NullValueHandling.Ignore)]
        public string AnioDeEmision { get; set; }
    }

    public partial class ValidaIneResponse
    {
        public static ValidaIneResponse FromJson(string json) => JsonConvert.DeserializeObject<ValidaIneResponse>(json, WhiteLabel.Models.ConverterIneResponse.Settings);
    }

    public static class SerializeIneResponse
    {
        public static string ToJson(this ValidaIneResponse self) => JsonConvert.SerializeObject(self, WhiteLabel.Models.ConverterIneResponse.Settings);
    }

    internal static class ConverterIneResponse
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
