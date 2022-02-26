using ACT.Core.Interfaces.Common.ErrorLogging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ACT.Core.Logger.Configuration
{

    public partial class WindowsEvent_ConfigurationData : I_ErrorLoggable_WindowsEvent_Configuration
    {
        [JsonProperty("base-method", NullValueHandling = NullValueHandling.Ignore)]
        public string BaseMethod { get; set; }

        [JsonProperty("base-config", NullValueHandling = NullValueHandling.Ignore, ItemConverterType = typeof(WindowsEvent_BaseConfig))]
        public I_WindowsEvent_BaseConfig BaseConfig { get; set; }

        public static WindowsEvent_ConfigurationData LoadFromJson(string json) => JsonConvert.DeserializeObject<WindowsEvent_ConfigurationData>(json, JSONConverter.Settings);

        public I_ErrorLoggable_WindowsEvent_Configuration FromJson(string json) => (I_ErrorLoggable_WindowsEvent_Configuration)JsonConvert.DeserializeObject<WindowsEvent_ConfigurationData>(json, JSONConverter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, JSONConverter.Settings);

    }

    public class WindowsEvent_BaseConfig : I_WindowsEvent_BaseConfig
    {
        [JsonProperty("event-source-name", NullValueHandling = NullValueHandling.Ignore)]
        public string EventSourceName { get; set; }

        [JsonProperty("event-log-name", NullValueHandling = NullValueHandling.Ignore)]
        public string EventLogName { get; set; }

        [JsonProperty("append-source-to-alllogs", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AppendSourceToAlllogs { get; set; }

        [JsonProperty("encrypt-log", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EncryptLog { get; set; }

        [JsonProperty("encryption-method", NullValueHandling = NullValueHandling.Ignore)]
        public string EncryptionMethod { get; set; }

        [JsonProperty("custom-data", NullValueHandling = NullValueHandling.Ignore, ItemConverterType = typeof(CustomNameValueData))]
        public List<Interfaces.Common.I_CustomNameValueData> CustomData { get; set; }

        [JsonProperty("plugin-package", NullValueHandling = NullValueHandling.Ignore)]
        public string Plugin_Package { get; set; }

        [JsonProperty("plugin-full-classname", NullValueHandling = NullValueHandling.Ignore)]
        public string Plugin_Full_ClassName { get; set; }
    }
}
