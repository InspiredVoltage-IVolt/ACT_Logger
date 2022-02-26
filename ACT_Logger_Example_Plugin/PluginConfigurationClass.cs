using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ACT.Applications.ACT_Logger.Plugins
{


    public class ActExamplePackageConfig
    {
        [JsonProperty("package-id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? PackageId { get; set; }

        [JsonProperty("package-name", NullValueHandling = NullValueHandling.Ignore)]
        public string PackageName { get; set; }

        [JsonProperty("certification-hash", NullValueHandling = NullValueHandling.Ignore)]
        public string CertificationHash { get; set; }

        [JsonProperty("filesystem", NullValueHandling = NullValueHandling.Ignore)]
        public Filesystem Filesystem { get; set; }

        [JsonProperty("verbose", NullValueHandling = NullValueHandling.Ignore)]
        public string Verbose { get; set; }

        [JsonProperty("emailcriticalerrors", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Emailcriticalerrors { get; set; }

        [JsonProperty("emailaddress", NullValueHandling = NullValueHandling.Ignore)]
        public string Emailaddress { get; set; }

        public static ActExamplePackageConfig FromJson(string json) => JsonConvert.DeserializeObject<ActExamplePackageConfig>(json, Converter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, Converter.Settings);

    }

    public class Filesystem
    {
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("fileformat", NullValueHandling = NullValueHandling.Ignore)]
        public string Fileformat { get; set; }

        [JsonProperty("maxfilesize", NullValueHandling = NullValueHandling.Ignore)]
        public long? Maxfilesize { get; set; }

        [JsonProperty("daysperfile", NullValueHandling = NullValueHandling.Ignore)]
        public long? Daysperfile { get; set; }
    }

    internal static class Converter
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
