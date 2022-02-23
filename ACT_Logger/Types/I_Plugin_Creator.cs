﻿using ACT.Core.Extensions;
using ACT.Core.Logger.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ACT.Core.Logger.Types
{


    public class Plugin_Assembly_Information
    {
        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_company_name")]
        public string AuthorCompanyName { get; set; }

        [JsonProperty("contact_email")]
        public string ContactEmail { get; set; }

        [JsonProperty("lastedited")]
        public string LastEdited { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("author_act_io_hash")]
        public string AuthorActIdHash { get; set; }

        [JsonProperty("dllfilename")]
        public string DllFileName { get; set; }

        [JsonProperty("githubpackagename")]
        public string GitHubPackageName { get; set; }

        [JsonProperty("githubpackageversion")]
        public string GitHubPackageVersion { get; set; }

        [JsonProperty("configpathoverride")]
        public string ConfigPathOverride { get; set; }

        [JsonProperty("fullclassname")]
        public string FullClassName { get; set; }

        [JsonProperty("systemtypeidentifier")]
        public string SystemTypeIdentifier { get; set; }

        /// <summary>
        /// Gets the Actual SystemType
        /// </summary>
        /// <returns></returns>
        [JsonIgnore()]
        public ACT_Logger_ManagementEngine.SystemTypes GetSystemType
        {
            get
            {
                return (ACT_Logger_ManagementEngine.SystemTypes)Enum.Parse(typeof(ACT_Logger_ManagementEngine.SystemTypes), SystemTypeIdentifier);
            }
        }

        public static Plugin_Assembly_Information FromJson(string json) => JsonConvert.DeserializeObject<Plugin_Assembly_Information>(json, JSONConverter.Settings);

        public string ToBase64Json() => JsonConvert.SerializeObject(this, JSONConverter.Settings).ToBase64();
        public string ToJson() => JsonConvert.SerializeObject(this, JSONConverter.Settings);
    }

#if DEBUG
    internal class PluginHashInstructionsInfo
    {
        public override string ToString()
        {
            return new Y().ToString();
        }
        private class Y
        {
            public override string ToString()
            {
                return "20d24b3e-5e8b-4f46-afe6-77e579083e3e__ACT__205412c1-e046-4b2d-af16-186a73f8cb9a  This is the EXAMPLE Source for ACT Example Plugins HASH (SHA3512)->Base64 The Second Guid Is DB UserID/CompanyID";
            }
        }
    }
#endif
}
