using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ACT.Core.Logger.Configuration
{


    public partial class ADONet_ConfigurationData
    {
        [JsonProperty("base-method", NullValueHandling = NullValueHandling.Ignore)]
        public string BaseMethod { get; set; }

        [JsonProperty("base-config", NullValueHandling = NullValueHandling.Ignore)]
        public ADONet_BaseConfig BaseConfig { get; set; }

        public static ADONet_ConfigurationData FromJson(string json) => JsonConvert.DeserializeObject<ADONet_ConfigurationData>(json, JSONConverter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, JSONConverter.Settings);

    }

    public class ADONet_BaseConfig
    {
        [JsonProperty("database-connection-string", NullValueHandling = NullValueHandling.Ignore)]
        public string DatabaseConnectionString { get; set; }

        [JsonProperty("plugin-dll-name", NullValueHandling = NullValueHandling.Ignore)]
        public string PluginDllName { get; set; }

        [JsonProperty("insert-query", NullValueHandling = NullValueHandling.Ignore)]
        public string InsertQuery { get; set; }

        [JsonProperty("update-query", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdateQuery { get; set; }

        [JsonProperty("delete-query", NullValueHandling = NullValueHandling.Ignore)]
        public string DeleteQuery { get; set; }

        [JsonProperty("encrypt-log", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EncryptLog { get; set; }

        [JsonProperty("encryption-method", NullValueHandling = NullValueHandling.Ignore)]
        public string EncryptionMethod { get; set; }

        [JsonProperty("custom-data", NullValueHandling = NullValueHandling.Ignore)]
        public List<CustomNameValueData> CustomData { get; set; }

        [JsonProperty("plugin-package", NullValueHandling = NullValueHandling.Ignore)]
        public string Plugin_Package { get; set; }

        [JsonProperty("plugin-full-classname", NullValueHandling = NullValueHandling.Ignore)]
        public string Plugin_Full_ClassName { get; set; }
        
    }
}
