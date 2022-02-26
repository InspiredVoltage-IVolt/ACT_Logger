using ACT.Core.Interfaces.Common;
using ACT.Core.Interfaces.Common.ErrorLogging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ACT.Core.Logger.Configuration
{


    public class WebService_ConfigurationData : I_ErrorLoggable_WebService_Configuration
    {
        [JsonProperty("base-method", NullValueHandling = NullValueHandling.Ignore)]
        public string BaseMethod { get; set; }

        [JsonProperty("base-config", NullValueHandling = NullValueHandling.Ignore, ItemConverterType = typeof(WebService_BaseConfig))]
        public I_WebService_BaseConfig BaseConfig { get; set; }

        public static WebService_ConfigurationData LoadFromJson(string json) => JsonConvert.DeserializeObject<WebService_ConfigurationData>(json, JSONConverter.Settings);

        public I_ErrorLoggable_WebService_Configuration FromJson(string json) { return (I_ErrorLoggable_WebService_Configuration)JsonConvert.DeserializeObject<WebService_ConfigurationData>(json, JSONConverter.Settings); }
        
        public string ToJson() => JsonConvert.SerializeObject(this, JSONConverter.Settings);        
    }

    public class WebService_BaseConfig : I_WebService_BaseConfig
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

        [JsonProperty("custom-data", NullValueHandling = NullValueHandling.Ignore, ItemConverterType = typeof(CustomNameValueData))]
        public List<I_CustomNameValueData> CustomData { get; set; }

        [JsonProperty("plugin-package", NullValueHandling = NullValueHandling.Ignore)]
        public string Plugin_Package { get; set; }

        [JsonProperty("plugin-full-classname", NullValueHandling = NullValueHandling.Ignore)]
        public string Plugin_Full_ClassName { get; set; }         
    }

}
