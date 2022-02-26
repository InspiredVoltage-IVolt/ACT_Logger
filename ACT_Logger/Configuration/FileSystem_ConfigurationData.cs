using ACT.Core.Interfaces.Common;
using ACT.Core.Interfaces.Common.ErrorLogging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ACT.Core.Logger.Configuration
{


    public class FileSystem_ConfigurationData : I_ErrorLoggable_FileSystem_Configuration
    {
        [JsonProperty("base-method", NullValueHandling = NullValueHandling.Ignore)]
        public string BaseMethod { get; set; }
                
        [JsonProperty("base-config", NullValueHandling = NullValueHandling.Ignore, ItemConverterType = typeof(FileSystem_BaseConfig))]
        public I_FileSystem_BaseConfig FilesystemConfig { get; set; }

        public static FileSystem_ConfigurationData LoadFromJson(string json) => JsonConvert.DeserializeObject<FileSystem_ConfigurationData>(json, JSONConverter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, JSONConverter.Settings);

        public I_ErrorLoggable_FileSystem_Configuration FromJson(string json)
        {
            var _tmp = FileSystem_ConfigurationData.LoadFromJson(json);
            this.BaseMethod = _tmp.BaseMethod;
            this.FilesystemConfig = _tmp.FilesystemConfig;
            return this;
        }
    }

    public class FileSystem_BaseConfig : I_FileSystem_BaseConfig
    {
        public FileSystem_BaseConfig()
        {
          
        }

        [JsonProperty("filesystem-root", NullValueHandling = NullValueHandling.Ignore)]
        public string FilesystemRoot { get; set; }

        [JsonProperty("log-filename-convention", NullValueHandling = NullValueHandling.Ignore)]
        public string LogFilenameConvention { get; set; }

        [JsonProperty("encrypt-log", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EncryptLog { get; set; }

        [JsonProperty("encryption-method", NullValueHandling = NullValueHandling.Ignore)]
        public string EncryptionMethod { get; set; }

        [JsonProperty("plugin-package", NullValueHandling = NullValueHandling.Ignore)]
        public string Plugin_Package { get; set; }

        [JsonProperty("plugin-full-classname", NullValueHandling = NullValueHandling.Ignore)]
        public string Plugin_Full_ClassName { get; set; }

        [JsonProperty("custom-data", NullValueHandling = NullValueHandling.Ignore, ItemConverterType = typeof(CustomNameValueData))]
        public List<I_CustomNameValueData> CustomData { get; set; }
    }

}
