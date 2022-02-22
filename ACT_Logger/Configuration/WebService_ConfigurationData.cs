using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ACT.Core.Logger.Configuration
{


    public class WebService_ConfigurationData
    {
        [JsonProperty("base-method", NullValueHandling = NullValueHandling.Ignore)]
        public string BaseMethod { get; set; }

        [JsonProperty("base-config", NullValueHandling = NullValueHandling.Ignore)]
        public WebService_BaseConfig BaseConfig { get; set; }

        public static WebService_ConfigurationData FromJson(string json) => JsonConvert.DeserializeObject<WebService_ConfigurationData>(json, JSONConverter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, JSONConverter.Settings);
    }

    public class WebService_BaseConfig
    {
        [JsonProperty("service-endpoint", NullValueHandling = NullValueHandling.Ignore)]
        public string ServiceEndpoint { get; set; }

        [JsonProperty("get-url-information", NullValueHandling = NullValueHandling.Ignore)]
        public string GetUrlInformation { get; set; }

        [JsonProperty("response-type", NullValueHandling = NullValueHandling.Ignore)]
        public string ResponseType { get; set; }

        [JsonProperty("encrypt-log", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EncryptLog { get; set; }

        [JsonProperty("encryption-method", NullValueHandling = NullValueHandling.Ignore)]
        public string EncryptionMethod { get; set; }

        [JsonProperty("custom-data", NullValueHandling = NullValueHandling.Ignore)]
        public List<CustomNameValueData> CustomData { get; set; }
    }

}
