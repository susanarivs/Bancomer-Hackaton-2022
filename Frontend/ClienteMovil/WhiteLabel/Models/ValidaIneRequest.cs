// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using WhiteLabel.Models;
//
//    var validaIneRequest = ValidaIneRequest.FromJson(jsonString);

namespace WhiteLabel.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ValidaIneRequest
    {
        [JsonProperty("idCiudadano", NullValueHandling = NullValueHandling.Ignore)]
        public string IdCiudadano { get; set; }

        [JsonProperty("cic", NullValueHandling = NullValueHandling.Ignore)]
        public string Cic { get; set; }
    }

    public partial class ValidaIneRequest
    {
        public static ValidaIneRequest FromJson(string json) => JsonConvert.DeserializeObject<ValidaIneRequest>(json, WhiteLabel.Models.ConverterIneRequest.Settings);
    }

    public static class SerializeIneRequest
    {
        public static string ToJson(this ValidaIneRequest self) => JsonConvert.SerializeObject(self, WhiteLabel.Models.ConverterIneRequest.Settings);
    }

    internal static class ConverterIneRequest
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
