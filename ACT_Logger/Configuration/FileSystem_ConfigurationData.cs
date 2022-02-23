using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ACT.Core.Logger.Configuration
{


    public class FileSystem_ConfigurationData
    {
        [JsonProperty("base-method", NullValueHandling = NullValueHandling.Ignore)]
        public string BaseMethod { get; set; }

        [JsonProperty("base-config", NullValueHandling = NullValueHandling.Ignore)]
        public FileSystem_BaseConfig FilesystemConfig { get; set; }

        public static FileSystem_ConfigurationData FromJson(string json) => JsonConvert.DeserializeObject<FileSystem_ConfigurationData>(json, JSONConverter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, JSONConverter.Settings);
    }

    public partial class FileSystem_BaseConfig
    {
        [JsonProperty("filesystem-root", NullValueHandling = NullValueHandling.Ignore)]
        public string FilesystemRoot { get; set; }

        [JsonProperty("log-filename-convention", NullValueHandling = NullValueHandling.Ignore)]
        public string LogFilenameConvention { get; set; }

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
